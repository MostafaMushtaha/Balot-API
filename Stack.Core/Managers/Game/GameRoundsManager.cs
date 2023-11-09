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
using Stack.Entities.Enums.Modules.Games;

namespace Stack.Core.Managers.Modules.Games
{
    public class GameRoundsManager : Repository<GameRound, ApplicationDbContext>
    {
        public DbSet<GameRound> dbSet;
        public ApplicationDbContext context;

        public GameRoundsManager(ApplicationDbContext _context)
            : base(_context)
        {
            dbSet = _context.Set<GameRound>();
            context = _context;
        }

        public async Task<IEnumerable<GameRound>> GetRoundsForGame(long gameID)
        {
            return await dbSet.Where(gr => gr.GameID == gameID).ToListAsync();
        }

        public async Task<GameRound> GetLatestRoundForGame(long gameID)
        {
            return await dbSet
                .Where(gr => gr.GameID == gameID && gr.Game.Status == (int)GamesStatus.Ended)
                .OrderByDescending(gr => gr.ID)
                .FirstOrDefaultAsync();
        }
    }
}
