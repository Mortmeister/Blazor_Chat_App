### What the AI produced
I used AI to discuss the architecture and decide how the different services should work together. 
I then started the project by using AI to generate issues with acceptance criteria for the Kanban board. 
This was a great benefit because each issue had clear tests that had to be satisfied before the code was ready to be committed. 
This gave my work process a clear structure and focused by breaking the project into smaller tasks.

During development, AI helped with parts of the implementation. For example, it helped me build parts of ChatService and CurrentUserService. 
I also used it to create PostComponent and implement the recursive rendering of replies, where I received drafts which I then adapted, tested and debugged. 
Another area where AI was particularly helpful was implementing asynchronous behaviour, especially when working with event notifications and updating the UI. 
These were concepts I needed some guidance with to get everything working as intended. 

I created a Figma design that I had AI implement after most of the functionality was finished. This was more of a “for fun” part of the project, so I let AI
handle the CSS and apply the classes directly. Related to the UI restyling was UiHelpers.cs, which AI also implemented.

I also used AI to write XML documentation which helped me communicate my code more clearly.

### What I changed and what I kept

I found missing requirements in the Kanban board during development, such as creating profiles, creating threads and visiting other users’ profiles. 
These were either explicit requirements or features I wanted to add to my project, so I updated the board to fit my needs. 
AI initially suggested keeping a “Deleted post” placeholder when a deleted post had replies. I did not think this was the right solution and instead decided 
that deleting a parent post should leave its replies visible as normal posts.

Most of the work was done through a back-and-forth process. I would work on an issue and, if I got stuck, ask AI for help with the instruction
“don’t hand me the code directly”. This helped point me in the right direction rather than giving me the solution. 
I reported errors or unexpected behaviour, while AI helped me investigate possible causes. I then tested the solutions and checked that they worked 
by referring to the acceptance criteria in the Kanban board.

### Why I made that call

I wanted AI to help me in development while having control over important decisions. Having clear issues and acceptance criteria made it easier
to evaluate whether a suggested solution actually fulfilled the requirements. If something did not make sense or did not fit the design, 
I would look for another solution.

### How AI has impacted me as a programmer

Working with AI has mostly had a positive impact on me as a programmer. It allows me to be more creative because functionality that might initially seem too 
difficult to implement feels achievable. There is usually a way to solve a problem or add a certain feature, and AI helps me explore different approaches. 
It is also a great way to support my curiosity by having concepts related to a topic explained.

The traditional role of a developer is changing from writing all the code themselves to working with AI agents. AI experience can also be seen as a requirement
in many job listings. Therefore, I think it is important to learn how to use AI responsibly to prepare for real-life development work. However, for learning purposes,
I think the important difference is whether you use AI to write the code for you or as a sparring partner while developing
it yourself. In my experience, working with AI has allowed me to work faster while still learning, making decisions and improving as a programmer.