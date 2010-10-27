using System;
using System.Collections.Generic;

namespace Weblitz.MessageBoard.Core.Domain.Model
{
    public class Forum : Entity
    {
        public virtual string Name { get; set; }

        public virtual ISet<Topic> Topics { get; set; }
    }
}