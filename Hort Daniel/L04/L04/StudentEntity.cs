using Azure;
using Azure.Data.Tables;

namespace L04;

public class StudentEntity : ITableEntity
{
    public string PartitionKey { get; set; }
    public string RowKey { get; set; }
    public DateTimeOffset? Timestamp { get; set; }
    public ETag ETag { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Faculty { get; set; }
    public int YearOfStudy { get; set; }
}
