using System;
using System.IO;
using System.Net.Mail;
using System.Net.Sockets;
using System.Text;
using Xunit;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;

namespace Codesanook.Examples.DotNet.Mail
{
    // https://www.labnol.org/software/verify-email-address/18220/
    // https://www.webdigi.co.uk/blog/2009/how-to-check-if-an-email-address-exists-without-sending-an-email/
    public class EmailValidationTest
    {
        [Fact]
        public void Test()
        {
            var a = Directory.GetCurrentDirectory();

            var executedPath = new Uri(System.AppContext.BaseDirectory).LocalPath;
            var executedDirectory = Path.GetDirectoryName(executedPath);
            var directoryInfo = new DirectoryInfo(executedDirectory);
            var projectRootPath = directoryInfo.Parent.Parent.Parent.FullName;

            // Example content of email-config.yml
            // smtpServer: 
            // smtpPort: 25
            // fromAddress: 
            // toAddress: 
            var configPath = Path.Combine(projectRootPath, "Mail", "email-config.yml");
            var emailConfig = GetConfig(configPath);
            Send(emailConfig);
        }

        private static EmailConfig GetConfig(string emailConfigPath)
        {
            using var streamReader = new StreamReader(emailConfigPath);
            var deserializer = new DeserializerBuilder()
                .WithNamingConvention(CamelCaseNamingConvention.Instance)
                .Build();

            return deserializer.Deserialize<EmailConfig>(streamReader.ReadToEnd());
        }

        private static void Send(EmailConfig emailConfig)
        {
            var client = new TcpClient(emailConfig.SmtpServer, emailConfig.SmtpPort);
            using (var stream = client.GetStream())
            {
                var response = Response(stream);
                if (response.Substring(0, 3) != "220")
                {
                    throw new SmtpException(response);
                }

                var message = "helo hi\r\n";
                Write(message, stream);
                response = Response(stream);
                if (response.Substring(0, 3) != "250")
                {
                    throw new SmtpException(response);
                }
                Console.WriteLine(response);

                message = "mail from: <" + emailConfig.FromAddress + ">\r\n";
                Write(message, stream);
                response = Response(stream);
                if (response.Substring(0, 3) != "250")
                {
                    throw new SmtpException(response);
                }
                Console.WriteLine(response);

                message = "rcpt to:<" + emailConfig.ToAddress + ">\r\n";
                Write(message, stream);
                response = Response(stream);
                if (response.Substring(0, 3) != "250")
                {
                    throw new SmtpException(response);
                }
                Console.WriteLine(response);
            }

            client.Close();
        }

        /// <summary>
        /// Writes the data to the socket.
        /// </summary>
        /// <param name="message"></param>
        private static void Write(string message, Stream stream)
        {
            ASCIIEncoding en = new ASCIIEncoding();
            var writeBuffer = en.GetBytes(message);
            stream.Write(writeBuffer, 0, writeBuffer.Length);
            stream.Flush();
        }

        /// <summary>
        /// Receives the data from the socket.
        /// </summary>
        /// <returns></returns>
        public static string Response(Stream stream)
        {
            ASCIIEncoding enc = new ASCIIEncoding();
            var serverbuff = new byte[1024];
            int count = stream.Read(serverbuff, 0, 1024);
            if (count == 0)
            {
                return string.Empty;
            }
            return enc.GetString(serverbuff, 0, count);
        }
    }
}

