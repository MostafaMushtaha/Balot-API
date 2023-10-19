using Microsoft.EntityFrameworkCore;
using Stack.DAL;
using Stack.Entities.DatabaseEntities.Modules.System;
using Stack.Entities.DatabaseEntities.Modules.User;
using Stack.Repository;


namespace Stack.Core.Managers.Social
{
    public class AppVersionsManager : Repository<AppVersion, ApplicationDbContext>
    {
        public DbSet<AppVersion> dbSet;
        public ApplicationDbContext context;
        public AppVersionsManager(ApplicationDbContext _context) : base(_context)
        {
            dbSet = _context.Set<AppVersion>();
            context = _context;
        }
    }

}