using Microsoft.AspNetCore.Authorization;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Reflection;
using MerchStore.WebUI.Authentication.ApiKey;

namespace MerchStore.WebUI.Infrastructure;

/// <summary>
/// Operation filter to add security requirements for controller-based endpoints
/// </summary>
public class SecurityRequirementsOperationFilter : IOperationFilter
{
    public void Apply(OpenApiOperation operation, OperationFilterContext context)
    {
        // Only add security requirements to controller-based endpoints
        // This excludes minimal API endpoints
        if (context.ApiDescription.ActionDescriptor.GetType().Name.Contains("ControllerActionDescriptor"))
        {
            // Check if the endpoint requires authorization
            var methodInfo = context.MethodInfo;
            var controllerType = methodInfo?.DeclaringType;

            if (methodInfo != null)
            {
                var hasAuthorizeAttribute = methodInfo.GetCustomAttribute<AuthorizeAttribute>() != null
                                         || controllerType?.GetCustomAttribute<AuthorizeAttribute>() != null;

                if (hasAuthorizeAttribute)
                {
                    // Add API key security requirement
                    operation.Security = new List<OpenApiSecurityRequirement>
                    {
                        new OpenApiSecurityRequirement
                        {
                            {
                                new OpenApiSecurityScheme
                                {
                                    Reference = new OpenApiReference
                                    {
                                        Type = ReferenceType.SecurityScheme,
                                        Id = ApiKeyAuthenticationDefaults.AuthenticationScheme
                                    }
                                },
                                Array.Empty<string>()
                            }
                        }
                    };
                }
            }
        }
    }
}