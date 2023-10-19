
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema;

namespace Stack.Entities.DatabaseEntities.Auth
{
    public class RegistrationRequest : BaseEntity
    {
        public string PhoneNumber { get; set; }
        public string PhoneNumberSynonym { get; set; }
        public string? Email { get; set; } //For social logins only

        public string OTP { get; set; }
        public DateTime OTPExpiryDate { get; set; }
    }

}
