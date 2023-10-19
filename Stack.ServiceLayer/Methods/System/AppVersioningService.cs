
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Stack.Core;
using Stack.DTOs;
using Stack.DTOs.Enums;
using Stack.DTOs.Responses.System;
using Stack.ServiceLayer.Primitives;

namespace Stack.ServiceLayer.Methods.System
{
    public class AppVersioningService : IAppVersioningService
    {

        private readonly IUnitOfWork unitOfWork;
        private readonly IHttpContextAccessor _httpContextAccessor;
        // private readonly IAppVersioningService _spotlightService;
        private readonly IConfiguration config;
        private readonly IMapper mapper;
        private readonly IMediaUploader _mediaUploader;
        private readonly ILogger<IAppVersioningService> _logger;


        public AppVersioningService(IUnitOfWork unitOfWork, IConfiguration config, IMapper mapper, IHttpContextAccessor httpContextAccessor, IMediaUploader mediaUploader,
        ILogger<IAppVersioningService> logger)
        {
            this.unitOfWork = unitOfWork;
            this.config = config;
            this.mapper = mapper;
            _httpContextAccessor = httpContextAccessor;
            _mediaUploader = mediaUploader;
            _logger = logger;
        }



        public async Task<ApiResponse<AppVersionModel>> VerifyAppVersion()
        {
            ApiResponse<AppVersionModel> result = new ApiResponse<AppVersionModel>();
            try
            {
                var appVersions = await unitOfWork.AppVersionsManager.GetAsync();
                var appVersion = appVersions.OrderByDescending(av => av.LatestVersion).FirstOrDefault();

                if (appVersion is not null)
                {
                    AppVersionModel response = new AppVersionModel
                    {
                        LatestVersion = appVersion.LatestVersion,
                        MinimumRequiredVersion = appVersion.MinimumRequiredVersion
                    };

                    _logger.LogInformation("Verifying app version {version}", appVersion.LatestVersion);
                    result.Succeeded = true;
                    result.Data = response;
                    return result;
                }
                else
                {
                    _logger.LogError("Error Verifying app version, no version records");
                    result.Succeeded = false;
                    result.Errors.Add("Exception occured");
                    result.Errors.Add("حدث استثناء");
                    return result;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Exception Verifying app version");
                result.Succeeded = false;
                result.Errors.Add(ex.Message);
                result.ErrorType = ErrorType.SystemError;
                return result;
            }
        }


    }

}
