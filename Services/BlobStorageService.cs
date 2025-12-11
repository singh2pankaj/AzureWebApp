using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;

namespace YourProject.Services
{
    public class BlobStorageService
    {
        private readonly string _connectionString;
        private readonly string _containerName;

        public BlobStorageService(IConfiguration config)
        {
            _connectionString = config["AzureStorage:ConnectionString"];
            _containerName = config["AzureStorage:ContainerName"];
        }

        private BlobContainerClient GetContainerClient()
        {
            var container = new BlobContainerClient(_connectionString, _containerName);
            container.CreateIfNotExists();
            return container;
        }

        public async Task UploadFileAsync(IFormFile file)
        {
            var container = GetContainerClient();
            var blob = container.GetBlobClient(file.FileName);
            await blob.UploadAsync(file.OpenReadStream(), overwrite: true);
        }

        public async Task<List<string>> ListFilesAsync()
        {
            var container = GetContainerClient();
            var list = new List<string>();

            await foreach (BlobItem item in container.GetBlobsAsync())
            {
                list.Add(item.Name);
            }

            return list;
        }

        public async Task<Stream> DownloadAsync(string fileName)
        {
            var container = GetContainerClient();
            var blob = container.GetBlobClient(fileName);

            var ms = new MemoryStream();
            await blob.DownloadToAsync(ms);
            ms.Position = 0;
            return ms;
        }

        public async Task DeleteAsync(string fileName)
        {
            var container = GetContainerClient();
            var blob = container.GetBlobClient(fileName);
            await blob.DeleteIfExistsAsync();
        }
    }
}