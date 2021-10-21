using System.Net;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Drive.v3;
using Google.Apis.Drive.v3.Data;
using Google.Apis.Services;
using Google.Apis.Util.Store;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace DriveQuickstart
{
    class Program
    {
        private static DriveService _service;
        private static string _token;
        static void Main(string[] args)
        {
            Initialize();

            Console.WriteLine("File upload response" + UploadFile(System.IO.File.Open("C:\\Users\\Adr\\OneDrive\\DATC\\L03\\random.txt",FileMode.Open),"random.txt","text/plain"));
        }

        static void Initialize()
        {
            string[] scopes = new string[]{
                DriveService.Scope.Drive,
                DriveService.Scope.DriveFile
            };

            var clientId = "47506398974-q081i9od0n6m98iii3652dbp5tho3e8o.apps.googleusercontent.com";
            var clientSectret = "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";

            var credential = GoogleWebAuthorizationBroker.AuthorizeAsync(
                new ClientSecrets
                {
                    ClientId = clientId,
                    ClientSecret = clientSectret
                },
                scopes,
                Environment.UserName,
                CancellationToken.None,

                new FileDataStore("Daimto.GoogleDrive.Auth.Store3")
            ).Result;

            _service = new DriveService(new Google.Apis.Services.BaseClientService.Initializer()
            {
                HttpClientInitializer = credential
            });

            _token = credential.Token.AccessToken;

            Console.Write("Token: " + credential.Token.AccessToken);

            GetMyFiles();
        }

        static void GetMyFiles()
        {
            var request = (HttpWebRequest)WebRequest.Create("https://www.googleapis.com/drive/v3/files?q='root'%20in%20parents");
            request.Headers.Add(HttpRequestHeader.Authorization, "Bearer " + _token);

            using(var response = request.GetResponse())
            {
                using(Stream data = response.GetResponseStream())
                using(var reader = new StreamReader(data))
                {
                    string text = reader.ReadToEnd();
                    var myData = JObject.Parse(text);
                    foreach(var file in myData["files"])
                    {
                        if(file["mimeType"].ToString() != "application/vnd.google-apps.folder")
                        {
                            Console.WriteLine("File name: " + file["name"]);
                        }
                    }

                }
            }
        }
        static string UploadFile(Stream file, string fileName, string fileMime)
        {
            
            var driveFile = new Google.Apis.Drive.v3.Data.File();
            driveFile.Name = fileName;
           // driveFile.Description = fileDescription;
            driveFile.MimeType = fileMime;
            //driveFile.Parents = new string[] { folder };
            
            var request = _service.Files.Create(driveFile, file, fileMime);
            request.Fields = "id";
            
            var response = request.Upload();
            if (response.Status != Google.Apis.Upload.UploadStatus.Completed)
                throw response.Exception;
            
            return request.ResponseBody.Id;
		}

    }
}