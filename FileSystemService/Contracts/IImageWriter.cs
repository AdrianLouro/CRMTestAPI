using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace FileSystemService.Contracts
{
    public interface IImageWriter
    {
        Task<string> Write(IFormFile file);
    }
}