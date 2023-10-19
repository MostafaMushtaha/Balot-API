using Microsoft.EntityFrameworkCore;
using Stack.DAL;
using Stack.Entities.DatabaseEntities.Modules.User;
using Stack.Entities.Models.Modules.Common;
using Stack.Repository;

namespace Stack.Core.Managers.Social
{
    public class UserConnectionIDsManager : Repository<UserConnectionID, ApplicationDbContext>
    {
        public DbSet<UserConnectionID> dbSet;
        public ApplicationDbContext context;

        public UserConnectionIDsManager(ApplicationDbContext _context)
            : base(_context)
        {
            dbSet = _context.Set<UserConnectionID>();
            context = _context;
        }

        public async Task<int> GetOnlineUserCountModeration()
        {
            return await dbSet.CountAsync();
        }
    }
}
