using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;

using Core.Enums;

using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Hosting;
using Service.Helpers;

using Services.LoggerService;

namespace Pal.Services.FileManager
{

    public class FileManagerService : IFileManagerService
    {
        private readonly IWebHostEnvironment _environment;
        private readonly IConfiguration _configuration;
        private readonly ILoggerService<FileManagerService> _loggerService;
        private readonly AzureStorageConfig _azureStorageConfig;

        //--------------------------------------------------------------------------------------------
        public FileManagerService(IWebHostEnvironment environment,
            IConfiguration configuration,
            ILoggerService<FileManagerService> loggerService)
        {
            _environment = environment;
            _configuration = configuration;
            _azureStorageConfig = _configuration.GetSection("AppSettings:AzureStorage").Get<AzureStorageConfig>();
            _loggerService = loggerService;
        }

        //----------------------------------------------------------------------------------------------
        private async Task<string> SaveBlobAsync(IFormFile file, string fullPath, string storageContainer)
        {
            try
            {
                var container = new BlobContainerClient(_azureStorageConfig.ConnectionString, storageContainer);
                var createResponse = await container.CreateIfNotExistsAsync();
                if (createResponse != null && createResponse.GetRawResponse().Status == 201)
                    await container.SetAccessPolicyAsync(PublicAccessType.BlobContainer);
                var blob = container.GetBlobClient(fullPath);
                await blob.DeleteIfExistsAsync();

                var stream = new MemoryStream();
                //var stream = new FileStream(blob.Uri.OriginalString, FileMode.Create);


                await file.CopyToAsync(stream);
                stream.Position = 0;
                await blob.UploadAsync(stream, new BlobHttpHeaders
                {
                    //ContentType = contentType,
                    CacheControl = "max-age=31536000",
                });


                return blob.Uri.ToString();
            }
            catch (Exception ex)
            {
                _ = _loggerService.LogErrorAsync(nameof(SaveBlobAsync), ex);
                throw;
            }

        }


        //---------------------------------------------------------------------------------------------------
        /// <summary>
        /// 
        /// </summary>
        /// <param name="file">File You want yo save</param>
        /// <param name="folder">sub folder if any</param>
        /// <param name="generateRandomFileName">to generate random file name</param>
        /// <param name="mediaType">what ever</param>
        /// <returns></returns>
        public async Task<string> UploadFileAsync(IFormFile file,
             FileReferenceType referenceType,
            bool generateRandomFileName = false,
            string referenceNo = "0", string filename = null)
        {

            try
            {
                if (file == null)
                {
                    return null;
                }


                // file name
                string fileName = file.FileName;
                if (filename != null)
                    fileName = filename;
                else if (generateRandomFileName)
                    fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);

                var pathToSave = Path.Combine(referenceType.ToString(), referenceNo, fileName);

                var url = await SaveBlobAsync(file, pathToSave, _azureStorageConfig.ContainerName);

                return url;
            }
            catch (Exception ex)
            {
                _ = _loggerService.LogErrorAsync(nameof(UploadFileAsync), ex);
                return null;
            }
        }

        //---------------------------------------------------------------------------------------------------
        public async Task DeleteFileAsync(string url)
        {
            try
            {

                if (url != null)
                {
                    url = url.Trim().Replace("/", "\\");
                    var filepath = _environment.WebRootPath + url; //Path.Combine(_environment.WebRootPath,"s\\ssss");
                    await Task.Run(() =>
                    {
                        if (File.Exists(filepath))
                            File.Delete(filepath);

                    });
                }

            }
            catch (Exception ex)
            {
                _ = _loggerService.LogErrorAsync(nameof(DeleteFileAsync), ex);
            }
        }
        //---------------------------------------------------------------------------------------------------


    }
}
