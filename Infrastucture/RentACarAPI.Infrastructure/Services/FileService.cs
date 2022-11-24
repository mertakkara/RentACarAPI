using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using RentACarAPI.Application.Services;
using RentACarAPI.Infrastructure.Operations;

namespace RentACarAPI.Infrastructure.Services
{
    public class FileService : IFileService
    {
        public readonly IWebHostEnvironment webHostEnvironment;
        public FileService(IWebHostEnvironment webHostEnvironment)
        {
            this.webHostEnvironment = webHostEnvironment;   
        }

        async Task<string> FileRenameAsync(string path, string fileName, bool first = true)
        {
            try
            {
                string newFileName = await Task.Run<string>(async () =>
                {
                    string extension = Path.GetExtension(fileName);

                    string newFileName = string.Empty;


                    if (first)
                    {
                        string oldName = Path.GetFileNameWithoutExtension(fileName);
                        newFileName = $"{NameOperation.CharacterRegulatory(oldName)}{extension}";

                    }
                    else
                    {
                        newFileName = fileName;
                        int index = newFileName.IndexOf("-");
                        if (index == -1)
                        {
                            newFileName = $"{Path.GetFileNameWithoutExtension(fileName)}-2{extension}";
                        }
                        else
                        {
                            int lastIndex = 0;
                            while (true)
                            {
                                lastIndex = index;
                                index = newFileName.IndexOf("-", index + 1);
                                if (index == -1)
                                {
                                    index = lastIndex;
                                    break;
                                }
                            }



                            int indexNo = newFileName.IndexOf(".");
                            string fileNo = newFileName.Substring(index + 1, indexNo - index - 1);
                            if (int.TryParse(fileNo, out int _fileNO))
                            {
                                _fileNO++;
                                newFileName = newFileName.Remove(index + 1, indexNo - index - 1).Insert(index + 1, _fileNO.ToString());

                            }
                            else
                            {
                                newFileName = $"{Path.GetFileNameWithoutExtension(fileName)}-2{extension}";
                            }

                        }
                    }




                    if (File.Exists($"{path}\\{newFileName}"))
                        return await FileRenameAsync(path, newFileName, false);
                    else
                        return newFileName;

                });
                return newFileName;
            }
            catch (Exception ex)
            {
                return null;
            }
            
        }

        public async Task<bool> SaveFileAsync(string path, IFormFile file)
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

        public async Task<List<(string filename, string path)>> UploadAsync(string path, IFormFileCollection files)
        {
            
            string uploadPath = Path.Combine(webHostEnvironment.WebRootPath, "resource/car-images"); //wwwroot/resource/car-images

            if (!Directory.Exists(uploadPath))
                Directory.CreateDirectory(uploadPath);

            List<(string filename, string path)> datas = new();

            List<bool> results = new List<bool>();
            foreach (IFormFile item in files)
            {
                string fileName =  await FileRenameAsync(uploadPath, item.FileName);
                bool state =  await SaveFileAsync($"{uploadPath}\\{fileName}",item);
                datas.Add((fileName, $"{path}\\{fileName}"));
                results.Add(state);
                
            }
            if(results.TrueForAll(x=> x.Equals(true)))
            {
                return datas;
            }
            return null;
        }
    }
}
