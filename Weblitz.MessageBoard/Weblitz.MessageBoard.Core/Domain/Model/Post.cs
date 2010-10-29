using System.Collections.Generic;

namespace Weblitz.MessageBoard.Core.Domain.Model
{
    public class Post : Entry
    {
        public virtual Topic Topic { get; set; }

        public virtual Post Parent { get; set; }

        private ISet<Post> _children = new HashSet<Post>();
        public virtual IEnumerable<Post> Children
        {
            get { return _children; }
            protected set { _children = value as ISet<Post>; }
        }

        public virtual void Add(Post child)
        {
            child.Parent = this;
            child.Topic = Topic;
            _children.Add(child);
        }

        public virtual bool Flagged { get; set; }
    }
}