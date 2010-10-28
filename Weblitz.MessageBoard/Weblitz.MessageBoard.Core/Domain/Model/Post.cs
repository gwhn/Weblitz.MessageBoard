using System.Collections.Generic;

namespace Weblitz.MessageBoard.Core.Domain.Model
{
    public class Post : Entry
    {
        public virtual Topic Topic { get; set; }

        public virtual Post Parent { get; set; }

        public virtual ISet<Post> Children { get; set; }

        public virtual bool Flagged { get; set; }
    }
}