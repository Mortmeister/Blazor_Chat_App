namespace Campus_Chat.Models;

public class DirectMessage
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public Guid DirectChatId { get; set; }
    public Guid SenderId { get; set; }
    public string Text { get; set; } = "";
    public DateTimeOffset SentAt { get; set; } = DateTimeOffset.UtcNow;
}