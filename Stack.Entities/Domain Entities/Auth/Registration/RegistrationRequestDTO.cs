
using Microsoft.AspNetCore.Identity;
using Stack.DTOs.Requests.Modules.Auth;
using System.ComponentModel.DataAnnotations.Schema;

namespace Stack.Entities.DomainEntities.Auth.Registration
{
    public class RegistrationRequestDTO : DomainBaseEntity
    {
        public string EMail { get; set; }
        public string? Email { get; set; } //For social logins only
        public string? ImageUrl { get; set; } //For social logins only
        public string Password { get; set; }
        public bool IsPotentialCircleCreator { get; set; }
        // public DateTime OTPExpiryDate { get; set; }


        public void CreateRequest(string Email, string Password)
        {
            this.Email = Email;
            this.Password = Password;

            //TODO: UTC 
            var timeZoneInfo = TimeZoneInfo.FindSystemTimeZoneById("Egypt Standard Time");
            DateTimeOffset localServerTime = DateTimeOffset.Now;
            DateTimeOffset localTime = TimeZoneInfo.ConvertTime(localServerTime, timeZoneInfo);
            this.CreationDate = localTime.DateTime.AddMinutes(2);
            // this.OTPExpiryDate = localTime.DateTime.AddMinutes(2);
        }

        public void Google_CreateRequest(Google_VerifyPhoneNumberModel model, string OTP)
        {
            // this.PhoneNumber = model.PhoneNumber;
            // this.PhoneNumberSynonym = model.PhoneNumberSynonym;
            this.Email = model.Email;
            this.ImageUrl = model.ImageUrl;
            // this.OTP = OTP;

            //TODO: UTC 
            var timeZoneInfo = TimeZoneInfo.FindSystemTimeZoneById("Egypt Standard Time");
            DateTimeOffset localServerTime = DateTimeOffset.Now;
            DateTimeOffset localTime = TimeZoneInfo.ConvertTime(localServerTime, timeZoneInfo);
            this.CreationDate = localTime.DateTime.AddMinutes(2);
            // this.OTPExpiryDate = localTime.DateTime.AddMinutes(2);
        }


        public void UpdateRequest(string phoneNumber, string phoneNumberSynonym, string OTP)
        {
            this.Email = Email;

            //TODO: UTC 
            var timeZoneInfo = TimeZoneInfo.FindSystemTimeZoneById("Egypt Standard Time");
            DateTimeOffset localServerTime = DateTimeOffset.Now;
            DateTimeOffset localTime = TimeZoneInfo.ConvertTime(localServerTime, timeZoneInfo);
            this.CreationDate = localTime.DateTime.AddMinutes(2);
            // this.OTPExpiryDate = localTime.DateTime.AddMinutes(2);
        }

        //Social login (No password included)
        public void Google_UpdateRequest(Google_VerifyPhoneNumberModel model, string OTP)
        {
            this.Email = model.Email;
            this.ImageUrl = model.ImageUrl;

            //TODO: UTC 
            var timeZoneInfo = TimeZoneInfo.FindSystemTimeZoneById("Egypt Standard Time");
            DateTimeOffset localServerTime = DateTimeOffset.Now;
            DateTimeOffset localTime = TimeZoneInfo.ConvertTime(localServerTime, timeZoneInfo);
            this.CreationDate = localTime.DateTime.AddMinutes(2);
        }

    }



}
