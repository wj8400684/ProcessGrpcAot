using System.IO.Pipes;
using System.Security.Principal;
using Grpc.Net.Client;

namespace Client;

public sealed class NamedPipesConnectionFactory(string pipeName)
{
    public static GrpcChannel CreateChannel(string pipeName)
    {
        var connectionFactory = new NamedPipesConnectionFactory(pipeName);
        var socketsHttpHandler = new SocketsHttpHandler
        {
            ConnectCallback = connectionFactory.ConnectAsync
        };

        return GrpcChannel.ForAddress("http://localhost", new GrpcChannelOptions
        {
            HttpHandler = socketsHttpHandler
        });
    }
    
    public async ValueTask<Stream> ConnectAsync(SocketsHttpConnectionContext _,
        CancellationToken cancellationToken = default)
    {
        var clientStream = new NamedPipeClientStream(
            serverName: ".",
            pipeName: pipeName,
            direction: PipeDirection.InOut,
            options: PipeOptions.WriteThrough | PipeOptions.Asynchronous,
            impersonationLevel: TokenImpersonationLevel.Anonymous);

        try
        {
            await clientStream.ConnectAsync(cancellationToken).ConfigureAwait(false);
            return clientStream;
        }
        catch
        {
            await clientStream.DisposeAsync();
            throw;
        }
    }
}