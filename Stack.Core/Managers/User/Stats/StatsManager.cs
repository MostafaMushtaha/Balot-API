using Microsoft.EntityFrameworkCore;
using Stack.DAL;
using Stack.Entities.DatabaseEntities.Modules.User;
using Stack.Entities.DatabaseEntities.User;
using Stack.Entities.DomainEntities.Users;
using Stack.Repository;

namespace Stack.Core.Managers.Users
{
    public class StatsManager  : Repository<Stats, ApplicationDbContext>
    {
        public DbSet<Stats> dbSet;
        public ApplicationDbContext context;

        public StatsManager (ApplicationDbContext _context)
            : base(_context)
        {
            dbSet = _context.Set<Stats>();
            context = _context;
        }

    }
}
