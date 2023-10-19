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

namespace Stack.ServiceLayer.Methods.Groups
{
    public class GroupsService : IGroupsService
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IMapper mapper;
        private readonly ILogger<IGroupsService> _logger;

        public GroupsService(
            IUnitOfWork unitOfWork,
            IMapper mapper,
            IHttpContextAccessor httpContextAccessor,
            ILogger<IGroupsService> logger
        )
        {
            this.unitOfWork = unitOfWork;
            this.mapper = mapper;
            _httpContextAccessor = httpContextAccessor;
            _logger = logger;
        }

        public async Task<ApiResponse<long>> CreateGroup(GroupCreationModel model)
        {
            ApiResponse<long> result = new ApiResponse<long>();

            try
            {
                var userID = await HelperFunctions.GetUserID(_httpContextAccessor);

                if (userID != null)
                {
                    var similarGroupResult = await unitOfWork.GroupsManager.GetAsync(
                        a => a.Name == model.GroupName
                    );

                    if (similarGroupResult.FirstOrDefault() != null)
                    {
                        result.Succeeded = false;
                        result.Errors.Add(
                            "Failed to create group.Similar group found, please try again!"
                        );
                        return result;
                    }

                    GroupDTO group = new GroupDTO();

                    group.CreateAsActive(model.GroupName, userID);
                    group.AddOwner();

                    var mappedGroup = mapper.Map<Group>(group);

                    var groupCreationResult = await unitOfWork.GroupsManager.CreateAsync(
                        mappedGroup
                    );
                    await unitOfWork.SaveChangesAsync();

                    if (groupCreationResult != null)
                    {
                        // Ensure the members (including owner) from group are added to groupCreationResult
                        groupCreationResult.Members = mapper.Map<ICollection<Group_Member>>(
                            group.Members
                        );

                        if (model.Members != null && model.Members.Count > 0)
                        {
                            var addGroupMembersResult = await AddGroupMembers(
                                model.Members,
                                groupCreationResult.ID
                            );

                            if (!addGroupMembersResult.Succeeded)
                            {
                                _logger.LogError(
                                    "Failed to add members for the group {GroupID}.",
                                    groupCreationResult.ID
                                );
                                result.Succeeded = false;
                                result.Errors.AddRange(addGroupMembersResult.Errors);
                                return result;
                            }
                            _logger.LogInformation(
                                "{user} - Group created - {Group}",
                                userID,
                                groupCreationResult.ID
                            );
                        }

                        await unitOfWork.SaveChangesAsync();
                        _logger.LogInformation("{user} Group created successfully", userID);
                        result.Succeeded = true;
                        result.Data = groupCreationResult.ID;
                        return result;
                    }
                    else
                    {
                        _logger.LogWarning("Group owner creation failed", userID);
                        result.Succeeded = false;
                        result.Errors.Add("Group owner creation failed");
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
                _logger.LogError(ex, "Exception occurred while creating the group");
                result.Succeeded = false;
                result.Errors.Add(ex.Message);
                result.ErrorType = ErrorType.SystemError;
                return result;
            }
        }

        public async Task<ApiResponse<bool>> AddGroupMembers(List<string> members, long GroupID)
        {
            ApiResponse<bool> result = new ApiResponse<bool>();
            try
            {
                ICollection<Group_Member> GroupMembersToAdd = mapper.Map<ICollection<Group_Member>>(
                    members
                );

                foreach (var member in GroupMembersToAdd)
                {
                    member.GroupID = GroupID;

                    var creationRes = await unitOfWork.GroupMembersManager.CreateAsync(member);

                    if (creationRes == null)
                    {
                        result.Errors.Add("Error creating group member: ");
                        return result;
                    }
                }

                if (result.Errors.Count == 0)
                {
                    await unitOfWork.SaveChangesAsync();
                    result.Succeeded = true;
                    return result;
                }
                else
                {
                    _logger.LogError("Exception adding Group members");
                    result.Succeeded = false;
                    result.Errors.Add("Exception adding Group members");
                    return result;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Exception adding Group members");
                result.Succeeded = false;
                result.Errors.Add(ex.Message);
                result.ErrorType = ErrorType.SystemError;
                return result;
            }
        }

        public async Task<ApiResponse<bool>> EditGroup(GroupEditModel model)
        {
            ApiResponse<bool> result = new ApiResponse<bool>();

            try
            {
                var userID = await HelperFunctions.GetUserID(_httpContextAccessor);

                if (userID != null)
                {
                    // Now retrieve the group based on the provided GroupID
                    var userGroup = await unitOfWork.GroupsManager.GetAsync(
                        g =>
                            g.ID == model.GroupID
                            && g.Members.Any(m => m.UserID == userID && m.IsOwner)
                    );

                    if (userGroup != null && userGroup.FirstOrDefault() != null)
                    {
                        var targetGroup = userGroup.FirstOrDefault();
                        if (targetGroup != null)
                        {
                            var groupMembers = await unitOfWork.GroupMembersManager.GetAsync(
                                m => m.GroupID == targetGroup.ID && m.Group != null
                            );
                            if (
                                groupMembers != null
                                && groupMembers.Any(m => m.UserID == userID && m.IsOwner)
                            )
                            {
                                userGroup.FirstOrDefault().Name = model.NewGroupName;
                                await unitOfWork.GroupsManager.UpdateAsync(
                                    userGroup.FirstOrDefault()
                                );
                                await unitOfWork.SaveChangesAsync();

                                result.Succeeded = true;
                                return result;
                            }
                            else
                            {
                                _logger.LogWarning(
                                    $"User {userID} tried to edit a group without ownership rights"
                                );
                                result.Succeeded = false;
                                result.Errors.Add("You do not have permission to edit this group.");
                                return result;
                            }
                        }
                        else
                        {
                            _logger.LogWarning(
                                $"User {userID} does not own the group {model.GroupID} or the group was not found"
                            );
                            result.Succeeded = false;
                            result.Errors.Add("Group not found or you are not the owner.");
                            return result;
                        }
                    }
                    else
                    {
                        _logger.LogWarning(
                            $"User {userID} tried to edit a non-existent group or a group they don't own."
                        );
                        result.Succeeded = false;
                        result.Errors.Add("Group not found or you are not the owner.");
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
                _logger.LogError(ex, "Exception occurred while editing the group");
                result.Succeeded = false;
                result.Errors.Add(ex.Message);
                result.ErrorType = ErrorType.SystemError;
                return result;
            }
        }
    }
}
