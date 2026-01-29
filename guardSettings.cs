using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace sharwapi.Plugin.guard
{
    public class guardSettings
    {
        // 受保护路由列表
        public List<ProtectedRoute> ProtectedRoutes { get; set; } = new();
    }
}
