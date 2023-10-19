using Microsoft.EntityFrameworkCore;
using Stack.DAL;
using Stack.Entities.DatabaseEntities.UserProfile;
using Stack.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Stack.Core.Managers.Modules.UserProfile
{
    public class ProfileSettingsManager : Repository<ProfileSettings, ApplicationDbContext>
    {
        public DbSet<ProfileSettings> dbSet;
        public ApplicationDbContext context;
        public ProfileSettingsManager(ApplicationDbContext _context) : base(_context)
        {
            dbSet = _context.Set<ProfileSettings>();
            context = _context;
        }



    }

}