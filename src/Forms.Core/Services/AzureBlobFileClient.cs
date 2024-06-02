using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Forms.Core.Services
{
    public class AzureBlobFileClient : IFileClient
    {
        private readonly CloudBlobClient _blobClient;

        public AzureBlobFileClient(string azureConectionString)
        {
            var account = CloudStorageAccount.Parse(azureConectionString);
            _blobClient = account.CreateCloudBlobClient();
        }

        public async Task<bool> DeleteFile(string storeName, string filePath)
        {
            var container = _blobClient.GetContainerReference(storeName);
            var blob = container.GetBlockBlobReference(filePath.ToLower());

            return await blob.DeleteIfExistsAsync();
        }

        public async Task<bool> FileExists(string storeName, string filePath)
        {
            var container = _blobClient.GetContainerReference(storeName);
            var blob = container.GetBlockBlobReference(filePath.ToLower());

            return await blob.ExistsAsync();
        }

        public async Task<Stream> GetFileStream(string storeName, string filePath)
        {
            var container = _blobClient.GetContainerReference(storeName);
            var blob = container.GetBlockBlobReference(filePath.ToLower());

            var mem = new MemoryStream();
            await blob.DownloadToStreamAsync(mem);
            mem.Seek(0, SeekOrigin.Begin);

            return mem;
        }

        public async Task<string> GetFileArray(string storeName, string filePath)
        {
            var container = _blobClient.GetContainerReference(storeName);
            var blob = container.GetBlockBlobReference(filePath.ToLower());

            string base64Data;

            using (var memoryStream = new MemoryStream())
            {
                await blob.DownloadToStreamAsync(memoryStream);
                var bytes = memoryStream.ToArray();
                var b64String = Convert.ToBase64String(bytes);
                base64Data = "data:image/png;base64," + b64String;
            }
            return base64Data;
        }

        public async Task<string> GetFileUrl(string storeName, string filePath)
        {
            var container = _blobClient.GetContainerReference(storeName);
            var blob = container.GetBlockBlobReference(filePath.ToLower());
            string url = null;

            if (await blob.ExistsAsync())
            {
                url = blob.Uri.AbsoluteUri;
            }

            return url;
        }

        public async Task<bool> SaveFile(string storeName, string filePath, Stream fileStream)
        {
            var container = _blobClient.GetContainerReference(storeName);
            var blob = container.GetBlockBlobReference(filePath.ToLower());

            await blob.UploadFromStreamAsync(fileStream);

            return true;
        }

        public bool IsImage(string filePath)
        {
            string[] formats = new string[] { ".jpg", ".png", ".gif", ".jpeg" };
            return formats.Any(item => filePath.EndsWith(item, StringComparison.OrdinalIgnoreCase));
        }

        public bool IsPdfFile(string filePath)
        {
            string[] formats = new string[] { ".pdf" };
            return formats.Any(item => filePath.EndsWith(item, StringComparison.OrdinalIgnoreCase));
        }
    }
}
