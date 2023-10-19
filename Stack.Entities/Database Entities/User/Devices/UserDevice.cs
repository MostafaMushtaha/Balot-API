
using Microsoft.AspNetCore.Identity;
using Stack.Entities.DatabaseEntities.User;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Stack.Entities.DatabaseEntities.Modules.User
{
    public class UserDevice
    {
        public long ID { get; set; }
        public string Token { get; set; }
        public string UserID { get; set; }

        public bool IsAndroid { get; set; }

        public bool IsActive { get; set; }

        [ForeignKey("UserID")]
        public virtual ApplicationUser User { get; set; }

    }

}
