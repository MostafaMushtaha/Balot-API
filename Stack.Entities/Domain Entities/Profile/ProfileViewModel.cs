
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;

namespace Stack.Entities.DomainEntities.Modules.Profile
{
    public class ProfileViewModel
    {
        public long ID { get; set; }
        public string UserID { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }

        public string FullName { get; set; }
        public virtual ProfileWorkRoleModel? WorkRole { get; set; }
        public string? Thumbnail { get; set; }
        public string? Address { get; set; }
        public string? Country { get; set; }
        public string? Biography { get; set; }
        public double Rating { get; set; }
        public double CompletionPercentage { get; set; }

        public int Status { get; set; }
        public int? ProfileStatus { get; set; }

        public string PhoneNumber { get; set; }
        public string? Email { get; set; }
        public int PostsCount { get; set; }
        public int FollowersCount { get; set; }
        public int FollowingCount { get; set; }
        public int CompletedJobsCount { get; set; }

        public string? TwitterHandle { get; set; }
        public string? FacebookHandle { get; set; }

        public bool FollowRequestSent { get; set; }
        public bool IsFollowed { get; set; }
        public bool IsFeatured { get; set; }
        public bool IsPendingFeatured { get; set; }
        public bool IsPrivateProfile { get; set; }

        public List<ProfileInterestModel> Interests { get; set; }
        public TutorialViewModel? Tutorial { get; set; }
    }

    public class TutorialViewModel
    {
        public bool Initial { get; set; }
        public bool CircleManagement { get; set; }
        public bool ApplicantFilteration { get; set; }
    }

    public class ProfileWorkRoleModel
    {
        public long ID { get; set; }
        public string TitleEN { get; set; }
        public string TitleAR { get; set; }
    }

    public class ProfileInterestModel
    {
        public long ID { get; set; }
        public string TitleEN { get; set; }
        public string TitleAR { get; set; }
    }

}
