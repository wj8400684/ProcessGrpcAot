using System.Diagnostics.CodeAnalysis;
using Microsoft.AspNetCore.Connections;
using Microsoft.Extensions.ObjectPool;

namespace ProcessGrpcAot;

/// <summary>
/// ServiceCollection扩展
/// </summary>
public static partial class ServiceCollectionExtensions
{
    /// <summary>
    /// 注册SocketConnectionFactory为IConnectionFactory
    /// </summary>
    /// <param name="services"></param>
    /// <returns></returns>
    [RequiresUnreferencedCode("")]
    public static IServiceCollection AddNamedPipeTransportFactory(this IServiceCollection services)
    {
        services.AddSingleton<ObjectPoolProvider, DefaultObjectPoolProvider>();
        var factoryType = ConnectionFactoryTypeUtil.FindNamedPipeTransportFactory();
        return services.AddSingleton(typeof(IConnectionListenerFactory), factoryType);
    }
}