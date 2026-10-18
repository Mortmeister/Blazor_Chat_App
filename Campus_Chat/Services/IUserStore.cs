using Campus_Chat.Models;

namespace Campus_Chat.Services;

public interface IUserStore
{
    Task<IReadOnlyList<User>> GetAllAsync();
    Task<User?> GetByIdAsync(Guid id);
    Task SaveAsync(User user); 
}