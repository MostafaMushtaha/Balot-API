using Microsoft.EntityFrameworkCore;
using Stack.DAL;
using Stack.Entities.DatabaseEntities.Groups;
using Stack.Entities.DatabaseEntities.Auth;

using Stack.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Stack.Entities.DomainEntities.Groups;
using Stack.Entities.DatabaseEntities.User;
using Stack.Entities.DomainEntities.Users;

namespace Stack.Core.Managers.Modules.Groups
{
    public class GroupMembersManager : Repository<Group_Member, ApplicationDbContext>
    {
        public DbSet<Group_Member> dbSet;
        public ApplicationDbContext context;

        public GroupMembersManager(ApplicationDbContext _context)
            : base(_context)
        {
            dbSet = _context.Set<Group_Member>();
            context = _context;
        }

        public async Task<bool> CreateGroupMembers(ICollection<Group_Member> members)
        {
            try
            {
                foreach (Group_Member member in members)
                {
                    await context.Group_Member.AddAsync(member);
                }

                await context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public async Task<UserGroupsModel> GetUserInitialGroups(string userID)
        {
            var userWithGroups = await dbSet
                .Where(t => t.UserID == userID)
                .OrderByDescending(t => t.Group.CreationDate)
                .Take(2)
                .Select(
                    t =>
                        new
                        {
                            t.User.FullName,
                            Group = new GroupModel { GroupID = t.GroupID, Name = t.Group.Name }
                        }
                )
                .ToListAsync();

            var groupedByUser = userWithGroups
                .GroupBy(t => t.FullName)
                .Select(
                    g =>
                        new UserGroupsModel
                        {
                            userName = g.Key,
                            Groups = g.Select(x => x.Group).ToList()
                        }
                )
                .FirstOrDefault();
            return groupedByUser;
        }

        public async Task<List<GroupModel>> GetUserGroups(string UserID)
        {
            return await dbSet
                .Where(t => t.UserID == UserID)
                .OrderBy(t => t.CreationDate)
                .Select(t => new GroupModel { GroupID = t.GroupID, Name = t.Group.Name })
                .ToListAsync();
        }

        public async Task<UserGroupDetailsModel> GetUserGroupDetails(long GroupID)
        {
            var groupMembers = await dbSet
                .Where(gm => gm.GroupID == GroupID)
                .Select(
                    member =>
                        new Group_MemberDTO
                        {
                            UserID = member.UserID,
                            GroupID = member.GroupID,
                            ReferenceNumber = member.User.ReferenceNumber,
                            Title = member.User.FullName,
                            IsOwner = member.IsOwner,
                            Stats = new UserStatsDTO
                            {
                                Wins = member.User.UserStats.Wins,
                                Loses = member.User.UserStats.Loses,
                                PlayerLevel = member.User.UserStats.PlayerLevel
                            }
                        }
                )
                .ToListAsync();

            var groupMedia = await dbSet
                .Where(gm => gm.GroupID == GroupID)
                .SelectMany(gm => gm.Group.Media)
                .ToListAsync();

            return new UserGroupDetailsModel { Media = groupMedia, GroupMembers = groupMembers };
        }

        public async Task<bool> AddGroupMembers(ICollection<Group_Member> members)
        {
            try
            {
                foreach (Group_Member member in members)
                {
                    await context.Group_Member.AddAsync(member);
                }

                await context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public async Task<List<Group_MemberDTO>> GetSelectedMembers(long groupId)
        {
            return await dbSet
                .Where(gm => gm.GroupID == groupId)
                .Select(
                    gm =>
                        new Group_MemberDTO
                        {
                            ID = gm.ID,
                            FullName = gm.User.FullName,
                            UserID = gm.UserID,
                            GroupID = gm.GroupID,
                            IsOwner = gm.IsOwner,
                            IsSelected = gm.IsSelected,
                        }
                )
                .ToListAsync();
        }
    }
}
