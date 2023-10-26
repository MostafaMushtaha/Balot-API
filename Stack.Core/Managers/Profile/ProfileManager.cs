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
                            FullName = u.FullName
                        }
                )
                .FirstOrDefaultAsync();

            if (userProfile != null)
            {
                var userStats = await context.UserStats
                    .Where(s => s.UserID == userID)
                    .GroupBy(s => s.UserID)
                    .Select(
                        g =>
                            new StatsViewModel
                            {
                                Wins = g.Sum(s => s.Wins),
                                Loses = g.Sum(s => s.Loses)
                            }
                    )
                    .FirstOrDefaultAsync();

                userProfile.Stats = userStats ?? new StatsViewModel { Wins = 0, Loses = 0 };
            }

            return userProfile;
        }
    }
}
