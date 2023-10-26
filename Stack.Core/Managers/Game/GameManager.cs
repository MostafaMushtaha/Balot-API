using Microsoft.EntityFrameworkCore;
using Stack.DAL;
using Stack.DTOs.Responses.Game;
using Stack.Entities.DatabaseEntities.Games;
using Stack.Entities.DatabaseEntities.Groups;
using Stack.Entities.Enums.Modules.User;
using Stack.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Stack.Core.Managers.Games
{
    public class GameManager : Repository<Game, ApplicationDbContext>
    {
        public DbSet<Game> dbSet;
        public ApplicationDbContext context;

        public GameManager(ApplicationDbContext _context)
            : base(_context)
        {
            dbSet = _context.Set<Game>();
            context = _context;
        }

        public async Task<Game> GetGameDetails(long gameId)
        {
            return await dbSet
                .Include(g => g.Rounds)
                .Include(g => g.GameMembers)
                .Include(g => g.Group)
                .Include(g => g.Rounds.OrderByDescending(r => r.ID).Take(1))
                .Where(g => g.ID == gameId)
                .FirstOrDefaultAsync();
        }

        public async Task<List<GameHistoryDTO>> GetUserGameHistory(string userId)
        {
            var gameHistories = await context.Game_Members
                .Include(gm => gm.Game)
                .ThenInclude(g => g.Rounds)
                .Include(gm => gm.GroupMember)
                .Where(gm => gm.GroupMember.UserID == userId)
                .Select(
                    gm =>
                        new GameHistoryDTO
                        {
                            GameId = gm.GameID,
                            DatePlayed = gm.Game.CreationDate,
                            UserTeamScore =
                                gm.Team == 1
                                    ? gm.Game.Rounds.Sum(r => r.FirstTeamScore)
                                    : gm.Game.Rounds.Sum(r => r.SecondTeamScore),
                            OpponentTeamScore =
                                gm.Team == 2
                                    ? gm.Game.Rounds.Sum(r => r.FirstTeamScore)
                                    : gm.Game.Rounds.Sum(r => r.SecondTeamScore),
                            GameMemberNames = gm.Game.GameMembers
                                .Select(gm2 => gm2.GroupMember.User.UserName)
                                .ToList()
                        }
                )
                .ToListAsync();

            return gameHistories;
        }
    }
}
