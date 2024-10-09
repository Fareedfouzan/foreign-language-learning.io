using ConversationApp.Data.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace ConversationApp.Data;

public partial class User
{
    public long Id { get; set; }

    public string? Name { get; set; }

    public ProficiencyLevel? Level { get; set; }

    public string EmailAddress { get; set; } = null!;

    [Column(TypeName = "Date")]
    public DateTime? JoinDate { get; set; }

    public User()
        {
            JoinDate = DateTime.Now.Date;
        }
    
    public long? LanguageId { get; set; }

    public virtual Language? Language { get; set; }

}
