using System;

namespace Weblitz.MessageBoard.Core.Domain.Model
{
    public interface IAuditableEntity : IEntity
    {
        DateTime CreatedOn { get; }

        string CreatedBy { get; }

        DateTime ModifiedOn { get; }

        string ModifiedBy { get; }
    }
}