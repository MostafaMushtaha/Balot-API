using AutoMapper;
using Microsoft.AspNetCore.Http;
using Stack.Core;
using Stack.DTOs;
using Stack.DTOs.Enums;
using Stack.Repository.Common;
using Stack.Entities.DomainEntities.User;
using Stack.Entities.DatabaseEntities.Modules.User;
using Stack.DTOs.Requests.Modules.Auth;
using Microsoft.Extensions.Logging;
using Stack.Entities.DomainEntities.Groups;
using Stack.Entities.DatabaseEntities.Groups;
using Stack.DTOs.Requests.Modules.Groups;
using Stack.DTOs.Requests.Groups;
using System.Text.Json;
using Stack.Entities.DatabaseEntities.User;

namespace Stack.ServiceLayer.Methods.Users
{
    public class FriendsService : IFriendsService
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IMapper mapper;
        private readonly ILogger<IFriendsService> _logger;

        public FriendsService(
            IUnitOfWork unitOfWork,
            IMapper mapper,
            IHttpContextAccessor httpContextAccessor,
            ILogger<IFriendsService> logger
        )
        {
            this.unitOfWork = unitOfWork;
            this.mapper = mapper;
            _httpContextAccessor = httpContextAccessor;
            _logger = logger;
        }

        public async Task<ApiResponse<bool>> AddFriend(string friendID)
        {
            ApiResponse<bool> result = new ApiResponse<bool>();

            try
            {
                var userID = await HelperFunctions.GetUserID(_httpContextAccessor);

                if (userID == null)
                {
                    result.Succeeded = false;
                    result.Errors.Add("Unable to determine logged-in user.");
                    return result;
                }

                if (userID == friendID)
                {
                    result.Succeeded = false;
                    result.Errors.Add("You cannot add yourself as a friend.");
                    return result;
                }

                var existingFriendshipQ = await unitOfWork.FriendsManager.GetAsync(
                    f =>
                        (f.UserID == userID && f.FriendID == friendID)
                        || (f.FriendID == userID && f.UserID == friendID)
                );

                var existingFriendship = existingFriendshipQ.FirstOrDefault();

                if (existingFriendship != null)
                {
                    result.Succeeded = false;
                    result.Errors.Add("You are already friends.");
                    return result;
                }

                var friend = await unitOfWork.UserManager.FindByIdAsync(friendID);

                if (friend == null)
                {
                    result.Succeeded = false;
                    result.Errors.Add("Specified friend ID does not exist.");
                    return result;
                }

                var friendToAdd = new Friends { UserID = userID, FriendID = friendID };

                var creationRes = await unitOfWork.FriendsManager.CreateAsync(friendToAdd);

                if (creationRes == null)
                {
                    result.Errors.Add("Error adding friend.");
                    return result;
                }

                if (result.Errors.Count == 0)
                {
                    await unitOfWork.SaveChangesAsync();
                    result.Succeeded = true;
                    return result;
                }
                else
                {
                    _logger.LogError("Exception adding friend");
                    result.Succeeded = false;
                    result.Errors.Add("Exception adding friend");
                    return result;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Exception adding friend");
                result.Succeeded = false;
                result.Errors.Add(ex.Message);
                result.ErrorType = ErrorType.SystemError;
                return result;
            }
        }
    }
}
