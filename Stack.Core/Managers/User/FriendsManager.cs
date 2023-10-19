using Microsoft.EntityFrameworkCore;
using Stack.DAL;
using Stack.Entities.DatabaseEntities.Modules.User;
using Stack.Entities.DatabaseEntities.User;
using Stack.Repository;


namespace Stack.Core.Managers.Users
{
    public class FriendsManager : Repository<Friends, ApplicationDbContext>
    {
        public DbSet<Friends> dbSet;
        public ApplicationDbContext context;
        public FriendsManager(ApplicationDbContext _context) : base(_context)
        {
            dbSet = _context.Set<Friends>();
            context = _context;
        }

    }

}