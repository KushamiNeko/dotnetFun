using System;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;

namespace TryingAzure
{
    class Program
    {
        private static readonly string _connectionString = "";

        static void Main(string[] args)
        {

        }

        private void SetCacheHeader()
        {

            // var connectionString = "";

            var client = new BlobServiceClient(_connectionString);

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
                    var properties = new BlobHttpHeaders { CacheControl = "max-age=3600", ContentType = prop.Value.ContentType };
                    blobCLient.SetHttpHeaders(properties);

                    // Console.WriteLine("\t" + item.Name);
                }
            }

        }

        private void SetServiceVersion()
        {

            // var connectionString = "";

            var client = new BlobServiceClient(_connectionString);

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
