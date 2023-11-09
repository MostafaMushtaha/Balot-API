using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Stack.DAL;
using Stack.DTOs.Responses.Search;
using Stack.Entities.DatabaseEntities.User;
using Stack.Entities.Enums.Modules.Auth;

namespace Stack.Core.Managers.Modules.Auth
{
    public class ApplicationUserManager : UserManager<ApplicationUser>
    {
        public DbSet<ApplicationUser> dbSet;
        public ApplicationDbContext context;

        public ApplicationUserManager(
            ApplicationDbContext _context,
            IUserStore<ApplicationUser> store,
            IOptions<IdentityOptions> optionsAccessor,
            IPasswordHasher<ApplicationUser> passwordHasher,
            IEnumerable<IUserValidator<ApplicationUser>> userValidators,
            IEnumerable<IPasswordValidator<ApplicationUser>> passwordValidators,
            ILookupNormalizer keyNormalizer,
            IdentityErrorDescriber errors,
            IServiceProvider services,
            ILogger<UserManager<ApplicationUser>> logger
        )
            : base(
                store,
                optionsAccessor,
                passwordHasher,
                userValidators,
                passwordValidators,
                keyNormalizer,
                errors,
                services,
                logger
            )
        {
            dbSet = _context.Set<ApplicationUser>();
            context = _context;
        }

        public ApplicationUserManager(
            IUserStore<ApplicationUser> store,
            IOptions<IdentityOptions> optionsAccessor,
            IPasswordHasher<ApplicationUser> passwordHasher,
            IEnumerable<IUserValidator<ApplicationUser>> userValidators,
            IEnumerable<IPasswordValidator<ApplicationUser>> passwordValidators,
            ILookupNormalizer keyNormalizer,
            IdentityErrorDescriber errors,
            IServiceProvider services,
            ILogger<UserManager<ApplicationUser>> logger
        )
            : base(
                store,
                optionsAccessor,
                passwordHasher,
                userValidators,
                passwordValidators,
                keyNormalizer,
                errors,
                services,
                logger
            ) { }

        public async Task<ApplicationUser> GetUserById(string userId)
        {
            return await Task.Run(() =>
            {
                var usersResult = dbSet.Where(a => a.Id == userId);
                if (usersResult.ToList().Count != 0)
                {
                    var applicationUserToReturn = usersResult.ToList().FirstOrDefault();

                    return applicationUserToReturn;
                }
                else
                {
                    return null;
                }
            });
        }

        public async Task<ApplicationUser> FindDuplicateUsername(string userId, string username)
        {
            return await Task.Run(() =>
            {
                var usersResult = dbSet.Where(a => a.Id != userId && a.UserName == username);
                if (usersResult.ToList().Count != 0)
                {
                    var applicationUserToReturn = usersResult.ToList().FirstOrDefault();

                    return applicationUserToReturn;
                }
                else
                {
                    return null;
                }
            });
        }

        public async Task<List<ApplicationUser>> GetAllSystemUsers()
        {
            var usersResult = dbSet.Where(a => a.Id != null);

            return await usersResult.ToListAsync();
        }

        public async Task<ApplicationUser> GetUserByPhoneNumber(string phoneNumber)
        {
            return await Task.Run(() =>
            {
                return dbSet.Where(a => a.PhoneNumber == phoneNumber).FirstOrDefault();
            });
        }

        public async Task<string> GetUsersPhonenumber(string userID)
        {
            var user = await dbSet.Where(a => a.Id == userID).FirstOrDefaultAsync();

            if (user is not null)
            {
                return user.PhoneNumber;
            }
            else
            {
                return null;
            }
        }

        public async Task<ApplicationUser> GetUserByEmail(string email)
        {
            return await Task.Run(() =>
            {
                return dbSet.Where(a => a.Email == email).FirstOrDefault();
            });
        }

        // public async Task<ApplicationUser> GetUserByAppleId(string appleId)
        // {
        //     return await Task.Run(() =>
        //     {
        //         return dbSet.Where(a => a.AppleId == appleId).FirstOrDefault();
        //     });
        // }

        public async Task<bool> CheckPhoneNumbersExistence(string phoneNumber)
        {
            return await Task.Run(() =>
            {
                var numberExists = dbSet.Where(a => a.PhoneNumber == phoneNumber).FirstOrDefault();
                if (numberExists != null)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            });
        }

        public async Task<ApplicationUser> FindDuplicatePhoneNumber(string PhoneNumber)
        {
            return await Task.Run(() =>
            {
                var usersResult = dbSet.Where(a => a.PhoneNumber != PhoneNumber);
                if (usersResult.ToList().Count != 0)
                {
                    var applicationUserToReturn = usersResult.ToList().FirstOrDefault();

                    return applicationUserToReturn;
                }
                else
                {
                    return null;
                }
            });
        }

        //Registeration User Count

        public async Task<int> GetRegisteredUsersCount()
        {
            return await Task.Run(() =>
            {
                int usersResult = dbSet.Where(a => a.Status == (int)UserStatus.Activated).Count();
                return usersResult;
            });
        }

        public async Task<List<ApplicationUser>> GetPreRegisteredUsers()
        {
            return await Task.Run(() =>
            {
                var usersResult = dbSet
                    .Where(a => a.Status == (int)UserStatus.InitialLogin)
                    .ToList();
                return usersResult;
            });
        }

        public async Task<int> GetUserCountModeration()
        {
            return await Task.Run(() =>
            {
                int usersResult = dbSet.Where(a => a.Status == (int)UserStatus.Activated).Count();
                return usersResult;
            });
        }

        public async Task<List<SearchResultsModel>> SearchUsers(
            string content,
            string currentUserId
        )
        {
            if (!string.IsNullOrWhiteSpace(content) && content.All(char.IsDigit))
            {
                return await dbSet
                    .Where(u => u.ReferenceNumber == content && u.Id != currentUserId)
                    .Select(
                        u =>
                            new SearchResultsModel
                            {
                                ID = u.Id,
                                FullName = u.FullName,
                                ReferenceNumber = u.ReferenceNumber
                            }
                    )
                    .Distinct()
                    .ToListAsync();
            }
            // else if (!string.IsNullOrWhiteSpace(content))
            // {
            //     var friendsList = await dbSet
            //         .SelectMany(u => u.Friends)
            //         .Where(f => f.Friend.FullName.Contains(content) && f.FriendID != currentUserId)
            //         .Select(
            //             f =>
            //                 new SearchResultsModel
            //                 {
            //                     ID = f.FriendID,
            //                     FullName = f.Friend.FullName,
            //                     ReferenceNumber = f.Friend.ReferenceNumber
            //                 }
            //         )
            //         .Distinct()
            //         .ToListAsync();

            //     if (friendsList.Any())
            //     {
            //         return friendsList;
            //     }

            //    return await dbSet
            //         .Where(u => u.Id != currentUserId)
            //         .Select(
            //             u =>
            //                 new SearchResultsModel
            //                 {
            //                     ID = u.Id,
            //                     FullName = u.FullName,
            //                     ReferenceNumber = u.ReferenceNumber
            //                 }
            //         )
            //         .Distinct()
            //         .ToListAsync();
            // }
            else
            {
                return await dbSet
                    .Where(u => u.FullName.Contains(content) && u.Id != currentUserId)
                    .Select(
                        u =>
                            new SearchResultsModel
                            {
                                ID = u.Id,
                                FullName = u.FullName,
                                ReferenceNumber = u.ReferenceNumber
                            }
                    )
                    .Distinct()
                    .ToListAsync();
            }
        }
    }
}
