var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapGet("/digits/{amount}", (int amount) =>
{
    var rnd = new Random();

    return Enumerable.Range(0, amount).Select(x => rnd.Next(1, 10)).ToArray();
})
.WithTags("Get some digits")
.WithOpenApi();

app.Run();