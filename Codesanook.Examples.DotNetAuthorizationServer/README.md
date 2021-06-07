## To run the project locally for development
- CD to root of the project
- Restore all Node.js packages:
```
$ yarn install
```
- Build and watch Node.js project with Webpack:
```
$ yarn watch
```
- Open a new shell and run the project with `dotnet watch run` command :
``` 
$ dotnet watch run
``` 
- Open a browser and navigate to https://localhost:5001

## Test configuration 
- Navigate to https://localhost:5001/.well-known/openid-configuration

# Useful Oauth 2 article
- https://dev.to/robinvanderknaap/setting-up-an-authorization-server-with-openiddict-part-i-introduction-4jid
- https://dev.to/robinvanderknaap/setting-up-an-authorization-server-with-openiddict-part-ii-create-aspnet-project-4949
- https://dev.to/robinvanderknaap/setting-up-an-authorization-server-with-openiddict-part-iii-client-credentials-flow-55lp
- https://dev.to/robinvanderknaap/setting-up-an-authorization-server-with-openiddict-part-iv-authorization-code-flow-3eh8
- https://dev.to/robinvanderknaap/setting-up-an-authorization-server-with-openiddict-part-v-openid-connect-a8j
- https://dev.to/robinvanderknaap/setting-up-an-authorization-server-with-openiddict-part-vi-refresh-tokens-5669
- sha256 certificate https://devblogs.microsoft.com/aspnet/bearer-token-authentication-in-asp-net-core/
- https://stackoverflow.com/a/40245880/1872200

## Known issues
- No email in id_token
- Cannot send token with JSON body
