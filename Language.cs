using System;
using System.Collections.Generic;

namespace ConversationApp.Data;

public partial class Language
{
    public long Id { get; set; }

    public string Name { get; set; } = null!;

    public virtual ICollection<Conversation> ConversationLearningLanguages { get; } = new List<Conversation>();

    public virtual ICollection<Conversation> ConversationNativeLanguages { get; } = new List<Conversation>();

    public virtual ICollection<User> Users { get; } = new List<User>();
}
