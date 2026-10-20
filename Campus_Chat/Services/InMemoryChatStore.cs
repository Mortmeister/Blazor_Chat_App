using Campus_Chat.Models;

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

     public Task SaveThreadAsync(ChatThread thread)
     {
          lock (gate)
          {
               var index = threads.FindIndex(t => t.Id == thread.Id);
               if (index >= 0) threads[index] = thread;
               else threads.Add(thread);
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

     public Task<int> GetMembershipCountAsync(Guid threadId)
     {
          lock (gate)
          {
               int count = memberships.Count(m => m.ThreadId == threadId);
               return Task.FromResult(count);
          }
     }

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

     public InMemoryChatStore() => Seed();
     private void Seed()
     {
          threads.Add(new ChatThread { Id = SeedIds.GeneralThread, Name = "Test" });
          threads.Add(new ChatThread { Name = "Private thread", JoinCode = "123", IsPrivate = true });
          threads.Add(new ChatThread { Id = SeedIds.PrivateThread, Name = "Study Group" });
          
          memberships.Add(new ThreadMembership{ThreadId = SeedIds.GeneralThread, UserId = SeedIds.Kari});
          memberships.Add(new ThreadMembership{ThreadId = SeedIds.GeneralThread, UserId = SeedIds.Ola});
          memberships.Add(new ThreadMembership{ThreadId = SeedIds.PrivateThread, UserId = SeedIds.Ola});
          
          var question = new Post
          {
               ThreadId = SeedIds.GeneralThread, 
               AuthorId = SeedIds.Kari,
               Text = "This is a post test!",
               Time = DateTimeOffset.UtcNow.AddHours(-5)
          };
          posts.Add(question);
          
          posts.Add(new Post
          {
               ThreadId = SeedIds.GeneralThread,
               AuthorId = SeedIds.Lecturer,
               Text = "It's on the course page under Documents.",
               Time = DateTimeOffset.UtcNow.AddHours(-4),
               ParentPostId = question.Id      // this is what makes it a reply
          });
          
          var questionPrivate = new Post
          {
               ThreadId = SeedIds.PrivateThread, 
               AuthorId = SeedIds.Ola,
               Text = "This is a PRIVATE MESSAGE!!",
               Time = DateTimeOffset.UtcNow.AddHours(-5)
          };
          posts.Add(questionPrivate);
          
          posts.Add(new Post
          {
               ThreadId = SeedIds.PrivateThread,
               AuthorId = SeedIds.Lecturer,
               Text = "Relaaax bro",
               Time = DateTimeOffset.UtcNow.AddHours(-4),
               ParentPostId = questionPrivate.Id
          });
     }
}
