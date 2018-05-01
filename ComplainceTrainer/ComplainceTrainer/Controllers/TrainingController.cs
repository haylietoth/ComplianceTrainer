using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ComplianceTrainer.helpers;
using System.DirectoryServices.AccountManagement;
using System.Configuration;
using ComplianceTrainer.Models;
using Microsoft.Ajax.Utilities;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using static ComplianceTrainer.Models.Deserializers;

namespace ComplianceTrainer.Controllers
{
    public class TrainingController : Controller
    {
        sessionStorage session = new sessionStorage();
        validation val = new validation();
        String managers = (ConfigurationManager.AppSettings["managers"]);
        String hrGroup = (ConfigurationManager.AppSettings["HRGroup"]);

        public ActionResult MyTraining()
        {
            if (session.getSessionUser() != null)
            {
                return View(session.getSessionUser());
            }
            else
            {
                if (val.getUserCredentials(Request))
                {
                    return View(session.getSessionUser());
                }
                else
                {
                    return RedirectToAction("Login", "Home");
                }
            }
        }

        public ActionResult GetAllUsers()
        {
            UserViewModel vmUser = session.getSessionUser();
            if (vmUser == null)
            {
                if (!val.getUserCredentials(Request))
                {
                    return RedirectToAction("Login", "Home");
                }
                vmUser = session.getSessionUser();
            }
            if (!val.checkUserAuth(vmUser, hrGroup))
            {
                return RedirectToAction("Index", "Home", new { error = "Invalid User Credentials" });
            }

            List<ManageEmployeeViewModel> userQuizes = new List<ManageEmployeeViewModel>();
            Queries query = new Queries();
            List<User> Users = query.getAllUsers();
            foreach (User user in Users)
            {
                ManageEmployeeViewModel userQuiz = new ManageEmployeeViewModel();
                userQuiz = userQuiz.generateViewModel(user);                
                List<UserQuizQuestionAnswer> usersQuizes = query.getAllUserQuizes(user.id);
                foreach (UserQuizQuestionAnswer uqqa in usersQuizes)
                {
                    if ((bool)!uqqa.isChecked)
                    {
                        userQuiz.hasUngradedQuiz = true;
                    }
                }
                userQuizes.Add(userQuiz);
            }
            return Json(userQuizes, JsonRequestBehavior.AllowGet);
        }

        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        //group comment start
        //sean uncomment start

        //public ActionResult GetAllManagersUsers()
        //{

        //    UserViewModel vmUser = session.getSessionUser();
        //    if (vmUser == null)
        //    {
        //        if (!val.getUserCredentials(Request))
        //        {
        //            return RedirectToAction("Login", "Home");
        //        }
        //        vmUser = session.getSessionUser();
        //    }
        //    if (!val.checkUserAuth(vmUser, managers))
        //    {
        //        return RedirectToAction("Index", "Home", new { error = "Invalid User Credentials" });
        //    }
        //    ADSearcher ad = new ADSearcher();
        //    List<User> allUsers = ad.getDirectReports(vmUser);
        //    return Json(allUsers, JsonRequestBehavior.AllowGet);
        //}

        //group comment end
        //sean uncomment end

        /// ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        public ActionResult ManageEmployees()
        {
            UserViewModel vmUser = session.getSessionUser();
            if (vmUser == null)
            {
                if (!val.getUserCredentials(Request))
                {
                    return RedirectToAction("Login", "Home");
                }
                vmUser = session.getSessionUser();
            }
            if (!val.checkUserAuth(vmUser, managers))
            {
                return RedirectToAction("Index", "Home", new { error = "Invalid User Credentials" });
            }
            return View(vmUser);
        }

        public ActionResult AllTraining()
        {
            UserViewModel vmUser = session.getSessionUser();
            if (vmUser == null)
            {
                if (!val.getUserCredentials(Request))
                {
                    return RedirectToAction("Login", "Home");
                }
                vmUser = session.getSessionUser();
            }
            if (!val.checkUserAuth(vmUser, hrGroup))
            {
                return RedirectToAction("Index", "Home", new { error = "Invalid User Credentials" });
            }
            return View(vmUser);
        }

        public ActionResult AddTraining()
        {
            UserViewModel vmUser = session.getSessionUser();
            if (vmUser == null)
            {
                if (!val.getUserCredentials(Request))
                {
                    return RedirectToAction("Login", "Home");
                }
                vmUser = session.getSessionUser();
            }
            if (!val.checkUserAuth(vmUser, hrGroup))
            {
                return RedirectToAction("Index", "Home", new { error = "Invalid User Credentials" });
            }
            return View(vmUser);
        }

        public ActionResult updateTraining(int id = 0)
        {
            UserViewModel vmUser = session.getSessionUser();
            if (vmUser == null)
            {
                if (!val.getUserCredentials(Request))
                {
                    return RedirectToAction("Login", "Home");
                }
                vmUser = session.getSessionUser();
            }
            Queries query = new Queries();
            if (id == 0)
            {
                return RedirectToAction("Index", "home", new { error = "Cannot Locate Quiz" });
            }
            UserQuizViewModel uqvmQuiz = GetQuizById(id);
            uqvmQuiz.isHR = vmUser.isHR;
            uqvmQuiz.isManager = vmUser.isManager;
            return View(uqvmQuiz);
        }

        public ActionResult Quiz(int id = 0)
        {
            UserViewModel vmUser = session.getSessionUser();
            if (vmUser == null)
            {
                if (!val.getUserCredentials(Request))
                {
                    return RedirectToAction("Login", "Home");
                }
                vmUser = session.getSessionUser();
            }
            Queries query = new Queries();
            if (id == 0)
            {
                return RedirectToAction("Index", "home", new { error = "Cannot Locate Quiz" });
            }
            else if (!vmUser.userGroups.Contains(query.getGroupById(query.getQuizById(id).groupId).name))
            {
                return RedirectToAction("Index", "home", new { error = "Invalid Quiz" });
            }
            UserQuizViewModel uqvmQuiz = GetQuizById(id);
            uqvmQuiz.isHR = vmUser.isHR;
            uqvmQuiz.isManager = vmUser.isManager;
            return View(uqvmQuiz);
        }
        
        public ActionResult gradeQuiz(int id = 0)
        {
            if (id == 0)
            {
                RedirectToAction("ManageEmployees", "Training");
            }
            UserViewModel vmUser = session.getSessionUser();
            Queries query = new Queries();
            if (vmUser == null)
            {
                if (!val.getUserCredentials(Request))
                {
                    return RedirectToAction("Login", "Home");
                }
                vmUser = session.getSessionUser();
            }
            if (!val.checkUserAuth(vmUser, hrGroup))
            {
                return RedirectToAction("Index", "Home", new { error = "Invalid User Credentials" });
            }
            User user = query.getUserBySam(vmUser.modelToUser(session.getSessionUser()).SAMAccountName);
            GradeViewModel gvmQuiz = GetGradedQuizById(user.id, id);
            return View(gvmQuiz);
        }

        public ActionResult GetAllGroups()
        {
            UserViewModel vmUser = session.getSessionUser();
            if (vmUser == null)
            {
                if (!val.getUserCredentials(Request))
                {
                    return RedirectToAction("Login", "Home");
                }
                vmUser = session.getSessionUser();
            }
            if (!val.checkUserAuth(vmUser, hrGroup))
            {
                return RedirectToAction("Index", "Home", new { error = "Invalid User Credentials" });
            }
            Queries q = new Queries();
            List<Group> groups = new List<Group>();
            groups = q.getAllGroups();          
            List<JGroup> result = new List<JGroup>();
            foreach (Group group in groups)
            {
                JGroup g = new JGroup();
                g = g.generateGroup(group);
                result.Add(g);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult AddQuiz(string quizData)
        {
            UserViewModel vmUser = session.getSessionUser();
            if (vmUser == null)
            {
                if (!val.getUserCredentials(Request))
                {
                    return RedirectToAction("Login", "Home");
                }
                vmUser = session.getSessionUser();
            }
            if (!val.checkUserAuth(vmUser, hrGroup))
            {
                return RedirectToAction("Index", "Home", new { error = "Invalid User Credentials" });
            }
            Queries query = new Queries();
            var result = JsonConvert.DeserializeObject<JQuiz>(quizData);
            if (query.getQuizByTitle(result.title).Count!=0)
            {
                return Json("Duplicate Title", JsonRequestBehavior.AllowGet);
            }
            List<int> quizIds = new List<int>();
            foreach (int groupId in result.groups)
            {
                Quize quiz = new Quize();
                quiz.groupId = Convert.ToInt32(groupId);
                quiz.title = result.title;
                quiz.description = result.description;
                quiz.media = result.media;
                quiz.startDate = result.startDate;
                quiz.preferDate = result.preferredDate;
                quiz.expiredDate = result.expirationDate;
                quizIds.Add(query.addQuiz(quiz));
            }
            return Json(quizIds, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult UpdateQuiz(string quizData)
        {
            UserViewModel vmUser = session.getSessionUser();
            if (vmUser == null)
            {
                if (!val.getUserCredentials(Request))
                {
                    return RedirectToAction("Login", "Home");
                }
                vmUser = session.getSessionUser();
            }
            if (!val.checkUserAuth(vmUser, hrGroup))
            {
                return RedirectToAction("Index", "Home", new { error = "Invalid User Credentials" });
            }
            Queries query = new Queries();
            var result = JsonConvert.DeserializeObject<JQuiz>(quizData);

            List<int> quizIds = new List<int>();
            List<Quize> quizes = query.getQuizByTitle(result.title);
            List<int> currentGroups = new List<int>();

            foreach(Quize q in quizes)
            {
                if (!result.groups.Contains(q.groupId))
                {
                    query.RemoveQuiz(q.id);
                }
                else
                {
                    quizIds.Add(q.id);
                    q.description = result.description;
                    q.media = result.media;
                    q.startDate = result.startDate;
                    q.preferDate = result.preferredDate;
                    q.expiredDate = result.expirationDate;
                    query.updateExistingQuiz(q);
                    currentGroups.Add(q.groupId);
                }
            }
            foreach(int newGroupId in result.groups)
            {
                if (!currentGroups.Contains(newGroupId)){
                    Quize newQuiz = new Quize();
                    newQuiz.title = result.title;
                    newQuiz.description = result.description;
                    newQuiz.groupId = newGroupId;
                    newQuiz.media = result.media;
                    newQuiz.startDate = result.startDate;
                    newQuiz.preferDate = result.preferredDate;
                    newQuiz.expiredDate = result.expirationDate;
                    quizIds.Add(query.addQuiz(newQuiz));
                }
            }
            
            return Json(quizIds, JsonRequestBehavior.AllowGet);
        }

        public ActionResult AddQuizQuestionAnswers(string title, string questionData)
        {
            UserViewModel vmUser = session.getSessionUser();
            if (vmUser == null)
            {
                if (!val.getUserCredentials(Request))
                {
                    return RedirectToAction("Login", "Home");
                }
                vmUser = session.getSessionUser();
            }
            if (!val.checkUserAuth(vmUser, hrGroup))
            {
                return RedirectToAction("Index", "Home", new { error = "Invalid User Credentials" });
            }
            var result = JsonConvert.DeserializeObject<JQuestion>(questionData);
            Question question = new Question();
            question.questionText = result.questionText;
            question.questionType = result.questionType;

            Queries query = new Queries();

            List<Quize> quizes = query.getQuizByTitle(title);
            List<int> questionIds = new List<int>();
            foreach (Quize quiz in quizes)
            {
                question.quizId = quiz.id;
                int questionId;
                questionId = query.addQuestion(question);
                questionIds.Add(questionId);
                if (!(result.questionType == "shortAnswer"))
                {
                    foreach (JAnswers jAnswer in result.answers)
                    {
                        Answer answer = new Answer();
                        answer.questionId = questionId;
                        answer.answerText = jAnswer.answerText;
                        answer.isCorrect = jAnswer.isCorrect;
                        query.addAnswer(answer);
                    }
                }
                else
                {
                    Answer answer = new Answer();
                    answer.questionId = questionId;
                    query.addAnswer(answer);
                }               
            }
            return Json(questionIds, JsonRequestBehavior.AllowGet);
        }

        public ActionResult UpdateQuizQuestionAnswers(int[] questionIds, JQuestion questionData)
        {
            UserViewModel vmUser = session.getSessionUser();
            if (vmUser == null)
            {
                if (!val.getUserCredentials(Request))
                {
                    return RedirectToAction("Login", "Home");
                }
                vmUser = session.getSessionUser();
            }
            if (!val.checkUserAuth(vmUser, hrGroup))
            {
                return RedirectToAction("Index", "Home", new { error = "Invalid User Credentials" });
            }
            Queries query = new Queries();
           
            foreach (int index in questionIds)
            {
                Question question = new Question();
                question.id = index;
                question.questionText = questionData.questionText;
                question.questionType = questionData.questionType;
                query.updateExistingQuestion(question);
                List<Answer> answers = query.getAnswersByQuestionId(index);
                if(questionData.questionType != "shortAnswer")
                {
                    for (int i = 0; i < questionData.answers.Count; i++)
                    {
                        answers[i].answerText = questionData.answers[i].answerText;
                        answers[i].isCorrect = questionData.answers[i].isCorrect;
                        query.updateExistingAnswer(answers[i]);
                    }
                }            
            }
            return Json(JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult GetUserQuizes()
        {
            UserViewModel vmUser = session.getSessionUser();
            if (vmUser == null)
            {
                if (!val.getUserCredentials(Request))
                {
                    return RedirectToAction("Login", "Home");
                }
                vmUser = session.getSessionUser();
            }
            Queries query = new Queries();
            User user = query.getUserBySam(vmUser.modelToUser(session.getSessionUser()).SAMAccountName);
            List<EmployeeQuizesViewModel> employeeQuizes = new List<EmployeeQuizesViewModel>();
            string[] groups = user.userGroups.Split(',');
            foreach (string group in groups)
            {
                int groupId = query.getGroupByName(group).id;
                List<Quize> quizesByGroup = query.getQuizesByGroupId(groupId);
                foreach (Quize quiz in quizesByGroup)
                {
                    if (DateTime.Now.Ticks >= quiz.startDate.Ticks)
                    {
                        List<Question> questions = query.getQuestionsByQuizId(quiz.id);
                        if (questions.Count > 0)
                        {
                            EmployeeQuizesViewModel employeeQuiz = new EmployeeQuizesViewModel();
                            employeeQuiz.userId = user.id;
                            employeeQuiz.firstName = user.firstName;
                            employeeQuiz.lastName = user.lastName;
                            employeeQuiz.quizId = quiz.id;
                            employeeQuiz.quizTitle = quiz.title;
                            employeeQuiz.startDate = quiz.startDate;
                            employeeQuiz.preferredDate = quiz.preferDate;
                            employeeQuiz.expirationDate = quiz.expiredDate;
                            employeeQuiz.isCompleted = false;
                            employeeQuiz.isGraded = false;
                            List<UserQuizQuestionAnswer> uqqas = query.getQuizByUserIdQuizId(user.id, quiz.id);
                            double uqqaCount = uqqas.Count();
                            if (uqqaCount > 0)
                            {
                                employeeQuiz.isCompleted = true;
                                employeeQuiz.isGraded = true;
                                double numberCorrect = 0;
                                foreach(UserQuizQuestionAnswer uqqa in uqqas)
                                {
                                    if ( uqqa.isApproved != null && (bool)uqqa.isApproved)
                                    {
                                        numberCorrect++;
                                    }
                                    else if(query.getQuestionById(uqqa.questionId).questionType == "shortAnswer" && uqqa.isChecked == false)
                                    {
                                        uqqaCount--;
                                        employeeQuiz.isGraded = false;
                                    }
                                }
                                employeeQuiz.percentCorrect = Math.Round((numberCorrect / uqqaCount), 2);
                            }                           
                            employeeQuizes.Add(employeeQuiz);
                        }                       
                    }                 
                }
            }
            return Json(employeeQuizes, JsonRequestBehavior.AllowGet);
        }


        [HttpPost]
        public ActionResult GetUserQuizes(int id)
        {
            UserViewModel vmUser = session.getSessionUser();
            if (vmUser == null)
            {
                if (!val.getUserCredentials(Request))
                {
                    return RedirectToAction("Login", "Home");
                }
                vmUser = session.getSessionUser();
            }
            if (!val.checkUserAuth(vmUser, managers))
            {
                return RedirectToAction("Index", "Home", new { error = "Invalid User Credentials" });
            }
            Queries query = new Queries();
            User user = query.getUserById(id);
            List<EmployeeQuizesViewModel> employeeQuizes = new List<EmployeeQuizesViewModel>();
            string[] groups = user.userGroups.Split(',');
            foreach (string group in groups)
            {
                int groupId = query.getGroupByName(group).id;
                List<Quize> quizesByGroup = query.getQuizesByGroupId(groupId);
                foreach (Quize quiz in quizesByGroup)
                {
                    EmployeeQuizesViewModel employeeQuiz = new EmployeeQuizesViewModel();
                    employeeQuiz.userId = user.id;
                    employeeQuiz.firstName = user.firstName;
                    employeeQuiz.lastName = user.lastName;
                    employeeQuiz.quizId = quiz.id;
                    employeeQuiz.quizTitle = quiz.title;
                    employeeQuiz.isCompleted = false;
                    employeeQuiz.isGraded = false;
                    List<UserQuizQuestionAnswer> uqqas = query.getQuizByUserIdQuizId(user.id, quiz.id);
                    if (uqqas.Count > 0)
                    {
                        employeeQuiz.isGraded = true;
                        employeeQuiz.isCompleted = true;
                        foreach(UserQuizQuestionAnswer uqqa in uqqas)
                        {
                            if(query.getQuestionById(uqqa.questionId).questionType == "shortAnswer")
                            {
                                if (uqqa.isApproved != null && !(bool)uqqa.isApproved)
                                {
                                    employeeQuiz.isGraded = false;
                                }
                            }
                        }
                    }
                    employeeQuizes.Add(employeeQuiz);
                }
            }
            return Json(employeeQuizes, JsonRequestBehavior.AllowGet);
        }

        public ActionResult EmployeeQuizes(int id)
        {
            UserViewModel vmUser = session.getSessionUser();
            if (vmUser == null)
            {
                if (!val.getUserCredentials(Request))

                {
                    return RedirectToAction("Login", "Home");
                }
                vmUser = session.getSessionUser();
            }
            if (!val.checkUserAuth(vmUser, managers))
            {
                return RedirectToAction("Index", "Home", new { error = "Invalid User Credentials" });
            }
            ViewBag.id = id;
            return View(vmUser);
        }

        public ActionResult GetAllQuizes()
        {
            UserViewModel vmUser = session.getSessionUser();
            if (vmUser == null)
            {
                if (!val.getUserCredentials(Request))
                {
                    return RedirectToAction("Login", "Home");
                }
                vmUser = session.getSessionUser();
            }
            if (!val.checkUserAuth(vmUser, hrGroup))
            {
                return RedirectToAction("Index", "Home", new { error = "Invalid User Credentials" });
            }
            Queries query = new Queries();
            List<Quize> quizes = query.getUniqueQuizes();
            List<Quize> results = new List<Quize>();
            foreach (Quize quiz in quizes)
            {
                Quize q = new Quize();
                q.id = quiz.id;
                q.title = quiz.title;
                q.description = quiz.description;
                q.startDate = quiz.startDate;
                q.expiredDate = quiz.expiredDate;
                results.Add(q);
            }
            return Json(results, JsonRequestBehavior.AllowGet);
        }
        public GradeViewModel GetGradedQuizById(int uId, int qId)
        {
            int userId = Convert.ToInt32(uId);
            int quizId = Convert.ToInt32(qId);
            Queries query = new Queries();
            Quize quiz = query.getQuizById(quizId);

            GradeViewModel gvmQuiz = new GradeViewModel();

            gvmQuiz.QuizId = quizId;
            gvmQuiz.title = quiz.title;
            gvmQuiz.description = quiz.description;
            gvmQuiz.media = quiz.media;
            gvmQuiz.questions = new List<GradeVMQuestion>();

            List<Question> questions = query.getQuestionsByQuizId(quiz.id);

            foreach (Question question in questions)
            {
                GradeVMQuestion gvmQuestion = new GradeVMQuestion();
                gvmQuestion.id = question.id;
                gvmQuestion.text = question.questionText;
                gvmQuestion.type = question.questionType;
                Answer answer = query.getUserAnswerByQuestionId(userId, question.id);

                gvmQuestion.answerId = answer.id;
                gvmQuestion.answerText = answer.answerText;

                gvmQuiz.questions.Add(gvmQuestion);
            }
            return gvmQuiz;
        }
        public ActionResult RemoveQuiz(int id)
        {
            UserViewModel vmUser = session.getSessionUser();
            if (vmUser == null)
            {
                if (!val.getUserCredentials(Request))
                {
                    return RedirectToAction("Login", "Home");
                }
                vmUser = session.getSessionUser();
            }
            if (!val.checkUserAuth(vmUser, hrGroup))
            {
                return RedirectToAction("Index", "Home", new { error = "Invalid User Credentials" });
            }
            Queries query = new Queries();
            if (query.RemoveQuiz(id))
            {
                return Json("success");
            }
            return Json("Failed");
        }

        public ActionResult ViewQuiz(int id)
        {
            UserViewModel vmUser = session.getSessionUser();
            if (vmUser == null)
            {
                if (!val.getUserCredentials(Request))
                {
                    return RedirectToAction("Login", "Home");
                }
                vmUser = session.getSessionUser();
            }
            if (!val.checkUserAuth(vmUser, hrGroup))
            {
                return RedirectToAction("Index", "Home", new { error = "Invalid User Credentials" });
            }
            UserQuizViewModel uqvm = new UserQuizViewModel();
            uqvm = GetQuizById(id);
            Queries query = new Queries();
            List<UserQuizQuestionAnswer> uqqas = query.getQuizByUserIdQuizId(query.getUserBySam(vmUser.SAMAccountName).id, id);

            if (uqqas.Count > 0)
            {
                uqvm.isTaken = true;
                ViewModelConverter vmConverter = new ViewModelConverter();
                uqvm.juqqas = new List<JUserQuizQuestionAnswer>();
                foreach (UserQuizQuestionAnswer uqqa in uqqas)
                {
                    uqvm.juqqas.Add(vmConverter.UserQuizQuestionAnswerToJModel(uqqa));
                }
            }
            return Json(uqvm, JsonRequestBehavior.AllowGet);
        }

        public ActionResult ViewGradeQuiz(int id)
        {
            UserViewModel vmUser = session.getSessionUser();
            Queries query = new Queries();
            if (vmUser == null)
            {
                if (!val.getUserCredentials(Request))
                {
                    return RedirectToAction("Login", "Home");
                }
                vmUser = session.getSessionUser();
            }
            if (!val.checkUserAuth(vmUser, hrGroup))
            {
                return RedirectToAction("Index", "Home", new { error = "Invalid User Credentials" });
            }
            GradeViewModel gvm = new GradeViewModel();
            User user = query.getUserBySam(vmUser.modelToUser(vmUser).SAMAccountName);
            gvm = GetGradedQuizById(user.id, id);
            List<UserQuizQuestionAnswer> uqqas = query.getQuizByUserIdQuizId(user.id, id);
            
            if (uqqas.Count > 0)
            {
                ViewModelConverter vmConverter = new ViewModelConverter();
                gvm.juqqas = new List<JUserQuizQuestionAnswer>();
                foreach (UserQuizQuestionAnswer uqqa in uqqas)
                {
                    gvm.juqqas.Add(vmConverter.UserQuizQuestionAnswerToJModel(uqqa));
                }
            }
            return Json(gvm, JsonRequestBehavior.AllowGet);
        }

        public UserQuizViewModel GetQuizById(int quizId)
        {
            Queries query = new Queries();
            Quize quiz = query.getQuizById(quizId);

            UserQuizViewModel uqvmQuiz = new UserQuizViewModel();
            List<Quize> quizzes = query.getQuizByTitle(quiz.title);
            List<UserQuizGroup> groups = new List<UserQuizGroup>();
            foreach(Quize q in quizzes)
            {
                groups.Add(uqvmQuiz.GroupToUserQuizGroup(query.getGroupById(q.groupId)));
            }
            uqvmQuiz.groups = groups;
            uqvmQuiz.QuizId = quizId;
            uqvmQuiz.title = quiz.title;
            uqvmQuiz.description = quiz.description;
            uqvmQuiz.media = quiz.media;
            uqvmQuiz.startDate = quiz.startDate;
            uqvmQuiz.preferDate = quiz.preferDate;
            uqvmQuiz.expiredDate = quiz.expiredDate;
            uqvmQuiz.questions = new List<UserQuizVMQuestion>();
            
            List<Question> questions = query.getQuestionsByQuizId(quiz.id);

            foreach (Question question in questions)
            {
                UserQuizVMQuestion uqvmQuestion = new UserQuizVMQuestion();
                uqvmQuestion.id = question.id;
                uqvmQuestion.text = question.questionText;
                uqvmQuestion.type = question.questionType;
                uqvmQuestion.answers = new List<UserQuizVMAnswer>();
                List<Answer> answers = query.getAnswersByQuestionId(question.id);

                foreach (Answer answer in answers)
                {
                    UserQuizVMAnswer uqvmAnswer = new UserQuizVMAnswer();
                    uqvmAnswer.id = answer.id;
                    uqvmAnswer.answerText = answer.answerText;
                    uqvmAnswer.isCorrect = answer.isCorrect;
                    uqvmQuestion.answers.Add(uqvmAnswer);
                }
                uqvmQuiz.questions.Add(uqvmQuestion);
            }
            return uqvmQuiz;
        }

        [HttpPost]
        public ActionResult SubmitQuiz(UserAnswer[] answers)
        {
            UserViewModel vmUser = session.getSessionUser();
            if (vmUser == null)
            {
                if (!val.getUserCredentials(Request))
                {
                    return RedirectToAction("Login", "Home");
                }
                vmUser = session.getSessionUser();
            }

            Queries query = new Queries();
            User user = query.getUserBySam(vmUser.modelToUser(session.getSessionUser()).SAMAccountName);
            List<UserQuizQuestionAnswer> uqqas = new List<UserQuizQuestionAnswer>();
            foreach (UserAnswer answer in answers)
            {              
                UserQuizQuestionAnswer uqqa = new UserQuizQuestionAnswer();
                uqqa.quizId = answer.quizId;
                uqqa.questionId = answer.questionId;
                uqqa.answerId = answer.answerId;
                uqqa.userId = user.id;
                uqqa.text = answer.answerText;
                uqqa.isChecked = false;
                uqqa.isApproved = false;
                uqqas.Add(uqqa);
            }
            uqqas = GradeSubmittedQuiz(GetQuizById(answers[0].quizId), uqqas);
            List<UserQuizQuestionAnswer> addedUqqas = new List<UserQuizQuestionAnswer>();
            addedUqqas = query.submitQuiz(uqqas);
            if(addedUqqas.Count > 0)
            {
                return Json("Quiz Completed", JsonRequestBehavior.AllowGet);
            }
            return Json("Failed to submit quiz", JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public ActionResult SubmitGrade(GradingDecision[] decisions)
        {
            UserViewModel vmUser = session.getSessionUser();
            if (vmUser == null)
            {
                if (!val.getUserCredentials(Request))
                {
                    return RedirectToAction("Login", "Home");
                }
                vmUser = session.getSessionUser();
            }

            Queries query = new Queries();
            User user = query.getUserBySam(vmUser.modelToUser(session.getSessionUser()).SAMAccountName);
            List<UserQuizQuestionAnswer> uqqas = new List<UserQuizQuestionAnswer>();
            foreach (GradingDecision decision in decisions)
            {
                UserQuizQuestionAnswer uqqa = new UserQuizQuestionAnswer();
                uqqa.quizId = decision.quizId;
                uqqa.userId = decision.userId;
                uqqa.questionId = decision.questionId;
                uqqa.isApproved = decision.isApproved;
                uqqas.Add(uqqa);
            }
            List<UserQuizQuestionAnswer> addedUqqas = new List<UserQuizQuestionAnswer>();
            addedUqqas = query.submitQuiz(uqqas);
            if (addedUqqas.Count > 0)
            {
                return Json("Grading Completed", JsonRequestBehavior.AllowGet);
            }
            return Json("Failed to submit grades", JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult RemoveQuestion(int[] ids)
        {
            try
            {
                Queries query = new Queries();               
                foreach (int i in ids)
                {
                    query.RemoveQuestion(i);
                }
                return Json(true, JsonRequestBehavior.AllowGet);
            }
            catch(Exception e)
            {
                return Json(false, JsonRequestBehavior.AllowGet);
            }         
        }

        public ActionResult EditTraining(int id)
        {
            Queries query = new Queries();
            UserQuizViewModel uqvmQuiz = GetQuizById(id);
            return Json(uqvmQuiz, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetQuestionAnswers(string questionId)
        {
            Queries query = new Queries();
            Question question = query.getQuestionById(Convert.ToInt32(questionId));
            List<Answer> answers = query.getAnswersByQuestionId(question.id);
            List<JAnswers> jAnswers = new List<JAnswers>();
            foreach(Answer ans in answers)
            {
                JAnswers jAnswer = new JAnswers();
                jAnswer.answerText = ans.answerText;
                jAnswer.isCorrect = ans.isCorrect;
                jAnswers.Add(jAnswer);
            }
            JQuestion jQuestion = new JQuestion();
            jQuestion.id = questionId;
            jQuestion.questionText = question.questionText;
            jQuestion.questionType = question.questionType;
            jQuestion.answers = jAnswers;

            return Json(jQuestion, JsonRequestBehavior.AllowGet);
        }

        public List<UserQuizQuestionAnswer> GradeSubmittedQuiz(UserQuizViewModel quiz, List<UserQuizQuestionAnswer> uqqas)
        {
            int questionCount = quiz.questions.Count();
            for(int i = 0; i < questionCount; i++)
            {
                if (quiz.questions[i].type != "shortAnswer")
                {
                    for (var j = 0; j < quiz.questions[i].answers.Count(); j++)
                    {
                        if (quiz.questions[i].answers[j].isCorrect && quiz.questions[i].answers[j].id == uqqas[i].answerId)
                        {
                            uqqas[i].isChecked = true;
                            uqqas[i].isApproved = true;
                        }
                    }
                }
            }
            return uqqas;
        }
    }
}