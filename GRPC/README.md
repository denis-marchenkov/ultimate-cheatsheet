# GRPC

Test GRPC (run both projects, keep consoles running, send request from client swagger UI)

<br/>
<br/>

nuget:

https://www.nuget.org/packages/Grpc.AspNetCore metapackage with references to:


Grpc.AspNetCore.Server:	gRPC server library for .NET.

Grpc.Tools:				Code-generation tooling package.

Google.Protobuf:		Protobuf serialization library.

<br/>
<br/>

.proto file defines gRPC methods, requests, and response models.

.proto file created manually (file syntax: https://protobuf.dev/programming-guides/proto3/)

.proto file should be added into both client and server in order to automatically generate serivce classes by Grpc.Tools in \obj\Debug\[TARGET_FRAMEWORK]\Protos\ on solution build.

<br/>
<br/>

.proto files should be included in .csproj files as:

<br/>

```
<ItemGroup>
	<Protobuf Include="Protos\TestGrpcService.proto" GrpcServices="Client" />
</ItemGroup>
```

<br/>

and

<br/>

```
<ItemGroup>
	<Protobuf Include="Protos\TestGrpcService.proto" GrpcServices="Server" />
</ItemGroup>
```

<br/>
<br/>

Grpc Endpoint in client appsettings.json should be the same as the application url in launch.settings of server. That's what server is listening to.

<br/>
<br/>

On client side register:

```
builder.Services.AddGrpcClient<TestGrpcServiceClient>(...) - define endpoint
```

<br/>
<br/>

On server side register:

```
builder.Services.AddGrpc();
...
app.MapGrpcService<GrpcServerService>();
```

<br />
<br />


![alt text](https://github.com/denis-marchenkov/assets-dump/blob/bb7bca89f4ab0bc4bf74f5a4fab08437f7f2cbfb/grpc.png)
