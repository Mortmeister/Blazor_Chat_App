namespace Campus_Chat.Models;

public record Post
{
    public Guid Id { get; init; } = Guid.NewGuid();
    public Guid ThreadId { get; init; }
    public Guid AuthorId { get; init; }
    public string Text { get; init; } = "";
    public DateTimeOffset Time { get; init; } = DateTimeOffset.UtcNow;
    public Guid? ParentPostId { get; init; }
}