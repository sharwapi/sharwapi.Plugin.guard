using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;

namespace sharwapi.Plugin.guard
{
    public class TokenAuthMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly guardSettings _authSettings;

        // 构造函数，注入 RequestDelegate 和 guardSettings
        public TokenAuthMiddleware(RequestDelegate next, IOptions<guardSettings> authSettings)
        {
            _next = next;
            _authSettings = authSettings.Value;
        }

        // 中间件的核心逻辑
        public async Task InvokeAsync(HttpContext context)
        {
            // 获取请求路径
            var path = context.Request.Path;

            // 查找是否有匹配的受保护路由
            var matchingRoute = _authSettings.ProtectedRoutes
                .FirstOrDefault(r => path.StartsWithSegments(r.Path, StringComparison.OrdinalIgnoreCase));

            // 如果没有匹配的路由，继续处理下一个中间件
            if (matchingRoute == null)
            {
                await _next(context);
                return;
            }

            // 检查请求头中是否包含 X-Api-Token
            if (!context.Request.Headers.TryGetValue("X-Api-Token", out var extractedToken))
            {
                context.Response.StatusCode = 401;
                await context.Response.WriteAsync("Unauthorized: Missing Token");
                return;
            }

            // 验证令牌是否匹配
            if (!matchingRoute.Token.Equals(extractedToken))
            {
                context.Response.StatusCode = 403;
                await context.Response.WriteAsync("Forbidden: Invalid Token");
                return;
            }

            await _next(context);
        }
    }
}
