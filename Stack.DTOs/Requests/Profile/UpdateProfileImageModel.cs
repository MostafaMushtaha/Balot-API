using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Http;

namespace Stack.DTOs.Requests.UserProfile
{
    public class UpdateProfileImageModel
    {
        public IFormFile Image { get; set; }

    }

}
