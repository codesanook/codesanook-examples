using Newtonsoft.Json;
using System;
using System.Dynamic;
using System.IO;
using System.Net;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using Xunit;

namespace Codesanook.Examples.DotNetCore.GoogleApi
{
    public class FirebaseCloudMessagingTest
    {
        // Get serverKey from Cloud Messaging setting in Firebase console 
        private const string serverKey = "";

        // Get registrationId from a mobile client application 
        // Alternatively, you can also get its value from GCM Notifications Chrome extension and test it without a mobile client application.
        // https://github.com/GoogleChrome/chrome-app-samples/tree/master/samples/gcm-notifications
        private const string registrationId = "";

        [Fact]
        public void SendPushNotifcation_ValidRequest_PushNotficationSentSuccessfully()
        {
            var pushNotification = new PushNotificationRequest()
            {
                Title = "Push message",
                Body = "FCM push message, credit admin Pong Codesanook",
                RegistrationId = registrationId,
                Platform = "Android"
            };

            var response = SendPushNotification(pushNotification);
            Assert.True(response.Success);
        }

        private PushNotificationResponse SendPushNotification(PushNotificationRequest pushNotification)
        {
            dynamic payload = new ExpandoObject();
            payload.registration_ids = new[] { pushNotification.RegistrationId };
            payload.priority = "high";
            const string soundName = "default";

            switch (pushNotification.Platform.ToUpperInvariant())
            {
                case "IOS":
                    payload.notification = new
                    {
                        title = pushNotification.Title,
                        body = pushNotification.Body,
                        sound = soundName,
                    };
                    break;
                case "ANDROID": // Work for start up and get push in background
                    payload.data = new
                    {
                        title = pushNotification.Title,
                        body = pushNotification.Body,
                        soundname = soundName,
                    };
                    break;
                default:
                    throw new InvalidOperationException("Invalid registration platform");
            }

            ServicePointManager.ServerCertificateValidationCallback += new RemoteCertificateValidationCallback(ValidateServerCertificate);
            var request = (HttpWebRequest)WebRequest.Create("https://fcm.googleapis.com/fcm/send");
            request.Method = "POST";
            request.KeepAlive = false;
            request.ContentType = "application/json";

            request.Headers.Add($"Authorization: key={serverKey}");
            // We don't need a header of Sender: id=xxx but some examples on the Internet add it.

            var payloadJson = JsonConvert.SerializeObject(payload);
            var payloadData = Encoding.UTF8.GetBytes(payloadJson);
            request.ContentLength = payloadData.Length;

            WebResponse response;
            using (var requestStream = request.GetRequestStream())
            {
                requestStream.Write(payloadData, 0, payloadData.Length);
                response = request.GetResponse();
            }

            var responseCode = ((HttpWebResponse)response).StatusCode;
            switch (responseCode)
            {
                case HttpStatusCode.OK:
                    using (var reader = new StreamReader(response.GetResponseStream()))
                    {
                        var responseContent = reader.ReadToEnd();
                        return JsonConvert.DeserializeObject<PushNotificationResponse>(responseContent);
                    }
                case HttpStatusCode.Unauthorized:
                case HttpStatusCode.Forbidden:
                    throw new InvalidOperationException("Unauthorized - need a new token");
                default:
                    throw new InvalidOperationException("Response from web service isn't OK");
            }
        }

        private static bool ValidateServerCertificate(
          object sender,
          X509Certificate certificate,
          X509Chain chain,
          SslPolicyErrors sslPolicyErrors
        ) => true;
    }
}

