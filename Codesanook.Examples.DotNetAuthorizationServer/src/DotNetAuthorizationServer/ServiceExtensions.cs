using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;

namespace DotNetAuthorizationServer
{
    public static class ServiceExtensions
    {
        public static void ConfigureAuthentication(this IServiceCollection services, IConfiguration Configuration)
        {
            // https://docs.microsoft.com/en-us/aspnet/core/security/authorization/limitingidentitybyscheme?view=aspnetcore-5.0
            // Remove `JwtBearerDefaults.AuthenticationScheme` from `.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)`
            // Because it'll be set default scheme to JwtBearer. The cookie-based login not work!
            services
                 .AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                 .AddCookie(CookieAuthenticationDefaults.AuthenticationScheme, options =>
                 {
                     options.LoginPath = "/account/login";
                 })
                .AddJwtBearer(options =>
                {
                    // JWT validation https://devblogs.microsoft.com/aspnet/jwt-validation-and-authorization-in-asp-net-core/
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = GetIssuerSigningKey(),
                        ValidateLifetime = true,

                        ValidateIssuer = false,
                        ValidateAudience = false,
                        ValidIssuer = null,
                        ValidAudience = null,
                    };
                });
        }

        public static void ConfigureOpenIddict(this IServiceCollection services, IConfiguration Configuration)
        {
            services.AddDbContext<DbContext>(options =>
                {
                    // Configure the context to use an in-memory store.
                    options.UseInMemoryDatabase(nameof(DbContext));

                    // Register the entity sets needed by OpenIddict.
                    options.UseOpenIddict();
                });

            services.AddOpenIddict()
                // Register the OpenIddict core components.
                .AddCore(options =>
                {
                    // Configure OpenIddict to use the EF Core stores/models.
                    options
                        .UseEntityFrameworkCore()
                        .UseDbContext<DbContext>();
                })

                // Register the OpenIddict server components.
                .AddServer(options =>
                {
                    options
                        .AllowClientCredentialsFlow() // Alow code flow
                        .AllowAuthorizationCodeFlow()
                        .RequireProofKeyForCodeExchange() // Use PKCE
                        .AllowRefreshTokenFlow(); // Enable refresh token

                    options
                        .SetAuthorizationEndpointUris("/connect/authorize")
                        .SetTokenEndpointUris("/connect/token")
                        .SetUserinfoEndpointUris("/connect/userinfo");

                    var signingCredentials = new SigningCredentials(GetIssuerSigningKey(), SecurityAlgorithms.HmacSha256);

                    // You can register an asymmetric key and a symmetric key
                    // that will be preferred for access tokens if one is registered.
                    options
                        .AddEphemeralEncryptionKey()
                        .AddEphemeralSigningKey()
                        .AddSigningCredentials(signingCredentials)
                        .DisableAccessTokenEncryption();

                    // Register scopes (permissions)
                    options.RegisterScopes("api");

                    // Register the ASP.NET Core host and configure the ASP.NET Core-specific options.
                    options
                        .UseAspNetCore()
                        .EnableTokenEndpointPassthrough()
                        .EnableAuthorizationEndpointPassthrough()
                        .EnableUserinfoEndpointPassthrough();
                });

            // The test data implements the IHostedService interface, 
            // which enables us to execute the generation of test data in Startup.cs when the application starts. 
            services.AddHostedService<ClientRegistrationService>();
        }

        private static SymmetricSecurityKey GetIssuerSigningKey()
        {
            var secretKey = Encoding.ASCII.GetBytes("your-256-bit-secret");
            return new SymmetricSecurityKey(secretKey);
        }
    }
}
