namespace Campus_Chat.Models.Post;

public class Post
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public Guid ThreadId { get; set; }
    public Guid AuthorId { get; set; }
    public string Text { get; set; } = "";
    public DateTimeOffset Time { get; set; } = DateTimeOffset.UtcNow;
    public Guid? ParentPostId { get; set; }
}