using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using Xunit;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;

namespace Codesanook.Examples.DotNetCore.Mailgun
{
    // Useful links: 
    // https://documentation.mailgun.com/en/latest/user_manual.html#mailing-lists
    // https://www.postmastery.com/list-unsubscribe-header-critical-for-sustained-email-delivery/
    public class MailgunSmtpTest
    {
        private readonly MailgunSmtpSetting setting;

        public MailgunSmtpTest()
        {
            var deserializer = new DeserializerBuilder()
                .WithNamingConvention(UnderscoredNamingConvention.Instance)
                .Build();

            // Don't forget to "Copy if newer"
            const string settingFile = "Mailgun/smtp-setting.yml";
            if (!File.Exists(settingFile))
            {
                throw new InvalidOperationException(
                    string.Join("\n",
                        $"Please create {settingFile}, set \"copy if newer\" and add the following contents:",
                        "smtp_username: \"SMTP username\"",
                        "smtp_password: \"SMTPpassword\"",
                        "from_address: \"\"",
                        "from_name: \"\"",
                        "to_address: \"\""
                    )
                );
            }

            using var streamReader = new StreamReader(settingFile);
            setting = deserializer.Deserialize<MailgunSmtpSetting>(streamReader.ReadToEnd());
        }

        [Fact]
        public void SendEmail_ValidSetting_EmailSent()
        {
            const string subject = "Mailgun SMTP using C#)";

            // The body of the email
            var contents = new[]
            {
                "Mailgun Test",
                "This email was sent through the <a href='https://www.mailgun.com'>Mailgun.com </a> SMTP",
                "It uses the .NET System.Net.Mail library",
                "Codesanook",
                "Address: Bangkok, Thailand 10500",
                "If you would like to unsubscribe our email, <a href='%unsubscribe_url%'>please click here</a>"
            };

            var body = contents.Aggregate(
                new StringBuilder(),
                (sb, currentLine) => sb.Append($"<p>{currentLine}</p>")
            ).ToString();

            // Create and build a new MailMessage object
            var message = new MailMessage
            {
                IsBodyHtml = true,
                // This address must be verified with Amazon SES.
                From = new MailAddress(setting.FromAddress, setting.FromName),
                Subject = subject,
                Body = body
            };

            // Hotmail only supports the mailto:link. When a user clicks on the 'unsubscribe'  option in Hotmail. 
            // Hotmail tries to read the mailto:link in the List-Unsubscribe header. 
            // If the mailto:link is missing, it moves all the messages to the Junk folder.
            // The mailto:link is supported by Gmail, Hotmail, Yahoo, AOL, ATT, Time Warner and Comcast; 
            // European ISPs such as GMX, Libero, Ziggo, Orange, BTInternet; Russian ISPs such as mail.ru and Yandex; 
            // and the Chinese domains qq.com, naver.com etc. So most ISPs support (and prefer) mailto:link.
            message.Headers.Add("List-Unsubscribe", "<%unsubscribe_email%>");

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

