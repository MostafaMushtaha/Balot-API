using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Stack.Core.Managers.Games;
using Stack.Core.Managers.Groups;
using Stack.Core.Managers.Modules.Auth;
using Stack.Core.Managers.Modules.Games;
using Stack.Core.Managers.Modules.Groups;
using Stack.Core.Managers.Modules.UserProfile;
using Stack.Core.Managers.Social;
using Stack.Core.Managers.Users;
using Stack.DAL;

namespace Stack.Core
{
    public class UnitOfWork : IUnitOfWork
    {
        public readonly ApplicationDbContext context;
        private IDbContextTransaction _transaction;

        public UnitOfWork(
            ApplicationDbContext context,
            ApplicationUserManager userManager,
            ApplicationRoleManager roleManager
        )
        {
            this.context = context;
            UserManager = userManager;
            RoleManager = roleManager;
        }

        public async Task<bool> SaveChangesAsync()
        {
            try
            {
                return await context.SaveChangesAsync() > 0;
            }
            catch (DbUpdateConcurrencyException ex)
            {
                // log message and enteries
            }
            catch (DbUpdateException ex)
            {
                // log message and enteries
            }
            catch (Exception ex)
            {
                // Log here.
            }
            return false;
        }

        public async Task BeginTransactionAsync()
        {
            _transaction = await context.Database.BeginTransactionAsync();
        }

        public async Task CommitTransactionAsync()
        {
            if (_transaction != null)
            {
                await _transaction.CommitAsync();
                await _transaction.DisposeAsync();
                _transaction = null;
            }
        }

        public async Task RollbackTransactionAsync()
        {
            if (_transaction != null)
            {
                await _transaction.RollbackAsync();
                await _transaction.DisposeAsync();
                _transaction = null;
            }
        }

        public async void Dispose()
        {
            if (_transaction != null)
            {
                await _transaction.DisposeAsync();
                _transaction = null;
            }

            context.Dispose();
        }

        public ApplicationUserManager UserManager { get; private set; } //Manager for application users table .
        public ApplicationRoleManager RoleManager { get; private set; } //Manager for application users table .

        private RegistrationRequestManager registrationRequestManager;
        public RegistrationRequestManager RegistrationRequestManager
        {
            get
            {
                if (registrationRequestManager == null)
                {
                    registrationRequestManager = new RegistrationRequestManager(context);
                }
                return registrationRequestManager;
            }
        }

        private ProfileManager profileManager;
        public ProfileManager ProfileManager
        {
            get
            {
                if (profileManager == null)
                {
                    profileManager = new ProfileManager(context);
                }

                return profileManager;
            }
        }
        private ProfileSettingsManager profileSettingsManager;
        public ProfileSettingsManager ProfileSettingsManager
        {
            get
            {
                if (profileSettingsManager == null)
                {
                    profileSettingsManager = new ProfileSettingsManager(context);
                }

                return profileSettingsManager;
            }
        }

        private NotificationsManager notificationsManager;
        public NotificationsManager NotificationsManager
        {
            get
            {
                if (notificationsManager == null)
                {
                    notificationsManager = new NotificationsManager(context);
                }

                return notificationsManager;
            }
        }

        private UserDeviceManager userDeviceManager;
        public UserDeviceManager UserDeviceManager
        {
            get
            {
                if (userDeviceManager == null)
                {
                    userDeviceManager = new UserDeviceManager(context);
                }

                return userDeviceManager;
            }
        }
        private GroupsManager groupsManager;
        public GroupsManager GroupsManager
        {
            get
            {
                if (groupsManager == null)
                {
                    groupsManager = new GroupsManager(context);
                }

                return groupsManager;
            }
        }
        private GroupMembersManager groupMembersManager;
        public GroupMembersManager GroupMembersManager
        {
            get
            {
                if (groupMembersManager == null)
                {
                    groupMembersManager = new GroupMembersManager(context);
                }

                return groupMembersManager;
            }
        }

        // private OTPRequestManager otpRequestManager;
        // public OTPRequestManager OTPRequestManager
        // {
        //     get
        //     {
        //         if (otpRequestManager == null)
        //         {
        //             otpRequestManager = new OTPRequestManager(context);
        //         }

        //         return otpRequestManager;
        //     }
        // }

        private UserConnectionIDsManager userConnectionIDsManager;
        public UserConnectionIDsManager UserConnectionIDsManager
        {
            get
            {
                if (userConnectionIDsManager == null)
                {
                    userConnectionIDsManager = new UserConnectionIDsManager(context);
                }

                return userConnectionIDsManager;
            }
        }

        private FriendsManager friendsManager;
        public FriendsManager FriendsManager
        {
            get
            {
                if (friendsManager == null)
                {
                    friendsManager = new FriendsManager(context);
                }

                return friendsManager;
            }
        }

        private AppVersionsManager appVersionsManager;
        public AppVersionsManager AppVersionsManager
        {
            get
            {
                if (appVersionsManager == null)
                {
                    appVersionsManager = new AppVersionsManager(context);
                }

                return appVersionsManager;
            }
        }
        private GameManager gameManager;
        public GameManager GameManager
        {
            get
            {
                if (gameManager == null)
                {
                    gameManager = new GameManager(context);
                }

                return gameManager;
            }
        }
        private GameMembersManager gameMembersManager;
        public GameMembersManager GameMembersManager
        {
            get
            {
                if (gameMembersManager == null)
                {
                    gameMembersManager = new GameMembersManager(context);
                }

                return gameMembersManager;
            }
        }
        private GameRoundsManager gameRoundsManager;
        public GameRoundsManager GameRoundsManager
        {
            get
            {
                if (gameRoundsManager == null)
                {
                    gameRoundsManager = new GameRoundsManager(context);
                }

                return gameRoundsManager;
            }
        }
        private StatsManager statsManager;
        public StatsManager StatsManager
        {
            get
            {
                if (statsManager == null)
                {
                    statsManager = new StatsManager(context);
                }

                return statsManager;
            }
        }
        private UserStatsManager userStatsManager;
        public UserStatsManager UserStatsManager
        {
            get
            {
                if (userStatsManager == null)
                {
                    userStatsManager = new UserStatsManager(context);
                }

                return userStatsManager;
            }
        }
        
    }
}
