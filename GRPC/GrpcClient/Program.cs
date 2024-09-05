using GrpcClient;
using Microsoft.AspNetCore.Mvc;
using static TestGrpc.TestGrpcService;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddTransient<IGrpcClientService, GrpcClientService>();

builder.Services
        .AddGrpcClient<TestGrpcServiceClient>(
            (services, options) =>
            {
                options.Address = new Uri(builder.Configuration["TestGrpc:Endpoint"]);
            }
        )
        .ConfigureChannel(o =>
        {
            o.HttpHandler = new HttpClientHandler
            {
                ServerCertificateCustomValidationCallback =
                    HttpClientHandler.DangerousAcceptAnyServerCertificateValidator
            };
        });



var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapGet(
        "/Get/{id}",
        async (IGrpcClientService service, [FromRoute] string id) =>
        {
            return await service.Get(id);
        }
    );

app.Run();
