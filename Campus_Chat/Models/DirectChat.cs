namespace Campus_Chat.Models;

public class DirectChat
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public Guid FirstUserId { get; set; }
    public Guid SecondUserId { get; set; }
}