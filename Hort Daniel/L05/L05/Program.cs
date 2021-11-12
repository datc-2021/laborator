using Azure.Data.Tables;
using L05;

var settings = new AzureStorageAccountSettings();

var studentClient = new TableClient(new Uri(settings.StorageUri), "Students",
    new TableSharedKeyCredential(settings.AccountName, settings.StorageAccountKey));
var metricsClient = new TableClient(new Uri(settings.StorageUri), "Metrics",
    new TableSharedKeyCredential(settings.AccountName, settings.StorageAccountKey));
await metricsClient.CreateIfNotExistsAsync();

var students = await studentClient.QueryAsync<StudentEntity>().AsEnumerable();

var tasks = from university in students.Select(a => a.PartitionKey).Distinct()
            let count = students.Where(a => a.PartitionKey == university).Count()
            let metric = new MetricEntity()
            {
                PartitionKey = university,
                RowKey = DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:ss"),
                Count = count
            }
            select metricsClient.AddEntityAsync(metric);

await Task.WhenAll(tasks.Append(metricsClient.AddEntityAsync(new MetricEntity()
{
    PartitionKey = "General",
    RowKey = DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:ss"),
    Count = students.Count()
})));
