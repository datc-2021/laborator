using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Table;
using Models;

namespace L05
{
    class Program
    {
        static async Task Main(string[] args)
        {
            string storageConnectionString = "DefaultEndpointsProtocol=https;"
            + "AccountName=azurestorageroxie"
            + ";AccountKey=UJDHZBA32pLOOjw4WjLNkNcXKw2ds/pf8+cjbxfltTBJMVvwhLU6D0uKvtvM2vlt8T8vEBkDdtOMB408WTlDVQ=="
            + ";EndpointSuffix=core.windows.net";

            var account = CloudStorageAccount.Parse(storageConnectionString);
            var tableClient = account.CreateCloudTableClient();

            var studentsTable = tableClient.GetTableReference("studenti");
            var studentsMetricsTable = tableClient.GetTableReference("metriciStudenti");

            await studentsMetricsTable.CreateIfNotExistsAsync();

            var students = new List<StudentEntity>();

            TableQuery<StudentEntity> query = new TableQuery<StudentEntity>();
            TableContinuationToken token = null;
            do
            {
                TableQuerySegment<StudentEntity> resultSegment = await studentsTable.ExecuteQuerySegmentedAsync(query, token);
                token = resultSegment.ContinuationToken;

            } while (token != null);

            var metrics = from university in students.Select(a => a.PartitionKey).Distinct()
                          let count = students.Where(a => a.PartitionKey == university).Count()
                          select new MetricsEntity(university, count);
            var tasks = from metric in metrics
                        let operation = TableOperation.Insert(metric)
                        select studentsMetricsTable.ExecuteAsync(operation);
            await Task.WhenAll(tasks.Append(studentsMetricsTable.ExecuteAsync(TableOperation
                .Insert(new MetricsEntity("General", students.Count)))));
        }

    }
}
