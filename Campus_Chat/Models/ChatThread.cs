namespace Campus_Chat.Models;

public record ChatThread
{
    public Guid Id { get; init; } = Guid.NewGuid();
    public string Name { get; init; } = "";
    public bool IsPrivate { get; init; }
    public string? JoinCode { get; init; }   // null for public threads
}