using System.Linq;
using Iesi.Collections.Generic;

namespace Weblitz.MessageBoard.Core.Domain.Model
{
    public abstract class Entry : AuditedEntity
    {
        public virtual string Body { get; set; }

        private readonly ISet<Attachment> _attachments = new HashedSet<Attachment>();

        public virtual Attachment[] Attachments
        {
            get { return _attachments.ToArray(); }
        }

        public virtual void Add(Attachment attachment)
        {
            attachment.Entry = this;
            _attachments.Add(attachment);
        }

        public virtual void Remove(Attachment attachment)
        {
            attachment.Entry = null;
            _attachments.Remove(attachment);
        }
    }
}