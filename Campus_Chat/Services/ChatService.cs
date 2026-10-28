using Campus_Chat.Models;

namespace Campus_Chat.Services;

public class ChatService(IChatStore store, UserService users)
{
    /// <summary>Gets every thread name. The join code is never included.</summary>
    /// <returns>All threads, or an empty list when there are none.</returns>
    public async Task<IReadOnlyList<ThreadSummary>> GetThreadSummariesAsync()
    {
        var threads = await store.GetAllThreadsAsync();
        var summaries = new List<ThreadSummary>();
        foreach (var thread in threads)
        {
            summaries.Add(await ToSummaryAsync(thread));
        }
        return summaries;
    }
    
    /// <summary>Writes a new post, or a reply when parentPostId is given.</summary>
    /// <returns>The saved post, with its author and time set by the service.</returns>
    /// <exception cref="ArgumentException">The text is empty or too long.</exception>
    /// <exception cref="KeyNotFoundException">The thread, or the post being replied to, does not exist.</exception>
    /// <exception cref="UnauthorizedAccessException">The thread is private and the caller is not a member.</exception>
    public async Task<Post> AddPostAsync(Guid callerId, Guid threadId, string text, Guid? parentPostId = null)
    {
        var clean = text?.Trim() ?? "";
        if (clean.Length == 0)
        {
            throw new ArgumentException("A post can't be empty.");
        }
        if (clean.Length > 2000)
        {
            throw new ArgumentException("A post can be at most 2000 characters.");
        }

        var thread = await store.GetThreadByIdAsync(threadId)
                     ?? throw new KeyNotFoundException("This thread does not exist.");

        // Anyone may post in a public thread. A private thread needs membership.
        // The audit role reads private threads but does not write in them.
        if (thread.IsPrivate && await store.GetMembershipAsync(callerId, threadId) is null)
        {
            throw new UnauthorizedAccessException("Join this thread with its code before you post.");
        }

        
       
        
        
        if (parentPostId is not null)
        {
            var existing = await store.GetPostsAsync(threadId);
            if (existing.All(p => p.Id != parentPostId || p.IsDeleted))
            {
                throw new KeyNotFoundException("The post you are replying to no longer exists.");
            }
            
            
        }

        var post = new Post
        {
            ThreadId = threadId,
            AuthorId = callerId,   // from the caller, never from the page's form
            Text = clean,
            ParentPostId = parentPostId
            // Id and Time are set by the model defaults
        };

        await store.SavePostAsync(post);
        return post;
    }
    
    /// <summary>Gets the public summary of one thread.</summary>
    /// <returns>The summary, or null when no thread has this ID.</returns>
    public async Task<ThreadSummary?> GetSingleThreadSummaryAsync(Guid threadId)
    {
        var thread = await store.GetThreadByIdAsync(threadId);
        return thread is null ? null : await ToSummaryAsync(thread);
    }
    
    
    private async Task<ThreadSummary> ToSummaryAsync(ChatThread thread)
    {
        var count = await store.GetMembershipCountAsync(thread.Id);
        return new ThreadSummary(thread.Id, thread.Name, count, thread.IsPrivate);
    }
    /// <summary>Gets the threads that a user has joined, ordered by name.</summary>
    /// <remarks>When someone else is viewing, private threads are left out, because membership of a private thread is not for others to see.</remarks>
    /// <returns>The joined threads, or an empty list when the user has joined none.</returns>
    
    public async Task<IReadOnlyList<ThreadSummary>> GetThreadsForUserAsync(Guid callerId, Guid userId)
    {
        var memberships = await store.GetMembershipsForUserAsync(userId);
        var result = new List<ThreadSummary>();

        foreach (var membership in memberships)
        {
            var thread = await store.GetThreadByIdAsync(membership.ThreadId);
            if (thread is null) continue;                          // thread no longer exists
            if (thread.IsPrivate && callerId != userId) continue;  // hide private threads from other viewers

            result.Add(await ToSummaryAsync(thread));
        }

        return result.OrderBy(t => t.Name).ToList();
    }
    
    public async Task<int> GetMembershipCountAsync(Guid threadId)
    {
        return await store.GetMembershipCountAsync(threadId);
    }
    
    /// <summary>
    /// 
    /// </summary>
    /// <param name="callerId"></param>
    /// <param name="threadId"></param>
    /// <param name="code"></param>
    /// <exception cref="KeyNotFoundException">No thrad with this ID</exception>
    /// <exception cref="Exception"></exception>
    public async Task JoinThreadAsync(Guid callerId, Guid threadId, string? code)
    {
        
        var thread = await store.GetThreadByIdAsync(threadId) ?? throw new KeyNotFoundException("This thread does not exist");
        
        
        // Checks is this user a member of the thread?
        var membership = await store.GetMembershipAsync(callerId,thread.Id);

        if (membership is not null)
        {
            return;
        }

        if (thread.IsPrivate)
        {
            var password = code?.Trim();
            if (thread.JoinCode is null || !string.Equals(password, thread.JoinCode, StringComparison.Ordinal))
            {
                throw new UnauthorizedAccessException("Wrong entered code password.");
            }
        }
        
        await store.SaveMembershipAsync(new ThreadMembership
        {
            UserId = callerId,
            ThreadId = threadId,
        });
    }
    /// <summary>Removes the caller from a thread. Leaving a thread you are not in does nothing.</summary>
    public Task LeaveThreadAsync(Guid callerId, Guid threadId) => store.DeleteMembershipAsync(callerId, threadId);
    
    
    /// <summary>Gets the posts of a thread that the caller is allowed to read.</summary>
    /// <returns>Posts oldest first, or an empty list when the thread has none.</returns>
    /// <exception cref="KeyNotFoundException">No thread has this ID.</exception>
    /// <exception cref="UnauthorizedAccessException">The thread is private and the caller is neither a member nor an auditor.</exception>
    public async Task<IReadOnlyList<Post>> GetPostsAsync(Guid callerId, Guid threadId)
    {
        var thread = await store.GetThreadByIdAsync(threadId)
                     ?? throw new KeyNotFoundException("This thread does not exist.");

        if (thread.IsPrivate)
        {
            var caller = await users.GetByIdAsync(callerId);
            var isAuditor = caller?.IsAuditor == true;
            var isMember = await store.GetMembershipAsync(callerId, threadId) is not null;

            if (!isAuditor && !isMember)
            {
                throw new UnauthorizedAccessException("This thread needs a code. Join it from the thread list.");
            }
        }

        return await store.GetPostsAsync(threadId);
    }
    
    /// <summary>Deletes one of the caller's own posts.</summary>
    /// <remarks>A post with replies is kept as an empty "deleted" post so the replies stay attached. A post without replies is removed.
    /// Deleting a post that is already deleted does nothing.</remarks>
    /// <exception cref="KeyNotFoundException">No post with this ID exists in this thread.</exception>
    /// <exception cref="UnauthorizedAccessException">The post belongs to somebody else.</exception>
    public async Task DeletePostAsync(Guid callerId, Guid threadId, Guid postId)
    {
        var all = await store.GetPostsAsync(threadId);
        var post = all.FirstOrDefault(p => p.Id == postId)
                   ?? throw new KeyNotFoundException("This post does not exist.");

        if (post.IsDeleted) return;

        // Checked here, not just by hiding the button. Not even the audit role may delete other people's posts.
        if (post.AuthorId != callerId)
        {
            throw new UnauthorizedAccessException("You can only delete your own posts.");
        }

        if (all.Any(p => p.ParentPostId == postId))
        {
            await store.SavePostAsync(post with { Text = "", AuthorId = Guid.Empty, IsDeleted = true });
        }
        else
        {
            await store.DeletePostAsync(postId);
        }
    }
    
}