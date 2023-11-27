using Core.Enums;

using Microsoft.AspNetCore.Http;

namespace Pal.Services.FileManager
{
    public interface IFileManagerService
    {

        public Task<string> UploadFileAsync(IFormFile file,
            FileReferenceType referenceType,
            bool generateRandomFileName = false,
            string referenceNo = "0", string filename = null);


        public Task DeleteFileAsync(string url);
    }
}