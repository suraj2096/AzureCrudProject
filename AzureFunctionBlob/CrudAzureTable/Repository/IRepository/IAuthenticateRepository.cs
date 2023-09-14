using CrudAzureTable.Models;

namespace CrudAzureTable.Repository.IRepository
{
    public interface IAuthenticateRepository
    {
        public UserCeredentials? authenticateUser(string userName,string password);

    }
}
