using System.Net;
using System.Net.Sockets;
using Grpc.Net.Client;

namespace Client;

public sealed class UnixDomainSocketsConnectionFactory(EndPoint endPoint)
{
    private static readonly string SocketPath = Path.Combine(Path.GetTempPath(), "socket.tmp");

    public static GrpcChannel CreateChannel()
    {
        var udsEndPoint = new UnixDomainSocketEndPoint(SocketPath);
        var connectionFactory = new UnixDomainSocketsConnectionFactory(udsEndPoint);
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
        var socket = new Socket(AddressFamily.Unix, SocketType.Stream, ProtocolType.Unspecified);

        try
        {
            await socket.ConnectAsync(endPoint, cancellationToken).ConfigureAwait(false);
            return new NetworkStream(socket, true);
        }
        catch
        {
            socket.Dispose();
            throw;
        }
    }
}