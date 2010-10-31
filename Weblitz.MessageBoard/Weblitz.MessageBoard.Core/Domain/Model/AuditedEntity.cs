namespace Weblitz.MessageBoard.Core.Domain.Model
{
    public abstract class AuditedEntity : Entity, IAuditable
    {
        public virtual AuditInfo AuditInfo { get; set; }

        public virtual bool Equals(AuditedEntity other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return base.Equals(other) && Equals(other.AuditInfo, AuditInfo);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            return ReferenceEquals(this, obj) || Equals(obj as AuditedEntity);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return (base.GetHashCode()*397) ^ (AuditInfo != null ? AuditInfo.GetHashCode() : 0);
            }
        }
    }
}