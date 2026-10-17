# Design choices

### The what
1. Profile
2. Threads
3. Direct chat


### Data persistance

- local storage. Access token saved in the local storage  upon login.
### Routing

- Home page  
- Chat room 
- Profile page
- Personal chat room (protected)
- Open, global chat room (anyone can read)


### Roles  
1. Not-logged in user
2. Student (logged in)
3. Lecturer (logged in)
4. Auditor


### Sketches to answer

# 5 Where does the data live?

## Options: in-memory collections inside a service, or a database such as SQLite with EF Core.
### Questions to answer:
- What happens to the data when the app restarts?
- Which one can you finish and test thoroughly in the time you have?
- Does the brief ask for persistence anywhere? Check requirement 6 closely.
- How does section 8.1 let you change this later without touching pages?

# 6 Service split and lifetimes.

## Questions to answer:
- Which data must every user see the same version of? A singleton shares one instance across the whole app.
- Which data belongs to one person’s connection, such as who is signed in or what they’ve read? A scoped service lives per circuit.
- What goes wrong if you put per-user state in a singleton? Think about two browsers open at once.
- Which component state is just UI, such as a text box or a search filter, and doesn’t belong in a service at all?
**Tip: sketch a box diagram of your services and what each one holds. It’s easy to explain and it goes straight into design.md.**

# 7 Route names.

## Questions to answer:
- What identifies a thread, a profile and a direct chat in an address? A number, a GUID or a slug each has trade-offs for guessability, readability and uniqueness.
- Should a direct chat address be based on the chat’s own ID or on the other person’s ID? How does that affect requirement 12 and chat privacy?
**Format: a table with columns for page, route and parameter. Include sign-in and not-found.**

# 8 The open section 6 choices. Take them one at a time:

- Extra post fields: what would help the returning student or the lecturer? Think edited time, or a pinned or answered flag.
- Thread topic, colour or owner: what does each add, and what does it cost you to build?
- Unread count on unjoined threads: does showing it leak anything? Does it help a new student find active threads? Pick a side and defend it.
- Replies to deleted posts: remove them, keep them, or show “deleted post”? Whichever you choose, say what each option does to unread counts.