namespace Campus_Chat.Models;

/// <summary>What kind of data changed.</summary>
public enum ChangeKind { Post, Direct, Read }

/// <summary>Says what changed. ScopeId is the thread ID for posts and the chat ID for direct messages.</summary>
public record ChatChange(ChangeKind Kind, Guid ScopeId);