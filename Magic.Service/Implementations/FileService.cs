using Magic.Service.Interfaces;
using Microsoft.AspNetCore.Http;

namespace Magic.Service;

public class FileService : IFileService
{
    public bool DeleteFile(string path)
    {
        var fileName = Path.Combine(Environment.CurrentDirectory, path.Replace("/", "\\"));
        if (!File.Exists(fileName)) return false;

        File.Delete(fileName);

        return true;
    }

    public async Task<string> UploadFile(IFormFile file)
    {
        if (file.Length <= 0) throw new Exception("Ошибка загрузки файла");

        var path = Path.GetFullPath(Path.Combine(Environment.CurrentDirectory, "storage/images"));
        var newName = Guid.NewGuid() + Path.GetExtension(file.FileName);
        if (!Directory.Exists(path))
        {
            Directory.CreateDirectory(path);
        }

        await using (var fileStream = new FileStream(Path.Combine(path, newName), FileMode.Create))
        {
            await file.CopyToAsync(fileStream);
        }
                
        return "storage/images/" + newName;

    }
}