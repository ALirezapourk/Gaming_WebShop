using Microsoft.AspNetCore.Authentication;

namespace MerchStore.WebUI.Authentication.ApiKey;

/// <summary>
/// Options for API key authentication.
/// </summary>
public class ApiKeyAuthenticationOptions : AuthenticationSchemeOptions
{
    /// <summary>
    /// The header name where the API key is expected to be transmitted.
    /// Defaults to "X-API-Key".
    /// </summary>
    public string HeaderName { get; set; } = ApiKeyAuthenticationDefaults.HeaderName;

    /// <summary>
    /// The API key that clients must provide to be authenticated.
    /// </summary>
    public string ApiKey { get; set; } = string.Empty;
}