﻿using Microsoft.EntityFrameworkCore;
using Stack.DAL;
using Stack.DTOs.Responses.Game;
using Stack.Entities.DatabaseEntities.Games;
using Stack.Entities.DatabaseEntities.Groups;
using Stack.Entities.DomainEntities.Games;
using Stack.Entities.Enums.Modules.Games;
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

        public async Task<List<GameHistoryDTO>> GetUserGameHistory(string userId, int pageNumber)
        {
            const int pageSize = 10; // You can make this configurable if needed
            var skip = (pageNumber - 1) * pageSize;

            var gameHistories = await context.Game_Members
                .Where(
                    gm =>
                        gm.GroupMember.UserID == userId && gm.Game.Status == (int)GamesStatus.Ended
                )
                .Select(
                    gm =>
                        new
                        {
                            gm.GameID,
                            gm.Game.CreationDate,
                            gm.Game.Group.Name,
                            gm.Team,
                            FirstTeamScore = gm.Game.Rounds.Sum(r => r.FirstTeamScore),
                            SecondTeamScore = gm.Game.Rounds.Sum(r => r.SecondTeamScore),
                            FirstTeamMembers = gm.Game.GameMembers
                                .Where(gm2 => gm2.Team == 0)
                                .Select(
                                    gm2 =>
                                        new TeamMemberDTO
                                        {
                                            UserID = gm2.GroupMember.UserID.ToString(),
                                            Fullname = gm2.GroupMember.User.FullName
                                        }
                                ),
                            SecondTeamMembers = gm.Game.GameMembers
                                .Where(gm2 => gm2.Team == 1)
                                .Select(
                                    gm2 =>
                                        new TeamMemberDTO
                                        {
                                            UserID = gm2.GroupMember.UserID.ToString(),
                                            Fullname = gm2.GroupMember.User.FullName
                                        }
                                ),
                            UserTeam = gm.Team
                        }
                )
                .OrderByDescending(gm => gm.CreationDate)
                .Skip(skip) // Skip the previous pages' items
                .Take(pageSize) // Take only items for the current page
                .ToListAsync();

            var gameHistoryDTOs = gameHistories
                .Select(
                    gm =>
                        new GameHistoryDTO
                        {
                            GameId = gm.GameID,
                            DatePlayed = gm.CreationDate,
                            GroupName = gm.Name,
                            UserTeamScore =
                                gm.UserTeam == 0 ? gm.FirstTeamScore : gm.SecondTeamScore,
                            OpponentTeamScore =
                                gm.UserTeam == 1 ? gm.FirstTeamScore : gm.SecondTeamScore,
                            Members = new GameMember
                            {
                                FirstTeamMember =
                                    gm.UserTeam == 0
                                        ? gm.FirstTeamMembers.ToList()
                                        : gm.SecondTeamMembers.ToList(),
                                SecondTeamMember =
                                    gm.UserTeam == 1
                                        ? gm.FirstTeamMembers.ToList()
                                        : gm.SecondTeamMembers.ToList(),
                            }
                        }
                )
                .ToList();

            return gameHistoryDTOs;
        }

        public async Task<List<GameHistoryDTO>> GetUserGameHistoryInGroup(
            string userId,
            GameHistoryModel model
        )
        {
            int pageSize = 10;
            int skip = (model.PageNumber - 1) * pageSize;

            var gameHistories = await context.Game_Members
                .Where(
                    gm =>
                        gm.GroupMember.UserID == userId
                        && gm.Game.GroupID == model.GroupID
                        && gm.Game.Status == (int)GamesStatus.Ended
                )
                .Select(
                    gm =>
                        new
                        {
                            gm.GameID,
                            gm.Game.CreationDate,
                            gm.Game.Group.Name,
                            gm.Team,
                            FirstTeamScore = gm.Game.Rounds.Sum(r => r.FirstTeamScore),
                            SecondTeamScore = gm.Game.Rounds.Sum(r => r.SecondTeamScore),
                            FirstTeamMembers = gm.Game.GameMembers
                                .Where(gm2 => gm2.Team == 0)
                                .Select(
                                    gm2 =>
                                        new TeamMemberDTO
                                        {
                                            UserID = gm2.GroupMember.UserID.ToString(),
                                            Fullname = gm2.GroupMember.User.FullName
                                        }
                                ),
                            SecondTeamMembers = gm.Game.GameMembers
                                .Where(gm2 => gm2.Team == 1)
                                .Select(
                                    gm2 =>
                                        new TeamMemberDTO
                                        {
                                            UserID = gm2.GroupMember.UserID.ToString(),
                                            Fullname = gm2.GroupMember.User.FullName
                                        }
                                ),
                            UserTeam = gm.Team
                        }
                )
                .OrderByDescending(gm => gm.CreationDate)
                .Skip(skip)
                .Take(pageSize)
                .ToListAsync();

            var gameHistoryDTOs = gameHistories
                .Select(
                    gm =>
                        new GameHistoryDTO
                        {
                            GameId = gm.GameID,
                            DatePlayed = gm.CreationDate,
                            GroupName = gm.Name,
                            UserTeamScore =
                                gm.UserTeam == 0 ? gm.FirstTeamScore : gm.SecondTeamScore,
                            OpponentTeamScore =
                                gm.UserTeam == 1 ? gm.FirstTeamScore : gm.SecondTeamScore,
                            Members = new GameMember
                            {
                                FirstTeamMember =
                                    gm.UserTeam == 0
                                        ? gm.FirstTeamMembers.ToList()
                                        : gm.SecondTeamMembers.ToList(),
                                SecondTeamMember =
                                    gm.UserTeam == 1
                                        ? gm.FirstTeamMembers.ToList()
                                        : gm.SecondTeamMembers.ToList(),
                            }
                        }
                )
                .ToList();

            return gameHistoryDTOs;
        }
    }
}
