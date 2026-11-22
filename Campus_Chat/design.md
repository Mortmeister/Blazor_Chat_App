# Design choices

### Layout
The product uses a shared layout with a dynamic sidebar that is rendered across all pages. The sidebar changes depending on whether the
user is logged in or not. Visitors can see threads, login and register because threads are public and can be read without an account.
When logged in, users get access to more options. This way, the sidebar only shows links that the user can actually access.

The product has 12 pages in total. The number of pages was based on the different features of the product.
For example, the thread overview and individual threads have separate pages. This also means that each thread, profile and chat has its own address. 
I could have combined some of these into fewer pages using a more modular design, but separate pages made more sense for giving each item its own address.

### Route names
- "/"
- "/chats"
- "/chat/{Id:guid}"
- "/register"
- "/login"
- "/Error"
- "/people"
- "/profile/{Id:guid}"
- "/not-found"
- "/protected"
- "/thread/{Id:guid}"
- "/threads"

## Which state lives in a service, and which state lives in a component

### singleton services. 
Users, threads, memberships, posts, direct chats and direct messages are stored in InMemoryUserStore and InMemoryChatStore.
UserService and ChatService handle the rules, such as who can join, post, delete or read a chat. ChatService also handles the Changed event.
All four services are singletons, meaning every browser uses the same shared data. If the stores were scoped, each tab would have its own lists, 
and users would not see each other’s messages. 

ChatService must also be a singleton because a  scoped service would give each tab its own Changed event. 
This means tabs would not receive updates when someone posts from another tab. 
UserService does not store data, but ChatService depends on it. Since a singleton cannot depend on a scoped service, UserService must also be a singleton. 
The downside is that all browsers use the same lists at the same time, so locks are used to protect the data.


### Live updates. 
The singleton only makes the data and the subscriber list shared. The updates themselves come from Changed. after a save,
ChatService raises the event, and subscribed pages reload their data through the service. The event runs in the poster's session, so each handler refreshes its screen
through InvokeAsync. Pages subscribe in OnInitialized and unsubscribe in Dispose. If a page does not unsubscribe when closed, it stays in the singleton’s event list, 
causing unnecessary updates and memory leaks.


### scoped service plus browser storage. 
The signed-in user is stored in CurrentUserService, which is scoped, because a singleton would sign every browser in as the same person.
However, a scoped service alone would lose the user after a refresh because a new circuit is created.
To avoid this, the user’s ID is saved in ProtectedSessionStorage, which is encrypted. I chose session storage instead of local storage
so two people can sign in using separate tabs on the same computer, 
which is what allows me to test direct chats and live updates. The cost is that closing a tab signs the user out.

Search text, form fields, the reply target, error messages and page lists are stored in the components. 
This state is only needed while the page is open, and components do not own shared data. 
Pages load fresh copies from the services when they open or receive a change notification.

### Where the data lives while the application runs

The data is stored in memory inside the server process, in private List<T> fields in the two stores. The lists are protected by lock and
filled with seed data at startup. There is no database or file. The only data stored in the browser is the signed-in user’s ID.
I chose memory storage because it requires no setup and lets the application run from a clean clone with one command.

Pages and services access the data only through IUserStore and IChatStore. This makes it easier to switch from in-memory storage to a database in the future. 
I would only need to create new implementations of these two interfaces and change their registration in Program.cs. No pages or services would need to change.

The downside of using in-memory storage is that restarting the server resets everything to the original seed data. 
This means new posts, profile edits and threads are lost. A database would solve this problem by keeping the data even after the server restarts.

### What a post holds beyond the text, the author and the time.

What a post holds beyond the text, author and time

- Id: Gives each post a unique ID so replies can reference it through ParentPostId. I use a Guid because it creates unique IDs without needing a database counter.
- ThreadId: Connects the post to a specific thread. This allows the store to load only the posts belonging to the thread that is open, instead of loading everything into one feed.
- AuthorId: Stores the user’s ID instead of their display name. This avoids saving the same user information on every post. If a user changes their display name, all their old posts will show the new name because the name is looked up using AuthorId.
- DateTime: Stores when the post was created. I use UTC DateTimeOffset so posts appear in the correct order, even when users are in different time zones.
- ParentPostId: Links a reply to the post it responds to. It is optional because normal posts do not reply to anything. However, without it replies would just appear at the bottom of the thread without showing which post they belong to.

### Whether a thread has a topic, a colour or an owner.
Threads do not have owners in the model. The idea is that threads should be open for everyone to join and participate in. 
The exception is private threads, which require a join code. In the future, adding thread owners and more control over who can manage a thread could be
an improvement.

Threads only have a topic through their name. They do not have a colour stored in the model either. Instead, each thread gets an accent colour in the UI. 
There is a fixed palette of five colours, and the colour is chosen based on the thread’s ID. This means the same thread always gets the same colour,
even after refreshing the page or opening it somewhere else. It also means I do not need to store extra colour information in the model.

The colour is used as the background for the thread icon in the overview. The icon also changes depending on whether the thread is public or private, 
showing a # for public threads and a lock for private ones.

### Count
The unread count only applies to threads the user has joined. This makes notifications more relevant and avoids overwhelming users with updates
from every thread in the program. Otherwise, it could potentially feel like spam as the total number of threads grow.

The membership count shows how many users have joined each thread. This was mainly a UI decision to give users a better idea of how active or popular a thread is.

