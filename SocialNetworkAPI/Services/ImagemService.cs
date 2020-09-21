using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Azure;
using Microsoft.Extensions.Configuration;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using SocialNetworkBLL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SocialNetworkAPI.Services
{
    public class ImagemService
    {
        private readonly IConfiguration _configuration;

        public ImagemService(IConfiguration configuration)
        {
            _configuration = configuration;            
        }

        public ImagemService(IFormFile files, Perfil perfil)
        {
            ImagemPerfil(files, perfil);
        }

        public ImagemService(IFormFile files, Post post)
        {
            ImagemPost(files,post);
        }

        public async Task<String> ImagemPerfil(IFormFile files, Perfil perfilLogado)
        {
            string dir = perfilLogado.PerfilId.ToString() + "/";
            string systemFileName = dir + files.FileName;
            string blobstorageconnection = _configuration.GetValue<string>("blobstorage");


            // Retrieve storage account from connection string.
            CloudStorageAccount cloudStorageAccount = CloudStorageAccount.Parse(blobstorageconnection);

            // Create the blob client.
            CloudBlobClient blobClient = cloudStorageAccount.CreateCloudBlobClient();

            // Retrieve a reference to a container.
            CloudBlobContainer container = blobClient.GetContainerReference("imagens");

            container.GetDirectoryReference(dir);

            // This also does not make a service call; it only creates a local object.
            CloudBlockBlob blockBlob = container.GetBlockBlobReference(systemFileName);
            await using (var data = files.OpenReadStream())
            {
                await blockBlob.UploadFromStreamAsync(data);                
            }

            var blobUri = new Uri(container.Uri +
                          systemFileName
                          ).ToString();

            return (blobUri);
        }

        public async Task<String> ImagemPost(IFormFile files, Post post)
        {            
            string systemFileName =  files.FileName;
            string blobstorageconnection = _configuration.GetValue<string>("blobstorage");


            // Retrieve storage account from connection string.
            CloudStorageAccount cloudStorageAccount = CloudStorageAccount.Parse(blobstorageconnection);

            // Create the blob client.
            CloudBlobClient blobClient = cloudStorageAccount.CreateCloudBlobClient();

            // Retrieve a reference to a container.
            CloudBlobContainer container = blobClient.GetContainerReference("imagens");

            container.GetDirectoryReference("imagens");

            // This also does not make a service call; it only creates a local object.
            CloudBlockBlob blockBlob = container.GetBlockBlobReference(systemFileName);
            await using (var data = files.OpenReadStream())
            {
                await blockBlob.UploadFromStreamAsync(data);
            }

            var blobUri = new Uri(container.Uri +
                          systemFileName
                          ).ToString();

            return (blobUri);
        }

    }
}
