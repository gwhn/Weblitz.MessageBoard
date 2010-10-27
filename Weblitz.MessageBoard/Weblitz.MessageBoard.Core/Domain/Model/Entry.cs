using System;

namespace Weblitz.MessageBoard.Core.Domain.Model
{
    public class Entry : IEntity
    {
        public virtual Guid Id { get; set; }

        public virtual string Body { get; set; }

        public virtual DateTime CreatedOn { get; set; }

        public virtual string CreatedBy { get; set; }

        public virtual DateTime ModifiedOn { get; set; }

        public virtual string ModifiedBy { get; set; }

        public virtual Attachment[] Attachments { get; set; }
    }
}