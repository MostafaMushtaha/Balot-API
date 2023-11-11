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
using Stack.Entities.DatabaseEntities.Games;
using Stack.Entities.DatabaseEntities.GroupMedia;
using Stack.DTOs.Requests.Groups;

namespace Stack.Core.Managers.Modules.Groups
{
    public class MediaManager : Repository<Media, ApplicationDbContext>
    {
        public DbSet<Media> dbSet;
        public ApplicationDbContext context;

        public MediaManager(ApplicationDbContext _context)
            : base(_context)
        {
            dbSet = _context.Set<Media>();
            context = _context;
        }

        
    }
}
