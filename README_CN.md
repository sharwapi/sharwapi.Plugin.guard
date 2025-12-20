# Route Guard

[简体中文](/README_CN.md) | [English](/README.md)

本项目是 [SharwAPI](https://github.com/sharwapi/sharwapi.Core) 的官方安全插件。

它提供了一种轻量级的路由保护机制。通过配置，您可以指定某些 URL 路径需要特定的令牌（Token）才能访问。插件会拦截请求并验证 HTTP 请求头中的令牌。

## 功能特性

* **基于路径的拦截**: 可配置多个受保护的路由路径，支持前缀匹配（例如配置 `/admin` 将保护 `/admin/users` 等所有子路径）。
* **Token 验证**: 对匹配的请求强制检查 `X-Api-Token` 请求头。
* **简单配置**: 通过 `appsettings.json` 即可动态管理受保护的资源。

## 安装与使用

1.  在 [Release](https://github.com/sharwapi/sharwapi.plugin.guard/releases) 获取插件编译后的 `.dll` 文件
2.  将其放入 API 本体的 `Plugins` 目录中
3.  配置 API 本体的 `appsettings.json` 文件（见下文）
4.  重启 API

## 配置文件

该插件读取配置文件中的 `AuthSettings` 节点。请在 SharwAPI 的 `appsettings.json` 中添加如下配置：

```json
{
  "Logging": { ... },
  "AllowedHosts": "*",
  
  // Route Guard 插件配置
  "AuthSettings": {
    "ProtectedRoutes": [
      {
        "Path": "/admin",        // 受保护的路径前缀
        "Token": "secret-123"    // 访问该路径所需的 Token
      },
      {
        "Path": "/private/data",
        "Token": "another-secret"
      }
    ]
  }
}

```

### 配置项说明

* **ProtectedRoutes**: 受保护路由的列表。
* **Path**: 路径前缀。插件使用 `StartsWithSegments` 进行匹配，这意味着 `/admin` 会匹配 `/admin`, `/admin/user`, `/admin/settings` 等。
* **Token**: 预期的安全令牌。

## 验证机制

当客户端发起请求时，如果请求路径匹配了配置中的 `Path`，插件将执行以下检查：

1. **检查 Header**: 查找名为 `X-Api-Token` 的 HTTP 请求头。
2. **验证内容**: 比对 Header 的值与配置中的 `Token` 是否完全一致。

**响应状态码**:

* **401 Unauthorized**: 请求头中缺少 `X-Api-Token`。
* **403 Forbidden**: Token 不匹配（密码错误）。

### 请求示例

```http
GET /admin/users HTTP/1.1
Host: localhost:5000
X-Api-Token: secret-123

```

## 许可证

本项目基于 [GNU Lesser General Public License v3.0](/LICENSE) 获得许可