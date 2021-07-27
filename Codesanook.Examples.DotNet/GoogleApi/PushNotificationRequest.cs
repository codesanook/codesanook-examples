namespace Codesanook.Examples.DotNet.GoogleApi
{
    public class PushNotificationRequest
    {
        public string RegistrationId { get; set; }
        public string Title { get; set; }
        public string Body { get; set; }
        public PushNotificationPlatform Platform {get;set;}
    }
}

