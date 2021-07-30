using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using OpenIddict.Abstractions;

namespace DotNetAuthorizationServer
{
    public class ClientRegistrationService : IHostedService
    {
        private readonly IServiceProvider _serviceProvider;

        public ClientRegistrationService(IServiceProvider serviceProvider) => _serviceProvider = serviceProvider;

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            using var scope = _serviceProvider.CreateScope();

            var context = scope.ServiceProvider.GetRequiredService<DbContext>();
            await context.Database.EnsureCreatedAsync(cancellationToken);

            var manager = scope.ServiceProvider.GetRequiredService<IOpenIddictApplicationManager>();

            // For react-client
            //
            // * Authorization URL: https://localhost:5001/connect/authorize
            // * Access token URL: https://localhost:5001/connect/token
            // * Redirect URI: http://localhost:3000/authentication/login-callback
            // * Client ID: react-spa
            // * Client secret: [blank] (not used with public clients)
            // * Scope: api openid offline_access
            // * Grant type: authorization code
            // * Request access token locally: yes

            if (await manager.FindByClientIdAsync("react-spa", cancellationToken) is null)
            {
                await manager.CreateAsync(new OpenIddictApplicationDescriptor
                {
                    ClientId = "react-spa",
                    DisplayName = "React Spa",
                    RedirectUris = { new Uri("http://localhost:3000/authentication/login-callback") },

                    Permissions =
                    {

                        // Allowed grant type 
                        OpenIddictConstants.Permissions.GrantTypes.ClientCredentials,
                        OpenIddictConstants.Permissions.GrantTypes.AuthorizationCode,
                        OpenIddictConstants.Permissions.GrantTypes.RefreshToken,

                        OpenIddictConstants.Permissions.Endpoints.Authorization,
                        OpenIddictConstants.Permissions.Endpoints.Token,

                        OpenIddictConstants.Permissions.Prefixes.Scope + "api",
                        OpenIddictConstants.Permissions.ResponseTypes.Code,

                    }
                }, cancellationToken);
            }

            // To test this sample with Postman, use the following settings:
            //
            // * Authorization URL: https://localhost:5001/connect/authorize
            // * Access token URL: https://localhost:5001/connect/token
            // * Client ID: postman
            // * Client secret: postman-secret
            // * Scope: api openid offline_access
            // * Grant type: authorization code

            if (await manager.FindByClientIdAsync("postman", cancellationToken) is null)
            {
                await manager.CreateAsync(new OpenIddictApplicationDescriptor
                {
                    ClientId = "postman",
                    ClientSecret = "postman-secret",
                    DisplayName = "Postman",
                    RedirectUris = { new Uri("https://oauth.pstmn.io/v1/callback") },

                    Permissions =
                    {
                        OpenIddictConstants.Permissions.GrantTypes.ClientCredentials,
                        OpenIddictConstants.Permissions.Endpoints.Token,
                        OpenIddictConstants.Permissions.Prefixes.Scope + "api",

                        OpenIddictConstants.Permissions.GrantTypes.AuthorizationCode,
                        OpenIddictConstants.Permissions.Endpoints.Authorization,
                        OpenIddictConstants.Permissions.ResponseTypes.Code,

                        OpenIddictConstants.Permissions.GrantTypes.RefreshToken,
                    }
                }, cancellationToken);
            }
        }

        public Task StopAsync(CancellationToken cancellationToken) => Task.CompletedTask;
    }
}
