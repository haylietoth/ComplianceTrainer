using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ComplianceTrainer.Models
{
    //Active Driectory User
    public class UserViewModel
    {
        // First Name
        public String firstName { get; set; }
        // Last Name
        public String lastName { get; set; }
        //email
        public String email { get; set; }

        public bool isHR { get; set; }

        public bool isManager { get; set; }

        public String SAMAccountName { get; set; }

        public String[] userGroups { get; set; }

        public String manager { get; set; }

        public User modelToUser(UserViewModel vmUser)
        {
            User myUser = new User();
            myUser.firstName = vmUser.firstName;
            myUser.lastName = vmUser.lastName;
            myUser.email = vmUser.email;
            myUser.SAMAccountName = vmUser.SAMAccountName;
            myUser.manager = vmUser.manager;
            myUser.userGroups = string.Join(",", vmUser.userGroups);
            return myUser;
        }
    }

}