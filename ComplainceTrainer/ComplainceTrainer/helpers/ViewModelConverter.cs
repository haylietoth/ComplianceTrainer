using ComplianceTrainer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using static ComplianceTrainer.Models.Deserializers;

namespace ComplianceTrainer.helpers
{
    public class ViewModelConverter
    {
        public UserViewModel userToModel(User myUser)
        {
            validation validate = new validation();
            UserViewModel vmUser = new UserViewModel();
            vmUser.firstName = myUser.firstName;
            vmUser.lastName = myUser.lastName;
            vmUser.email = myUser.email;
            vmUser.SAMAccountName = myUser.SAMAccountName;
            vmUser.manager = myUser.manager;
            vmUser.userGroups = myUser.userGroups.Split(',');
            vmUser.isHR = false;
            vmUser.isManager = false;
            if (validate.validateHR(vmUser.userGroups))
            {
                vmUser.isHR = true;
            }
            else
            {
                if (validate.validateManager(vmUser.userGroups))
                {
                    vmUser.isManager = true;
                }
            }

            return vmUser;
        }

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

        public JUserQuizQuestionAnswer UserQuizQuestionAnswerToJModel(UserQuizQuestionAnswer uqqa)
        {
            JUserQuizQuestionAnswer juqqa = new JUserQuizQuestionAnswer();
            juqqa.id = uqqa.id;
            juqqa.quizId = uqqa.quizId;
            juqqa.questionId = uqqa.questionId;
            juqqa.answerId = uqqa.answerId;
            juqqa.userId = uqqa.userId;
            juqqa.text = uqqa.text;
            if (uqqa.isChecked != null)
            {
                juqqa.isChecked = (bool)uqqa.isChecked;
            }
            else
            {
                juqqa.isChecked = false;
            }
            if(uqqa.isApproved != null)
            {
                juqqa.isApproved = (bool)uqqa.isApproved;
            }
            else
            {
                juqqa.isApproved = false;
            }
            return juqqa;
        }
    }
}