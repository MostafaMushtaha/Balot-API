using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;

namespace Stack.Entities.DomainEntities.Auth
{
    public class ApplicationUsersModerationModel
    {
        public List<UsersDetailsModerationModel> Users { get; set; }
        public int TotalCount { get; set; }
    }

    public class UsersDetailsModerationModel
    {
        public string ID { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }

        public string FullName { get; set; }
        public string ProfileImage { get; set; }

        public string PhoneNumber { get; set; }
        public int Status { get; set; }
        public usersListWorkRoleModel WorkRole { get; set; }
    }

    public class usersListWorkRoleModel
    {
        public long? ID { get; set; }
        public string TitleEN { get; set; }
        public string TitleAR { get; set; }
    }
}
