using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Stack.DTOs.Responses.Auth
{
    public class UserCharacteristicsModel
    {
        public int? Gender { get; set; }
        public int? Weight { get; set; }
        public int? Height { get; set; }
        public int? Age { get; set; }
        public int? Physique { get; set; }
        public int? SkinColor { get; set; }
        public int? EyeColor { get; set; }
        public int? HairColor { get; set; }
        public int? FaceShape { get; set; }

        public string? Country { get; set; }
        public string? City { get; set; }
        public string? Street { get; set; }
    }
}
