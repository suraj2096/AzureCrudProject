using CrudAzureTable.Hubs.IHubs;
using CrudAzureTable.Models;
using CrudAzureTable.Repository.IRepository;
using Microsoft.AspNetCore.SignalR;
using Newtonsoft.Json;

namespace CrudAzureTable.Hubs
{
    public class ListenHub:Hub<IHubClient>
    {
        public async Task SendMessage(ICollection<FileData> datas)
        {
           
           await Clients.All.SendMessage(datas);
        }



    }
}
