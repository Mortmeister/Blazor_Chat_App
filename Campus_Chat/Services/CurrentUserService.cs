namespace Campus_Chat.Services;

using Campus_Chat.Models;
using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;


/// <summary>Holds who is signed in for one browser tab (one circuit).</summary>
public class CurrentUserService(ProtectedSessionStorage storage, UserService users)
{
    private const string Key = "currentUserId";
    private bool Loaded;
    public event Action? OnChange;

    /// <summary>The signed-in user, or null when nobody is signed in.</summary>
    public User? User { get; private set; }
    
    /// <summary>Reads the stored choice once. Calls from a page's OnInitializedAsync.</summary>
    public async Task InitializeAsync()
    {
        if (Loaded) return;

        var result = await storage.GetAsync<Guid>(Key);

        if (result.Success)
        {
            User = await users.GetByIdAsync(result.Value);
        }

        Loaded = true;
        NotifyStateChanged();
    }

    public async Task SignInUser(Guid userId)
    {
        User = await users.GetByIdAsync(userId) ?? throw new KeyNotFoundException("This user does not exist");
        await storage.SetAsync(Key, userId);
        Loaded = true;
        
        NotifyStateChanged();
    }

    public async Task SignOutUser()
    {
        User = null;
        await storage.DeleteAsync(Key);
        NotifyStateChanged();
    }
    
    private void NotifyStateChanged() => OnChange?.Invoke();
}
