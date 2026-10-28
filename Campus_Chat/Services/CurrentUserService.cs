using Microsoft.AspNetCore.Components;

namespace Campus_Chat.Services;

using Campus_Chat.Models;
using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;


/// <summary>Holds who is signed in for one browser tab (one circuit).</summary>
public class CurrentUserService(ProtectedSessionStorage storage, UserService users, NavigationManager nav)
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

    
    /// <summary>Makes sure somebody is signed in. Otherwise sends the browser to the login page.</summary>
    /// <returns>True when a user is signed in. False when the caller should stop, because a redirect has started.</returns>
    public async Task<bool> EnsureSignedInAsync()
    {
        await InitializeAsync();
        if (User is not null) return true;

        nav.NavigateTo("/login");
        return false;
    }
    
    public async Task SignOutUser()
    {
        User = null;
        await storage.DeleteAsync(Key);
        NotifyStateChanged();
    }
    /// <summary>Reloads the signed-in user from the store, e.g. after a profile edit.</summary>
    public async Task RefreshAsync()
    {
        if (User is null) return;
        User = await users.GetByIdAsync(User.Id);
        NotifyStateChanged();
    }
    
    private void NotifyStateChanged() => OnChange?.Invoke();
}
