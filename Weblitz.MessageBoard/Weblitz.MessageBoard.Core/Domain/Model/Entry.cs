using System;
using System.Collections.Generic;

namespace Weblitz.MessageBoard.Core.Domain.Model
{
    public abstract class Entry : Entity, IAuditableEntity
    {
        public virtual string Body { get; set; }

        public virtual ISet<Attachment> Attachments { get; set; }

        public virtual DateTime CreatedOn { get; protected set; }

        public virtual string CreatedBy { get; protected set; }

        public virtual DateTime ModifiedOn { get; protected set; }

        public virtual string ModifiedBy { get; protected set; }
    }
}