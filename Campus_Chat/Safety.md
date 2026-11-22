1. 
The program stores display name, note, avatar colour, study year, the audit flag, post text with author and time,
direct message text with sender and time, who is in which chat, thread memberships, and LastReadAt.
Profile and thread addresses contain user IDs, and the server's default request logging records addresses.
2. 
Currently, a direct chat is kept until the app restarts because all data is stored in memory. 
The application has no delete or expiry function. Ideally, chats would be kept as long as the users have a profile, so they do not disappear unexpectedly.
3. 
If a student leaves, their profile would be deleted after a grace period of two weeks. However, their posts may still be relevant to existing threads, 
so these remain. In that regard, the application has a fallback if the author cannot be found, as the post displays “Unknown user” instead of causing an error. 
However, this would be due for future improvements as there is no such feature for deleting users present. 
4. 
Currently, users can only delete their own posts. Even the audit role cannot delete posts made by others. 
There is no moderator role or report button, so nobody can remove abusive posts. This is a weakness of the project. 
In a real-life version, the auditor should be responsible for deciding if posts are should be removed after a report has been made.
5.
The service only allows the two participants and the audit role to read direct messages. 
Only the two participants can send messages. The page also checks access, but the service provides the actual protection.
Currently, the users are not informed that an auditor can read their direct messages. This could is a privacy violation and should be noted in terms of services
for future improvements.
6.
The product must never log private messages, post text or thread join codes. 
7.
The join code is stored as plain text in ChatThread.JoinCode inside the in-memory store. It is not included in ThreadSummary, 
so pages never receive it. The code is only checked inside JoinThreadAsync.