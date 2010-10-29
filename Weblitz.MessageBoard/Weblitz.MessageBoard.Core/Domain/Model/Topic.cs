using System.Collections.Generic;

namespace Weblitz.MessageBoard.Core.Domain.Model
{
    public class Topic : Entry
    {
        public virtual string Title { get; set; }

        public virtual Forum Forum { get; set; }

        private ISet<Post> _posts = new HashSet<Post>();
        public virtual ISet<Post> Posts
        {
            get { return _posts; }
            protected set { _posts = value; }
        }

        public virtual void AddPost(Post post)
        {
            post.Topic = this;
            post.Parent = null;
            _posts.Add(post);
        }

        public virtual bool Sticky { get; set; }

        public virtual bool Closed { get; set; }
    }
}