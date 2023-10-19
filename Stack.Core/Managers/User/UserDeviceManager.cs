using Microsoft.EntityFrameworkCore;
using Stack.DAL;
using Stack.Entities.DatabaseEntities.Modules.User;
using Stack.Repository;


namespace Stack.Core.Managers.Social
{
    public class UserDeviceManager : Repository<UserDevice, ApplicationDbContext>
    {
        public DbSet<UserDevice> dbSet;
        public ApplicationDbContext context;
        public UserDeviceManager(ApplicationDbContext _context) : base(_context)
        {
            dbSet = _context.Set<UserDevice>();
            context = _context;
        }


        public async Task<UserDevice> GetUserDevices(string userID)
        {
            return await Task.Run(() =>
            {
                return dbSet.Where(a => a.UserID == userID && a.IsActive == true).FirstOrDefault();
            });
        }

    }

}