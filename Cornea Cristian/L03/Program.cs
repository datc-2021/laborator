using System;
using System.IO;
using System.Net;
using System.Text;
using System.Threading;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Drive.v3;
using Google.Apis.Util.Store;
using Newtonsoft.Json.Linq;

namespace L03
{
    class Program
    {
        private static string _token;

        static void Main()
        {
            Initialize();
        }

        static void Initialize()
        {
            string[] scopes = new string[]{
                DriveService.Scope.Drive,
                DriveService.Scope.DriveFile
            };
            var clientId = "1046461755374-0jeatkchhepp5kf3t922c30jv09lg0rp.apps.googleusercontent.com";
            var clientSecret = "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";
            var credential = GoogleWebAuthorizationBroker.AuthorizeAsync(
                new ClientSecrets
                {
                    ClientId = clientId,
                    ClientSecret = clientSecret
                },
                scopes,
                Environment.UserName,
                CancellationToken.None,
                new FileDataStore("Dainto.GoogleDrive.Auth.Store")
            ).Result;
            _token = credential.Token.AccessToken;
            Console.WriteLine(_token);
            GetAllFiles();
            UploadTxtFile();
        }

        static void GetAllFiles()
        {
            var request = (HttpWebRequest)WebRequest.Create("https://www.googleapis.com/drive/v3/files?q='root'%20in%20parents");
            request.Headers.Add(HttpRequestHeader.Authorization, "Bearer " + _token);
            using var response = request.GetResponse();
            using Stream data = response.GetResponseStream();
            using var reader = new StreamReader(data);
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
        static void UploadTxtFile()
        {
            string path;
            byte[] text;
            Console.Write("Dati calea fisierului txt pe care doriti sa il incarcati pe drive: ");
            path = Console.ReadLine();
            text = Encoding.ASCII.GetBytes("--not_so_random_boundary\nContent-Type: application/json; charset = utf-8\nContent-Disposition: form-data; name=\"metadata\"\n\n{\"name\":\""+Path.GetFileName(path)+"\",\"mimeType\":\"text/plain\"}\n--not_so_random_boundary\nContent-Type: text/plain\nContent-Disposition: form-data; name=\"file\"\n\n"+ File.ReadAllText(path) + "\n--not_so_random_boundary--");
            var request = (HttpWebRequest)WebRequest.Create("https://www.googleapis.com/upload/drive/v3/files?uploadType=multipart");
            request.Method = WebRequestMethods.Http.Post;
            request.Headers.Add(HttpRequestHeader.Authorization, "Bearer " + _token);
            request.Headers.Add(HttpRequestHeader.ContentType, "multipart/related; boundary=not_so_random_boundary");
            request.Headers.Add(HttpRequestHeader.ContentLength, text.Length.ToString());
            Stream body = request.GetRequestStream();
            body.Write(text, 0, text.Length);
            using var response = request.GetResponse();
            using Stream data = response.GetResponseStream();
            using var reader = new StreamReader(data);
            string info = reader.ReadToEnd();
            var myData = JObject.Parse(info);
            Console.WriteLine(myData);
        }
    }
}
