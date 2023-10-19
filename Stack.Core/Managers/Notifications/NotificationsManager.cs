using Microsoft.EntityFrameworkCore;
using Stack.DAL;
using Stack.DTOs.Responses.Notifications;
using Stack.Entities.DatabaseEntities.Notifications;
using Stack.Entities.DomainEntities.Notifications;
using Stack.Repository;


namespace Stack.Core.Managers.Social
{
    public class NotificationsManager : Repository<Notification, ApplicationDbContext>
    {
        public DbSet<Notification> dbSet;
        public ApplicationDbContext context;
        public NotificationsManager(ApplicationDbContext _context) : base(_context)
        {
            dbSet = _context.Set<Notification>();
            context = _context;
        }

        public async Task<bool> SetNotificationRead(long ID, string userID)
        {

            return await Task.Run(async () =>
            {
                Notification notification = context.Notifications.Where(t => t.ID == ID && t.UserID == userID).FirstOrDefault();
                if (notification != null)
                {
                    notification.IsRead = true;
                    context.Notifications.Update(notification);
                    await context.SaveChangesAsync();
                    return true;
                }
                else
                {
                    return false;
                }
            });
        }

        public async Task<bool> SetNotificationsRead(string userID)
        {


            List<Notification> notifications = await context.Notifications.Where(t => t.UserID == userID).ToListAsync();
            if (notifications is not null && notifications.Count > 0)
            {
                for (int i = 0; i < notifications.Count; i++)
                {
                    var notification = notifications[i];
                    notification.IsRead = true;
                    context.Notifications.Update(notification);
                }

                await context.SaveChangesAsync();
                return true;
            }
            else
            {
                return false;
            }

        }

        public async Task<int> GetUnreadNotificationsCount(string userID)
        {
            return await context.Notifications.Where(t => t.UserID == userID && !t.IsRead).CountAsync();
        }


        public async Task<NotificationGroupViewModel> GetNotifications(string userID)
        {
            // Get today's date at midnight in UTC
            DateTime todayUtc = DateTime.UtcNow.Date;

            // Get all notifications for the user
            List<NotificationDTO> allNotifications = await context.Notifications.Where(t => t.UserID == userID)
                .OrderByDescending(t => t.CreationDate)
                .Select(a => new NotificationDTO
                {
                    ID = a.ID,
                    CreationDate = a.CreationDate,
                    IsRead = a.IsRead,
                    Message = a.Message,
                    ReferenceID = a.ReferenceID,
                    SenderID = a.SenderID,
                    Thumbnail = a.Thumbnail,
                    Title = a.Title,
                    Type = a.Type,
                    UserID = a.UserID,
                })
                .ToListAsync();

            // Group the notifications by creation date
            var groupedNotifications = allNotifications.GroupBy(n => n.CreationDate.ToUniversalTime().Date);

            // Create a list of today's notifications and a list of older notifications
            var todaysNotifications = groupedNotifications.FirstOrDefault(g => g.Key == todayUtc)?.ToList() ?? new List<NotificationDTO>();
            var olderNotifications = groupedNotifications.Where(g => g.Key < todayUtc).SelectMany(g => g).ToList();

            return new NotificationGroupViewModel
            {
                TodaysNotifications = todaysNotifications.Select(n => new NotificationViewModel
                {
                    ID = n.ID,
                    CreationDate = n.CreationDate,
                    IsRead = n.IsRead,
                    Message = n.Message,
                    ReferenceID = n.ReferenceID,
                    SenderID = n.SenderID,
                    Thumbnail = n.Thumbnail,
                    Title = n.Title,
                    Type = n.Type,
                    UserID = n.UserID,
                }).ToList(),
                OlderNotifications = olderNotifications.Select(n => new NotificationViewModel
                {
                    ID = n.ID,
                    CreationDate = n.CreationDate,
                    IsRead = n.IsRead,
                    Message = n.Message,
                    ReferenceID = n.ReferenceID,
                    SenderID = n.SenderID,
                    Thumbnail = n.Thumbnail,
                    Title = n.Title,
                    Type = n.Type,
                    UserID = n.UserID,
                }).ToList(),
            };
        }

    }

}