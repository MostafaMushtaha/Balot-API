using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Stack.DTOs;

namespace Stack.ServiceLayer.Primitives
{
    public interface IMediaUploader
    {
        Task<string> UploadMedia(string filePath, IFormFile media);
        Task<bool> DeleteMedia(string filePath);
    }
}