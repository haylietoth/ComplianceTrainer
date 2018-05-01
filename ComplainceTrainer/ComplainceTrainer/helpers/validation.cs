using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.DirectoryServices;
using ComplianceTrainer.Models;
using System.Configuration;
using System.Data.SqlClient;
using System.Data;
using ComplianceTrainer.helpers;
using System.DirectoryServices.AccountManagement;
using System.Web.Mvc;

namespace ComplianceTrainer
{
    public class validation
    {
        string managers = (ConfigurationManager.AppSettings["managers"]);
        string hrGroup = (ConfigurationManager.AppSettings["HRGroup"]);

        public bool getUserCredentials(HttpRequestBase Request)
        {
            ADSearcher ad = new ADSearcher();
            UserPrincipal user = ad.findCurrentUserName(Request);

            using (var context = new PrincipalContext(ContextType.Domain))
            {
                try
                {
                    User myUser = new User();
                    myUser = ad.findByUserName(user);

                    //checks to see if the user exists in the database, if not add them
                    Queries query = new Queries();
                    query.checkExistingUser(myUser);

                    UserViewModel vmUser = new UserViewModel();
                    vmUser = myUser.userToModel(myUser);

                    if (user.IsMemberOf(GroupPrincipal.FindByIdentity(context, hrGroup)))
                    {
                        vmUser.isHR = true;
                        vmUser.isManager = true;
                    }
                    else if (user.IsMemberOf(GroupPrincipal.FindByIdentity(context, managers)))
                    {
                        vmUser.isHR = false;
                        vmUser.isManager = true;
                    }
                    else
                    {
                        vmUser.isManager = false;
                        vmUser.isHR = false;
                    }

                    sessionStorage session = new sessionStorage();
                    session.setSessionUser(vmUser);
                    return true;
                }
                catch (Exception e)
                {
                    return false;
                }
            }           
        }

        public bool checkUserAuth(UserViewModel vmUser, string level)
        {        
            if (level == hrGroup)
            {
                return vmUser.isHR;
            }
            else if(level == managers)
            {
                return vmUser.isManager;
            }
            return false;
        }


        //used when converting a user to a model
        public bool validateHR(string[] userGroups)
        {
            foreach(string group in userGroups)
            {
                if(group == hrGroup)
                {
                    return true;
                }
            }
            return false;
        }

        //used when converting a user to a model
        public bool validateManager(string[] userGroups)
        {
            foreach (string group in userGroups)
            {
                if (group == managers)
                {
                    return true;
                }
            }
            return false;
        }

        // not used in actual application this is for test purposes where there is no access to active directory
        public User validate()
        {
            DBContext db = new DBContext();
            User myUser = new User();
            sessionStorage session = new sessionStorage();
            myUser = db.Users.First(User => User.SAMAccountName == "Administrator");
            return myUser;
        }
    }
}