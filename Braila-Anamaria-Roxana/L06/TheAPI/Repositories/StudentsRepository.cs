using System.Data;
using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.WindowsAzure.Storage.Table;
using Models;
using Repositories;
using System.Collections.Generic;
using Microsoft.WindowsAzure.Storage;
using System.Net;
using LanguageExt;
using Newtonsoft.Json;
using Azure.Storage.Queues;

namespace Repositories
{
    public class StudentsRepository : IStudentRepository
    {
        private CloudTableClient _tableClient;
        private CloudTable _studentsTable;
        private string _connectionString;
        public StudentsRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetValue(typeof(string), "AzureStorageAccountConnectionString").ToString();
            Task.Run(async () => { await InitializeTable(); })
                .GetAwaiter()
                .GetResult();
        }
        public async Task CreateStudent(StudentEntity student)
        {
            // var insertOperation = TableOperation.Insert(student);
            // var result = await _studentsTable.ExecuteAsync(insertOperation);
            // return (StudentEntity)result.Result;
            var jsonStudent = JsonConvert.SerializeObject(student);
            var plainTextBytes = System.Text.Encoding.UTF8.GetBytes(jsonStudent);
            var base64String = System.Convert.ToBase64String(plainTextBytes);

            QueueClient queueClient = new QueueClient(
                _connectionString,
                "students-q"
            );
            queueClient.CreateIfNotExists();
            await queueClient.SendMessageAsync(base64String);
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
            } while (token != null);
            return students;
        }
        public async Task<StudentEntity> FindStudent(string university, string cnp)
        {
            var partitionKey = university;
            var rowKey = cnp;

            var query = TableOperation.Retrieve<StudentEntity>(partitionKey, rowKey);
            var result = await _studentsTable.ExecuteAsync(query);
            return (StudentEntity)result.Result;
        }
        public TryAsync<StudentEntity> EditStudent(StudentEntity student) => async () =>
        {
            student.ETag = "*";

            var editOperation = TableOperation.Merge(student);
            var result = await _studentsTable.ExecuteAsync(editOperation);
            return (StudentEntity)result.Result;
        };
        public TryAsync<Unit> DeleteStudent(string university, string cnp) => async () =>
        {
            var deleteOperation = TableOperation.Delete(new TableEntity(university, cnp) { ETag = "*" });
            return await _studentsTable.ExecuteAsync(deleteOperation).ToUnit();
        };
        private async Task InitializeTable()
        {
            var account = CloudStorageAccount.Parse(_connectionString);
            _tableClient = account.CreateCloudTableClient();
            _studentsTable = _tableClient.GetTableReference("studenti");

            await _studentsTable.CreateIfNotExistsAsync();
        }
    }
}