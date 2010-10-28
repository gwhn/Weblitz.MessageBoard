using System;
using System.Collections.Generic;

namespace Weblitz.MessageBoard.Core.Domain.Model
{
    public abstract class Entry : Entity, IAuditable
    {
        public virtual string Body { get; set; }

        public virtual ISet<Attachment> Attachments { get; set; }

        public virtual AuditInfo AuditInfo { get; protected set; }
    }
}