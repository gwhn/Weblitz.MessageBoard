namespace Weblitz.MessageBoard.Core.Domain.Model
{
    public abstract class AuditedEntity : Entity, IAuditable
    {
        public virtual AuditInfo AuditInfo { get; set; }
    }
}