using System;

namespace Weblitz.MessageBoard.Core.Domain.Model
{
    public interface IEntity
    {
        Guid Id { get; }
    }
}