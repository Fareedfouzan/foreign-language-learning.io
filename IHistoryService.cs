
namespace ConversationApp.Framework.Interfaces
{
    public interface IHistoryService
    {
        void InsertHistoryRecord(string message, DateTime date);
    }
}