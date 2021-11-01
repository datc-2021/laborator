using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Table;
using Models;

namespace L04
{
    public class Program
    {
        
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });


                
        // private static CloudTableClient tableClient;
        // private static CloudTable studentsTable;
        // public static void Main(string[] args)
        // {
        //      Task.Run(async () => { await Initialize(); })
        //          .GetAwaiter()
        //          .GetResult();


        // }


        // static async Task Initialize()
        // {
        //     string storageConnectionString = "DefaultEndpointsProtocol=https;"
        //     + "AccountName=azurestoragel4;"
        //     + "AccountKey=nlnYBvUdjFydTzfMteH2T9IrZqWAP3b5oRWskgJHOm9WjGmTadD6QdRM5Xa9zHA63Z8fPwqmOfFa6NnqPU4/zw==;"
        //     + "EndpointSuffix=core.windows.net";

        //     var account = CloudStorageAccount.Parse(storageConnectionString);
        //     tableClient = account.CreateCloudTableClient();

        //     studentsTable = tableClient.GetTableReference("studenti");

        //     await studentsTable.CreateIfNotExistsAsync();

        //     //await AddNewStudent();
        //     await GetAllStudents();
        // }

        // private static async Task AddNewStudent()
        // {
        //     var student = new StudentEntity("UVT", "1234567890");
        //     student.FirstName = "Gigi";
        //     student.LastName = "Becali";
        //     student.Email = "oita.nebuna@fcsb.ro";
        //     student.Year = -1;
        //     student.PhoneNumber = "0767777777";
        //     student.Faculty = "AC";

        //     var insertOperation = TableOperation.Insert(student);

        //     await studentsTable.ExecuteAsync(insertOperation);
        // }

        // private static async Task GetAllStudents()
        // {
        //     Console.WriteLine("UNIVERSITATE\tCNP\tNUME\tPRENUME\tNUMAR TELEFON\tAN");
        //     TableQuery<StudentEntity> query = new TableQuery<StudentEntity>();

        //     TableContinuationToken token = null;
        //     do
        //     {
        //         TableQuerySegment<StudentEntity> resultSegment = await studentsTable.ExecuteQuerySegmentedAsync(query, token);
        //         token = resultSegment.ContinuationToken;

        //         foreach(StudentEntity entity in resultSegment.Results)
        //             Console.WriteLine("{0}\t{1}\t{2} {3}\t{4}\t{5}", entity.PartitionKey, entity.RowKey, entity.FirstName, entity.LastName, entity.PhoneNumber, entity.Year);
                
        //     }while(token != null);
        // }
    }
}
