namespace Weblitz.MessageBoard.Core.Domain.Model
{
    public abstract class AuditedEntity : Entity, IAuditable
    {
        private AuditInfo _auditInfo = new AuditInfo();

        public virtual AuditInfo AuditInfo
        {
            get { return _auditInfo; }
            protected set { _auditInfo = value; }            
        }
    }
}