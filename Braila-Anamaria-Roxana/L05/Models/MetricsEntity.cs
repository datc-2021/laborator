using System;
using Microsoft.WindowsAzure.Storage.Table;

namespace Models
{
    public class MetricsEntity : TableEntity
    {
        public MetricsEntity(string university, int count)
        {
            this.PartitionKey = university;
            this.RowKey = DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:ss");
            this.Count = count;
        }
        public MetricsEntity()
        {

        }
        public int Count { get; set; }
    }
}