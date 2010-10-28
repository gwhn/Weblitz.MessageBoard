using System;
using System.Collections.Generic;

namespace Weblitz.MessageBoard.Core.Domain.Model
{
    public class Forum : Entity, IAuditableEntity
    {
        public virtual string Name { get; set; }

        public virtual ISet<Topic> Topics { get; set; }

        public virtual DateTime CreatedOn { get; protected set; }

        public virtual string CreatedBy { get; protected set; }

        public virtual DateTime ModifiedOn { get; protected set; }

        public virtual string ModifiedBy { get; protected set; }
    }
}