using System.Reflection;
using System.Text.Json.Serialization;
using MerchStore.Application;
using MerchStore.Infrastructure;
using MerchStore.WebUI.Authentication.ApiKey;
using MerchStore.WebUI.Infrastructure;
using MerchStore.WebUI.Endpoints;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

var builder = WebApplication.CreateBuilder(args);

// ✅ Snake_case JSON formatting
builder.Services.AddControllersWithViews()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.PropertyNamingPolicy = new JsonSnakeCaseNamingPolicy();
        options.JsonSerializerOptions.DictionaryKeyPolicy = new JsonSnakeCaseNamingPolicy();
        options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
    });

// ✅ CORS policy
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAllOrigins", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

// ✅ API Key authentication
builder.Services.AddAuthentication()
   .AddApiKey(builder.Configuration["ApiKey:Value"]
   ?? throw new InvalidOperationException("API Key is not configured in the application settings."));

// ✅ API Key authorization
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("ApiKeyPolicy", policy =>
        policy.AddAuthenticationSchemes(ApiKeyAuthenticationDefaults.AuthenticationScheme)
              .RequireAuthenticatedUser());
});

// ✅ Application & Infrastructure services
builder.Services.AddApplication();
builder.Services.AddInfrastructure(builder.Configuration);

// ✅ Swagger with API Key and Minimal API support
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "MerchStore API",
        Version = "v1",
        Description = "API for MerchStore product catalog",
        Contact = new OpenApiContact
        {
            Name = "MerchStore Support",
            Email = "support@merchstore.example.com"
        }
    });

    // XML comments support (if enabled)
    var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
    if (File.Exists(xmlPath))
    {
        options.IncludeXmlComments(xmlPath);
    }

    // Operation IDs for minimal APIs (to prevent conflicts)
    options.CustomOperationIds(apiDesc =>
        apiDesc.TryGetMethodInfo(out var methodInfo) ? methodInfo.Name : null);

    // API Key Swagger input
    options.AddSecurityDefinition(ApiKeyAuthenticationDefaults.AuthenticationScheme, new OpenApiSecurityScheme
    {
        Description = "API Key Authentication. Enter your API key in the field below.",
        Name = ApiKeyAuthenticationDefaults.HeaderName,
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey,
        Scheme = ApiKeyAuthenticationDefaults.AuthenticationScheme
    });

    // Tell Swagger: apply API key to every endpoint
    options.AddSecurityRequirement(new OpenApiSecurityRequirement
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
    });

    // Optional: custom operation filter
    options.OperationFilter<SecurityRequirementsOperationFilter>();
});

var app = builder.Build();

// ✅ Error handling and dev seeding
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}
else
{
    app.Services.SeedDatabaseAsync().Wait();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

// ✅ CORS must come BEFORE auth
app.UseCors("AllowAllOrigins");

app.UseAuthentication();
app.UseAuthorization();

// ✅ Swagger UI
app.UseSwagger();
app.UseSwaggerUI(options =>
{
    options.SwaggerEndpoint("/swagger/v1/swagger.json", "MerchStore API V1");
});

// ✅ MVC Controller routes
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

// ✅ Minimal API endpoints
app.MapMinimalProductEndpoints();

app.Run();
