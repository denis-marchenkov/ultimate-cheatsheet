using HttpClientMemo.TypedClientWithCustomHandler;

var builder = WebApplication.CreateBuilder(args);

var baseUri = new Uri("http://localhost:5051/");

// tell builder to use HttpClient
builder.Services.AddHttpClient();

// example of a typed client with custom message handler
builder.Services.Configure<AuthOptions>(builder.Configuration.GetSection("Auth")); // configure settings section in services so it can be resolved in handler
builder.Services.AddTransient<CustomAuthMessageHandler>();

builder.Services.AddHttpClient<TypedClientWithCustomMessageHandler>()
    .ConfigureHttpClient((services, httpClient) =>
    {
        httpClient.BaseAddress = baseUri;
        httpClient.Timeout = TimeSpan.FromMinutes(10);
    })
    .SetHandlerLifetime(TimeSpan.FromMinutes(2))
    .ConfigurePrimaryHttpMessageHandler(x => new SocketsHttpHandler()
    {
        // maximal connection lifetime in the pool
        // connections will be reestablised periodically to reflect DNS changes
        PooledConnectionLifetime = TimeSpan.FromMinutes(10)
    })
    .AddHttpMessageHandler<CustomAuthMessageHandler>();


// example of a named client
builder.Services.AddHttpClient("FancyClientName", (serivceProvider, client) =>
{
    client.BaseAddress = baseUri;
});


var app = builder.Build();


// example of creating client using injected HttpClientFactory
app.MapGet("/from-factory", async (IHttpClientFactory httpClientFactory) =>
{
    var client = httpClientFactory.CreateClient();

    // base address for subsequent requests
    client.BaseAddress = baseUri;

    // add custom headers if necessary
    client.DefaultRequestHeaders.Accept.Clear();
    client.DefaultRequestHeaders.Add("X-Custom-Header", "This is my value");

    // add bearer token if necessary
    client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", "Token_Here");

    var response = await client.GetAsync("test/from-factory");
    var result = await response.Content.ReadAsStringAsync();
    return Results.Content(result);
});


// call typed client configured earlier
app.MapGet("/typed-client", async (TypedClientWithCustomMessageHandler client) =>
{
    var result = await client.GetAsync("from-type");
    return Results.Content(result);
});


// call named client configured earlier
app.MapGet("/named-client", async (IHttpClientFactory httpClientFactory) =>
{
    var client = httpClientFactory.CreateClient("FancyClientName");

    var response = await client.GetAsync("test/named-client");
    var result = await response.Content.ReadAsStringAsync();
    return Results.Content(result);
});


// emulate API endpoints

// general endpoint to be called by every client
app.MapGet("/test/{info}", (string info) => $"Hello, HttpClient {info}");

// emulate auth endpoint
app.MapPost("/test/auth", async () => await Task.FromResult(System.Text.Json.JsonSerializer.Serialize(new AuthToken { AccessToken = "token"})));


// beautiful UI
app.MapGet("/", () =>
{
    return Results.Content($"""
                                <a href="/from-factory">Create client using injected HttpClientFactory</a>
                                <br/><br/>
                                <a href="/typed-client">Typed client injected in endpoint</a>
                                <br/><br/>
                                <a href="/named-client">Named client using injected HttpClientFactory</a>
                            """, "text/html");
});

app.Run();


