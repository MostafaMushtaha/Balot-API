using Microsoft.AspNetCore.Identity;
using Stack.Entities.DatabaseEntities.UserProfile;
using System.ComponentModel.DataAnnotations.Schema;
using Stack.Entities.DatabaseEntities.Notifications;
using Stack.Entities.DatabaseEntities.Modules.User;
using Stack.Entities.DatabaseEntities.Auth;
using Stack.Entities.DatabaseEntities.Groups;

namespace Stack.Entities.DatabaseEntities.User
{
    public class ApplicationUser : IdentityUser
    {
        public string FullName { get; set; }
        public string Email { get; set; }
        public string ReferenceNumber { get; set; }

        // public string? AppleId { get; set; }
        public int Status { get; set; }
        public int Language { get; set; }
        public int Gender { get; set; }
        public int VerificationMethod { get; set; }
        public string? RefreshToken { get; set; }
        public DateTime? Birthdate { get; set; }
        public DateTime? LastLogin { get; set; }
        public virtual Profile UserProfile { get; set; }
        public virtual UserStats UserStats { get; set; }
        public virtual ICollection<UserDevice>? Devices { get; set; }

        // public virtual ICollection<OTPRequest>? OTPRequests { get; set; }
        public virtual ICollection<Notification>? Notifications { get; set; }
        public virtual ICollection<Group_Member>? GroupMember { get; set; }
        public virtual ICollection<Friends>? Friends { get; set; }
    }
}
