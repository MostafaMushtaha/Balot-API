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

namespace Stack.Core.Managers.Modules.Games
{
    public class GameMembersManager : Repository<Game_Member, ApplicationDbContext>
    {
        public DbSet<Game_Member> dbSet;
        public ApplicationDbContext context;

        public GameMembersManager(ApplicationDbContext _context)
            : base(_context)
        {
            dbSet = _context.Set<Game_Member>();
            context = _context;
        }

        public async Task<IEnumerable<Game_Member>> GetMembersByGameId(long gameId)
        {
            return await dbSet.Where(gm => gm.GameID == gameId).ToListAsync();
        }
    }
}
