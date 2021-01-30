using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace Codesanook.Examples.DotNetAuthorizationServer
{
    public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllersWithViews();
            services
                .AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                .AddCookie(CookieAuthenticationDefaults.AuthenticationScheme, options =>
                {
                    options.LoginPath = "/account/login";
                });
            // https://docs.microsoft.com/en-us/aspnet/core/migration/1x-to-2x/identity-2x?view=aspnetcore-5.0#jwt-bearer-authentication
            services
                .AddAuthentication(JwtBearerDefaults.AuthenticationScheme);

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
                        .AllowClientCredentialsFlow()
                        .AllowAuthorizationCodeFlow().RequireProofKeyForCodeExchange()
                        // https://github.com/openiddict/openiddict-core/issues/437
                        // Nope, it's not. The OAuth2 specification explicitly requires sending the token request parameters using the "formurl" encoding.
                        // JSON is not allowed and thus not supported.
                        .AllowRefreshTokenFlow();

                    options
                        .SetAuthorizationEndpointUris("/connect/authorize")
                        .SetTokenEndpointUris("/connect/token")
                        .SetUserinfoEndpointUris("/connect/userinfo");

                    //var RSA = new RSACryptoServiceProvider(2048);
                    //var KeyParam = RSA.ExportParameters(true);
                    //var key = new RsaSecurityKey(KeyParam);
                    //var credentials = new SigningCredentials(key, SecurityAlgorithms.RsaSha256Signature);

                    var secretKey = Encoding.UTF8.GetBytes("MysecretMysecretMysecret");
                    var securityKey = new SymmetricSecurityKey(secretKey);
                    var signingCredentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

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

            services.AddHostedService<TestData>();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();
            // The call to UseAuthentication is made after the call to UseRouting,
            // so that route information is available for authentication decisions, but before UseEndpoints,
            // so that users are authenticated before accessing the endpoints.
            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapDefaultControllerRoute();
            });
        }
    }
}
