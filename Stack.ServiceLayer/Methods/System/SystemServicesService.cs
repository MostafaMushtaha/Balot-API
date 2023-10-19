
using AutoMapper;
using Hangfire;
using MediatR;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Stack.Core;
using Stack.DTOs;
using Stack.DTOs.Enums;
using Stack.Entities.DatabaseEntities.Auth;
using Stack.Entities.Enums.Modules.Auth;
using Stack.ServiceLayer.Primitives;

namespace Stack.ServiceLayer.Methods.System
{
    public class SystemServicesService : ISystemServicesService
    {

        private readonly IUnitOfWork unitOfWork;
        private readonly IHttpContextAccessor _httpContextAccessor;
        // private readonly ISystemServicesService _spotlightService;
        private readonly IConfiguration config;
        private readonly IMapper mapper;
        private readonly IMediaUploader _mediaUploader;
        private readonly IMediator _mediator;
        private readonly ILogger<ISystemServicesService> _logger;

        public SystemServicesService(IUnitOfWork unitOfWork, IConfiguration config, IMapper mapper, IHttpContextAccessor httpContextAccessor, IMediator mediator, IMediaUploader mediaUploader,
        ILogger<ISystemServicesService> logger)
        {
            this.unitOfWork = unitOfWork;
            this.config = config;
            this.mapper = mapper;
            _httpContextAccessor = httpContextAccessor;
            _mediaUploader = mediaUploader;
            _mediator = mediator;
            _logger = logger;
        }

        public async Task<ApiResponse<bool>> InitializeSystem()
        {
            ApiResponse<bool> result = new ApiResponse<bool>();
            try
            {
                bool moderatorRoleExists = await unitOfWork.RoleManager.RoleExistsAsync(UserRoles.Moderator.ToString());
                if (!moderatorRoleExists)
                {
                    ApplicationRole moderatorRole = new ApplicationRole
                    {
                        Name = UserRoles.Moderator.ToString(),
                        NameAR = "مدير",
                        DescriptionEN = "Moderator",
                        DescriptionAR = "Moderator",
                        ParentRoleID = "",
                        HasParent = false
                    };

                    var res = await unitOfWork.RoleManager.CreateAsync(moderatorRole);

                    if (!res.Succeeded)
                    {
                        result.Succeeded = false;
                        result.Errors.Add("Exception occured");
                        result.Errors.Add("حدثت مشكلة ، يرجى المحاولة مرة أخرى");
                        return result;
                    }
                }

                //Schedule inactivity checks for next week
                var currentTime = DateTime.UtcNow;
                var scheduleTime = currentTime.AddMinutes(5);
                var scheduledTimespan = scheduleTime - currentTime;

                // BackgroundJob.Schedule(() => SendRoleRecommendations(), scheduledTimespan);
                // BackgroundJob.Schedule(() => SendInactivityNotification(), scheduledTimespan);
                // BackgroundJob.Schedule(() => SendInactivityNotification_CircleMembers(), scheduledTimespan);

                _logger.LogInformation("[System] System initialized");
                result.Succeeded = true;
                result.Data = true;
                return result;

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Exception initializing system");
                result.Succeeded = false;
                result.Errors.Add(ex.Message);
                result.ErrorType = ErrorType.SystemError;
                return result;
            }
        }

    }

}
