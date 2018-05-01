using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using static ComplianceTrainer.Models.Deserializers;

namespace ComplianceTrainer.Models
{
    public class UserQuizViewModel
    {
        public bool isHR { get; set; }
        public bool isManager { get; set; }
        public int QuizId { get; set; }
        public string title { get; set; }
        public string description { get; set; }
        public string media { get; set; }
        public bool isTaken { get; set; }
        public DateTime startDate { get; set; }
        public DateTime preferDate { get; set; }
        public DateTime expiredDate { get; set; }
        public List<UserQuizVMQuestion> questions { get; set; }
        public List<UserQuizGroup> groups { get; set; }
        public List<JUserQuizQuestionAnswer> juqqas { get; set; }

        public UserQuizGroup GroupToUserQuizGroup(Group group)
        {
            UserQuizGroup uqGroup = new UserQuizGroup();
            uqGroup.id = group.id;
            uqGroup.name = group.name;
            return uqGroup;
        }
    }

    public class UserQuizVMQuestion
    {
        public int id { get; set; }
        public string text { get; set; }
        public string type { get; set; }
        public List<UserQuizVMAnswer> answers { get; set; }
    }

    public class UserQuizVMAnswer
    {
        public int id { get; set; }
        public string answerText { get; set; }
        public bool isCorrect { get; set; }
    }

    public class UserQuizGroup
    {
        public int id { get; set; }
        public string name { get; set; }
    }

}