using Iesi.Collections.Generic;

namespace Weblitz.MessageBoard.Core.Domain.Model
{
    public class Post : Entry
    {
        public virtual Topic Topic { get; set; }

        public virtual Post Parent { get; set; }

        private ISet<Post> _children = new HashedSet<Post>();

        public virtual ISet<Post> Children
        {
            get { return _children; }
            protected set { _children = value; }
        }

        public virtual int ChildCount
        {
            get { return _children.Count; }
        }

        public virtual void Add(Post child)
        {
            child.Parent = this;
            child.Topic = Topic;
            _children.Add(child);
        }

        public virtual void Remove(Post child)
        {
            child.Parent = null;
            child.Topic = null;
            _children.Remove(child);
        }

        public virtual bool Flagged { get; set; }
    }
}