using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Table;
using Models;


namespace L04
{
    public class StudentsRepository : IStudentRepository
    {
        private CloudTableClient _tableClient;

        private CloudTable _studentsTable;

        private string _connectionString = "DefaultEndpointsProtocol=https;"
            + "AccountName=azurestoragel4;"
            + "AccountKey=nlnYBvUdjFydTzfMteH2T9IrZqWAP3b5oRWskgJHOm9WjGmTadD6QdRM5Xa9zHA63Z8fPwqmOfFa6NnqPU4/zw==;"
            + "EndpointSuffix=core.windows.net";

        public StudentsRepository(IConfiguration configuration)
        {
            _connectionString = "DefaultEndpointsProtocol=https;"
            + "AccountName=azurestoragel4;"
            + "AccountKey=nlnYBvUdjFydTzfMteH2T9IrZqWAP3b5oRWskgJHOm9WjGmTadD6QdRM5Xa9zHA63Z8fPwqmOfFa6NnqPU4/zw==;"
            + "EndpointSuffix=core.windows.net";

            Task.Run(async () => {await InitializeTable();})
                .GetAwaiter()
                .GetResult();
        }

        public async Task CreateStudent(StudentEntity student)
        {
            var insertOperation = TableOperation.Insert(student);

            await _studentsTable.ExecuteAsync(insertOperation);
        }

        public async Task<List<StudentEntity>> GetAllStudents()
        {
            var students = new List<StudentEntity>();

            TableQuery<StudentEntity> query = new TableQuery<StudentEntity>();

            TableContinuationToken token = null;

            do
            {
                TableQuerySegment<StudentEntity> resultSegment = await _studentsTable.ExecuteQuerySegmentedAsync(query, token);
                token = resultSegment.ContinuationToken;

                students.AddRange(resultSegment.Results);
            }while (token != null);

            return students;

        }
        
        public async Task<StudentEntity> GetStudent(string partKey, string rowKey)
        {
            var students = new StudentEntity();

            TableQuery<StudentEntity> query = new TableQuery<StudentEntity>().Where(TableQuery.GenerateFilterCondition("PartitionKey", QueryComparisons.Equal, partKey)).Where(TableQuery.GenerateFilterCondition("RowKey", QueryComparisons.Equal, rowKey));

            TableContinuationToken token = null;

            do
            {
                TableQuerySegment<StudentEntity> resultSegment = await _studentsTable.ExecuteQuerySegmentedAsync(query, token);
                token = resultSegment.ContinuationToken;

                students = resultSegment.Results[0];
            }while (token != null);

            return students;

        }

        public async Task Modify(string partKey, string rowKey, StudentEntity student)
        {
            TableQuery<StudentEntity> query = new TableQuery<StudentEntity>().Where(TableQuery.GenerateFilterCondition("PartitionKey", QueryComparisons.Equal, partKey)).Where(TableQuery.GenerateFilterCondition("RowKey", QueryComparisons.Equal, rowKey));

            TableContinuationToken token = null;

            do
            {
                TableQuerySegment<StudentEntity> resultSegment = await _studentsTable.ExecuteQuerySegmentedAsync(query, token);
                token = resultSegment.ContinuationToken;

                var del = TableOperation.InsertOrReplace(student);
                await _studentsTable.ExecuteAsync(del);
            }while (token != null);
        }
        
        public async Task Delete(string partKey, string rowKey)
        {
            TableQuery<StudentEntity> query = new TableQuery<StudentEntity>().Where(TableQuery.GenerateFilterCondition("PartitionKey", QueryComparisons.Equal, partKey)).Where(TableQuery.GenerateFilterCondition("RowKey", QueryComparisons.Equal, rowKey));

            TableContinuationToken token = null;

            do
            {
                TableQuerySegment<StudentEntity> resultSegment = await _studentsTable.ExecuteQuerySegmentedAsync(query, token);
                token = resultSegment.ContinuationToken;
                
                var del = TableOperation.Delete(resultSegment.Results[0]);
                await _studentsTable.ExecuteAsync(del);
            }while (token != null);
        }

        private async Task InitializeTable()
        {
            _connectionString = "DefaultEndpointsProtocol=https;"
            + "AccountName=azurestoragel4;"
            + "AccountKey=nlnYBvUdjFydTzfMteH2T9IrZqWAP3b5oRWskgJHOm9WjGmTadD6QdRM5Xa9zHA63Z8fPwqmOfFa6NnqPU4/zw==;"
            + "EndpointSuffix=core.windows.net";
            
            var account = CloudStorageAccount.Parse(_connectionString);
            _tableClient = account.CreateCloudTableClient();

            _studentsTable = _tableClient.GetTableReference("studenti");

            await _studentsTable.CreateIfNotExistsAsync();
        }
    }

}