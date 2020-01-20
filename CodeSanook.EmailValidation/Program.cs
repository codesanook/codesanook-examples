using System;
using System.IO;
using System.Net.Mail;
using System.Net.Sockets;
using System.Text;

namespace Codesanook.EmailValidation
{
    // https://www.labnol.org/software/verify-email-address/18220/
    // https://www.webdigi.co.uk/blog/2009/how-to-check-if-an-email-address-exists-without-sending-an-email/
    public class Program
    {
        static void Main(string[] args)
        {
           Send("outlook-com.olc.protection.outlook.com", "theeranitp@outlook.com");
          //  Send("smtp.outlook.com", "theeranitp@outlook.com");
        }

        public static void Send(string server, string to)
        {
            var client = new TcpClient(server, 25);
            var from = "contact@meddbtech.com";

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

                message = "mail from: <" + from + ">\r\n";
                Write(message, stream);
                response = Response(stream);
                if (response.Substring(0, 3) != "250")
                {
                    throw new SmtpException(response);
                }
                Console.WriteLine(response);

                message = "rcpt to:<" + to + ">\r\n";
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
        public static void Write(string message, Stream stream)
        {
            ASCIIEncoding en = new ASCIIEncoding();
            byte[] WriteBuffer = new byte[1024];
            WriteBuffer = en.GetBytes(message);
            stream.Write(WriteBuffer, 0, WriteBuffer.Length);
            stream.Flush();
        }

        /// <summary>
        /// Receives the data from the socket.
        /// </summary>
        /// <returns></returns>
        public static string Response(Stream stream)
        {
            ASCIIEncoding enc = new ASCIIEncoding();
            byte[] serverbuff = new Byte[1024];
            int count = stream.Read(serverbuff, 0, 1024);
            if (count == 0)
            {
                return "";
            }
            return enc.GetString(serverbuff, 0, count);
        }
    }
}
