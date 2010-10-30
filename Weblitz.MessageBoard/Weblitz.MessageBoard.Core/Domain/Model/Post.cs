using System.Linq;
using Iesi.Collections.Generic;

namespace Weblitz.MessageBoard.Core.Domain.Model
{
    public class Post : Entry
    {
        public virtual Topic Topic { get; set; }

        public virtual Post Parent { get; set; }

        private readonly ISet<Post> _children = new HashedSet<Post>();

        public virtual Post[] Children
        {
            get
            {
                _children.Select(grandchild => grandchild.Children);
                return _children.ToArray();
            }
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