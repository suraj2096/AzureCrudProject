using Azure;
using Azure.Data.Tables;

namespace CrudAzureTable.Models
{
    public class FileData:ITableEntity
    {
        public string? ImageName { get; set; }
        public string ImageExtension { get; set; }
        public DateTime? FileCreated { get; set; }
        public string PartitionKey { get; set; }
        public string RowKey { get; set; }
        public int UserId { get; set; }
        public DateTimeOffset? Timestamp { get; set; }
        
        public ETag ETag { get; set; }



        public FileData() {
            this.RowKey = Guid.NewGuid().ToString();
        }
    }
}
