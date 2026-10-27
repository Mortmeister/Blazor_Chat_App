using Campus_Chat.Models;

namespace Campus_Chat.Services;

public class ChatService(IChatStore store)
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
}