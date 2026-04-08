using Azure.Storage.Blobs;
using System;
using System.IO;
using System.Threading.Tasks;

namespace ConsoleAppAzure
{
    internal class Program
    {
        static string str_connection = "DefaultEndpointsProtocol=https;AccountName=storageofnikhilkr;AccountKey=343FGgyuqBhNCP6KBf4D4J4J6874QN+3nU4rf5310ThU9PqDudk9DfcAe1Ui/zDcYNWZKWW6+YFQ+AStxtwQ+A==;EndpointSuffix=core.windows.net";
        // connection like this becasue when u try to commit it tells secret information is there so for security reason i cannot do commit

        static async Task Main()
        {
            BlobServiceClient blobServiceClient = new BlobServiceClient(str_connection);
            BlobContainerClient container_client =
            blobServiceClient.GetBlobContainerClient("data");
            BlobClient blob_client = container_client.GetBlobClient("millie.jpeg");
            using FileStream uploadFileStream = File.OpenRead(@"D:\NET\repos\60\ConsoleAppAzure\millie.jpeg");
            await blob_client.UploadAsync(uploadFileStream, true);
            uploadFileStream.Close();
            Console.WriteLine("File uploaded");
            Console.WriteLine("Operation complete");
        }
    }
}
