using BenchmarkDotNet.Attributes;
using ProcessGrpcAot;

namespace Client;

[MemoryDiagnoser]
public class Test
{
    private Greeter.GreeterClient _unixClient = null!;
    private Greeter.GreeterClient _namePipeClient = null!;

    [GlobalSetup]
    public void Setup()
    {
        var client = UnixDomainSocketsConnectionFactory.CreateChannel();

        _unixClient = new Greeter.GreeterClient(client);

        _namePipeClient = new Greeter.GreeterClient(NamedPipesConnectionFactory.CreateChannel("swatch1"));
    }

    [Benchmark]
    public async ValueTask Unix()
    {
        await _unixClient.SayHelloAsync(new HelloRequest { Name = "John Doe" });
    }
    
    [Benchmark]
    public async ValueTask NamedPipe()
    {
        await _namePipeClient.SayHelloAsync(new HelloRequest { Name = "John Doe" });
    }
}