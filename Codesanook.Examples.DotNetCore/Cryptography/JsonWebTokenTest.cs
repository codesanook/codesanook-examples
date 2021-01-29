using Microsoft.IdentityModel.Tokens;
using Microsoft.IdentityModel.JsonWebTokens;
using System;
using System.Security.Claims;
using System.Text;
using Xunit;
using Xunit.Abstractions;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;

namespace Codesanook.Examples.DotNetCore.Cryptography
{
    public class JsonWebTokenTest
    {
        private readonly ITestOutputHelper output;
        public JsonWebTokenTest(ITestOutputHelper output) => this.output = output;

        [Fact]
        public void CreateToken_ValidInput_ReturnValidToken()
        {
            // Arange
            // generate token that is valid for 7 days
            var tokenHandler = new JsonWebTokenHandler();
            var secretKey = Encoding.ASCII.GetBytes("MysecretMysecretMysecret");
            var securityKey = new SymmetricSecurityKey(secretKey);
            var signingCredentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[] { new Claim("id", "1") }),
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = signingCredentials
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            output.WriteLine($"token: {token}");

            // output on jwt.io
            /*
            header:
            {
              "alg": "HS256",
              "typ": "JWT"
            }
            playload:
            {
              "id": "1",
              "nbf": 1611932414,
              "exp": 1612537214,
              "iat": 1611932414
            }
            */
            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = securityKey,
                ValidateIssuer = false,
                ValidateAudience = false,
                ValidIssuer = null,
                ValidAudience = null,
            };

            var validationResult = tokenHandler.ValidateToken(token, tokenValidationParameters);
            // var exception = Record.Exception(() => { });
            // Assert.IsNotType<SecurityTokenInvalidSignatureException>(exception);

            // Assert
            Assert.True(validationResult.IsValid);
        }

        // https://github.com/AzureAD/azure-activedirectory-identitymodel-extensions-for-dotnet/blob/dev/src/Microsoft.IdentityModel.Tokens/SymmetricSignatureProvider.cs#L188
        [Fact]
        public void CreateTokenWithKeyHashDirectly_ValidInput_ReturnValidToken()
        {
            // Arange
            // generate token that is valid for 7 days
            var secretKey = Encoding.ASCII.GetBytes("MysecretMysecretMysecret");
            var securityKey = new SymmetricSecurityKey(secretKey);
            // var cryptoProviderFactory = securityKey.CryptoProviderFactory;
            var cryptoProviderFactory = new CryptoProviderFactory();
            var keyBytes = securityKey.Key;
            var keyedHash = cryptoProviderFactory.CreateKeyedHashAlgorithm(keyBytes, SecurityAlgorithms.HmacSha256);

            var header = new JObject()
            {
                { JwtHeaderParameterNames.Alg, SecurityAlgorithms.HmacSha256 }, // HS256
                { "typ", "JWT" }
            };

            var payload = new JObject()
            {
                { "id", "1" }
            };

            var rawHeader = Base64UrlEncoder.Encode(Encoding.UTF8.GetBytes(header.ToString(Formatting.None)));
            var rawPayloader = Base64UrlEncoder.Encode(Encoding.UTF8.GetBytes(payload.ToString(Formatting.None)));

            var inputForSignature = $"{rawHeader}.{rawPayloader}";
            var signature = Base64UrlEncoder.Encode(keyedHash.ComputeHash(Encoding.UTF8.GetBytes(inputForSignature)));

            var token = $"{rawHeader}.{rawPayloader}.{signature}";
            output.WriteLine($"token: {token}");
            /*
            {
              "alg": "HS256",
              "typ": "JWT"
            }
            {
              "id": "1"
            }
            */
        }
    }
}
