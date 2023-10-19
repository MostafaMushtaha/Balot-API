using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Stack.DTOs.Responses.Notifications
{
    public class NotificationGroupViewModel
    {
        public List<NotificationViewModel> TodaysNotifications { get; set; }
        public List<NotificationViewModel> OlderNotifications { get; set; }
    }

    public class NotificationViewModel
    {
        public long ID { get; set; }
        public string? Title { get; set; }
        public string SenderID { get; set; }
        public string UserID { get; set; }
        public string Message { get; set; }
        public string? Thumbnail { get; set; }
        public int Type { get; set; }
        public string ReferenceID { get; set; }
        public DateTime CreationDate { get; set; }
        public bool IsRead { get; set; }
    }
}
