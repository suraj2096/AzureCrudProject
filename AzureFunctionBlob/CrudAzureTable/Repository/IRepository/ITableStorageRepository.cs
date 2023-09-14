using Azure;
using CrudAzureTable.Models;

namespace CrudAzureTable.Repository.IRepository
{
    public interface ITableStorageRepository<T>
    {
        public Task<ICollection<FileData>> GetAllEntityAsync(int id);
        Task<FileData> GetEntityAsync(string partitonKeys, string rowKeys);

        Task<bool> CreateEntityAsync(FileData entity);
        Task<bool> UpsertEntityAsync(FileData entity);
        Task<bool> DeleteEntityAsync(string partitonKeys, string rowKeys);

    }
}
