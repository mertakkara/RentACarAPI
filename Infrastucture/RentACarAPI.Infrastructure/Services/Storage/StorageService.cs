﻿using Microsoft.AspNetCore.Http;
using RentACarAPI.Application.Abstractions.Storage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RentACarAPI.Infrastructure.Services.Storage
{
    public class StorageService : IStorageService
    {
        readonly IStorage _storage;

        public StorageService(IStorage storage)
        {
            _storage = storage;
        }

        public string StorageName { get => _storage.GetType().Name; }

        public async Task DeleteAsync(string pathOrContainer, string fileName)
        => await _storage.DeleteAsync(pathOrContainer, fileName);

        public  List<string> GetFiles(string pathOrContainer)
        =>  _storage.GetFiles(pathOrContainer);

        public bool HasFile(string pathOrContainer, string fileName)
        => _storage.HasFile(pathOrContainer, fileName); 

        public Task<List<(string fileName, string path)>> UploadAsync(string pathOrContainer, IFormFileCollection files)
        => _storage.UploadAsync(pathOrContainer, files);
    }
}
