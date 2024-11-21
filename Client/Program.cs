using Client;
using Grpc.Net.Client;
using ProcessGrpcAot;

var client = NamedPipesConnectionFactory.CreateChannel("swatch");

var clientGreeter = new Greeter.GreeterClient(client);

var ss = await clientGreeter.SayHelloAsync(new HelloRequest { Name = "GreeterClient" });

Console.ReadKey();