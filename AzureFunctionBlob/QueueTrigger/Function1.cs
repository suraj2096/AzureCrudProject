using System;
using System.Net.Http;
using System.Net.Http.Json;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using QueueTrigger.Model;

namespace QueueTrigger
{
    public class Function1
    {
        [FunctionName("Function1")]
        public static void  Run([QueueTrigger("messagequeue", Connection = "AzureWebJobsStorage")]string myQueueItem, ILogger log)
        {
            var data = myQueueItem;
            var finalData = JsonConvert.DeserializeObject<FileData>(myQueueItem);
            sendDataToTable(finalData);
        }

        private static bool sendDataToTable(FileData queueData)
        {
            // now we will call owr web api to send the data to the table.
            HttpClient httpClient = new HttpClient();
            using var content = new StringContent(JsonConvert.SerializeObject(queueData),Encoding.UTF8,"application/json");
            HttpResponseMessage response = httpClient.PostAsync(Environment.GetEnvironmentVariable("WebApi"), content).Result;
            var dataStatus = response.RequestMessage;
               

            return true;
        }
    }
}
