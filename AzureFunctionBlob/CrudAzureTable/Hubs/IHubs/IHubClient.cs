using CrudAzureTable.Models;

namespace CrudAzureTable.Hubs.IHubs
{
    public interface IHubClient
    {
        public Task SendMessage(ICollection<FileData> datas);
    }
}
