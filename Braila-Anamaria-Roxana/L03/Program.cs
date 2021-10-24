using Google.Apis.Auth.OAuth2;
using Google.Apis.Drive.v3;
using Google.Apis.Services;
using Google.Apis.Util.Store;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mime;
using System.Threading;
using System.Threading.Tasks;

namespace L03
{
    class Program
    {
        private static DriveService service;
        private static string token;
        static void Main(string[] args)
        {
            GetAccessToken();
            UploadFile();
        }
        static void GetAccessToken()
        {
            var clientId = "369380080835-hf4mn9i2d4dvmb3uc1o3h86kevnf0jtn.apps.googleusercontent.com";
            var clientSecret = "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";

            var clientSecrets = new ClientSecrets
            {
                ClientId = clientId,
                ClientSecret = clientSecret
            };

            string[] scopes = new string[]
            {
                DriveService.Scope.Drive,
                DriveService.Scope.DriveFile
            };

            var credential = GoogleWebAuthorizationBroker.AuthorizeAsync(
            clientSecrets,
            scopes,
            Environment.UserName,
            CancellationToken.None,
            new FileDataStore("Daimto.GoogleDrive.Auth.Store")
            ).Result;

            service = new DriveService(new Google.Apis.Services.BaseClientService.Initializer()
            { HttpClientInitializer = credential });
            token = credential.Token.AccessToken;
            Console.Write("Token: " + token);

            GetAllFiles();
        }

        static void GetAllFiles()
        {

            var request = (HttpWebRequest)WebRequest.Create("https://www.googleapis.com/drive/v3/files?q='root'%20in%20parents");
            request.Headers.Add(HttpRequestHeader.Authorization, "Bearer " + token);

            using (var response = request.GetResponse())
            {
                using (Stream data = response.GetResponseStream())
                using (var reader = new StreamReader(data))
                {
                    string text = reader.ReadToEnd();
                    var myData = JObject.Parse(text);
                    foreach (var file in myData["files"])
                    {
                        if (file["mimeType"].ToString() != "application/vnd.google-apps.folder")
                        {
                            Console.WriteLine("File name: " + file["name"]);
                        }
                    }
                }
            }
        }
        static void UploadFile()
        {
            using var stream = new MemoryStream();
            using var writer = new StreamWriter(stream);
            writer.WriteLine("Token: ya29.a0ARrdaM9VK_x3V4dbvM7_6WwKNdybjvQ9JZKhBVZTesgBSckgeJBBc-t3ig8PrwbtyOl_zKeQv86zBTsMmZ-a6a6GeuFbs9wEduYCwQm2Jp2bpIL7cUXuoA15PlUCB7JsY64Qe02KzP7w_dywtCvyu_wr-Zbo");
            writer.Flush();
            stream.Position = 0;

            var file = new Google.Apis.Drive.v3.Data.File();
            file.Name = "Token.txt";
            file.Description = "The file contains a TOKEN";
            file.MimeType = MediaTypeNames.Text.Plain;
            file.Parents = new string[] { "root" };
            var fileCreateRequest = service.Files.Create(file, stream, MediaTypeNames.Text.Plain);
            fileCreateRequest.Upload();
        }
    }
}
