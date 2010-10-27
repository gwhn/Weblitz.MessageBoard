using System;

namespace Weblitz.MessageBoard.Core.Domain.Model
{
    public abstract class Entity : IEntity
    {
        public virtual Guid Id { get; set; }
    }
}