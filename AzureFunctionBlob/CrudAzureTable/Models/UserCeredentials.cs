using Azure;
using Azure.Data.Tables;

namespace CrudAzureTable.Models
{
    public class UserCeredentials:ITableEntity
    {
        public string? UserName { get; set;}
        public string? Password { get; set;}
        public string PartitionKey { get; set; }
        public string RowKey { get; set; }
        public DateTimeOffset? Timestamp { get; set; }
        public ETag ETag { get; set; }
    }
}
