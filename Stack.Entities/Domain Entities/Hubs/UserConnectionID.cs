using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Stack.Entities.DatabaseEntities.User;

namespace Stack.Entities.Models.Modules.Common
{
    public class UserConnectionID
    {
        [Key]
        [MaxLength(256)]
        public string ID { get; set; }

        [Required]
        [MaxLength(450)]
        public string UserID { get; set; }

        [ForeignKey("UserID")]
        public virtual ApplicationUser User { get; set; }

    }

}
