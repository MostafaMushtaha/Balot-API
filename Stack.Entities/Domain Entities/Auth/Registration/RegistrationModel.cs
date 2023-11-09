﻿
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using Stack.Entities.DatabaseEntities.User;

namespace Stack.Entities.DomainEntities.Auth
{
    public class RegistrationModel
    {
        public string Email { get; set; }
        public string Password { get; set; }
        public string Fullname { get; set; }
        public int Gender { get; set; }
        // public UserStats UserStats { get; set; }
        // public string UserName { get; set; }



    }

}
