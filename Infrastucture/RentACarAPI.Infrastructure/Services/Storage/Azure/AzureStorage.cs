using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using RentACarAPI.Application.Abstractions.Storage.Azure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RentACarAPI.Infrastructure.Services.Storage.Azure
{
    public class AzureStorage : Storage, IAzureStorage
    {
        readonly BlobServiceClient _blobServiceClient;
        BlobContainerClient _blobContainerClient;

        public AzureStorage(IConfiguration configuration)
        {
            _blobServiceClient = new BlobServiceClient(configuration["Storage:Azure"]);   
        }
        public async Task DeleteAsync(string pathOrContainer, string fileName)
        {
            _blobContainerClient = _blobServiceClient.GetBlobContainerClient(pathOrContainer);
            BlobClient blobClient = _blobContainerClient.GetBlobClient(fileName);
            await blobClient.DeleteAsync();
        }

        public List<string> GetFiles(string pathOrContainer)
        {
            _blobContainerClient = _blobServiceClient.GetBlobContainerClient(pathOrContainer);
           return _blobContainerClient.GetBlobs().Select(x=> x.Name).ToList();
           
        }

        public bool HasFile(string pathOrContainer, string fileName)
        {
            _blobContainerClient = _blobServiceClient.GetBlobContainerClient(pathOrContainer);
            return _blobContainerClient.GetBlobs().Any(x => x.Name == fileName);
        }

        public async Task<List<(string fileName, string path)>> UploadAsync(string pathOrContainer, IFormFileCollection files)
        {
            _blobContainerClient =  _blobServiceClient.GetBlobContainerClient(pathOrContainer);
            await _blobContainerClient.CreateIfNotExistsAsync();
            await _blobContainerClient.SetAccessPolicyAsync(PublicAccessType.BlobContainer);

            List<(string fileName, string path)> datas = new();
            foreach (var item in files)
            {
               string fileNewName = await FileRenameAsync(pathOrContainer,item.Name,HasFile);
               BlobClient blobClient =   _blobContainerClient.GetBlobClient(fileNewName);
               await blobClient.UploadAsync(item.OpenReadStream());
               datas.Add((fileNewName, pathOrContainer));
                

            }
            return datas;
        }
    }
}
