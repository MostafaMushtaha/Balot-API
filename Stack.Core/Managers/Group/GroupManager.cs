using Microsoft.EntityFrameworkCore;
using Stack.DAL;
using Stack.Entities.DatabaseEntities.Groups;
using Stack.Entities.Enums.Modules.User;
using Stack.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Stack.Core.Managers.Groups
{
    public class GroupsManager : Repository<Group, ApplicationDbContext>
    {
        public DbSet<Group> dbSet;
        public ApplicationDbContext context;
        public GroupsManager(ApplicationDbContext _context) : base(_context)
        {
            dbSet = _context.Set<Group>();
            context = _context;
        }
        
    }

}
