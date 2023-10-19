using Microsoft.EntityFrameworkCore;
using Stack.DAL;
using Stack.Entities.DatabaseEntities.UserProfile;
using Stack.Entities.DomainEntities.Modules.Profile;
using Stack.Entities.DomainEntities.Modules.UserProfile;
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

        public async Task<Profile> GetProfileByUserID(string userID)
        {
            return await Task.Run(() =>
            {
                return context.Profiles
                    .Where(t => t.UserID == userID)
                    .Include(a => a.User)
                    .FirstOrDefault();
            });
        }

        public async Task<Profile> GetProfile(long ID)
        {
            return await Task.Run(() =>
            {
                return context.Profiles.Where(t => t.ID == ID).FirstOrDefault();
            });
        }

        string CleanPhoneNumber(string rawNumber)
        {
            //Remove spaces and "+" characters
            string sanitizedNumber = Regex.Replace(rawNumber, @"[\s+]", "");

            // If the number starts with "20", remove the "2" part
            if (sanitizedNumber.StartsWith("20"))
            {
                sanitizedNumber = sanitizedNumber.Substring(1);
            }

            return sanitizedNumber;
        }

    }
}
