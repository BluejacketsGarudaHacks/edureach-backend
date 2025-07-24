using System;
using System.IO;

namespace Backend.Shared.Utils
{
    public class ImageUtil
    {
        public ImageUtil() { }

        public string GenerateImageFileName(string originalFileName)
        {
            var extension = Path.GetExtension(originalFileName);
            var uniqueName = $"{Guid.NewGuid()}{extension}";
            return uniqueName;
        }

        public string SaveImage(byte[] imageBytes, string originalFileName)
        {
            var fileName = GenerateImageFileName(originalFileName);
            var imagesFolder = Path.Combine(Directory.GetCurrentDirectory(), "Images");
            if (!Directory.Exists(imagesFolder))
            {
                Directory.CreateDirectory(imagesFolder);
            }
            var filePath = Path.Combine(imagesFolder, fileName);
            File.WriteAllBytes(filePath, imageBytes);

            filePath = "/" + Path.Combine("Images", fileName).Replace("\\", "/");
            return filePath;
        }

        public string GetImagePath(string imageFileName)
        {
            var imagesFolder = Path.Combine(Directory.GetCurrentDirectory(), "Images");
            return Path.Combine(imagesFolder, imageFileName);
        }
    }
}

