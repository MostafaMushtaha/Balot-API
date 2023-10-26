using Microsoft.EntityFrameworkCore;
using Stack.DAL;
using Stack.Entities.DatabaseEntities.Modules.User;
using Stack.Entities.DatabaseEntities.User;
using Stack.Entities.DomainEntities.Users;
using Stack.Repository;

namespace Stack.Core.Managers.Users
{
    public class UserStatsManager : Repository<UserStats, ApplicationDbContext>
    {
        public DbSet<UserStats> dbSet;
        public ApplicationDbContext context;

        public UserStatsManager(ApplicationDbContext _context)
            : base(_context)
        {
            dbSet = _context.Set<UserStats>();
            context = _context;
        }

    }
}
