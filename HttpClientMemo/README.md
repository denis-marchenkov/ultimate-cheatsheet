# HttpClient memo

A reminder on proper HttpClient usage

<br/>
<br/>

A few key points:

<br/>

- ```HttpClient``` meant to be long-lived;
- each instance of HttpClient has it's own connection pool;
- there's a feature of TCP protocol: when TCP connection is closed port will be holded for some time in TIME_WAIT (up to 4 minutes) status in order to ensure the last FIN-ACK packet is recieved. Thus a lot of HttpClient instances may lead to port exhaustion (socket exhaustion);
- ```HttpClient``` only resolves DNS entries when a connection is created;
- internally ```HttpClient``` uses ```HttpClientHandler``` for sending HTTP requests and receiving HTTP responses, this one is implicitly instantiated whenever HttpClient is created;
- ```HttpClient``` can be extended with various handlers for authentication, request validation and logging, applying retry policies;
- Typed clients are transient, so if injected in singleton it won't reflect DNS changes;
- there's a ```HttpClientFactory``` which helps to avoid socket exhaustion and DNS rotations. ```HttpClientFactory``` creates a pool of ```HttpClientHandler``` handlers, it will rotate between handlers in the pool, dispose and recreate them automatically. Each time HttpClient is created through the factory it will be assigned handler from the internal pool.

<br/>
<br/>

In order to use (and re-use) different configurations of HttpClient in different services there are two options:
<br/>
- Named HttpClient (configured and resolved by a string name)
- Typed HttpClient (wrapped in type or impements interface, resolved by type or interface)