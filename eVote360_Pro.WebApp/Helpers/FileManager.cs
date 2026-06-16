using System;
using System.IO;
using System.Linq;
using Microsoft.AspNetCore.Http;

namespace eVote360_Pro.WebApp.Helpers
{
    public static class FileManager
    {
        private static readonly string[] _allowedExtensions = { ".jpg", ".jpeg", ".png" };

        public static string? Upload(IFormFile? file, Guid id, string folderName, bool isEditMode = false, string? imagePath = "")
        {
            if (isEditMode && file == null)
                return imagePath;

            if (file == null)
                return string.Empty;

            var extension = Path.GetExtension(file.FileName).ToLowerInvariant();
            if (!_allowedExtensions.Contains(extension))
                return string.Empty;

            string basePath = $"Images/{folderName}/{id}";
            string absolutePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", basePath);

            if (!Directory.Exists(absolutePath))
                Directory.CreateDirectory(absolutePath);

            string fileName = $"{Guid.NewGuid()}{extension}";
            string fullFilePath = Path.Combine(absolutePath, fileName);

            using (var stream = new FileStream(fullFilePath, FileMode.Create))
            {
                file.CopyTo(stream);
            }

            if (isEditMode && !string.IsNullOrWhiteSpace(imagePath))
            {
                string[] parts = imagePath.Split('/');
                string oldFileName = parts[^1];
                string oldFullPath = Path.Combine(absolutePath, oldFileName);

                if (File.Exists(oldFullPath))
                    File.Delete(oldFullPath);
            }

            return $"{basePath}/{fileName}";
        }

        public static bool Delete(Guid id, string folderName)
        {
            string path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Images", folderName, id.ToString());

            if (Directory.Exists(path))
            {
                Directory.Delete(path, true);
                return true;
            }

            return false;
        }
    }
}
