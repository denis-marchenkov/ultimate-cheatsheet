# CQRS Pattern + Mediatr library

CQRS stands for Command Query Responsibility Segregation. Which essentially means separating read and write operations. Commands act, queries - return data. Commands - write, queries - read.
This helps in decoupling and simplifying convoluted domain logic.

<br/>

Potentially read and write operations may be conducted on different db contexts since we most likely will have a replicated master-slave database
in this scenario. However this is not required for using CQRS pattern.

<br/>

Usually it requires a bit of code but there's a convenient ```mediatr``` library which does all the heavy lifting, so all we need is to implement
```IRequest``` for commands and queries and ```IRequestHandler``` for handlers.

<br/>

Additionally ```mediatr``` supports raising and handling notifications.
