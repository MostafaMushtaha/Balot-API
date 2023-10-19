
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;

namespace Stack.Entities.DomainEntities.User
{
    public class UserDevicesDTO
    {
        public long ID { get; set; }
        public string Token { get; set; }
        public string UserID { get; set; }
        public bool IsAndroid { get; set; }
        public bool IsActive { get; set; }

        public UserDevicesDTO(string Token, string UserID, bool IsAndroid)
        {
            this.Token = Token;
            this.UserID = UserID;
            this.IsAndroid = IsAndroid;
            this.IsActive = true;
        }
    }

}
