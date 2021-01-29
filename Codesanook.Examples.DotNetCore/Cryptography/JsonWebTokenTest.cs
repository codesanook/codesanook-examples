using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Xunit;
using Xunit.Abstractions;

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
            var tokenHandler = new JwtSecurityTokenHandler();
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
            var tokenString = tokenHandler.WriteToken(token);

            output.WriteLine($"token: {tokenString}");

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

            var exception = Record.Exception(() => 
            {
               tokenHandler.ValidateToken(tokenString, tokenValidationParameters, out SecurityToken validatedToken);
            });

            // Assert
            // Assert.IsNotType<SecurityTokenInvalidSignatureException>(exception);
            Assert.Null(exception);
        }
    }
}
