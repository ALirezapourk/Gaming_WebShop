using Microsoft.AspNetCore.Authentication;

namespace MerchStore.WebUI.Authentication.ApiKey;

/// <summary>
/// Extension methods for API key authentication.
/// </summary>
public static class ApiKeyAuthenticationExtensions
{
    /// <summary>
    /// Adds API key authentication to the authentication builder.
    /// </summary>
    /// <param name="builder">The authentication builder.</param>
    /// <param name="configureOptions">A delegate to configure the options.</param>
    /// <returns>The authentication builder for method chaining.</returns>
    public static AuthenticationBuilder AddApiKey(
        this AuthenticationBuilder builder,
        Action<ApiKeyAuthenticationOptions>? configureOptions = null)
    {
        return builder.AddScheme<ApiKeyAuthenticationOptions, ApiKeyAuthenticationHandler>(
            ApiKeyAuthenticationDefaults.AuthenticationScheme,
            configureOptions);
    }

    /// <summary>
    /// Adds API key authentication to the authentication builder with a specific API key.
    /// </summary>
    /// <param name="builder">The authentication builder.</param>
    /// <param name="apiKey">The API key that clients must provide.</param>
    /// <returns>The authentication builder for method chaining.</returns>
    public static AuthenticationBuilder AddApiKey(
        this AuthenticationBuilder builder,
        string apiKey)
    {
        return builder.AddApiKey(options => options.ApiKey = apiKey);
    }
}
