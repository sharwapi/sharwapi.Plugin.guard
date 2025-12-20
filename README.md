# Route Guard

[简体中文](/README_CN.md) | [English](/README.md)

This project is the official security plugin for [SharwAPI](https://github.com/sharwapi/sharwapi.Core).

It provides a lightweight route protection mechanism. Through configuration, you can specify that certain URL paths require a specific token to be accessed. The plugin intercepts requests and verifies the token in the HTTP request headers.

## Features

- **Path-based Interception**: Supports configuring multiple protected route paths with prefix matching (e.g., configuring `/admin` will protect `/admin/users` and all its sub-paths).
- **Token Validation**: Enforces validation of the `X-Api-Token` request header for matched requests.
- **Simple Configuration**: Dynamically manage protected resources via `appsettings.json`.

## Installation & Usage

1. Download the compiled `.dll` file from [Releases](https://github.com/sharwapi/sharwapi.plugin.guard/releases).
2. Place it into the `Plugins` directory of the Core API.
3. Configure the `appsettings.json` file of the Core API (see below).
4. Restart the API.

## Configuration

This plugin reads the `AuthSettings` node from the configuration file. Please add the following configuration to SharwAPI's `appsettings.json`:

```json
{
  "Logging": { ... },
  "AllowedHosts": "*",
  
  // Route Guard Plugin Configuration
  "AuthSettings": {
    "ProtectedRoutes": [
      {
        "Path": "/admin",        // Protected path prefix
        "Token": "secret-123"    // Token required to access this path
      },
      {
        "Path": "/private/data",
        "Token": "another-secret"
      }
    ]
  }
}

```

### Configuration Items

* **ProtectedRoutes**: A list of protected routes.
* **Path**: The path prefix. The plugin uses `StartsWithSegments` for matching, meaning `/admin` will match `/admin`, `/admin/user`, `/admin/settings`, etc.
* **Token**: The expected security token.

## Verification Mechanism

When a client initiates a request, if the request path matches a configured `Path`, the plugin performs the following checks:

1. **Check Header**: Looks for the HTTP request header named `X-Api-Token`.
2. **Verify Content**: Compares the header value with the configured `Token` for an exact match.

**Response Status Codes**:

* **401 Unauthorized**: The `X-Api-Token` header is missing.
* **403 Forbidden**: The token does not match (invalid token).

### Request Example

```http
GET /admin/users HTTP/1.1
Host: localhost:5000
X-Api-Token: secret-123

```

## 📄 License

This project is licensed under the [GNU Lesser General Public License v3.0](https://www.google.com/search?q=/LICENSE).