using ConversationApp.Framework.Interfaces;
using ConversationApp.Data;

namespace ConversationApp.Framework.Services
{
    public class HistoryService : IHistoryService
    {
        private readonly CaDbContext caDbContext;

        public HistoryService(CaDbContext caDbContext)
        {
            this.caDbContext = caDbContext;
        }

        public void InsertHistoryRecord(string message, DateTime date)
        {
            caDbContext.Histories.Add(new History
            {
                Message = message,
                DateTime = date
            });
            caDbContext.SaveChanges();
        }

       
    }

}
