namespace Campus_Chat.Models;

public record DirectChat
{
    public Guid Id { get; init; } = Guid.NewGuid();
    public Guid FirstUserId { get; init; }
    public Guid SecondUserId { get; init; }
}