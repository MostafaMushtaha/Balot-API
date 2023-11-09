using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Stack.Core;
using Stack.DTOs;
using Stack.DTOs.Enums;
using Stack.DTOs.Requests.Search;
using Stack.DTOs.Responses.Search;
using Stack.Repository.Common;

namespace Stack.ServiceLayer.Methods.users
{
    public class SearchService : ISearchService
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IConfiguration config;
        private readonly IMapper mapper;
        private readonly ILogger<ISearchService> _logger;

        public SearchService(
            IUnitOfWork unitOfWork,
            IConfiguration config,
            IMapper mapper,
            IHttpContextAccessor httpContextAccessor,
            ILogger<ISearchService> logger
        )
        {
            this.unitOfWork = unitOfWork;
            this.config = config;
            this.mapper = mapper;
            _httpContextAccessor = httpContextAccessor;
            _logger = logger;
        }

        public async Task<ApiResponse<List<SearchResultsModel>>> SearchUsers(SearchModel model)
        {
            ApiResponse<List<SearchResultsModel>> result =
                new ApiResponse<List<SearchResultsModel>>();

            try
            {
                var userID = await HelperFunctions.GetUserID(_httpContextAccessor);

                if (userID != null)
                {
                    var searchResults = await unitOfWork.UserManager.SearchUsers(
                        model.Content,
                        userID
                    );

                    if (searchResults is not null)
                    {
                        _logger.LogInformation("{keyword} searched", model.Content);
                        result.Succeeded = true;
                        result.Data = searchResults;
                        return result;
                    }
                    else
                    {
                        _logger.LogInformation(
                            "{keyword} searched - No user results",
                            model.Content
                        );
                        result.Succeeded = false;
                        // result.Errors.Add("No results found");
                        result.Errors.Add("لم يتم العثور على نتائج.");
                        return result;
                    }
                }
                else
                {
                    _logger.LogWarning("Unauthorized access: User ID not found");
                    result.Succeeded = false;
                    // result.Errors.Add("Unauthorized");
                    result.Errors.Add("غير مُصرَّح به");
                    return result;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Exception searching");
                result.Succeeded = false;
                result.Errors.Add("استثناء أثناء البحث");
                result.ErrorType = ErrorType.SystemError;
                return result;
            }
        }
    }
}
