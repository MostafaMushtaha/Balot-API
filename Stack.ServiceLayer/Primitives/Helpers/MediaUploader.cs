
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Options;
using Stack.Entities.DomainEntities.Auth;

namespace Stack.ServiceLayer.Primitives
{

    public class MediaUploader : IMediaUploader
    {

        private IHostingEnvironment _hostingEnvironment;

        private static string _apiUrl;

        public MediaUploader(IHostingEnvironment hostingEnvironment, IOptions<APIUrlModel> apiUrl)
        {
            _hostingEnvironment = hostingEnvironment;
            _apiUrl = apiUrl.Value.ApiUrl;
        }

        public async Task<string> UploadMedia(string filePath, IFormFile media)
        {
            var uploads = Path.Combine(_hostingEnvironment.WebRootPath, filePath);
            var fileName = Guid.NewGuid().ToString() + Path.GetExtension(media.FileName);

            using (var fileStream = new FileStream(Path.Combine(uploads, fileName), FileMode.Create))
            {
                await media.CopyToAsync(fileStream);
            }

            //_apiUrl + 
            return filePath + "/" + fileName;
        }

        public async Task<bool> DeleteMedia(string filePath)
        {
            var path = Path.Combine(_hostingEnvironment.WebRootPath, filePath);

            if (File.Exists(path))
            {
                try
                {
                    File.Delete(path);
                    return true;
                }
                catch (IOException ex)
                {
                    // Handle any errors that may occur during file deletion
                    Console.WriteLine($"An error occurred while deleting file {path}: {ex.Message}");
                    return false;
                }
            }
            else
            {
                // File not found
                return false;
            }
        }

    }

}
