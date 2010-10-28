using System;
using Iesi.Collections.Generic;

namespace Weblitz.MessageBoard.Core.Domain.Model
{
    public class Forum : Entity, IAuditable
    {
        public Forum()
        {
            Topics = new HashedSet<Topic>();
        }

        public virtual string Name { get; set; }

        public virtual ISet<Topic> Topics { get; set; }

        public virtual AuditInfo AuditInfo { get; protected set; }
    }
}