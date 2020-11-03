using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.IO;
using System.Net;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using Xunit;

namespace Codesanook.Examples.DotNetCore.GoogleApi
{
    // Alternatively, you can use SDK to send a message https://www.nuget.org/packages/FirebaseAdmin/
    // More info https://firebase.google.com/docs/reference/admin/dotnet/class/firebase-admin/messaging/firebase-messaging
    public class FirebaseCloudMessagingTest
    {
        public FirebaseCloudMessagingTest() =>
            ServicePointManager.ServerCertificateValidationCallback += new RemoteCertificateValidationCallback(ValidateServerCertificate);

        // Get serverKey from "Cloud Messaging" setting in Firebase console 
        private const string serverKey = "";

        // Get registrationId from a mobile client application.
        // Alternatively, you can also get its value from GCM Notifications Chrome extension and test it without a mobile client application.
        // More information
        // https://github.com/GoogleChrome/chrome-app-samples/tree/master/samples/gcm-notifications
        // Link to GCM Notifications https://chrome.google.com/webstore/detail/gcm-notifications/gpededflkpcoehfjpdecdkoiagajloin
        // Use Sender ID from "Cloud Messaging" setting in Firebase console to get register ID with GCM Notifications. 
        private const string registrationId = "";

        [Fact]
        public void SendPushNotifcation_ValidRequest_PushNotficationSentSuccessfully()
        {
            var pushNotification = new PushNotificationRequest()
            {
                Title = "Push message",
                Body = "FCM push message, credit admin Pong Codesanook",
                RegistrationId = registrationId,
                Platform = PushNotificationPlatform.Android
            };

            var response = SendPushNotification(pushNotification);
            Assert.True(response.Success);
        }

        private PushNotificationResponse SendPushNotification(PushNotificationRequest pushNotification)
        {
            var message = CreateMessage(pushNotification);
            var request = CreateRequest();

            var payloadData = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(message));
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
                    // TODO need to log more exception information 
                    throw new InvalidOperationException($"Response from web service with status code {responseCode}");
            }
        }

        private static HttpWebRequest CreateRequest()
        {
            var request = (HttpWebRequest)WebRequest.Create("https://fcm.googleapis.com/fcm/send");
            request.Method = "POST";
            request.KeepAlive = false;
            request.ContentType = "application/json";

            // We don't need a header of Sender: id=xxx but some examples on the Internet add it.
            request.Headers.Add($"Authorization: key={serverKey}");
            return request;
        }

        private static dynamic CreateMessage(PushNotificationRequest pushNotification)
        {
            dynamic message = new ExpandoObject();
            message.registration_ids = new[] { pushNotification.RegistrationId };
            message.priority = "high";
            const string soundName = "default";

            // REF Firebase Cloud Messaging (FCM)
            // https://firebase.google.com/docs/cloud-messaging/concept-options

            // Use notification messages when you want FCM to handle displaying a notification on your client app's behalf. 
            // Use data messages when you want to process the messages on your client app.

            // Common fields for iOS and Android 
            // message.notification.title
            // message.notification.body
            // message.data

            switch (pushNotification.Platform)
            {
                case PushNotificationPlatform.IOS:
                    // Use notification message
                    message.notification = new
                    {
                        title = pushNotification.Title,
                        body = pushNotification.Body,
                        sound = soundName,
                    };
                    break;
                case PushNotificationPlatform.Android: // Work for start up code block and get push in background
                    // Use data message
                    message.data = new Dictionary<string, object>()
                    {
                        { "title", pushNotification.Title },
                        { "body", pushNotification.Body },
                        { "soundname", soundName },
                        // Just dump property to show that we need to use snake case naming. 
                        // If we use camel case, the data won't sent.
                        { "transaction_id", 1 },
                        { "additionalData", "other" } // This field won't sent
                    };
                    break;
                default:
                    throw new InvalidOperationException("Invalid registration platform");
            }

            return message;
        }

        private static bool ValidateServerCertificate(
          object sender,
          X509Certificate certificate,
          X509Chain chain,
          SslPolicyErrors sslPolicyErrors
        ) => true;
    }
}

