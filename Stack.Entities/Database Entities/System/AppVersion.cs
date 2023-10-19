
using Microsoft.AspNetCore.Identity;
using Stack.Entities.DatabaseEntities.User;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Stack.Entities.DatabaseEntities.Modules.System
{
    public class AppVersion
    {
        public string LatestVersion { get; set; }
        public string MinimumRequiredVersion { get; set; }
    }

}
