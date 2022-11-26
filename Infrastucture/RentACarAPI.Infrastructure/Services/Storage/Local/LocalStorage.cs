using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using RentACarAPI.Application.Abstractions.Storage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RentACarAPI.Infrastructure.Services.Storage.Local
{
    public class LocalStorage :Storage, ILocalStorage
    {
        public readonly IWebHostEnvironment webHostEnvironment;
        public LocalStorage(IWebHostEnvironment webHostEnvironment)
        {
            this.webHostEnvironment = webHostEnvironment;
        }
        public async Task DeleteAsync(string pathOrContainer, string fileName)
        =>
            File.Delete($"{pathOrContainer}\\{fileName}");
        

        public List<string> GetFiles(string pathOrContainer)
        {
            DirectoryInfo directory = new(pathOrContainer);
            return directory.GetFiles().Select(f=>f.Name).ToList();   
        }

        public bool HasFile(string pathOrContainer, string fileName)
        => File.Exists($"{pathOrContainer}\\{fileName}");

        public async Task<List<(string fileName, string path)>> UploadAsync(string pathOrContainer, IFormFileCollection files)
        {
            string uploadPath = Path.Combine(webHostEnvironment.WebRootPath, pathOrContainer); //wwwroot/resource/car-images

            if (!Directory.Exists(uploadPath))
                Directory.CreateDirectory(uploadPath);

            List<(string filename, string path)> datas = new();

         
            foreach (IFormFile item in files)
            {
                string fileNewName = await FileRenameAsync(pathOrContainer, item.Name, HasFile);
                await SaveFileAsync($"{uploadPath}\\{fileNewName}", item);
                datas.Add((fileNewName, $"{pathOrContainer}\\{fileNewName}"));
            }
            return datas;
        }
        private async Task<bool> SaveFileAsync(string path, IFormFile file)
        {
            try
            {
                //Random r = new();
                //string fullPath = Path.Combine(path, $"{r.Next() }{Path.GetExtension(file.FileName)}");
                await using FileStream fileStream = new(path, FileMode.Create, FileAccess.Write, FileShare.None, 1024 * 1024, useAsync: false); // using Idisposable dean türetildiğinden dolayı dispose etmek istiyoruz onun için dispose ettiriyoruz.fonkisyon bulana kadar karadar dispose edilir. block ise block bitene kadar
                await file.CopyToAsync(fileStream);
                await fileStream.FlushAsync();
                return true;

            }
            catch (Exception e)
            {

                throw e;
            }
        }
    }
}
