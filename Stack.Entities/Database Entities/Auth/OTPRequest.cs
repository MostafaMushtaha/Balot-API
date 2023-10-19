
using Microsoft.AspNetCore.Identity;
using Stack.Entities.DatabaseEntities.User;
using System.ComponentModel.DataAnnotations.Schema;

namespace Stack.Entities.DatabaseEntities.Auth
{
    public class OTPRequest
    {
        public long ID { get; set; }
        public string UserID { get; set; }

        public string InternationalNumber { get; set; }
        public string NationalNumber { get; set; }
        public string DialCode { get; set; }
        public string ISOCode { get; set; }

        public string OTP { get; set; }

        public DateTime CreationDate { get; set; }
        public DateTime ExpiryDate { get; set; }

        public string? PasswordResetToken { get; set; }
        public DateTime? PasswordResetExpiryDate { get; set; }
        public bool? PasswordTokenIsUsed { get; set; }

        public int RequestType { get; set; }

        public bool IsUsed { get; set; }

        [ForeignKey("UserID")]

        public ApplicationUser User { get; set; }

    }

}
