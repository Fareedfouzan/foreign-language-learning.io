using System;
using System.Collections.Generic;

namespace ConversationApp.Data;

public partial class Conversation
{
    public long Id { get; set; }

    public string Level { get; set; }

    public string Context { get; set; } = null;

    public string Subcontext { get; set; } = null;

    public long? NativeLanguageId { get; set; }

    public long? LearningLanguageId { get; set; }

    public virtual Language LearningLanguage { get; set; } = null;

    public virtual Language NativeLanguage { get; set; } = null;

    public ICollection<ConversationContent> Contents { get; set; } = new List<ConversationContent>();

    public ICollection<Hint> WordHints { get; set; } = new List<Hint>();

}
