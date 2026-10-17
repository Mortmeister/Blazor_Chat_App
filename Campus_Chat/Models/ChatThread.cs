namespace Campus_Chat.Models;

public class ChatThread
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string Name { get; set; } = "";
    public bool IsPrivate { get; set; }
}