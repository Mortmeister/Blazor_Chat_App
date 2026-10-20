using Campus_Chat.Models;
using Campus_Chat.Models.Post;

namespace Campus_Chat.Services;

public interface IChatStore
{
  // Threads
  Task<IReadOnlyList<ChatThread>> GetAllThreadsAsync();
  Task<ChatThread?> GetThreadByIdAsync(Guid id);
  Task SaveThreadAsync(ChatThread thread);

  // Memberships
  Task<IReadOnlyList<ThreadMembership>> GetMembershipsForUserAsync(Guid userId);
  Task<ThreadMembership?> GetMembershipAsync(Guid userId, Guid threadId);
  Task<int> GetMembershipCountAsync(Guid threadId);
  Task SaveMembershipAsync(ThreadMembership threadMembership);
  Task DeleteMembershipAsync(Guid userId, Guid threadId);
  
  // Posts
  Task<IReadOnlyList<Post>> GetPostsAsync(Guid threadId);
  Task SavePostAsync(Post post);
  Task DeletePostAsync(Guid id);
}