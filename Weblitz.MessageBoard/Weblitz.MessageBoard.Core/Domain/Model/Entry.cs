using System;
using System.Collections.Generic;

namespace Weblitz.MessageBoard.Core.Domain.Model
{
    public abstract class Entry : Entity
    {
        public virtual string Body { get; set; }

        public virtual DateTime CreatedOn { get; set; }

        public virtual string CreatedBy { get; set; }

        public virtual DateTime ModifiedOn { get; set; }

        public virtual string ModifiedBy { get; set; }

        public virtual ISet<Attachment> Attachments { get; set; }
    }
}