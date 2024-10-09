using System;
using System.Collections.Generic;

namespace ConversationApp.Data;

public partial class ConversationContent
{
    public long Id { get; set; }

    public long ConversationId { get; set; }

    public string Person { get; set; }

    public string Line { get; set; } = null;

    public Conversation Conversation { get; set; } = null;

    public ICollection<Hint> Hints { get; set; } = new List<Hint>();

}
