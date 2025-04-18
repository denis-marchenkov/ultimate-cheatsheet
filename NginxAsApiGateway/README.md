# Nginx as an API Gateway.


<br/>

Setting up Nginx in a docker container to act as a gateway for containerized api projects.

<br/>

Benefits of Nginx as an API gateway:
<br />
- Centralized entry point for the api;
- Request routing decouples clients from the internal system structure allowing to change services without affecting users;
- Can be used for load balancing by distributing traffic across multiple instances of a service;
- Nginx can handle TLS and provide various security enchancements;
- Data can be cached at the gateway;
- Nginx can log all requests and responses;
- Nginx can control API throttling;
- API versioning can be set up at the gateway by routing requests to different API versions;
- Nginx can handle CORS headers, allowing or restricting cross-origin requests to API;
<br/>
<br/>


Assuming we have two API projects (NumbersApi and TextApi) in different containers, nginx can be configured for instance with catch-all statements to route requests through nginx to specific api:

```bash
server {
	listen 80;

	location /custom_gateway/text/ {
		proxy_pass http://text-api:8080/;
	}

	location /custom_gateway/numbers/ {
		proxy_pass http://numbers-api:8080/;
	}
}
```

Hence the api now can be accessed through Nginx.