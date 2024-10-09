using ConversationApp.Data;
using ConversationApp.Data.Enums;
using ConversationApp.Framework.Interfaces;
using System.Linq;

namespace ConversationApp.Framework.Services
{
    public class UserService : IUserService
    {
        private readonly CaDbContext caDbContext;

        public UserService(CaDbContext caDbContext)
        {
            this.caDbContext = caDbContext;
        }

        public void AddUserRecord(string emailAddress, string name, DateTime joinDate, int languageId)
        {
            User newUser = new User { 
                EmailAddress = emailAddress,
                Name = name,
                Level = ProficiencyLevel.Beginner,
                JoinDate = joinDate.Date,
                LanguageId = languageId
            };

            caDbContext.Users.Add(newUser);
            caDbContext.SaveChanges();
        }

        public void ChangeProficiencyLevel(User user, ProficiencyLevel level)
        {
            if(user == null){
                return;
            }
            
           else{
              caDbContext.Users.Where(x => x.Id == user.Id).FirstOrDefault().Level = level;
              caDbContext.SaveChanges();
            }
           
        //  Console.WriteLine($"Error changing the proficiency level {e.Message}");
        }

        public User GetUserById(long id)
        {
            return caDbContext.Users.Where(x => x.Id == id).FirstOrDefault();
        }

        public User GetUserByEmail(string email)
        {
            return caDbContext.Users.Where(x => x.EmailAddress == email).FirstOrDefault();
        }

        public bool UserExistsInDatabase(string emailAddress)
        {
            return caDbContext.Users.Where(x => x.EmailAddress == emailAddress).Any();
        }

    } 
}