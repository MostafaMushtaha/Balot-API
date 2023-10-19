using Stack.Entities.DatabaseEntities.User;
using System.ComponentModel.DataAnnotations.Schema;

namespace Stack.Entities.DatabaseEntities.Notifications
{
    public class Notification
    {
        public long ID { get; set; }
        public string UserID { get; set; }
        public string SenderID { get; set; }
        public string ReferenceID { get; set; }
        public string? Title { get; set; }
        public string Message { get; set; }
        public string? Thumbnail { get; set; }
        public int Type { get; set; }
        public bool IsRead { get; set; }
        public DateTime CreationDate { get; set; }

        [ForeignKey("UserID")]
        public ApplicationUser User { get; set; }

    }

}
