namespace L06;

public class StudentEntity
{
    public string PartitionKey { get; set; }
    public string RowKey { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Faculty { get; set; }
    public int YearOfStudy { get; set; }
}
