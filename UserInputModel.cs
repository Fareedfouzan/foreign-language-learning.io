using ConversationApp.Data;
using ConversationApp.Data.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConversationApp.Framework.Models
{
    public class UserInputModel{

        public Conversation? Conversation { get; set; }

        public ConversationContent? ConversationContent { get; set; }

        public List<ConversationContent> conversationContent;
        public List<Language> languages;
        public List<Hint> Hints;

        public User? User { get; set; }

        public Language? Language { get; set; }

        public Hint? Hint { get; set; }

        public string? UserInput { get; set; }

    }
}