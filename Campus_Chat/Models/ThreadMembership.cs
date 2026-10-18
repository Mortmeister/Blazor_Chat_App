namespace Campus_Chat.Models;


    public record ThreadMembership
    {
        public Guid UserId { get; init; }
        public Guid ThreadId { get; init; }
        public DateTimeOffset LastReadAt { get; init; } = DateTimeOffset.UtcNow;  // used for unread counts later
    }
