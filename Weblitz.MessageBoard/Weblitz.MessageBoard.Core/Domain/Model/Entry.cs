using Iesi.Collections.Generic;

namespace Weblitz.MessageBoard.Core.Domain.Model
{
    public abstract class Entry : AuditedEntity
    {
        public virtual string Body { get; set; }

        private ISet<Attachment> _attachments = new HashedSet<Attachment>();

        public virtual ISet<Attachment> Attachments
        {
            get { return _attachments; }
            protected set { _attachments = value; }
        }

        public virtual int AttachmentCount
        {
            get { return _attachments.Count; }
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