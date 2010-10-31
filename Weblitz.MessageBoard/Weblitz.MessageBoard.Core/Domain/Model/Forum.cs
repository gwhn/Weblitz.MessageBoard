using System.Diagnostics;
using System.Linq;
using Iesi.Collections.Generic;

namespace Weblitz.MessageBoard.Core.Domain.Model
{
    public class Forum : AuditedEntity
    {
        public virtual string Name { get; set; }

        private readonly ISet<Topic> _topics = new HashedSet<Topic>();

        public virtual Topic[] Topics
        {
            get { return _topics.ToArray(); }
        }

        public virtual void Add(Topic topic)
        {
            topic.Forum = this;
            _topics.Add(topic);
        }

        public virtual void Remove(Topic topic)
        {
            topic.Forum = null;
            _topics.Remove(topic);
        }

        public virtual bool Equals(Forum other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return base.Equals(other) && Equals(other.Name, Name);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            return ReferenceEquals(this, obj) || Equals(obj as Forum);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return (base.GetHashCode()*397) ^ (Name != null ? Name.GetHashCode() : 0);
            }
        }
    }
}