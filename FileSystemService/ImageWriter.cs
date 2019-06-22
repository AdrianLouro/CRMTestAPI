using System;
using System.IO;
using System.Threading.Tasks;
using FileSystemService.Contracts;
using Microsoft.AspNetCore.Http;
using static System.IO.Directory;
using static System.IO.FileMode;
using static System.IO.Path;

namespace FileSystemService
{
    public class ImageWriter : IImageWriter
    {
        private string _relativePath;

        public ImageWriter(string relativePath)
        {
            _relativePath = relativePath;
        }

        public async Task<string> Write(IFormFile file)
        {
            return await WriteFile(file);
        }

        private async Task<string> WriteFile(IFormFile file)
        {
            string fileName = Guid.NewGuid() + GetExtension(file.FileName);

            try
            {
                using (var stream = new FileStream(
                        Combine(GetCurrentDirectory(), _relativePath, fileName),
                        Create
                    )
                )
                {
                    await file.CopyToAsync(stream);
                }
            }
            catch (Exception ex)
            {
                return null;
            }

            return fileName;
        }
    }
}