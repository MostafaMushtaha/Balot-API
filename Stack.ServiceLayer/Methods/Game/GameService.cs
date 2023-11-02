using AutoMapper;
using Microsoft.AspNetCore.Http;
using Stack.Core;
using Stack.DTOs;
using Stack.DTOs.Enums;
using Stack.Repository.Common;
using Stack.Entities.DomainEntities.User;
using Stack.Entities.DatabaseEntities.Modules.User;
using Stack.DTOs.Requests.Modules.Auth;
using Microsoft.Extensions.Logging;
using Stack.Entities.DomainEntities.Groups;
using Stack.Entities.DatabaseEntities.Groups;
using Stack.DTOs.Requests.Modules.Groups;
using Stack.DTOs.Requests.Groups;
using System.Text.Json;
using Stack.DTOs.Requests.Modules.Games;
using Stack.Entities.DatabaseEntities.Games;
using Stack.Entities.Enums.Modules.Games;
using Stack.Entities.DatabaseEntities.User;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using Stack.DTOs.Responses.Game;

namespace Stack.ServiceLayer.Methods.Games
{
    public class GameService : IGameService
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IMapper mapper;
        private readonly ILogger<IGameService> _logger;

        public GameService(
            IUnitOfWork unitOfWork,
            IMapper mapper,
            IHttpContextAccessor httpContextAccessor,
            ILogger<IGameService> logger
        )
        {
            this.unitOfWork = unitOfWork;
            this.mapper = mapper;
            _httpContextAccessor = httpContextAccessor;
            _logger = logger;
        }

        public async Task<ApiResponse<long>> CreateGame(GameCreationModel model)
        {
            ApiResponse<long> result = new ApiResponse<long>();

            try
            {
                var userID = await HelperFunctions.GetUserID(_httpContextAccessor);

                if (userID != null)
                {
                    // 1. Create a new game
                    var newGame = new Game
                    {
                        GroupID = model.GroupID,
                        GameMembers = new List<Game_Member>(),
                        Rounds = new List<GameRound>(),
                        Status = (int)GamesStatus.Active,
                        CreationDate = DateTime.UtcNow
                    };

                    var gameCreationRes = await unitOfWork.GameManager.CreateAsync(newGame);
                    await unitOfWork.SaveChangesAsync();

                    if (gameCreationRes != null)
                    {
                        foreach (var member in model.GameMembers)
                        {
                            var gameMember = new Game_Member
                            {
                                GroupMemberID = member.GameMemberID,
                                GameID = newGame.ID,
                                Team = member.Team
                            };
                            newGame.GameMembers.Add(gameMember);
                            await unitOfWork.SaveChangesAsync();
                        }

                        if (newGame.GameMembers.Count == 4)
                        {
                            var firstRound = new GameRound
                            {
                                FirstTeamScore = 0,
                                SecondTeamScore = 0
                            };
                            newGame.Rounds.Add(firstRound);
                            await unitOfWork.SaveChangesAsync();

                            if (newGame.Rounds == null)
                            {
                                _logger.LogWarning("Round creation failed");
                                result.Succeeded = false;
                                result.Errors.Add("Round creation failed");
                                return result;
                            }
                            else
                            {
                                await unitOfWork.SaveChangesAsync();
                                result.Succeeded = true;
                                result.Data = newGame.ID;
                                return result;
                            }
                        }
                        else
                        {
                            _logger.LogWarning("Game Member creation failed");
                            result.Succeeded = false;
                            result.Errors.Add("Game Member creation failed");
                            return result;
                        }
                    }
                    else
                    {
                        _logger.LogWarning("Game Creation Failed");
                        result.Succeeded = false;
                        result.Errors.Add("Game Creation Failed");
                        return result;
                    }
                }
                else
                {
                    _logger.LogWarning("Unauthorized access: User ID not found");
                    result.Succeeded = false;
                    result.Errors.Add("Unauthorized");
                    result.Errors.Add("غير مُصرَّح به");
                    return result;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Exception creating new game");
                result.Succeeded = false;
                result.Errors.Add(ex.Message);
                result.ErrorType = ErrorType.SystemError;
                return result;
            }
        }

        public async Task<ApiResponse<bool>> CheckRoundStatus(RoundStatusModel model)
        {
            ApiResponse<bool> result = new ApiResponse<bool>();
            const int WinningScore = 152;

            try
            {
                var userID = await HelperFunctions.GetUserID(_httpContextAccessor);

                if (userID != null)
                {
                    var game = await unitOfWork.GameManager.GetGameDetails(model.GameID);

                    if (game is not null)
                    {
                        var newRound = new GameRound
                        {
                            FirstTeamScore = model.FirstTeamScore,
                            SecondTeamScore = model.SecondTeamScore
                        };
                        game.Rounds.Add(newRound);

                        var lastRound = game.Rounds.OrderByDescending(r => r.ID).FirstOrDefault();

                        if (lastRound != null)
                        {
                            int accumulatedFirstTeamScore =
                                lastRound.FirstTeamScore + model.FirstTeamScore;
                            int accumulatedSecondTeamScore =
                                lastRound.SecondTeamScore + model.SecondTeamScore;

                            if (
                                accumulatedFirstTeamScore >= WinningScore
                                || accumulatedSecondTeamScore >= WinningScore
                            )
                            {
                                if (accumulatedFirstTeamScore >= WinningScore)
                                {
                                    if (accumulatedFirstTeamScore >= WinningScore)
                                    {
                                        model.WinningTeam = (int)WinningTeam.FirstTeam;
                                    }
                                    else if (accumulatedSecondTeamScore >= WinningScore)
                                    {
                                        model.WinningTeam = (int)WinningTeam.SecondTeam;
                                    }
                                }
                                var StatsAdjustmentResult = AdjustStats(model);

                                if (StatsAdjustmentResult.Result.Succeeded)
                                {
                                    game.Status = (int)GamesStatus.Ended;
                                    await unitOfWork.SaveChangesAsync();
                                    result.Succeeded = true;
                                    return result;
                                }
                                else
                                {
                                    _logger.LogWarning("Error adjusting stats");
                                    result.Succeeded = false;
                                    result.Errors.Add("Error adjusting stats");
                                    return result;
                                }
                            }
                            else
                            {
                                await unitOfWork.SaveChangesAsync();
                                result.Succeeded = true;
                                return result;
                            }
                        }
                        else
                        {
                            _logger.LogWarning($"No rounds found for game with ID {model.GameID}.");
                            result.Succeeded = false;
                            result.Errors.Add("No rounds found");
                            return result;
                        }
                    }
                    else
                    {
                        _logger.LogWarning($"Game with ID {model.GameID} not found.");
                        result.Succeeded = false;
                        result.Errors.Add("Game not found");
                        return result;
                    }
                }
                else
                {
                    _logger.LogWarning("Unauthorized access: User ID not found");
                    result.Succeeded = false;
                    result.Errors.Add("Unauthorized");
                    result.Errors.Add("غير مُصرَّح به");
                    return result;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Exception while checking round status");
                result.Succeeded = false;
                result.Errors.Add(ex.Message);
                result.ErrorType = ErrorType.SystemError;
                return result;
            }
        }

        public async Task<ApiResponse<bool>> AdjustStats(RoundStatusModel model)
        {
            ApiResponse<bool> result = new ApiResponse<bool>();
            const long StreakThreshold = 5;
            const long WinStreakBonus = 2;
            const long RegularWinBonus = 1;

            try
            {
                var allGroupMembers = (
                    await unitOfWork.GroupMembersManager.GetAsync(
                        gm =>
                            model.Teams.FirstTeamMembers.Contains(gm.ID)
                            || model.Teams.SecondTeamMembers.Contains(gm.ID)
                    )
                ).ToList();

                if (allGroupMembers != null)
                {
                    var winnersUserId = allGroupMembers
                        .Where(gm => model.Teams.FirstTeamMembers.Contains(gm.ID))
                        .Select(gm => gm.UserID)
                        .ToList();
                    var losersUserId = allGroupMembers
                        .Where(gm => model.Teams.SecondTeamMembers.Contains(gm.ID))
                        .Select(gm => gm.UserID)
                        .ToList();

                    var userStatsWinners = (
                        await Task.WhenAll(
                            winnersUserId.Select(
                                async userId =>
                                    await unitOfWork.UserStatsManager.GetAsync(
                                        us => us.UserID == userId
                                    )
                            )
                        )
                    )
                        .SelectMany(x => x)
                        .ToList();

                    if (userStatsWinners == null)
                    {
                        _logger.LogWarning("Error while fetching winning team stats");
                        result.Succeeded = false;
                        result.Errors.Add("Error while fetching winning team stats");
                        return result;
                    }

                    var userStatsLosers = (
                        await Task.WhenAll(
                            losersUserId.Select(
                                async userId =>
                                    await unitOfWork.UserStatsManager.GetAsync(
                                        us => us.UserID == userId
                                    )
                            )
                        )
                    )
                        .SelectMany(x => x)
                        .ToList();

                    if (userStatsLosers == null)
                    {
                        _logger.LogWarning("Error while fetching losing team user stats");
                        result.Succeeded = false;
                        result.Errors.Add("Error while fetching losing team user stats");
                        return result;
                    }

                    // var groupStatsWinners = (
                    //     await unitOfWork.StatsManager.GetAsync(
                    //         s => model.Teams.FirstTeamMembers.Contains(s.GroupMemberID)
                    //     )
                    // ).ToList();

                    // if (groupStatsWinners == null)
                    // {
                    //     _logger.LogWarning("Error while fetching winning team group stats");
                    //     result.Succeeded = false;
                    //     result.Errors.Add("Error while fetching winning team group stats");
                    //     return result;
                    // }

                    // var groupStatsLosers = (await unitOfWork.StatsManager.GetAsync()).ToList();

                    // if (groupStatsLosers == null)
                    // {
                    //     _logger.LogWarning("Error while fetching losing team group stats");
                    //     result.Succeeded = false;
                    //     result.Errors.Add("Error while fetching losing team group stats");
                    //     return result;
                    // }

                    AdjustStatsForWinners(
                        userStatsWinners,
                        // groupStatsWinners,
                        StreakThreshold,
                        WinStreakBonus,
                        RegularWinBonus
                    );
                    AdjustStatsForLosers(userStatsLosers);

                    await unitOfWork.SaveChangesAsync();

                    result.Succeeded = true;
                    return result;
                }
                else
                {
                    _logger.LogWarning("Error while fetching teams members");
                    result.Succeeded = false;
                    result.Errors.Add("Error while fetching teams members");
                    return result;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Exception adjusting status for users");
                result.Succeeded = false;
                result.Errors.Add(ex.Message);
                result.ErrorType = ErrorType.SystemError;
                return result;
            }
        }

        private void AdjustStatsForWinners(
            List<UserStats> userStats,
            // List<Stats> groupStats,
            long streakThreshold,
            long winStreakBonus,
            long regularWinBonus
        )
        {
            foreach (var stats in userStats)
            {
                stats.WinningStreak += 1;
                stats.Wins += 1;
                stats.TotalGames += 1;
                stats.PlayerLevel +=
                    stats.WinningStreak >= streakThreshold ? winStreakBonus : regularWinBonus;

                unitOfWork.UserStatsManager.UpdateAsync(stats);
                unitOfWork.SaveChangesAsync();
            }

            // foreach (var stats in groupStats)
            // {
            //     stats.WinningStreak += 1;
            //     stats.Wins += 1;
            //     stats.TotalGames += 1;
            //     stats.GroupMemberLevel +=
            //         stats.WinningStreak >= streakThreshold ? winStreakBonus : regularWinBonus;

            //     unitOfWork.StatsManager.UpdateAsync(stats);
            // }
        }

        private void AdjustStatsForLosers(List<UserStats> userStats)
        {
            foreach (var stats in userStats)
            {
                stats.WinningStreak = 0;
                stats.Loses += 1;
                stats.TotalGames += 1;
                stats.PlayerLevel -= 1;

                unitOfWork.UserStatsManager.UpdateAsync(stats);
                unitOfWork.SaveChangesAsync();
            }

            // foreach (var stats in groupStats)
            // {
            //     stats.WinningStreak = 0;
            //     stats.Loses += 1;
            //     stats.TotalGames += 1;
            //     stats.GroupMemberLevel -= 1;

            //     unitOfWork.StatsManager.UpdateAsync(stats);
            // }
        }

        public async Task<ApiResponse<bool>> DeleteGame(long gameID)
        {
            ApiResponse<bool> result = new ApiResponse<bool>();

            try
            {
                var userID = await HelperFunctions.GetUserID(_httpContextAccessor);

                if (userID != null)
                {
                    var game = await unitOfWork.GameManager.GetByIdAsync(gameID);

                    if (game != null)
                    {
                        if (game.GameMembers != null && game.GameMembers.Count > 0)
                        {
                            foreach (var member in game.GameMembers)
                            {
                                await unitOfWork.GameMembersManager.RemoveAsync(member);
                                await unitOfWork.SaveChangesAsync();
                            }
                        }

                        if (game.Rounds != null && game.Rounds.Count > 0)
                        {
                            foreach (var round in game.Rounds)
                            {
                                await unitOfWork.GameRoundsManager.RemoveAsync(round);
                                await unitOfWork.SaveChangesAsync();
                            }
                        }

                        await unitOfWork.GameManager.RemoveAsync(game);
                        await unitOfWork.SaveChangesAsync();

                        result.Succeeded = true;
                        result.Data = true;
                        return result;
                    }
                    else
                    {
                        _logger.LogWarning($"Game with ID {gameID} not found");
                        result.Succeeded = false;
                        result.Errors.Add($"Game with ID {gameID} not found");
                        return result;
                    }
                }
                else
                {
                    _logger.LogWarning("Unauthorized access: User ID not found");
                    result.Succeeded = false;
                    result.Errors.Add("Unauthorized");
                    result.Errors.Add("غير مُصرَّح به");
                    return result;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Exception deleting game");
                result.Succeeded = false;
                result.Errors.Add(ex.Message);
                result.ErrorType = ErrorType.SystemError;
                return result;
            }
        }

        public async Task<ApiResponse<List<GameHistoryDTO>>> GetRecentGames()
        {
            ApiResponse<List<GameHistoryDTO>> result = new ApiResponse<List<GameHistoryDTO>>();

            try
            {
                var userID = await HelperFunctions.GetUserID(_httpContextAccessor);

                if (userID != null)
                {
                    var gameHistory = await unitOfWork.GameManager.GetUserGameHistory(userID);
                    result.Succeeded = true;
                    result.Data = gameHistory;
                    return result;
                }
                else
                {
                    _logger.LogWarning("Unauthorized access: User ID not found");
                    result.Succeeded = false;
                    result.Errors.Add("Unauthorized");
                    result.Errors.Add("غير مُصرَّح به");
                    return result;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Exception fetching recent games for current user");
                result.Succeeded = false;
                result.Errors.Add(ex.Message);
                result.ErrorType = ErrorType.SystemError;
                return result;
            }
        }

        public async Task<ApiResponse<List<GameHistoryDTO>>> GetUserGameHistoryInGroup(long groupID)
        {
            ApiResponse<List<GameHistoryDTO>> result = new ApiResponse<List<GameHistoryDTO>>();

            try
            {
                var userID = await HelperFunctions.GetUserID(_httpContextAccessor);

                if (userID != null)
                {
                    var gameHistory = await unitOfWork.GameManager.GetUserGameHistoryInGroup(
                        userID,
                        groupID
                    );
                    if (gameHistory != null)
                    {
                        result.Succeeded = true;
                        result.Data = gameHistory;
                        return result;
                    }
                    else
                    {
                        _logger.LogWarning("unable to get user games for this group");
                        result.Succeeded = false;
                        result.Errors.Add("unable to get user games for this group");
                        return result;
                    }
                }
                else
                {
                    _logger.LogWarning("Unauthorized access: User ID not found");
                    result.Succeeded = false;
                    result.Errors.Add("Unauthorized");
                    result.Errors.Add("غير مُصرَّح به");
                    return result;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Exception fetching recent games for current user");
                result.Succeeded = false;
                result.Errors.Add(ex.Message);
                result.ErrorType = ErrorType.SystemError;
                return result;
            }
        }
    }
}
