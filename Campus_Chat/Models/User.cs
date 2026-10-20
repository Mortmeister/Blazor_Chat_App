using Microsoft.VisualBasic;

namespace Campus_Chat.Models;

public record User
{
    public Guid Id { get; init; } = Guid.NewGuid();
    public string DisplayName { get; init; } = "";
    public string Note { get; init; } = "";
    public string AvatarColour { get; init; } = "";
    public int StudyYear { get; init; }
    public bool IsAuditor { get; init; }
}

