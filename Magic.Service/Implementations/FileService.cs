using Magic.Common;
using Magic.Common.Models.Response;
using Magic.Common.Options;
using Magic.DAL;
using Magic.Domain.Entities;
using Magic.Domain.Enums;
using Magic.Service.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace Magic.Service
{
    public class FileService : IFileService
    {
        public bool DeleteFile(string path)
        {
            string fileName = Path.Combine(Environment.CurrentDirectory, path.Replace("/storage/", ""));
            if (File.Exists(fileName))
            {
                File.Delete(fileName);
                return true;
            }
            return false;
        }

        public async Task<string> UploadFile(IFormFile file)
        {
            string path = "";
            if (file.Length > 0)
            {
                path = Path.GetFullPath(Path.Combine(Environment.CurrentDirectory, "storage/images"));
                var newName = Guid.NewGuid().ToString() + System.IO.Path.GetExtension(file.FileName);
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }
                using (var fileStream = new FileStream(Path.Combine(path, newName), FileMode.Create))
                {
                    await file.CopyToAsync(fileStream);
                }
                
                return "/storage/images/" + newName;
            }
            else
            {
                throw new Exception("Ошибка загрузки файла");
            }
        }
    }
}
