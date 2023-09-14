using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Microsoft.WindowsAzure.Storage.Blob;
using Microsoft.WindowsAzure.Storage;
using Microsoft.Extensions.Configuration;
using Azure.Storage.Blobs;
using Azure;
using System.Web.Http;
using AzureFunctionBlob.Models;
using Microsoft.WindowsAzure.Storage.Queue;
using Microsoft.WindowsAzure.Storage.Queue.Protocol;
using Azure.Storage.Queues;
using System.Text;
using System.Linq;
using Azure.Core;
using System.Net.Mime;

namespace AzureFunctionBlob
{
    public static class BlobHttpTriggerFunc
    {
        [FunctionName("FileUpload")]
        public static async Task<IActionResult> Runss(
           [HttpTrigger(AuthorizationLevel.Anonymous ,"post", Route = null)] HttpRequest req, ILogger log)
        {
            
            req.Headers.TryGetValue("userId", out var userId);
            int id = Convert.ToInt32(userId);
            string connection = Environment.GetEnvironmentVariable("AzureWebJobsStorage");
            string containerName = Environment.GetEnvironmentVariable("ContainerName");

            // we will create container or check container 

            var createOrCheckContainer = await CreateContainerIfNotExists(log, containerName, connection);
            if (createOrCheckContainer == null)
            {
                return new InternalServerErrorResult();
            }




            // now we will send the file to the container 

            var file = req.Form.Files[0];
            if (file == null) return new InternalServerErrorResult();

            using var fileDataRead = file.OpenReadStream();

            var blobClient = new BlobContainerClient(connection, createOrCheckContainer);
            var blob = blobClient.GetBlobClient(file.FileName);
            try
            {
                var fileUploadStatus = await blob.UploadAsync(fileDataRead);

            }
            catch (Exception ex)
            {
                return new ObjectResult(ex.Message);
            }


            // now we will create a queue to send the detail to of the image.
            //int id = 0;

            var confDataSendToQueue = await sendDataToQueue(file.FileName,id, connection, Environment.GetEnvironmentVariable("queuename"));
            if (confDataSendToQueue)
            {
                return new OkObjectResult("Your file is uploaded successfully");
            }
            else
            {
                return new InternalServerErrorResult();
            }





        }

        /// <summary>
        /// here we will create a container .....
        /// </summary>
        /// <param name="logger"></param>
        /// <param name="containerName"></param>
        /// <param name="connection"></param>
        /// <returns></returns>
        private static async Task<string> CreateContainerIfNotExists(ILogger logger, string containerName, string connection)
        {

            BlobServiceClient blobServiceClient = new BlobServiceClient(connection);

            try
            {
                // first we will check the container that we create is available or not.
                var getAlreadyCreateContainer = blobServiceClient.GetBlobContainerClient(containerName);
                bool containerExist = await getAlreadyCreateContainer.ExistsAsync();
                if (containerExist)
                {
                    return containerName;
                }


                // here we will create container if the specified name container not available.
                var createContainer = await blobServiceClient.CreateBlobContainerAsync(containerName);
                return createContainer.Value.Name;

            }
            catch (Exception ex) {

                logger.LogInformation(ex.ToString());
                return null;
            }



        }


        // here is the function that will send message to the queue
        private static async Task<bool> sendDataToQueue(string imageDetail,int id, string connection, string queueName)
        {

            // here we will split the strin in two parts.
            string[] strname = imageDetail.Split('.');

            // here we will create the information that will be store in the queue.
            var fileData = new FileData()
            {
                ImageExtension = strname[1],
                ImageName = strname[0],
                FileCreated = DateTime.Now.ToUniversalTime(),
                UserId = id
            };

            // now we will create a queue.
            QueueServiceClient queueServiceClient = new QueueServiceClient(connection);
            try
            {
                // first we will check the queue present of the specified name or not
                // here we will add a message to the queue.
                QueueClient queueClient = new QueueClient(connection, queueName);
                queueClient.CreateIfNotExists();
                var bytes = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(fileData).ToString());
                await queueClient.SendMessageAsync(Convert.ToBase64String(bytes));
                return true;

            }
            catch (Exception ex) {
                return false;
            }


        }



        [FunctionName("DownloadImage")]
        public static async Task<Stream> Run(
           [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "DownloadImage/{fileName}")] HttpRequest req, string fileName,
           ILogger log)
        {
         
           
            string connection = Environment.GetEnvironmentVariable("AzureWebJobsStorage");
            string containerName = Environment.GetEnvironmentVariable("ContainerName");

            var container = new BlobContainerClient(connection, containerName);
            if (await container.ExistsAsync())
            {
                var blobClient = container.GetBlobClient(fileName);
                if (blobClient.Exists())
                {

                    // 1st way to download an image .......
                    /*
                    MemoryStream memoryStream= new MemoryStream();

                    await blobClient.DownloadToAsync(memoryStream);

                    memoryStream.Position = 0;
                   
                    return new FileStreamResult(memoryStream);
                    */

                   // 2nd way to download an image...........

                    var downloadFile = await blobClient.DownloadStreamingAsync();

                    return downloadFile.Value.Content;

                 


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
            [HttpTrigger(AuthorizationLevel.Anonymous, "delete", Route = "DeleteImage/{fileNameBlob}")] HttpRequest req, string fileNameBlob,
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
          

        
    }

















}
