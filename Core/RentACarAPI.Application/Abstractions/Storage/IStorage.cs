using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RentACarAPI.Application.Abstractions.Storage
{
    public interface IStorage
    {
        Task<List<(string fileName, string path)>> UploadAsync(string pathOrContainer, IFormFileCollection files);
        Task DeleteAsync(string pathOrContainer, string fileName);
        List<string> GetFiles(string pathOrContainer);
        bool HasFile(string pathOrContainer,string fileName);
    }
}
