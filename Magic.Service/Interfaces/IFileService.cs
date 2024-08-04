using Microsoft.AspNetCore.Http;

namespace Magic.Service.Interfaces;

public interface IFileService
{
    Task<string> UploadFile(IFormFile file);
    bool DeleteFile(string path);
}