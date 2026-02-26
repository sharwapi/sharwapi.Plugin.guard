using System.Collections.Generic;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using sharwapi.Contracts.Core;

namespace sharwapi.Plugin.guard;

public class guardPlugin : IApiPlugin
{
    // 插件名称
    public string Name => "sharwapi.guard";
    // 插件显示名称
    public string DisplayName => "Route Guard";
    // 插件版本
    public string Version => "0.1.0";
    // 启用自动路由前缀
    public bool UseAutoRoutePrefix => true;

    // 插件注册服务方法
    public void RegisterServices(IServiceCollection services, IConfiguration configuration)
    {
        // 注册 guardSettings (使用隔离配置的根节点)
        services.Configure<guardSettings>(configuration);
    }

    // 默认配置
    public object? DefaultConfig => new guardSettings
    {
        ProtectedRoutes = new List<ProtectedRoute>
        {
            new ProtectedRoute { Path = "/api/secure", Token = "change-me" }
        }
    };

    // 插件中间件配置方法
    public void Configure(WebApplication app)
    {
        // 添加 TokenAuthMiddleware 中间件
        app.UseMiddleware<TokenAuthMiddleware>();
    }

    // 插件路由注册方法 (此插件不需要注册路由，使用默认函数)
    public void RegisterRoutes(IEndpointRouteBuilder app, IConfiguration configuration) { }
}
