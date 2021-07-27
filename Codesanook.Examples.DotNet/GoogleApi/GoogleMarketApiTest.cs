using Google.Apis.AndroidPublisher.v3;
using Google.Apis.AndroidPublisher.v3.Data;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Services;
using System;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using Xunit;

namespace Codesanook.Examples.DotNet.GoogleApi
{
    /// <summary>
    /// This example uses the discovery API to list all APIs in the discovery repository.
    /// https://developers.google.com/discovery/v1/using.
    /// <summary>

    public class GoogleMarketApiTest
    {

        [Fact]
        public async Task TestGetReleaseVersion()
        {
            var scopes = new string[] { AndroidPublisherService.Scope.Androidpublisher }; // view and manage your Google Analytics data
                                                                                          // Download p12 file service account key from https://console.developers.google.com
            var keyFilePath = @"c:\file.p12";
            // found https://console.developers.google.com
            var serviceAccountEmail = "your-service-account-key";

            // Load the Key file
            var certificate = new X509Certificate2(keyFilePath, "notasecret", X509KeyStorageFlags.Exportable);
            var credential = new ServiceAccountCredential(
                new ServiceAccountCredential.Initializer(serviceAccountEmail) { Scopes = scopes }.FromCertificate(certificate)
            );

            var service = new AndroidPublisherService(
                new BaseClientService.Initializer { HttpClientInitializer = credential, ApplicationName = "test app" }
            );

            var edits = service.Edits;
            // Create a new edit to make changes.
            var packageName = "com.company.app-name";
            var appEdit = new AppEdit();
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
