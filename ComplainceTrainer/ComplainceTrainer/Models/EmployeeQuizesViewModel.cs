using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ComplianceTrainer.Models
{
    public class EmployeeQuizesViewModel
    {
        public int userId { get; set; }
        public string firstName { get; set; }
        public string lastName { get; set; }
        public int quizId { get; set; }
        public string quizTitle { get; set; }
        public bool isCompleted { get; set; }
        public bool isGraded { get; set; }
        public double percentCorrect { get; set; }
        public System.DateTime startDate { get; set; }
        public System.DateTime preferredDate { get; set; }
        public System.DateTime expirationDate { get; set; }
    }
}