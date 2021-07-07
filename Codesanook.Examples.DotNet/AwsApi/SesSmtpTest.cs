using System;
using System.IO;
using System.Net;
using System.Net.Mail;
using Xunit;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;

namespace Codesanook.Examples.DotNetCore.AwsApi
{
    public class SesSmtpTest
    {
        private readonly SesSmtpSetting setting;

        public SesSmtpTest()
        {
            var deserializer = new DeserializerBuilder()
                .WithNamingConvention(UnderscoredNamingConvention.Instance)
                .Build();

            // Don't forget to "Copy if newer"
            const string settingFile = "AwsApi/smtp-setting.yml";
            if (!File.Exists(settingFile))
            {
                throw new InvalidOperationException(
                    string.Join("\n",
                        $"Please create {settingFile} set \"copy if newer\" and add the following contents:",
                        "smtp_username: \"IAM access key id\"",
                        "smtp_password: \"IAM SMTP password derived from IAM secret access key\"",
                        "from_address: \"\"",
                        "from_name: \"\"",
                        "to_address: \"\""
                    )
                );
            }

            using var streamReader = new StreamReader(settingFile);
            setting = deserializer.Deserialize<SesSmtpSetting>(streamReader.ReadToEnd());
        }

        [Fact]
        public void SendEmail_ValidSetting_EmailSent()
        {
            // The subject line of the email
            const string subject = "Amazon SES test (SMTP interface accessed using C#)";

            // The body of the email
            const string body =
                "<h1>Amazon SES Test</h1>" +
                "<p>This email was sent through the " +
                "<a href='https://aws.amazon.com/ses'>Amazon SES</a> SMTP interface " +
                "using the .NET System.Net.Mail library.</p>";

            // Create and build a new MailMessage object
            var message = new MailMessage
            {
                IsBodyHtml = true,
                // This address must be verified with Amazon SES.
                From = new MailAddress(setting.FromAddress, setting.FromName),
                Subject = subject,
                Body = body
            };
            // This address must be verified with Amazon SES.
            message.To.Add(new MailAddress(setting.ToAddress));

            using var client = new SmtpClient(setting.Host, setting.Port)
            {
                // Pass SMTP credentials
                Credentials = new NetworkCredential(
                    setting.SmtpUsername,
                    setting.SmtpPassword
                ),
                // Enable SSL encryption
                EnableSsl = true
            };

            // Try to send the message. Fail unit test if error 
            client.Send(message);
        }
    }
}
