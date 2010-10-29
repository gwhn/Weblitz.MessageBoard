using System.Collections.Generic;

namespace Weblitz.MessageBoard.Core.Domain.Model
{
    public class Forum : AuditedEntity
    {
        public virtual string Name { get; set; }

        private ISet<Topic> _topics = new HashSet<Topic>();
        public virtual IEnumerable<Topic> Topics
        {
            get { return _topics; }
            protected set { _topics = value as ISet<Topic>; }
        }

        public virtual void Add(Topic topic)
        {
            topic.Forum = this;
            _topics.Add(topic);
        }
    }
}