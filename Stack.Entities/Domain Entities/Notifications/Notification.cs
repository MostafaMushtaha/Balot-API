using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Stack.Entities.DatabaseEntities.User;
using Stack.Entities.Enums.Modules.Notifications;

namespace Stack.Entities.DomainEntities.Notifications
{
    public class NotificationDTO
    {

        public long ID { get; set; }
        public string? Title { get; set; }
        public string SenderID { get; set; }
        public string UserID { get; set; }
        public string Message { get; set; }
        public string? Thumbnail { get; set; }
        public int Type { get; set; }
        public string ReferenceID { get; set; }
        public DateTime CreationDate { get; set; }
        public bool IsRead { get; set; }

        public bool? IsPushOnly { get; set; }

        public void CreateUserRegistered(string SenderID, string RecipientID, string ReferenceID, string FullName, string? Thumbnail)
        {
            this.SenderID = SenderID;
            this.UserID = RecipientID;
            this.Title = "A new talent emerged!";
            this.Message = FullName + " from your contacts has joined, connect with them.";
            this.Thumbnail = Thumbnail;
            this.Type = (int)NotificationTypes.UserRegistered;
            this.ReferenceID = ReferenceID; //Profile id
            this.CreationDate = DateTime.UtcNow;
            this.IsRead = false;
        }

        public void CreateFollowRequest(string SenderID, string RecipientID, long ProfileID, string FullName, string? Thumbnail)
        {
            this.SenderID = SenderID;
            this.UserID = RecipientID;
            this.Title = "Connection Request";
            this.Message = FullName + " sent you a connection request";
            this.Thumbnail = Thumbnail;
            this.Type = (int)NotificationTypes.FollowRequest;
            this.ReferenceID = ProfileID.ToString(); //Profile id
            this.CreationDate = DateTime.UtcNow;
            this.IsRead = false;
        }

        public void CreateCircleInvitation(long CircleID, long InvitationID, string RecipientID, string CircleName, string Role, string? Thumbnail)
        {
            this.SenderID = CircleID.ToString();
            this.UserID = RecipientID;
            this.Title = "Circle Invitation";
            this.Message = "You have been invited to join circle" + CircleName + " as a " + Role;
            this.Thumbnail = Thumbnail;
            this.Type = (int)NotificationTypes.CircleInvitation;
            this.ReferenceID = InvitationID.ToString();
            this.CreationDate = DateTime.UtcNow;
            this.IsRead = false;
        }

        public void PostLiked(string UserID, long LikerProfileID, string LikerName, long PostID)
        {
            this.SenderID = LikerProfileID.ToString();
            this.UserID = UserID;
            this.Title = "New post like !";
            this.Message = LikerName + " liked your post";
            this.Thumbnail = Thumbnail;
            this.Type = (int)NotificationTypes.PostLiked;
            this.ReferenceID = PostID.ToString();
            this.CreationDate = DateTime.UtcNow;
            this.IsRead = false;
        }

        public void PostCommented(string UserID, long CommenterProfileID, string CommenterName, long PostID)
        {
            this.SenderID = CommenterProfileID.ToString();
            this.UserID = UserID;
            this.Title = "New post comment !";
            this.Message = CommenterName + " commented on your post";
            this.Thumbnail = Thumbnail;
            this.Type = (int)NotificationTypes.PostCommented;
            this.ReferenceID = PostID.ToString();
            this.CreationDate = DateTime.UtcNow;
            this.IsRead = false;
        }

        public void ProfileFollowed(string SenderID, string RecipientID, string FollowerName, long SenderProfileID, string? Thumbnail)
        {
            this.SenderID = SenderID;
            this.UserID = RecipientID;
            this.Title = "You have a new follow";
            this.Message = FollowerName + " is following you";
            this.Thumbnail = Thumbnail;
            this.Type = (int)NotificationTypes.UserFollowed;
            this.ReferenceID = SenderProfileID.ToString();
            this.CreationDate = DateTime.UtcNow;
            this.IsRead = false;
        }

        public void FollowRequestSent(string SenderID, string RecipientID, string FollowerName, long SenderProfileID, long FollowRequestID, string? Thumbnail)
        {
            this.SenderID = SenderID;
            this.UserID = RecipientID;
            this.Title = "Follow request";
            this.Message = FollowerName + " is requesting to follow you";
            this.Thumbnail = Thumbnail;
            this.Type = (int)NotificationTypes.FollowRequest;
            this.ReferenceID = SenderProfileID.ToString();
            this.CreationDate = DateTime.UtcNow;
            this.IsRead = false;
        }

        public void FollowRequestAccepted(string SenderID, string RecipientID, string FollowerName, long SenderProfileID, string? Thumbnail)
        {
            this.SenderID = SenderID;
            this.UserID = RecipientID;
            this.Title = "Follow request accepted";
            this.Message = FollowerName + " accepted your follow request";
            this.Thumbnail = Thumbnail;
            this.Type = (int)NotificationTypes.FollowAcceptance;
            this.ReferenceID = SenderProfileID.ToString();
            this.CreationDate = DateTime.UtcNow;
            this.IsRead = false;
        }

        public void ApplicationSubmitted(long RequestID, string ApplicantID, string PhaseHandlerUserID, string ApplicantName, string RoleName, string? Thumbnail)
        {
            this.SenderID = ApplicantID;
            this.UserID = PhaseHandlerUserID;
            this.Title = "New submission for " + RoleName;
            this.Message = ApplicantName + " submitted to role '" + RoleName + "'";
            this.Thumbnail = Thumbnail;
            this.Type = (int)NotificationTypes.UserAppliedToRequest;
            this.ReferenceID = RequestID.ToString();
            this.CreationDate = DateTime.UtcNow;
            this.IsRead = false;
        }

        public void JobInvitationSent(string userID, long CircleID, long RequestID, string CircleName, string RequestTitle, string? Thumbnail)
        {
            this.SenderID = CircleID.ToString();
            this.UserID = userID;
            this.Title = "Job invitation from " + CircleName + "!";
            this.Message = "You have a job invitation to request " + RequestTitle;
            this.Thumbnail = Thumbnail;
            this.Type = (int)NotificationTypes.JobInvitation;
            this.ReferenceID = RequestID.ToString();
            this.CreationDate = DateTime.UtcNow;
            this.IsRead = false;
        }

        public void ApplicantAccepted(string userID, long CircleID, string CircleName, long ApplicationID, string? Thumbnail)
        {
            this.SenderID = CircleID.ToString();
            this.UserID = userID;
            this.Title = "Job acceptance 🎉";
            this.Message = "Your submission for circle '" + CircleName + " have been accepted";
            this.Thumbnail = Thumbnail;
            this.Type = (int)NotificationTypes.ApplicantAccepted;
            this.ReferenceID = ApplicationID.ToString();
            this.CreationDate = DateTime.UtcNow;
            this.IsRead = false;
        }

        public void ApplicantForfeit(string userID, string ApplicantID, string ApplicantName, string RoleName, long RequestID, string? Thumbnail)
        {
            this.SenderID = ApplicantID;
            this.UserID = userID;
            this.Title = "Applicant forfeit";
            this.Message = ApplicantName + " has forfeit from role " + RoleName;
            this.Thumbnail = Thumbnail;
            this.Type = (int)NotificationTypes.ApplicantForfeit;
            this.ReferenceID = RequestID.ToString();
            this.CreationDate = DateTime.UtcNow;
            this.IsRead = false;
        }

        public void RoleLaunched(string userID, string RoleName, long ApplicationID, string? Thumbnail)
        {
            this.SenderID = "system";
            this.UserID = userID;
            this.Title = "Get to work 💼";
            this.Message = "Your job for role " + RoleName + " is starting";
            this.Thumbnail = Thumbnail;
            this.Type = (int)NotificationTypes.RoleJobLaunched;
            this.ReferenceID = ApplicationID.ToString();
            this.CreationDate = DateTime.UtcNow;
            this.IsRead = false;
        }

        public void RoleDateChanged(string userID, string RoleName, long ApplicationID, string? Thumbnail)
        {
            this.SenderID = "system";
            this.UserID = userID;
            this.Title = "Role date changed";
            this.Message = "Your job for role " + RoleName + " date changed, please review";
            this.Thumbnail = Thumbnail;
            this.Type = (int)NotificationTypes.RoleJobDateChanged;
            this.ReferenceID = ApplicationID.ToString();
            this.CreationDate = DateTime.UtcNow;
            this.IsRead = false;
        }

        public void RequestCancelled(string userID, string RoleName, long ApplicationID, string? Thumbnail)
        {
            this.SenderID = "system";
            this.UserID = userID;
            this.Title = "Job cancelled";
            this.Message = "The role " + RoleName + " has been cancelled";
            this.Thumbnail = Thumbnail;
            this.Type = (int)NotificationTypes.RequestCancelled;
            this.ReferenceID = ApplicationID.ToString();
            this.CreationDate = DateTime.UtcNow;
            this.IsRead = false;
        }

        public void CircleApproved(string userID, long CircleID, string CircleName, string? Thumbnail)
        {
            this.SenderID = "system";
            this.UserID = userID;
            this.Title = "Circle approved";
            this.Message = "Your circle " + CircleName + " have been approved";
            this.Thumbnail = Thumbnail;
            this.Type = (int)NotificationTypes.CircleApproved;
            this.ReferenceID = CircleID.ToString();
            this.CreationDate = DateTime.UtcNow;
            this.IsRead = false;
        }

        public void RecommendedRoles(string userID, int RolesCount)
        {
            this.SenderID = "system";
            this.UserID = userID;
            this.Title = "New roles for you";
            this.Message = "There are " + RolesCount + " roles that matches you, apply to them now !";
            this.Thumbnail = Thumbnail;
            this.Type = (int)NotificationTypes.RecommendedRoles;
            this.ReferenceID = "-";
            this.CreationDate = DateTime.UtcNow;
            this.IsRead = false;
            this.IsPushOnly = true;
        }

        public void RecommendedTalents(string UserID, string RoleTitle, int TalentsCount)
        {
            this.SenderID = "system";
            this.UserID = UserID;
            this.Title = "Recommended Talents";
            this.Message = "There are " + TalentsCount + " talents that matches role " + RoleTitle + ", go scout them now !";
            this.Thumbnail = Thumbnail;
            this.Type = (int)NotificationTypes.RecommendedTalents;
            this.ReferenceID = "-";
            this.CreationDate = DateTime.UtcNow;
            this.IsRead = false;
            this.IsPushOnly = true;
        }

        public void InactivityNotification(string userID, string message)
        {
            this.SenderID = "system";
            this.UserID = userID;
            this.Title = "LConnect";
            this.Message = message;
            this.Thumbnail = Thumbnail;
            this.Type = (int)NotificationTypes.RecommendedRoles;
            this.ReferenceID = "-";
            this.CreationDate = DateTime.UtcNow;
            this.IsRead = false;
            this.IsPushOnly = true;
        }
    }
}
