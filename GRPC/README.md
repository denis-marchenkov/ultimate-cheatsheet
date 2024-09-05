# GRPC

Test GRPC (run both projects, keep server console running, send request from client swagger UI)

nuget:

https://www.nuget.org/packages/Grpc.AspNetCore metapackage with references to:

Grpc.AspNetCore.Server:	gRPC server library for .NET.
Grpc.Tools:				Code-generation tooling package.
Google.Protobuf:		Protobuf serialization library.

.proto file defines gRPC methods, requests, and response models. Proto file syntax: https://protobuf.dev/programming-guides/proto3/

.proto file should be added into both client and server in order to automatically generate serivce classes by Grpc.Tools in \obj\Debug\[TARGET_FRAMEWORK]\Protos\ on solution build.

.proto files should be included in .csproj files as:

<ItemGroup>
	<Protobuf Include="Protos\TestGrpcService.proto" GrpcServices="Client" />
</ItemGroup>

and

<ItemGroup>
	<Protobuf Include="Protos\TestGrpcService.proto" GrpcServices="Server" />
</ItemGroup>

Grpc Endpoint in client appsettings.json should be the same as the application url in launch.settings of server. That's what server is listening to.

On client side register:
builder.Services.AddGrpcClient<TestGrpcServiceClient>(...) - define endpoint


On server side register:

builder.Services.AddGrpc();
app.MapGrpcService<GrpcServerService>();