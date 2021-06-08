using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.Extensions.Hosting;

using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore.InMemory;
using Microsoft.EntityFrameworkCore;

namespace DotNetAuthorizationServer
{
    public static class ServiceExtensions
    {

        public static void ConfigureOidc(this IServiceCollection services, IConfiguration Configuration)
        {
           services
                .AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                .AddCookie(CookieAuthenticationDefaults.AuthenticationScheme, options =>
                {
                    options.LoginPath = "/account/login";
                });

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

            services.AddHostedService<WebClientWorker>();
        }
      
        public static void ConfigureJWT(this IServiceCollection services, IConfiguration Configuration)
        {
           
        }

    }
}