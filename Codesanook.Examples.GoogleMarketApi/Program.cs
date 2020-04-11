using Google.Apis.Auth.OAuth2;
using Google.Apis.Services;
using System;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;

namespace Codesanook.GoogleMarketApi
{
    /// <summary>
    /// This example uses the discovery API to list all APIs in the discovery repository.
    /// https://developers.google.com/discovery/v1/using.
    /// <summary>
    public class Program
    {
        [STAThread]
        static void Main(string[] args)
        {
            Console.WriteLine("Discovery API Sample");
            Console.WriteLine("====================");
            try
            {
                new Program().Run().Wait();
            }
            catch (AggregateException ex)
            {
                foreach (var e in ex.InnerExceptions)
                {
                    Console.WriteLine("ERROR: " + e.Message);
                }
            }
            Console.WriteLine("Press any key to continue...");
            Console.ReadKey();
        }

        private async Task Run()
        {
            string[] scopes = new string[] { Google.Apis.AndroidPublisher.v3.AndroidPublisherService.Scope.Androidpublisher }; // view and manage your Google Analytics data
            var keyFilePath = @"c:\file.p12";    // Downloaded from https://console.developers.google.com
            var serviceAccountEmail = "google-play-api@thailand-fls-app.iam.gserviceaccount.com";  // found https://console.developers.google.com

            //loading the Key file
            var certificate = new X509Certificate2(keyFilePath, "notasecret", X509KeyStorageFlags.Exportable);
            var credential = new ServiceAccountCredential(new ServiceAccountCredential.Initializer(serviceAccountEmail)
            {
                Scopes = scopes
            }.FromCertificate(certificate));

            var service = new Google.Apis.AndroidPublisher.v3.AndroidPublisherService(
                new BaseClientService.Initializer
                {
                    HttpClientInitializer = credential,
                    ApplicationName = "test app"
                });

            var edits = service.Edits;
            // Create a new edit to make changes.
            var packageName = "com.meddbtech.thailandflsapp";
            var appEdit = new Google.Apis.AndroidPublisher.v3.Data.AppEdit();
            var editRequest = edits.Insert(appEdit, packageName);
            appEdit = await editRequest.ExecuteAsync();

            // Run the request.
            Console.WriteLine("Executing a list request...");
            var response = edits.Apks.List(packageName, appEdit.Id).Execute();
            // Display the results.
            var latestApk = response.Apks.OrderByDescending(x => x.VersionCode).First();
            Console.WriteLine($"latestVersion {latestApk.VersionCode}");
        }
    }
}
