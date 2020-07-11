using Google.Apis.Auth.OAuth2;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Mail;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using Google.Apis.Gmail.v1;
using Google.Apis.Services;
using Google.Apis.Gmail.v1.Data;

namespace Codesanook.Examples.DotNetCore.GoogleApi
{
    public class GmailServiceApiTest
    {
        [Fact]
        public async Task SendMailTest()
        {
            var certificate = new X509Certificate2(
                @"C:\projects\topf\gmail-service-account-key.p12", "notasecret", X509KeyStorageFlags.Exportable
            );
            const string user = "contact@meddbtech.com";
            var credential = new ServiceAccountCredential(
                new ServiceAccountCredential.Initializer("gmail-service-account@topf-project.iam.gserviceaccount.com")
                {
                    // Note: other scopes can be found here: https://developers.google.com/gmail/api/auth/scopes
                    Scopes = new[] { GmailService.Scope.GmailSend },
                    User = user,
                }.FromCertificate(certificate));


            var service = new GmailService(
                new BaseClientService.Initializer { HttpClientInitializer = credential, ApplicationName = "test app" }
            );

            var content =
                "From: aaron@realman.com" + Environment.NewLine +
                "To: theeranitp@gmail.com" + Environment.NewLine +
                "Subject: This is the Subject" + Environment.NewLine +
                "Content-Type: text/html; charset=UTF-8" + Environment.NewLine + Environment.NewLine +
                "สวัสดี";
            var message = new Message();
            message.Raw = Base64UrlEncode(content);
            var request = service.Users.Messages.Send(message, user);
            request.Execute();
        }

        private static string Base64UrlEncode(string input)
        {
            var inputBytes = Encoding.UTF8.GetBytes(input);
            // Special "url-safe" base64 encode.
            return Convert.ToBase64String(inputBytes)
              .Replace('+', '-')
              .Replace('/', '_')
              .Replace("=", "");
        }
    }
}
