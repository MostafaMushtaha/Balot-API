using Stack.Core.Managers.Groups;
using Stack.Core.Managers.Modules.Auth;
using Stack.Core.Managers.Modules.Groups;
using Stack.Core.Managers.Modules.UserProfile;
using Stack.Core.Managers.Social;
using Stack.Core.Managers.Users;

namespace Stack.Core
{
    public interface IUnitOfWork
    {
        public Task<bool> SaveChangesAsync();
        public Task BeginTransactionAsync();
        public Task CommitTransactionAsync();
        public Task RollbackTransactionAsync();
        public ApplicationUserManager UserManager { get; }
        public ApplicationRoleManager RoleManager { get; }
        public RegistrationRequestManager RegistrationRequestManager { get; }
        public ProfileManager ProfileManager { get; }
        public FriendsManager FriendsManager { get; }
        public GroupsManager GroupsManager { get; }
        public GroupMembersManager GroupMembersManager { get; }
        public ProfileSettingsManager ProfileSettingsManager { get; }
        public NotificationsManager NotificationsManager { get; }
        public UserDeviceManager UserDeviceManager { get; }

        // public OTPRequestManager OTPRequestManager { get; }
        public UserConnectionIDsManager UserConnectionIDsManager { get; }
        public AppVersionsManager AppVersionsManager { get; }
    }
}
