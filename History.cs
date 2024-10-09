using System;
using System.Collections.Generic;

namespace ConversationApp.Data;

public partial class History
{
    public long Id { get; set; }

    public string? Message { get; set; }

    public DateTime? DateTime { get; set; }
}
