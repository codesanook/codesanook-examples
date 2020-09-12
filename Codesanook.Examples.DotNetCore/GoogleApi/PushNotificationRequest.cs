namespace Codesanook.Examples.DotNetCore.GoogleApi
{
    public class PushNotificationRequest
    {
        public string RegistrationId { get; set; }
        public string Title { get; set; }
        public string Body { get; set; }
        public string Platform {get;set;}
    }
}

