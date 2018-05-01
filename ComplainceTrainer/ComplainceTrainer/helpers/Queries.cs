using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ComplianceTrainer.Models;
using System.Data.Entity;
using System.Configuration;
using System.Data.SqlClient;
using System.Data;

namespace ComplianceTrainer.helpers
{
    public class Queries
    {
        string conn = ConfigurationManager.ConnectionStrings["DBContext"].ConnectionString;

        public void checkExistingUser(User myUser)
        {
            DBContext db = new DBContext();
            try
            {
                User thisUser = db.Users.First(user => user.SAMAccountName == myUser.SAMAccountName);
                if (myUser.firstName != thisUser.firstName || myUser.lastName != thisUser.lastName || myUser.email != thisUser.email || myUser.userGroups != thisUser.userGroups || myUser.manager != thisUser.manager)
                {
                    thisUser = myUser;
                }
            }
            catch (Exception e)
            {
                db.Users.Add(myUser);
            }
            finally
            {
                db.SaveChanges();
            }
        }
    
        public List<User> getAllUsers()
        {
            DBContext db = new DBContext();
            List<User> users = db.Users.ToList();     
            return users;
        }

        public List<Group> getAllGroups()
        {
            DBContext db = new DBContext();
            return db.Groups.ToList();
        }

        public Quize getQuizById(int id)
        {
            DBContext db = new DBContext();
            return db.Quizes.Find(id);
        }
        public int addQuiz(Quize q)
        {          
            DBContext db = new DBContext();
            db.Quizes.Add(q);
            db.SaveChanges();
            return q.id;           
        }

        public Question getQuestionById(int id)
        {
            DBContext db = new DBContext();
            return db.Questions.Find(id);
        }

        public int addQuestion(Question q)
        {
            DBContext db = new DBContext();
            db.Questions.Add(q);
            db.SaveChanges();
            return q.id;
        }

        public Answer getAnswer(int id)
        {
            DBContext db = new DBContext();
            return db.Answers.Find(id);
        }

        public int addAnswer(Answer answer)
        {          
            DBContext db = new DBContext();
            db.Answers.Add(answer);
            db.SaveChanges();
            return answer.id;         
        }

        public List<UserQuizQuestionAnswer> getAllUserQuizes(int id)
        {
            DBContext db = new DBContext();
            List<UserQuizQuestionAnswer> userQuizes = new List<UserQuizQuestionAnswer>();
            userQuizes = db.UserQuizQuestionAnswers.Where(quiz => quiz.userId == id).ToList();
            return userQuizes;
        }

        public List<Question> getQuestionsByQuizId(int id)
        {
            DBContext db = new DBContext();
            return db.Questions.Where(q => q.quizId == id).ToList();
        }

        public List<Answer> getAnswersByQuestionId(int id)
        {
            DBContext db = new DBContext();
            return db.Answers.Where(a => a.questionId == id).ToList();
        }

        public Answer getUserAnswerByQuestionId(int userId, int questionId)
        {
            DBContext db = new DBContext();
            UserQuizQuestionAnswer uqqa = db.UserQuizQuestionAnswers.First(u => u.userId == userId && u.questionId == questionId);
            return getAnswerById(uqqa.answerId);
        }

        public Answer getAnswerById(int id)
        {
            DBContext db = new DBContext();
            return db.Answers.Find(id);
        }

        public User getUserById(int id)
        {
            DBContext db = new DBContext();
            return db.Users.First(u => u.id == id);
        }

        public List<Quize> getQuizesByGroupId(int id)
        {
            DBContext db = new DBContext();
            return db.Quizes.Where(q => q.groupId == id).ToList();
        }
        public Group getGroupByName(string name)
        {
            DBContext db = new DBContext();
            try
            {
                return db.Groups.First(g => g.name == name);
            }
            catch (Exception e)
            {
                Group group = new Group();
                return group;
            }
        }

        public List<UserQuizQuestionAnswer> getQuizByUserIdQuizId(int userId, int quizId)
        {
            DBContext db = new DBContext();
            List<UserQuizQuestionAnswer> uqqas = new List<UserQuizQuestionAnswer>();
            try
            {
                uqqas = db.UserQuizQuestionAnswers.Where(uq => uq.userId == userId && uq.quizId == quizId).ToList();
            }
            catch (Exception e)
            {
            }
            return uqqas;
        }

        public List<Quize> getAllQuizes()
        {
            DBContext db = new DBContext();
            return db.Quizes.ToList();
        }

        public List<UserQuizQuestionAnswer> submitQuiz(List<UserQuizQuestionAnswer> answers)
        {
            DBContext db = new DBContext();
            List<UserQuizQuestionAnswer> uqqas = new List<UserQuizQuestionAnswer>();
            try
            {
                foreach (UserQuizQuestionAnswer uqqa in answers)
                {
                    uqqa.isChecked = false;
                    db.UserQuizQuestionAnswers.Add(uqqa);
                    db.SaveChanges();
                    uqqas.Add(uqqa);
                }
                return uqqas;
            }
            catch(Exception e)
            {
                return uqqas;
            }
        }

        public User getUserBySam(string sam)
        {
            DBContext db = new DBContext();
            return db.Users.First(u => u.SAMAccountName == sam);
        }

        public bool RemoveQuiz(int id)
        {
            DBContext db = new DBContext();
            Quize chosenQuiz = db.Quizes.First(q => q.id == id);
            List<Quize> quizes = db.Quizes.Where(q => q.title == chosenQuiz.title).ToList();
            foreach(Quize quiz in quizes)
            {
                List<Question> Questions = db.Questions.Where(quest => quest.quizId == quiz.id).ToList();
                try
                {
                    foreach (Question question in Questions)
                    {
                        List<Answer> Answers = db.Answers.Where(ans => ans.questionId == question.id).ToList();
                        foreach (Answer answer in Answers)
                        {
                            List<UserQuizQuestionAnswer> UserQuizzes = db.UserQuizQuestionAnswers.Where(uqqa => uqqa.answerId == answer.id).ToList();
                            foreach (UserQuizQuestionAnswer uqqa in UserQuizzes)
                            {
                                db.UserQuizQuestionAnswers.Remove(db.UserQuizQuestionAnswers.First(u => u.id == uqqa.id));
                                db.SaveChanges();
                            }
                            db.Answers.Remove(db.Answers.First(ans => ans.id == answer.id));
                            db.SaveChanges();
                        }
                        db.Questions.Remove(db.Questions.First(quest => quest.id == question.id));
                        db.SaveChanges();
                    }
                    db.Quizes.Remove(db.Quizes.First(q => q.id == quiz.id));
                    db.SaveChanges();
                }
                catch (Exception e)
                {
                    return false;
                }
            }
            return true;
        }

        public bool RemoveQuestion(int id)
        {
            DBContext db = new DBContext();
            List<Answer> Answers = db.Answers.Where(ans => ans.questionId == id).ToList();
            try
            {
                foreach (Answer answer in Answers)
                {
                    db.Answers.Remove(db.Answers.First(ans => ans.id == answer.id));
                }
                db.Questions.Remove(db.Questions.First(quest => quest.id == id));
                db.SaveChanges();
            }
            catch(Exception e)
            {
                return false;
            }
            return true;
       }

        public Group getGroupById(int id)
        {
            DBContext db = new DBContext();
            return db.Groups.First(g => g.id == id);
        }

        public List<Quize> getQuizByTitle(string title)
        {
            DBContext db = new DBContext();
            Quize quiz = new Quize();
            try
            {
                List<Quize> quizes = db.Quizes.Where(q => q.title == title).ToList();
                return quizes;
            }
            catch(Exception e)
            {
                List<Quize> quizes = new List<Quize>();
                return quizes;
            }
        }

        public List<Quize> getUniqueQuizes()
        {
            DBContext db = new DBContext();
            List<Quize> returnedList = new List<Quize>();
            var result = db.Quizes.GroupBy(q => q.title).ToList();
            foreach (var list in db.Quizes.GroupBy(q => q.title).ToList())
            {
                var quizList = list.ToList();
                returnedList.Add((Quize)quizList[0]);
            }
            return returnedList;
        }

        public bool updateExistingQuiz(Quize updatedQuiz)
        {
            try
            {
                Quize quiz = new Quize();
                using (var ctx = new DBContext())
                {
                    quiz = ctx.Quizes.First(q => q.id == updatedQuiz.id);
                }
                quiz.description = updatedQuiz.description;
                quiz.media = updatedQuiz.media;
                quiz.startDate = updatedQuiz.startDate;
                quiz.preferDate = updatedQuiz.preferDate;
                quiz.expiredDate = updatedQuiz.expiredDate;
                using (var dbCtx = new DBContext())
                {
                    dbCtx.Entry(quiz).State = EntityState.Modified;
                    dbCtx.SaveChanges();
                }
            }
            catch(Exception e)
            {
                return false;
            }
            return true;
        }

        public int updateExistingQuestion(Question updatedQuestion)
        {
            try
            {
                Question question = new Question();
                using (var ctx = new DBContext())
                {
                    question = ctx.Questions.First(q => q.id == updatedQuestion.id);
                }

                question.questionType = updatedQuestion.questionType;
                question.questionText = updatedQuestion.questionText;

                //save modified entity using new Context
                using (var dbCtx = new DBContext())
                {
                    dbCtx.Entry(question).State = System.Data.Entity.EntityState.Modified;

                    dbCtx.SaveChanges();

                    return question.id;
                }
            }
            catch (Exception e)
            {
                return 0;
            }           
        }

        public int updateExistingAnswer(Answer updatedAnswer)
        {
            try
            {
                Answer answer = new Answer();
                using (var ctx = new DBContext())
                {
                    answer = ctx.Answers.First(a => a.id == updatedAnswer.id);
                }
                answer.answerText = updatedAnswer.answerText;
                answer.isCorrect = updatedAnswer.isCorrect;
                using (var dbCtx = new DBContext())
                {
                    dbCtx.Entry(answer).State = System.Data.Entity.EntityState.Modified;

                    dbCtx.SaveChanges();

                    return answer.id;
                }
            }
            catch (Exception e)
            {
                return 0;
            }
        }

    }
}
