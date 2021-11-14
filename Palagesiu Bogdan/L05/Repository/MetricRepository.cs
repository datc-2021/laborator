using L05.Models;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Table;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace L05.Repository
{
    class MetricRepository:IMetricRepository
    {
        private CloudTableClient _tableClient;
        private CloudTable _metricTable;
        private string _connectionString;
        private List<StudentEntity> students;
        private async Task InitializeTable()
        {
            var account = CloudStorageAccount.Parse(_connectionString);
            _tableClient = account.CreateCloudTableClient();
            _metricTable = _tableClient.GetTableReference("metric");
            await _metricTable.CreateIfNotExistsAsync();
        }
        private async Task InsertMetric(MetricEntity metric)
        {
            var insertOperation = TableOperation.Insert(metric);
            await _metricTable.ExecuteAsync(insertOperation);
        }
        public MetricRepository(List<StudentEntity> _students)
        {
            students = _students;
            _connectionString = "DefaultEndpointsProtocol=https;AccountName=laborator4danmircea;AccountKey=jnnBmoGcigMef3GK85zPFcnuDE7x5kC4KnzZqRYGzj1RvU6Tmk3QgxekDCR9aZDVilKFrVood2R/Tr1Zrg1jwQ==;EndpointSuffix=core.windows.net";
            Task.Run(async () => { await InitializeTable(); })
                .GetAwaiter()
                .GetResult();
        }
        public void GenerateMetric()
        {
            MetricEntity metric;
            int count;
            List<string> facultati = new List<string>();
            foreach (var student in students)
            {
                if (!facultati.Contains(student.PartitionKey))
                    facultati.Add(student.PartitionKey);
            }
            foreach (var facultate in facultati)
            {
                count = 0;
                foreach (var student in students)
                {
                    if (student.PartitionKey == facultate)
                        count++;
                }
                metric = new MetricEntity(facultate, DateTime.Now.ToLongDateString() + " " + DateTime.Now.ToLongTimeString());
                metric.Count = count;
                Task.Run(async () => { await InsertMetric(metric); })
                        .GetAwaiter()
                        .GetResult();
            }
            count = 0;
            foreach(var student in students)
            {
                count++;
            }
            metric = new MetricEntity("General", DateTime.Now.ToLongDateString() + " " + DateTime.Now.ToLongTimeString());
            metric.Count = count;
            Task.Run(async () => { await InsertMetric(metric); })
                    .GetAwaiter()
                    .GetResult();
        }
    }
}
