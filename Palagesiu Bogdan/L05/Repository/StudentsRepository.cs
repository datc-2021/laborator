using L05.Models;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Table;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace L05.Repository
{
    public class StudentsRepository:IStudentsRepository
    {
        private CloudTableClient _tableClient;
        private CloudTable _studentsTable;
        private string _connectionString;

        private async Task InitializeTable()
        {
            var account = CloudStorageAccount.Parse(_connectionString);
            _tableClient = account.CreateCloudTableClient();
            _studentsTable = _tableClient.GetTableReference("studenti");
            await _studentsTable.CreateIfNotExistsAsync();
        }

        public StudentsRepository()
        {
            _connectionString = "DefaultEndpointsProtocol=https;AccountName=tema4pala;AccountKey=3VTefCba//yX6EJVvFEc8TdA55sx7OAVWgPXq5gAKt89gqW4NVxE/qeV3NJkPcdt/NddAjjbDbqjG8BOC1NpKA==;EndpointSuffix=core.windows.net";
            Task.Run(async () => { await InitializeTable(); })
                .GetAwaiter()
                .GetResult();
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
                students.AddRange(resultSegment);
            } while (token != null);
            return students;
        }
    }
}
