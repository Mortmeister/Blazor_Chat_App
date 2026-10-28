namespace Campus_Chat.Models;

public record Post
{
    public Guid Id { get; init; } = Guid.NewGuid();
    public Guid ThreadId { get; init; }
    public Guid AuthorId { get; init; }
    public string Text { get; init; } = "";
    public DateTimeOffset Time { get; init; } = DateTimeOffset.UtcNow;
    public Guid? ParentPostId { get; init; }
    /// <summary>True when the author deleted this post but it still has replies. Text and author are cleared.</summary>
    public bool IsDeleted { get; init; }
}