
using BulkPaymentServer.Application.DTOs;
namespace BulkPaymentServer.Application.Interfaces;


public interface IUploadService
{
    Task<UploadResult> UploadFileAsync(string userId, Stream fileStream, string fileName);
}