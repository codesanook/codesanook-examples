using Amazon;

namespace Codesanook.Examples.DotNetCore.AwsApi
{
    public class SmtpSetting
    {
        // Use IAM key Id
        public string SmtpUsername { get; set; }

        // Use Get-SmtpCredentials to get password from IAM secret value
        public string SmtpPassword { get; set; }

        // If you're using Amazon SES in a region other than US EAST 2 (Ohio), 
        // replace email-smtp.us-east-2.amazonaws.com with the Amazon SES SMTP  
        // endpoint in the appropriate AWS Region.
        public string Host { get; } =
            $"email-smtp.{RegionEndpoint.USEast2.SystemName}.amazonaws.com";

        // The port you will connect to on the Amazon SES SMTP endpoint. 
        // We are choosing port 587 because we will use STARTTLS to encrypt the connection.
        public int Port { get; } = 587;

        public string FromAddress { get; set; }
        public string FromName { get; set; }
        public string ToAddress { get; set; }
    }
}
