using ConversationApp.Data;
using ConversationApp.Data.Enums;

namespace ConversationApp.Framework.Interfaces
{
    public interface IUserService
    {
        User GetUserById(long id);
        void ChangeProficiencyLevel(User user, ProficiencyLevel level);
        bool UserExistsInDatabase(string emailAddress);
        void AddUserRecord(string emailAddress, string name, DateTime joinDate, int languageId);
        User? GetUserByEmail(string emailAddress);
    }
}