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


app.MapGet("/sentences/{amount}", (int amount) =>
{
    return Enumerable.Range(0, amount).Select(x => "The quick brown fox jumps over the lazy dog").ToArray();
})
.WithTags("Get some letters")
.WithOpenApi();


app.Run();
