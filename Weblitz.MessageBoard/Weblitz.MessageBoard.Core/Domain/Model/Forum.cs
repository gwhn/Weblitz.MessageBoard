using System;

namespace Weblitz.MessageBoard.Core.Domain.Model
{
    public class Forum : IEntity
    {
        public virtual Guid Id { get; set; }

        public virtual string Name { get; set; }

        public virtual Topic[] Topics { get; set; }
    }
}