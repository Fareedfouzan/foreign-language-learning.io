using ConversationApp.Data;
using ConversationApp.Data.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConversationApp.Framework.Models
{
    public class UserProfileModel
    {
        public User? User { get; set; }

        public History? History { get; set; }

        public Language? Language { get; set; }

        public ProficiencyLevel ProficiencyLevel { get; set; }
    }
}
