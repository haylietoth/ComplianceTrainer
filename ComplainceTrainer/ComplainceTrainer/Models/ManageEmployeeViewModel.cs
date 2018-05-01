using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ComplianceTrainer.Models
{
    public class ManageEmployeeViewModel
    {
        public int id { get; set; }
        public string firstName { get; set; }
        public string lastName { get; set; }
        public string email { get; set; }
        public string SAMAccountName { get; set; }
        public string manager { get; set; }
        public bool hasUngradedQuiz { get; set; }

        public ManageEmployeeViewModel generateViewModel(User user)
        {
            ManageEmployeeViewModel vm = new ManageEmployeeViewModel();
            vm.id = user.id;
            vm.firstName = user.firstName;
            vm.lastName = user.lastName;
            vm.email = user.email;
            vm.manager = user.manager;
            vm.SAMAccountName = user.SAMAccountName;
            vm.hasUngradedQuiz = false;
            return vm;
        }
    }
}