using Campus_Chat.Models;



namespace Campus_Chat.Services;

public class InMemoryUserStore : IUserStore
{
    private readonly List<User> users = new();
    private readonly object gate = new();   // many browsers use this at once, so we lock

    public Task<IReadOnlyList<User>> GetAllAsync()
    {
        lock (gate)
        {
            return Task.FromResult<IReadOnlyList<User>>(users.ToList());
        }
    }

    public Task<User?> GetByIdAsync(Guid id)
    {
        lock (gate)
        {
            return Task.FromResult(users.FirstOrDefault(u => u.Id == id));
        }
    }

    public Task SaveAsync(User user)
    {
        lock (gate)
        {
            var index = users.FindIndex(u => u.Id == user.Id);
            if (index >= 0) users[index] = user;
            else users.Add(user);
        }
        return Task.CompletedTask;
    }
    
    public InMemoryUserStore()
    {
        users.Add(new User { Id=SeedIds.Ola, DisplayName = "Ola", StudyYear = 2, AvatarColour = "green"});
        users.Add(new User { Id=SeedIds.Kari,DisplayName = "Kari", StudyYear = 2, AvatarColour = "red"});
        users.Add(new User { Id=SeedIds.Auditor,DisplayName = "Auditor", StudyYear = 4, IsAuditor = true, AvatarColour = "blue"});
        users.Add(new User { Id=SeedIds.Lecturer,DisplayName = "Lecturer", StudyYear = 5, AvatarColour = "red"});
    } 
        
/*
 
      public Guid Id { get; init; } = Guid.NewGuid();
    public string DisplayName { get; init; } = "";
      public string Note { get; init; } = "";
    public string AvatarColour { get; init; } = "";
      public int StudyYear { get; init; }
    public bool IsAuditor { get; init; }
 
 */
    
    
}
