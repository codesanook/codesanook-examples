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
            // Arrange
            var tokenHandler = new JsonWebTokenHandler();
            var secretKey = Encoding.ASCII.GetBytes("MysecretMysecretMysecret");
            var securityKey = new SymmetricSecurityKey(secretKey);
            var signingCredentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
            var cryptoProviderFactory =  signingCredentials.Key.CryptoProviderFactory;
            var signatureProvider  = cryptoProviderFactory.CreateForSigning(signingCredentials.Key, signingCredentials.Algorithm);
            // signatureProvider is SymmetricSignatureProvider which use KeyedHashAlgorithm
            // https://github.com/AzureAD/azure-activedirectory-identitymodel-extensions-for-dotnet/blob/dev/src/Microsoft.IdentityModel.Tokens/SymmetricSignatureProvider.cs#L188

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[] { new Claim("id", "1") }),
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = signingCredentials,
                EncryptingCredentials = null
            };
            // How sign token under the hood
            // https://github.com/AzureAD/azure-activedirectory-identitymodel-extensions-for-dotnet/blob/dev/src/Microsoft.IdentityModel.JsonWebTokens/JwtTokenUtilities.cs#L89
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
            var validationResult = ValidateToken(tokenHandler, securityKey, token);
            // var exception = Record.Exception(() => { });
            // Assert.IsNotType<SecurityTokenInvalidSignatureException>(exception);

            // Assert
            Assert.True(validationResult.IsValid);
        }

        [Fact]
        public void CreateTokenWithKeyHashDirectly_ValidInput_ReturnValidToken()
        {
            // Arrange
            var secretKey = Encoding.UTF8.GetBytes("MysecretMysecretMysecret");
            var securityKey = new SymmetricSecurityKey(secretKey);
            var cryptoProviderFactory = new CryptoProviderFactory();
            // var cryptoProviderFactory = securityKey.CryptoProviderFactory;
            var keyBytes = securityKey.Key;

            //*** As you can see, we can completely by pass SymmetricSecurityKey.
            var keyedHash = cryptoProviderFactory.CreateKeyedHashAlgorithm(keyBytes, SecurityAlgorithms.HmacSha256);

            var header = new JObject()
            {
                { JwtHeaderParameterNames.Alg, SecurityAlgorithms.HmacSha256 }, // HS256
                { "typ", "JWT" }
            };

            var payload = new JObject()
            {
                { "id", "1" },
                { "exp", EpochTime.GetIntDate(DateTime.UtcNow.AddDays(7)) }
            };

            // Act
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
              "id": "1",
              "exp": 1612587194
            }
            */

            var tokenHandler = new JsonWebTokenHandler();
            // Assert
            var validationResult = ValidateToken(tokenHandler, securityKey, token);
            Assert.True(validationResult.IsValid);
        }

        private static TokenValidationResult ValidateToken(JsonWebTokenHandler tokenHandler, SymmetricSecurityKey securityKey, string token)
        {
            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = securityKey,
                ValidateIssuer = false,
                ValidateAudience = false,
                ValidIssuer = null,
                ValidAudience = null,
            };

            return tokenHandler.ValidateToken(token, tokenValidationParameters);
        }
    }
}
