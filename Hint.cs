using System;
using System.Collections.Generic;

namespace ConversationApp.Data;

public partial class Hint
{
    public long Id { get; set; }

    public long ConversationId { get; set; }

    public long ConversationContentId { get; set; }

    public string? Language { get; set; }

    public string? Word { get; set; }

    public string? Content { get; set; }

    public string? TargetLanguage { get; set; }

    public string? TargetLanguageWord { get; set; }

    public Conversation Conversation { get; set; }
    public ConversationContent ConversationContent { get; set; } 

}
