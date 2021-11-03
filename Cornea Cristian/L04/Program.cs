using System;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Table;
using Models;

namespace L04
{
    class Program
    {
        private static CloudTableClient tableClient;

        private static CloudTable studentsTable;

        static void Main(string[] args)
        {
            Task.Run(async () => { await Initialize(); })
            .GetAwaiter()
            .GetResult();
        }

        static async Task Initialize()
        {
            string storageConnectionString = "DefaultEndpointsProtocol=https;AccountName=tema4cornea;AccountKey=eBPbPNhE+pOnBrNuypRsTH7WPp5e9QLs10SSuj4uGjSYjz9G58VzWx1MpOqZBZSZbC7W7Ay6EfEG8QAm+89qFg==;EndpointSuffix=core.windows.net";
            var account = CloudStorageAccount.Parse(storageConnectionString);
            tableClient = account.CreateCloudTableClient();

            studentsTable = tableClient.GetTableReference("studenti");

            await studentsTable.CreateIfNotExistsAsync();

            await AddNewStudent();
            await GetAllStudents();
        }

        private static async Task GetAllStudents()
        {
            Console.WriteLine("Universitate\tCNP\tNume\tEmail\tNumar telefon\tAn");
            TableQuery<StudentEntity> query = new TableQuery<StudentEntity>();

            TableContinuationToken token = null;
            do
            {
                TableQuerySegment<StudentEntity> resultSegment = await studentsTable.ExecuteQuerySegmentedAsync(query, token);
                token = resultSegment.ContinuationToken;

                foreach (StudentEntity entity in resultSegment.Results)
                {
                    Console.WriteLine("{0}\t{1}\t{2} {3}\t{4}\t{5}\t{6}", entity.PartitionKey, entity.RowKey, entity.FirstName, entity.LastName,
                    entity.Email, entity.PhoneNumber, entity.Year);
                }
            } while (token != null);
        }

        private static async Task AddNewStudent()
        {
            var student = new StudentEntity("UPT", "1990215350085");

            student.FirstName = "Cristian";
            student.LastName = "Cornea";
            student.Email = "cristi.1990.cristi@gmail.com";
            student.Year = 4;
            student.PhoneNumber = "0711070122";
            student.Faculty = "AC";

            var insertOperation = TableOperation.Insert(student);

            await studentsTable.ExecuteAsync(insertOperation);
        }
    }
}
