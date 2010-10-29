using System.Collections.Generic;

namespace Weblitz.MessageBoard.Core.Domain.Model
{
    public abstract class Entry : AuditedEntity
    {
        public virtual string Body { get; set; }

        private ISet<Attachment> _attachments = new HashSet<Attachment>();
        public virtual IEnumerable<Attachment> Attachments
        {
            get { return _attachments; }
            protected set { _attachments = value as ISet<Attachment>; }
        }

        public virtual void Add(Attachment attachment)
        {
            attachment.Entry = this;
            _attachments.Add(attachment);
        }
    }
}