using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace sharwapi.Plugin.guard
{
    public class ProtectedRoute
    {
        // 受保护的路由路径
        public string Path { get; set; } = string.Empty;
        // 访问该路由所需的令牌
        public string Token { get; set; } = string.Empty;
    }
}
