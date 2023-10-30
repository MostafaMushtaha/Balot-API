using Microsoft.EntityFrameworkCore;
using Stack.DAL;
using Stack.Entities.DatabaseEntities.UserProfile;
using Stack.Entities.DomainEntities.Modules.Profile;
using Stack.Entities.Enums.Modules.User;
using Stack.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;

namespace Stack.Core.Managers.Modules.UserProfile
{
    public class ProfileManager : Repository<Profile, ApplicationDbContext>
    {
        public DbSet<Profile> dbSet;
        public ApplicationDbContext context;

        public ProfileManager(ApplicationDbContext _context)
            : base(_context)
        {
            dbSet = _context.Set<Profile>();
            context = _context;
        }

        public async Task<long> GetProfileID(string userID)
        {
            return await Task.Run(() =>
            {
                return context.Profiles.Where(t => t.UserID == userID).FirstOrDefault().ID;
            });
        }

        public async Task<ProfileViewModel> ViewPersonalProfile(string userID)
        {
            var userProfile = await context.Users
                .Where(u => u.Id == userID)
                .Select(
                    u =>
                        new ProfileViewModel
                        {
                            UserID = u.Id,
                            ReferenceNumber = u.ReferenceNumber,
                            FullName = u.FullName,
                            Stats = new StatsViewModel
                            {
                                Wins = u.UserStats.Wins,
                                Loses = u.UserStats.Loses,
                                PlayerLevel = u.UserStats.PlayerLevel
                            }
                        }
                )
                .FirstOrDefaultAsync();

            return userProfile;
        }
    }
}
