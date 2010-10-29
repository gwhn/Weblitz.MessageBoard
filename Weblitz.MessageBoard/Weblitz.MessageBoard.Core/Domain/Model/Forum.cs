using Iesi.Collections.Generic;

namespace Weblitz.MessageBoard.Core.Domain.Model
{
    public class Forum : AuditedEntity
    {
        public virtual string Name { get; set; }

        private ISet<Topic> _topics = new HashedSet<Topic>();

        public virtual ISet<Topic> Topics
        {
            get { return _topics; }
            protected set { _topics = value; }
        }

        public virtual int TopicCount
        {
            get { return _topics.Count; }
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
    }
}