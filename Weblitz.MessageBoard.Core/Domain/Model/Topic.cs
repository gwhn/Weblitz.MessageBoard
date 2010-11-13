using System.Linq;
using Iesi.Collections.Generic;

namespace Weblitz.MessageBoard.Core.Domain.Model
{
    public class Topic : Entry
    {
        public virtual string Title { get; set; }

        public virtual Forum Forum { get; set; }

        private readonly ISet<Post> _posts = new HashedSet<Post>();

        public virtual Post[] Posts
        {
            get { return _posts.ToArray(); }
        }

        public virtual void Add(Post post)
        {
            post.Topic = this;
            post.Parent = null;
            _posts.Add(post);
        }

        public virtual void Remove(Post post)
        {
            post.Topic = null;
            _posts.Remove(post);
        }

        public virtual bool Sticky { get; set; }

        public virtual bool Closed { get; set; }

        public virtual bool Equals(Topic other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return base.Equals(other) && Equals(other.Title, Title);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            return Equals(obj as Topic);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return (base.GetHashCode()*397) ^ (Title != null ? Title.GetHashCode() : 0);
            }
        }
    }
}