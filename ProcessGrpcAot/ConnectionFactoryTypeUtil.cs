using System.Diagnostics.CodeAnalysis;
using Microsoft.AspNetCore.Server.Kestrel.Transport.NamedPipes;

namespace ProcessGrpcAot;

internal static class ConnectionFactoryTypeUtil
{
    private const string _socketConnectionFactoryTypeName =
        "Microsoft.AspNetCore.Server.Kestrel.Transport.NamedPipes.Internal.NamedPipeTransportFactory";

    /// <summary>
    /// 查找SocketConnectionFactory的类型
    /// </summary>
    /// <returns></returns>
    [RequiresUnreferencedCode("Calls System.Reflection.Assembly.GetType(String)")]
    public static Type FindNamedPipeTransportFactory()
    {
        var assembly = typeof(NamedPipeTransportOptions).Assembly;
        var connectionFactoryType = assembly.GetType(_socketConnectionFactoryTypeName) ??
                                    throw new InvalidOperationException(
                                        $"Could not find {_socketConnectionFactoryTypeName}.");
        return connectionFactoryType ?? throw new NotSupportedException($"找不到类型{_socketConnectionFactoryTypeName}");
    }
}