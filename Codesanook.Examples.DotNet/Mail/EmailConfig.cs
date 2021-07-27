namespace Codesanook.Examples.DotNet.Mail
{
    public class EmailConfig
    {
        public string SmtpServer { get; set; }
        public int SmtpPort { get; set; }
        public string FromAddress { get; set; }
        public string ToAddress { get; set; }
    }
}
