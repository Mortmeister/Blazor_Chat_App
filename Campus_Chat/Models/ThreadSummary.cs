namespace Campus_Chat.Models;


/// <summary>The page-safe view of a thread. It never contains the join code.</summary>
public record ThreadSummary(
    Guid Id,
    string Name,
    int MembershipCount,
    bool IsPrivate
);
