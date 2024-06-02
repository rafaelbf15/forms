using System.IO;
using System.Threading.Tasks;

namespace Forms.Core.Services
{
    public interface IFileClient
    {
        Task<bool> DeleteFile(string storeName, string filePath);
        Task<bool> FileExists(string storeName, string filePath);
        Task<Stream> GetFileStream(string storeName, string filePath);
        Task<string> GetFileArray(string storeName, string filePath);
        Task<string> GetFileUrl(string storeName, string filePath);
        Task<bool> SaveFile(string storeName, string filePath, Stream fileStream);
        bool IsImage(string filePath);
        bool IsPdfFile(string filePath);

    }
}
