using System;
using System.IO;
using System.Linq;
using System.Security.Authentication;
using System.Threading.Tasks;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Azure.Storage.Blobs.Specialized;
using MongoDB.Bson;
using MongoDB.Driver;

namespace TryingAzure
{
    static class Program
    {
        private static string _blobStorageConnectionString = "";

        // private static readonly string _cosmosConnectionString = "mongodb://localhost:27017";

        static async Task Main(string[] args)
        {
            // var sourceBlobServiceClient = new BlobServiceClient(SourceStorageConnectionString);
            // var targetBlobServiceClient = new BlobServiceClient(TargetStorageConnectionString);
            //
            // var sourceContainer = sourceBlobServiceClient.GetBlobContainerClient("media-db");
            // var targetContainer = targetBlobServiceClient.GetBlobContainerClient("media-db");
            //
            // var blobs = sourceContainer.GetBlobsAsync();

            // await foreach (var blob in blobs)
            // {
            //     Console.WriteLine(blob.Name);
            //
            //     var sourceBlobClient = sourceContainer.GetBlobClient(blob.Name);
            //     var response = await sourceBlobClient.DownloadStreamingAsync();
            //     var data = response.Value.Content;
            //
            //     var targetBlobClient = targetContainer.GetBlobClient(blob.Name);
            //     await targetBlobClient.UploadAsync(data, true);
            // }

            /*var sourceSettings = MongoClientSettings.FromUrl(
                new MongoUrl(SourceCosmosConnectionString)
            );

            var targetSettings = MongoClientSettings.FromUrl(
                new MongoUrl(TargetCosmosConnectionString)
            );

            // settings.SslSettings = new SslSettings
            // {
            //     EnabledSslProtocols = SslProtocols.Tls12
            // };

            var sourceClient = new MongoClient(sourceSettings);
            var targetClient = new MongoClient(targetSettings);

            var sourceDb = sourceClient.GetDatabase("protocolDb");
            var targetDb = targetClient.GetDatabase("protocolDb");

            var sourceProtocolsCollection = sourceDb.GetCollection<ProtocolModel>("protocols");
            var targetProtocolsCollection = targetDb.GetCollection<ProtocolModel>("protocols");

            var cursor = await sourceProtocolsCollection.FindAsync(FilterDefinition<ProtocolModel>.Empty);

            var protocols = cursor.ToEnumerable();

            foreach (var newProtocol in protocols.Select(protocol => protocol with { Id = null }))
            {
                // Console.WriteLine(newProtocol.ProtocolId);
                await targetProtocolsCollection.InsertOneAsync(newProtocol);
            }*/
        }

        private static async Task Delete()
        {
            const string fileName = "1.png";

            var client = new BlobServiceClient(_blobStorageConnectionString);

            var containerClient = client.GetBlobContainerClient("test");

            var blobClient = containerClient.GetBlobClient(fileName);

            var response = await blobClient.DeleteIfExistsAsync(DeleteSnapshotsOption.IncludeSnapshots);

            if (response.Value is false)
            {
                Console.WriteLine("file does not exist");
            }
        }

        private static async Task Upload()
        {
            var home = Environment.GetEnvironmentVariable("HOME");

            if (home is null)
            {
                return;
            }

            const string fileName = "1.png";

            var image = Path.Combine(home, "Pictures", fileName);

            var client = new BlobServiceClient(_blobStorageConnectionString);

            var containerClient = client.GetBlobContainerClient("test");

            var blobClient = containerClient.GetBlobClient(Path.Combine("pic", fileName));

            Console.WriteLine($"Uploading to Blob storage as blob:\n{blobClient.Uri}\n");

            // Upload data from the local file
            await blobClient.UploadAsync(image, true);
        }

        private static void SetCacheHeader()
        {
            // var connectionString = "";

            var client = new BlobServiceClient(_blobStorageConnectionString);

            var containerClient = client.GetBlobContainerClient("");

            // var prop = blobClient.GetProperties();
            // Console.WriteLine(prop.Value.ContentType);
            // Console.WriteLine(prop.Value.Metadata);
            // prop.Value.GetHashCode();

            // var properties = new BlobHttpHeaders { CacheControl = "max-age=3600", ContentType = prop.Value.ContentType };

            // var response = blobClient.SetHttpHeaders(properties);

            foreach (BlobItem item in containerClient.GetBlobs())
            {
                if (item.Name.Contains("mp3") || item.Name.Contains("mp4"))
                {
                    var blobCLient = containerClient.GetBlobClient(item.Name);
                    var prop = blobCLient.GetProperties();
                    var properties = new BlobHttpHeaders
                        { CacheControl = "max-age=3600", ContentType = prop.Value.ContentType };
                    blobCLient.SetHttpHeaders(properties);

                    // Console.WriteLine("\t" + item.Name);
                }
            }
        }

        private static void SetServiceVersion()
        {
            // var connectionString = "";

            var client = new BlobServiceClient(_blobStorageConnectionString);

            // var properties = new BlobServiceProperties();

            var response = client.GetProperties();
            var properties = response.Value;
            properties.DefaultServiceVersion = "2020-04-08";

            var response2 = client.SetProperties(properties);

            // Console.WriteLine(response2.Status);
            // Console.WriteLine(response.Value.DefaultServiceVersion);

            // Console.WriteLine(response.Status);
            // Console.WriteLine(response.ToString());

            // old code reference

            // var credentials = new StorageCredentials("myaccountname", "mysecretkey");
            // var account = new CloudStorageAccount(credentials, true);
            // var client = account.CreateCloudBlobClient();

            // var properties = client.GetServiceProperties();
            // properties.DefaultServiceVersion = "2012-02-12";
            // client.SetServiceProperties(properties);
        }
    }
}