using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace IMandCRM.UI.HelperMethods
{
    public static class ImageUpload
    {
        public static async Task<string> Upload(IFormFile formFile, string url)
        {
            var extension = Path.GetExtension(formFile.FileName);
            var randomName = string.Format($"{DateTime.Now.Ticks}{extension}");
            var path = Path.Combine(Directory.GetCurrentDirectory(), url, randomName);
            if (System.IO.File.Exists(path))
            {
                System.IO.File.Delete(path);
            }
            using (var stream = new FileStream(path, FileMode.Create))
            {
                await formFile.CopyToAsync(stream);
            }
            return randomName;
        }
    }

    public static class FileUpload
    {
        public static async Task<string> Upload(IFormFile formFile, string url)
        {
            var path = Path.Combine(Directory.GetCurrentDirectory(), url, formFile.FileName);
            if (System.IO.File.Exists(path))
            {
                System.IO.File.Delete(path);
            }
            using (var stream = new FileStream(path, FileMode.Create))
            {
                await formFile.CopyToAsync(stream);
            }
            return formFile.FileName;
        }
    }
}
