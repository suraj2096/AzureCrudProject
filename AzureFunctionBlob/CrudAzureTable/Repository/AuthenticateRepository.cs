using Azure.Data.Tables;
using CrudAzureTable.Models;
using CrudAzureTable.Repository.IRepository;

namespace CrudAzureTable.Repository
{
    public class AuthenticateRepository : IAuthenticateRepository
    {
        private readonly string TableName;
        private readonly string StorageConnectionString;

        public AuthenticateRepository(IConfiguration configuration)
        {
            StorageConnectionString = configuration["StorageConnectionString"];
            TableName = configuration["TableName2"];
        }
        public UserCeredentials? authenticateUser(string userName,string password)
        {
            try
            {
                var getServiceClient = new TableServiceClient(StorageConnectionString);
                var tableClient = getServiceClient.GetTableClient(TableName);
                // check enter details correct or not
                var checkUser = tableClient.Query<UserCeredentials>(m=>m.UserName == userName && m.Password == password);
                 if(checkUser.FirstOrDefault() != null) {
                    return checkUser.FirstOrDefault();
                }
                else
                {
                    return null;
                }

               
            }
            catch (Exception ex)
            {
                return null;
            }

        }
    }
}
