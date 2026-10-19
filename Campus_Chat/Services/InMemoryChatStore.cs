using Campus_Chat.Models;
using Campus_Chat.Models.Post;

namespace Campus_Chat.Services;

public class InMemoryChatStore : IChatStore
{
     
     private readonly List<ChatThread> threads = new();
     private readonly List<ThreadMembership> memberships = new();
     private readonly List<Post> posts = new();
     
     private readonly object gate = new();   // many browsers use this at once, so we lock

     public Task<IReadOnlyList<ChatThread>> GetAllThreadsAsync()
     {
          lock (gate)
          {
               return Task.FromResult<IReadOnlyList<ChatThread>>(threads.ToList());
          }
     }

     public Task<ChatThread?> GetThreadByIdAsync(Guid id)
     {
          lock (gate)
          {
               return Task.FromResult(threads.FirstOrDefault(t => t.Id == id));
          }
     }

     public Task SaveThreadAsync(ChatThread chat)
     {
          lock (gate)
          {
               var index = threads.FindIndex(t => t.Id == chat.Id);
               if (index >= 0) threads[index] = chat;
               else threads.Add(chat);
          }

          return Task.CompletedTask;
     }

     // posts
     
     public Task<IReadOnlyList<Post>> GetPostsAsync(Guid threadId)
     {
          lock (gate)
          {
               var results = posts
                    .Where(p => p.ThreadId == threadId)
                    .OrderBy(p => p.Time).ToList();
               return Task.FromResult<IReadOnlyList<Post>>(results);
          }
     }

     public Task SavePostAsync(Post post)
     {
          lock (gate)
          {
               var index = posts.FindIndex(p => p.Id == post.Id);
               if (index >= 0) posts[index] = post;
               else posts.Add(post);
          }

          return Task.CompletedTask;
     }
     
     public Task DeletePostAsync(Guid id)
     {
          lock (gate)
          { 
               posts.RemoveAll(p => p.Id == id);
          }
          return Task.CompletedTask;
     }


     public Task<IReadOnlyList<ThreadMembership>> GetMembershipsForUserAsync(Guid userId)
     {
          lock (gate)
          {
               var result = memberships.Where(m => m.UserId == userId).ToList();
               return Task.FromResult<IReadOnlyList<ThreadMembership>>(result);
          }
     }
     
     
     public Task<ThreadMembership?> GetMembershipAsync(Guid userId, Guid threadId)
     {
          lock (gate)
          {
               
               return Task.FromResult(
                    memberships.FirstOrDefault(m => m.ThreadId == threadId && m.UserId == userId));
          }
     }

     
     /*
      * SaveMembershipAsync matches on UserId only. When a user joins a second thread,
      * it overwrites their first membership. Match on both UserId and ThreadId.
      */
     public Task SaveMembershipAsync(ThreadMembership threadMembership)
     {
          lock (gate)
          {
               var index = memberships.FindIndex(m => m.UserId == threadMembership.UserId && m.ThreadId == threadMembership.ThreadId);
               if (index >= 0) memberships[index] = threadMembership;
               else memberships.Add(threadMembership);
          }

          return Task.CompletedTask;
     }

     public Task DeleteMembershipAsync(Guid userId, Guid threadId)
     {
          lock (gate)
          {
               memberships.RemoveAll(m => m.UserId == userId && m.ThreadId == threadId);
          }

          return Task.CompletedTask;
     }
     
}
