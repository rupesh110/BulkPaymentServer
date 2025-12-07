using System.IO;
using System.Threading.Tasks;

namespace BulkPaymentServer.Application.Interfaces;

public interface IFileStorageService
{
    Task<string> UploadFileAsync(string userId, Stream fileStream, string fileName);

}