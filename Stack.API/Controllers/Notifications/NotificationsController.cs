using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Stack.API.Controllers.Common;
using Stack.Entities.DomainEntities.Notifications;
using Stack.ServiceLayer.Methods.Notifications;

namespace Stack.API.Controllers.Notifications
{
    [Route("api/Notifications")]
    [ApiController]
    [Authorize]
    public class NotificationsController : BaseResultHandlerController<INotificationsService>
    {
        public NotificationsController(INotificationsService _service) : base(_service)
        {

        }

        [HttpPost("SendNotification")]
        public async Task<IActionResult> SendNotification(NotificationDTO model)
        {
            return await AddItemResponseHandler(async () => await service.CreateNotification(model));
        }

        [HttpGet("ReadNotifications")]
        public async Task<IActionResult> ReadNotifications()
        {
            return await GetResponseHandler(async () => await service.ReadNotifications());
        }

        [HttpGet("ReadNotification/{id}")]
        public async Task<IActionResult> ReadNotification(long ID)
        {
            return await GetResponseHandler(async () => await service.ReadNotification(ID));
        }

        [HttpGet("GetUnreadNotificationsCount")]
        public async Task<IActionResult> GetUnreadNotificationsCount()
        {
            return await GetResponseHandler(async () => await service.GetUnreadNotificationsCount());
        }

        [HttpGet("GetNotifications")]
        public async Task<IActionResult> GetNotifications()
        {
            return await GetResponseHandler(async () => await service.GetNotifications());
        }

    }

}
