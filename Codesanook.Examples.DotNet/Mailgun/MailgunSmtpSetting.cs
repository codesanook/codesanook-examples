namespace Codesanook.Examples.DotNet.Mailgun
{
    public class MailgunSmtpSetting
    {
        public string SmtpUsername { get; set; }
        public string SmtpPassword { get; set; }

        public string Host { get; } = "smtp.mailgun.org";
        public int Port { get; } = 587;

        public string FromAddress { get; set; }
        public string FromName { get; set; }
        public string ToAddress { get; set; }
    }
}

