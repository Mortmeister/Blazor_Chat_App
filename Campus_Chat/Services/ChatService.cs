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
            int count = await store.GetMembershipCountAsync(thread.Id);

            summaries.Add(new ThreadSummary(
                thread.Id,
                thread.Name,
                count,
                thread.IsPrivate
            ));
        }

        return summaries;
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