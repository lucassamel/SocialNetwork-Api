using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Azure.Storage.Files.DataLake;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using SocialNetworkBLL.Models;
using SocialNetworkDLL;

namespace SocialNetworkAPI.Controllers
{   
    //[Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class ImgController : ControllerBase
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SocialNetworkContext _context;
        private readonly IConfiguration _configuration;


        public ImgController(IConfiguration configuration,
            UserManager<IdentityUser> userManager,
            SocialNetworkContext context)
        {
            _configuration = configuration;
            _userManager = userManager;
            _context = context;
        }

        [HttpPost]
        public async Task<IActionResult> Create(IFormFile files)
        {
            var account = await _userManager.GetUserAsync(this.User);
            var perfilLogado = await _context.Perfis
                .FirstAsync(p => p.Usuario.Email == account.Email);

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
            return Ok();
        }

        [HttpGet]
        public async Task<IActionResult> ShowAllBlobs()
        {
            string blobstorageconnection =
           _configuration.GetValue<string>("blobstorage");
            CloudStorageAccount cloudStorageAccount =
           CloudStorageAccount.Parse(blobstorageconnection);
            // Create the blob client.
            CloudBlobClient blobClient = cloudStorageAccount.CreateCloudBlobClient();
            CloudBlobContainer container =
           blobClient.GetContainerReference("imagens");
            CloudBlobDirectory dirb =
           container.GetDirectoryReference("imagens");
            BlobResultSegment resultSegment = await
           container.ListBlobsSegmentedAsync(string.Empty,
            true, BlobListingDetails.Metadata, 100, null, null, null);
            List<FileData> fileList = new List<FileData>();
            foreach (var blobItem in resultSegment.Results)
            {
                // A flat listing operation returns only blobs, not virtual directories.

                var blob = (CloudBlob)blobItem;
                fileList.Add(new FileData()
                {
                    FileName = blob.Name,
                    FileSize = Math.Round((blob.Properties.Length / 1024f) / 1024f,
               2).ToString(),
                    ModifiedOn =
               DateTime.Parse(blob.Properties.LastModified.ToString()).ToLocalTime().ToString(
               )
                });
            }
            return Ok();
        }

        // [HttpDelete("")]
        // public async Task<IActionResult> Delete(string blobName)
        // {
        //     string blobstorageconnection = _configuration.GetValue<string>("blobstorage");
        //     CloudStorageAccount cloudStorageAccount = CloudStorageAccount.Parse(blobstorageconnection);
        //     CloudBlobClient cloudBlobClient = cloudStorageAccount.CreateCloudBlobClient();
        //     string strContainerName = "imagens";
        //     CloudBlobContainer cloudBlobContainer = cloudBlobClient.GetContainerReference(strContainerName);
        //     var blob = cloudBlobContainer.GetBlobReference(blobName);
        //     await blob.DeleteIfExistsAsync();
        //     return RedirectToAction("Galeria", "Demo");
        // }


    }
}
