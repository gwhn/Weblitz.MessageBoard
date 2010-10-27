﻿using System.Collections.Generic;

namespace Weblitz.MessageBoard.Core.Domain.Model
{
    public class Topic : Entry
    {
        public virtual string Title { get; set; }

        public virtual Forum Forum { get; set; }

        public virtual ISet<Post> Posts { get; set; }

        public virtual bool Sticky { get; set; }

        public virtual bool Closed { get; set; }
    }
}