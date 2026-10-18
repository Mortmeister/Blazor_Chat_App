namespace Campus_Chat.Models;

public record DirectMessage
{
    public Guid Id { get; init; } = Guid.NewGuid();
    public Guid DirectChatId { get; init; }
    public Guid SenderId { get; init; }
    public string Text { get; init; } = "";
    public DateTimeOffset SentAt { get; init; } = DateTimeOffset.UtcNow;
}