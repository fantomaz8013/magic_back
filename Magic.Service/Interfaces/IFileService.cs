using Magic.Common.Models.Response;
using Magic.Domain.Entities;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace Magic.Service.Interfaces
{
    public interface IFileService
    {
        Task<string> UploadFile(IFormFile file);
        bool DeleteFile(string path);
    }
}
