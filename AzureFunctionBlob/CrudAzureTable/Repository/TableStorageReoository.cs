using Azure;
using Azure.Data.Tables;
using Azure.Data.Tables.Models;
using CrudAzureTable.Hubs;
using CrudAzureTable.Hubs.IHubs;
using CrudAzureTable.Models;
using CrudAzureTable.Repository.IRepository;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Azure;
using Newtonsoft.Json;
using System.Collections.Concurrent;
using System.Text;
using System.Text.Unicode;

namespace CrudAzureTable.Repository
{
    public class TableStorageReoository<T> : ITableStorageRepository<T> where T : class,ITableEntity,new ()
    {

    
    
        private readonly string TableName;
        private readonly string StorageConnectionString;


        private readonly IHubContext<ListenHub,IHubClient> _iHubContext;
       

        public TableStorageReoository(IConfiguration configuration,IHubContext<ListenHub,IHubClient> iHubContext)
        {
            StorageConnectionString = configuration["StorageConnectionString"];
            TableName = configuration["TableName"];
            _iHubContext = iHubContext;
        }


        /// <summary>
        /// this function will get the table uri in the azure and also check table already exists in the database or not if not then it will create the table.
        /// </summary>
        /// <returns></returns>




        private async Task<TableClient> GetTableClient()
        {
            try
            {
            var getServiceClient = new TableServiceClient(StorageConnectionString);
            var tableClient = getServiceClient.GetTableClient(TableName);
            await tableClient.CreateIfNotExistsAsync();
                return tableClient ;
            }
            catch(Exception ex)
            {
                return null;
            }
           
        }

        /// <summary>
        /// this function will create the entity in the table.
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>


       
        public async Task<bool> CreateEntityAsync(FileData entity)
        {
            entity.PartitionKey = entity.ImageExtension;
            var tableClient = await GetTableClient(); 
            try
            {
                await tableClient.AddEntityAsync<FileData>(entity);
                return true;
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex);
                return false;
            }
        }
        /// <summary>
        /// this function will delete the table entity
        /// </summary>
        /// <param name="partitonKeys"></param>
        /// <param name="rowKeys"></param>
        /// <returns></returns>

        public async Task<bool> DeleteEntityAsync(string partitonKeys, string rowKeys)
        {
            var getEntity =GetEntityAsync(partitonKeys, rowKeys).Result;
            if (getEntity == null) return false;

            var tableClient = await GetTableClient();
            var removeData = tableClient.DeleteEntity(partitonKeys, rowKeys);
            
            if(removeData.Status!=204) return false;
            // we will call the azure function to delete 
            HttpClient httpClient = new HttpClient();
            HttpResponseMessage response = httpClient.DeleteAsync($"http://localhost:7266/api/DeleteImage/{getEntity.ImageName+'.'+getEntity.ImageExtension}").Result;
            if (response.IsSuccessStatusCode)
            {
                return true;
            }
            return false;
        }

        /// <summary>
        /// this function will get all the table entity.
        /// </summary>
        /// <returns></returns>


        public async Task<ICollection<FileData>> GetAllEntityAsync(int id)

        {
            if (id == 0) return null;
            ICollection<FileData> getAllData = new List<FileData>();

            var tableClient = await GetTableClient();

            var celebs = tableClient.QueryAsync<FileData>(filter: m=>m.UserId == id);


            await foreach (var fileDatas in celebs)
            {
                getAllData.Add(fileDatas);
            }
            return getAllData;
        }

        /// <summary>
        /// this function will get the specific entity in the table.
        /// </summary>
        /// <param name="partitonKeys"></param>
        /// <param name="rowKeys"></param>
        /// <returns></returns>


        public async Task<FileData> GetEntityAsync(string partitonKeys, string rowKeys)
        {
            var tableClient = await GetTableClient();
            var data = tableClient.GetEntityIfExists<FileData>(partitionKey:partitonKeys,rowKey:rowKeys);
            if (data == null) return null;
            return data.Value;
        }

        /// <summary>
        /// this function will update the entity in the table
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>



        public async Task<bool> UpsertEntityAsync(FileData entity)
        {
            if (entity == null) return false;
            var tableClient = await GetTableClient();
            var updateData = tableClient.UpdateEntity<FileData>(entity,entity.ETag);

            if (updateData.Status == 204)
            {
                var getData = await GetAllEntityAsync(entity.UserId);
                await _iHubContext.Clients.All.SendMessage(getData);
                return true;
            }
       
            else return false;
        }
    }
}
