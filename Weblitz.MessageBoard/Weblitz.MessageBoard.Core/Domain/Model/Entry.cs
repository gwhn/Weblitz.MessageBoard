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

        public virtual bool Equals(Entry other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return base.Equals(other) && Equals(other.Body, Body);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            return ReferenceEquals(this, obj) || Equals(obj as Entry);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return (base.GetHashCode()*397) ^ (Body != null ? Body.GetHashCode() : 0);
            }
        }
    }
}