
using BulkPaymentServer.Application.DTOs;
namespace BulkPaymentServer.Application.Interfaces;


public interface IUploadService
{
    Task<UploadResultDto> UploadFileAsync(string userId, Stream fileStream, string fileName);
}