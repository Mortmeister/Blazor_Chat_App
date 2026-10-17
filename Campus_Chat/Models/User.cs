using Microsoft.VisualBasic;

namespace Campus_Chat.Models;

public class User
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string DisplayName { get; set; } = "";
    public string Note { get; set; } = "";
    public string AvatarColour { get; set; } = "";
    public int StudyYear { get; set; }
    public bool IsAuditor { get; set; }
}


/*#10 Models
   
   Create a Models folder. For each type, ask what it must hold:
   
   User/profile: ID, display name, note, avatar colour, study year, an audit flag
   Thread: ID, name, whether it’s private, and the code (think about where the code lives so it can’t leak to a page)
   Post: ID, thread ID, author ID, text, time, and an optional parent post ID for replies
   Direct chat and message: which two users, and who sent each message and when
   
   Questions to decide as you write:
   
   Should the model that pages see include the private thread’s code? 
   Probably not. Consider a separate type for what pages are allowed to receive.
   Use required and init where it makes sense. Is a post meant to be editable after creation?
   Store time as UTC (DateTime.UtcNow or DateTimeOffset) and convert only when displaying.*/