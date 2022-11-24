

using Microsoft.AspNetCore.Http;

namespace RentACarAPI.Application.Services
{
    public interface IFileService
    {
        Task<List<(string filename,string path)>> UploadAsync(string path, IFormFileCollection files);
        Task<bool> SaveFileAsync(string path,IFormFile file);


    }
}
