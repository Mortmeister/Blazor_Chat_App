# Campus Web Chat

Campus Web Chat is a thread-based chat application built with Blazor, Razor and C#. It allows students, lecturers and auditors
to communicate through public and private threads, as well as direct messages.

## Features

* User selection: Select a user from a dropdown when signing in. The application has three user types: Student, Lecturer and Auditor. No passwords are required.
* Threads: Create new threads, join existing ones and leave threads.
* Public and private threads: Public threads are open to everyone. Private threads require a join code to join.
* Posts and replies: Write posts in threads and reply to existing posts. Replies are displayed in a nested structure.
* Direct messages: Send private messages to other users.
* Notifications: Get notifications when receiving direct messages or when new posts are made in threads you have joined.
* Real-time updates: Changes are shared between open browser tabs, allowing users to see new posts and messages without manually refreshing the page.
* User profiles: View and update your profile information and view other users' profiles.

## Getting Started

To run the project, you need:

* .NET SDK.
* An IDE such as Rider or Visual Studio Code.

## Installation

Clone the repository:

```git clone https://github.com/Mortmeister/Blazor_Chat_App.git```

```cd Campus_Chat```

Run the application using:

```dotnet run```

Open the URL shown in the terminal to access the application.


## Technology

* C# – Main programming language.
* Blazor – Component-based web framework.
* Razor – Used to build the UI components and pages.
* CSS – Styling and layout.

## Data Storage

The application currently uses in-memory storage for users, threads, memberships, posts and direct messages.
This means:

* Data is shared between all connected users.
* Refreshing the browser does not delete any data.
* Restarting the server resets everything to the original seed data.
* Each browser tab can have its own signed-in user through session storage.


## Notes

This project is a demonstration of a campus communication platform. User selection is used instead of traditional authentication, and the application is intended for demonstration and testing rather than production use.


## Seeds
### Users
- Ola (student)
- Kari (student)
- Lecturer 
- Auditor 

### Threads
- GeneralThread 
- PrivateThread 
  - Join code: 123