using CQRS.Commands;
using CQRS.Db;
using CQRS.Notifications;
using CQRS.Queries;
using MediatR;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<AppDbContext>();

// add mediatr library and tell it to look for corresponding interface implementations in current assembly
builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()));

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}


app.MapGet("/orders/{id:guid}", async (ISender mediator, Guid id) =>
{
    var result = await mediator.Send(new GetOrderQuery(new OrderId(id)));

    return Results.Ok(result);
})
.WithTags(["get order"]);


app.MapGet("/orders", async (ISender mediator) =>
{
    var result = await mediator.Send(new ListOrdersQuery());

    return Results.Ok(result);
})
.WithTags(["list orders"]);


app.MapPost("/orders", async (CreateOrderCommand command, IMediator mediator) =>
{
    var orderId = await mediator.Send(command);

    // raise domain notification
    await mediator.Publish(new OrderPlacedNotification(orderId, command.Name));

    return Results.Created($"/orders/{orderId}", new { id = orderId.Value });
})
.WithTags(["create order"]);



app.Run();
