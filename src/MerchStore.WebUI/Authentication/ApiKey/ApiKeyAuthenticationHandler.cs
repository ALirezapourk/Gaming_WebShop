using System.Security.Claims;
using System.Text.Encodings.Web;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Options;

namespace MerchStore.WebUI.Authentication.ApiKey;

/// <summary>
/// Authentication handler for API key authentication.
/// </summary>
public class ApiKeyAuthenticationHandler : AuthenticationHandler<ApiKeyAuthenticationOptions>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="ApiKeyAuthenticationHandler"/> class.
    /// </summary>
    public ApiKeyAuthenticationHandler(
        IOptionsMonitor<ApiKeyAuthenticationOptions> options,
        ILoggerFactory logger,
        UrlEncoder encoder) : base(options, logger, encoder)
    {
    }

    /// <summary>
    /// Verifies that the request contains a valid API key in the header.
    /// </summary>
    protected override Task<AuthenticateResult> HandleAuthenticateAsync()
    {
        // Check if the request header contains the API key
        if (!Request.Headers.TryGetValue(Options.HeaderName, out var apiKeyHeaderValues))
        {
            Logger.LogWarning("API key missing. Header '{HeaderName}' not found in the request.", Options.HeaderName);
            return Task.FromResult(AuthenticateResult.Fail($"API key header '{Options.HeaderName}' not found."));
        }

        // Check if the header value is empty
        var providedApiKey = apiKeyHeaderValues.FirstOrDefault();
        if (string.IsNullOrWhiteSpace(providedApiKey))
        {
            Logger.LogWarning("API key is empty. Header '{HeaderName}' has no value.", Options.HeaderName);
            return Task.FromResult(AuthenticateResult.Fail("API key is empty."));
        }

        // Validate the API key against the configured value
        if (providedApiKey != Options.ApiKey)
        {
            Logger.LogWarning("Invalid API key provided: {ProvidedKey}", providedApiKey);
            return Task.FromResult(AuthenticateResult.Fail("Invalid API key."));
        }

        // If the API key is valid, create a claims identity and authentication ticket
        var claims = new[] { new Claim(ClaimTypes.Name, "API User") };
        var identity = new ClaimsIdentity(claims, Scheme.Name);
        var principal = new ClaimsPrincipal(identity);
        var ticket = new AuthenticationTicket(principal, Scheme.Name);

        Logger.LogInformation("API key authentication successful");
        return Task.FromResult(AuthenticateResult.Success(ticket));
    }
}