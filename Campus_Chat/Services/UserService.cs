using Campus_Chat.Models;

namespace Campus_Chat.Services;

public class UserService(IUserStore store)
{
    /// <summary>Gets every user.</summary>
    /// <returns>All users, or an empty list when there are none.</returns>
    public Task<IReadOnlyList<User>> GetAllAsync() => store.GetAllAsync();

    /// <summary>Gets one user by ID.</summary>
    /// <returns>The user, or null when no user has that ID.</returns>
    public Task<User?> GetByIdAsync(Guid id) => store.GetByIdAsync(id);
    
    
    /// <summary>Updates a user's own profile.</summary>
    /// <returns>The updated user.</returns>
    /// <exception cref="UnauthorizedAccessException">The caller is not the profile owner.</exception>
    /// <exception cref="ArgumentException">The name, note or study year is invalid.</exception>
    /// <exception cref="KeyNotFoundException">No user has this ID.</exception>
    public async Task<User?> UpdateProfileAsync(Guid callerId, Guid userId, string displayName, string note, string avatarColour, int studyYear)
    {
        
        if (callerId != userId)
        {
            throw new UnauthorizedAccessException("Unauthorized user. You cannot edit another profile");
        }
        
        
        var name = displayName?.Trim() ?? "";
        var cleanNote = note?.Trim() ?? "";
        
        
        if (name.Length == 0)
        {
            throw new ArgumentException("Display name cannot be empty.");
        }

        if (name.Length is < 2 or > 30)
        {
            throw new ArgumentException("Display name must be between 2 and 30 characters.");
        }

        if (cleanNote.Length > 200)
        {
            throw new ArgumentException("Note can be at most 200 characters.");
        }

        if (studyYear is < 1 or > 5)
        {
            throw new ArgumentException("Study year must be between 1 and 5.");
        }

        var existing = await store.GetByIdAsync(userId) ?? throw new KeyNotFoundException("This profile does not exist");
        
        if (existing is null)
        {
            return null;
        }
        
        var updated = existing with
        {
            DisplayName = displayName.Trim(), 
            Note = note.Trim(), 
            AvatarColour = avatarColour.Trim(), 
            StudyYear = studyYear 
        };

        await store.SaveAsync(updated);

        return updated;
    }
}