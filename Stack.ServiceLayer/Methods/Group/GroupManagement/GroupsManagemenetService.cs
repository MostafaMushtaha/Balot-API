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
using Stack.Entities.DatabaseEntities.GroupMedia;

namespace Stack.ServiceLayer.Methods.Groups
{
    public class GroupsManagemenetService : IGroupsManagemenetService
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IMapper mapper;
        private readonly ILogger<IGroupsManagemenetService> _logger;

        public GroupsManagemenetService(
            IUnitOfWork unitOfWork,
            IMapper mapper,
            IHttpContextAccessor httpContextAccessor,
            ILogger<IGroupsManagemenetService> logger
        )
        {
            this.unitOfWork = unitOfWork;
            this.mapper = mapper;
            _httpContextAccessor = httpContextAccessor;
            _logger = logger;
        }

        public async Task<ApiResponse<List<GroupModel>>> GetUserGroups()
        {
            ApiResponse<List<GroupModel>> result = new ApiResponse<List<GroupModel>>();
            try
            {
                var userID = await HelperFunctions.GetUserID(_httpContextAccessor);

                if (userID != null)
                {
                    var userGroups = await unitOfWork.GroupMembersManager.GetUserGroups(userID);

                    if (userGroups != null)
                    {
                        _logger.LogInformation("User groups fetched {user}", userID);
                        result.Succeeded = true;
                        result.Data = userGroups;
                        return result;
                    }
                    else
                    {
                        _logger.LogError("User groups not found {user}", userID);
                        result.Succeeded = false;
                        result.Errors.Add("Unauthorized");
                        result.Errors.Add("غير مُصرَّح به");
                        return result;
                    }
                }
                else
                {
                    _logger.LogWarning("Unauthorized access: User ID not found");
                    result.Succeeded = false;
                    result.Errors.Add("Unauthorized");
                    result.Errors.Add("غير مُصرَّح به");
                    return result;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Exception fetching user groups");
                result.Succeeded = false;
                result.Errors.Add(ex.Message);
                result.ErrorType = ErrorType.SystemError;
                return result;
            }
        }

        public async Task<ApiResponse<UserGroupsModel>> GetUserInitialGroups()
        {
            ApiResponse<UserGroupsModel> result = new ApiResponse<UserGroupsModel>();
            try
            {
                var userID = await HelperFunctions.GetUserID(_httpContextAccessor);

                if (userID != null)
                {
                    var userGroups = await unitOfWork.GroupMembersManager.GetUserInitialGroups(
                        userID
                    );

                    if (userGroups != null)
                    {
                        _logger.LogInformation("User groups fetched {user}", userID);
                        result.Succeeded = true;
                        result.Data = userGroups;
                        return result;
                    }
                    else
                    {
                        _logger.LogError("User groups not found {user}", userID);
                        result.Succeeded = false;
                        result.Errors.Add("Unauthorized");
                        result.Errors.Add("غير مُصرَّح به");
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
                _logger.LogError(ex, "Exception fetching user groups");
                result.Succeeded = false;
                result.Errors.Add("حدث استثناء أثناء جلب مجموعات المستخدم");
                result.ErrorType = ErrorType.SystemError;
                return result;
            }
        }

        public async Task<ApiResponse<UserGroupDetailsModel>> GetUserGroupDetails(long groupID)
        {
            ApiResponse<UserGroupDetailsModel> result = new ApiResponse<UserGroupDetailsModel>();

            try
            {
                var userID = await HelperFunctions.GetUserID(_httpContextAccessor);

                if (userID != null)
                {
                    var userGroupDetails = await unitOfWork.GroupMembersManager.GetUserGroupDetails(
                        groupID
                    );

                    if (userGroupDetails != null)
                    {
                        _logger.LogInformation("Group details fetched {user}", userID);
                        result.Succeeded = true;
                        result.Data = userGroupDetails;
                        return result;
                    }
                    else
                    {
                        _logger.LogError("Group Details not found {user}", userID);
                        result.Succeeded = false;
                        result.Errors.Add("Unauthorized");
                        result.Errors.Add("غير مُصرَّح به");
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
                _logger.LogError(ex, "Exception fetching user groups");
                result.Succeeded = false;
                result.Errors.Add("استثناء أثناء استرجاع مجموعات المستخدم");
                result.ErrorType = ErrorType.SystemError;
                return result;
            }
        }

        public async Task<ApiResponse<List<Group_MemberDTO>>> GetSelectedMembers(long groupID)
        {
            ApiResponse<List<Group_MemberDTO>> result = new ApiResponse<List<Group_MemberDTO>>();

            try
            {
                var groupMembers = await unitOfWork.GroupMembersManager.GetSelectedMembers(groupID);

                if (groupMembers == null)
                {
                    _logger.LogError($"No members found for group with ID {groupID}");
                    result.Succeeded = false;
                    result.Errors.Add("No members found for the given group.");
                    return result;
                }

                if (groupMembers.Count() < 4)
                {
                    _logger.LogError(
                        $"Not enough members (less than 4) found for group with ID {groupID}"
                    );
                    result.Succeeded = false;
                    result.Errors.Add(
                        "لم يتم العثور على عدد كافٍ من الأعضاء (أقل من 4) للمجموعة المعطاة."
                    );
                    return result;
                }

                foreach (var member in groupMembers.Take(4))
                {
                    member.IsSelected = true;
                }

                // Log information
                _logger.LogInformation($"Selected members for group with ID {groupID}");

                // Map the entities to DTOs
                var groupMemberDTOs = mapper.Map<List<Group_MemberDTO>>(groupMembers);

                result.Succeeded = true;
                result.Data = groupMemberDTOs;

                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Exception fetching members for group with ID {groupID}");
                result.Succeeded = false;
                result.Errors.Add("استثناء أثناء جلب أعضاء المجموعة بمعرف الهوية");
                result.ErrorType = ErrorType.SystemError;
                return result;
            }
        }

        public async Task<ApiResponse<bool>> RemoveMember(string groupMemberID)
        {
            ApiResponse<bool> result = new ApiResponse<bool>();
            try
            {
                var userID = await HelperFunctions.GetUserID(_httpContextAccessor);

                if (userID != null)
                {
                    var groupMemberQ = await unitOfWork.GroupMembersManager.GetAsync(
                        t => t.UserID == groupMemberID
                    );
                    var groupMember = groupMemberQ.FirstOrDefault();

                    if (groupMember is not null)
                    {
                        if (groupMember.UserID == userID)
                        {
                            _logger.LogError(
                                "Unauthorized removal attempt by {user} for group member {groupMember}",
                                userID,
                                groupMemberID
                            );
                            result.Succeeded = false;
                            result.Errors.Add("محاولة إزالة غير مصرح بها");
                            return result;
                        }

                        await unitOfWork.GroupMembersManager.RemoveAsync(groupMember);
                        await unitOfWork.SaveChangesAsync();
                        result.Succeeded = true;
                        return result;
                    }
                    else
                    {
                        _logger.LogError(
                            "Error removing member - Member not found {user} - group {group}",
                            userID,
                            groupMemberID
                        );
                        result.Succeeded = false;
                        // result.Errors.Add("Member not found");
                        result.Errors.Add("العضو غير موجود");
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
                _logger.LogError(ex, "Exception removing group member");
                result.Succeeded = false;
                result.Errors.Add("استثناء أثناء إزالة عضو من المجموعة");
                result.ErrorType = ErrorType.SystemError;
                return result;
            }
        }
        public async Task<ApiResponse<List<Media>>> GetGroupMediaList(GroupMediaModel model)
        {
            ApiResponse<List<Media>> result = new ApiResponse<List<Media>>();
            try
            {
                var userID = await HelperFunctions.GetUserID(_httpContextAccessor);

                if (userID != null)
                {
                    var groupMediaDetails = await unitOfWork.GroupMembersManager.GetGroupMediaDetails(model);

                    if (groupMediaDetails != null)
                    {
                        _logger.LogInformation("Group details fetched {user}", userID);
                        result.Succeeded = true;
                        result.Data = groupMediaDetails;
                        return result;
                    }
                    else
                    {
                        _logger.LogError("Group Details not found {user}", userID);
                        result.Succeeded = false;
                        result.Errors.Add("غير مُصرَّح به");
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
                _logger.LogError(ex, "Exception removing group member");
                result.Succeeded = false;
                result.Errors.Add("استثناء أثناء إزالة عضو من المجموعة");
                result.ErrorType = ErrorType.SystemError;
                return result;
            }
        }
    }
}
