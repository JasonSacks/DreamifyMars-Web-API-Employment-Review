using Azure.Storage.Blobs;
using DreamInMars.Configuration;
using Microsoft.Extensions.Options;
using System;
using System.IO;
using System.Threading.Tasks;

namespace DreamInMars.Logic
{
    public class AzureStorageLogic : IFileStorageLogic
    {
        private readonly AzureConfiguration _configuration;
        public AzureStorageLogic(IOptions<AzureConfiguration> config) =>
            _configuration = config.Value;

        public async Task<string> SaveImageAsync(Stream stream)
        {
            var service = new BlobServiceClient(_configuration.ConnectionString);
            BlobContainerClient container = service.GetBlobContainerClient(_configuration.ContainerName);
            var fileName = $"{Guid.NewGuid()}.jpg";
            BlobClient blobClient = container.GetBlobClient(fileName);
            await blobClient.UploadAsync(stream, true);
            return $@"{_configuration.FileUrl}/{fileName}";
        }
    }
}
