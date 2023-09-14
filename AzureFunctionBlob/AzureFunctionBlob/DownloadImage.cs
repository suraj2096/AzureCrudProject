using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Azure.Storage.Blobs;
using System.Web.Http;
using System.Net.Security;

namespace AzureFunctionBlob
{
    public static class DownloadImage
    {
        /* [FunctionName("DownloadImage")]
         public static async Task<Stream> Run(
             [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "download/{fileName}")] HttpRequest req,string fileName,
             ILogger log)
         {

             string connection = Environment.GetEnvironmentVariable("AzureWebJobsStorage");
             string containerName = Environment.GetEnvironmentVariable("ContainerName");

             var container = new BlobContainerClient(connection,containerName);
             if (await container.ExistsAsync())
             {
                 var blobClient = container.GetBlobClient(fileName) ;
                 if (blobClient.Exists())
                 {

                     var content = await blobClient.DownloadStreamingAsync();
                     return content.Value.Content;

                 }
                 else
                 {
                     throw new FileNotFoundException();
                 }
             }
             else
             {
                 throw new FileNotFoundException();
             }
         }



         [FunctionName("DeleteImage")]
         public static async Task<IActionResult> Runs(
             [HttpTrigger(AuthorizationLevel.Anonymous, "delete", Route = "DeleteImage/{fileNameBlob}")] HttpRequest req,string fileNameBlob,
             ILogger log)
         {
             string connection = Environment.GetEnvironmentVariable("AzureWebJobsStorage");
             string containerName = Environment.GetEnvironmentVariable("ContainerName");

             var container = new BlobContainerClient(connection, containerName);
             if (!await container.ExistsAsync())
             {
                 return new InternalServerErrorResult();
             }

             var blobClient = container.GetBlobClient(fileNameBlob);

             if (await blobClient.ExistsAsync())
             {
                 await blobClient.DeleteIfExistsAsync();
                 return new OkResult();
             }
             else
             {
                 return new InternalServerErrorResult();
             }
         }
     }*/
    }
}
