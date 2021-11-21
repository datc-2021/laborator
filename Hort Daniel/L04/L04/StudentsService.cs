using Azure;
using Azure.Data.Tables;
using Azure.Storage.Queues;
using Azure.Storage.Queues.Models;
using LanguageExt;
using Newtonsoft.Json;
using System.Text;

namespace L04;

public class StudentsService
{
    private readonly AzureStorageAccountSettings _settings;
    private readonly TableClient _client;
    private readonly QueueClient _queueClient;
    public StudentsService(AzureStorageAccountSettings settings)
    {
        _settings = settings;
        _client = new TableClient(new Uri(_settings.StorageUri), "Students",
            new TableSharedKeyCredential(_settings.AccountName, _settings.StorageAccountKey));
        _client.CreateIfNotExists();
        _queueClient = new QueueClient(_settings.ConnectionString, "datc6-queue", 
            new() { MessageEncoding = QueueMessageEncoding.Base64 });
    }

    public async Task<IEnumerable<StudentEntity>> GetAll()
        => await _client.QueryAsync<StudentEntity>().AsEnumerable();

    public TryAsync<StudentEntity> Get(string partitionKey, string rowKey)
        => async () => (await _client.GetEntityAsync<StudentEntity>(partitionKey, rowKey)).Value;

    public async Task<Response<SendReceipt>> Add(StudentEntity student)
        => await _queueClient.SendMessageAsync(JsonConvert.SerializeObject(student));
        //=> async () => await _client.AddEntityAsync(student).ToUnit();

    public TryAsync<Unit> Edit(StudentEntity student)
        => async () => await _client.UpdateEntityAsync(student, student.ETag).ToUnit();

    public TryAsync<Unit> Delete(string partitionKey, string rowKey)
        => async () => await _client.DeleteEntityAsync(partitionKey, rowKey).ToUnit();
}
