using GrpcServer;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddGrpc();

var app = builder.Build();

app.UseHttpsRedirection();

app.MapGrpcService<GrpcServerService>();
app.Run();
