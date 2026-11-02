namespace Campus_Chat.Models;
/// <summary>One row in a user's list of direct chats.</summary>
public record DirectChatSummary(Guid ChatId, Guid OtherUserId, string OtherName, string? LastMessage, DateTimeOffset LastActivity);