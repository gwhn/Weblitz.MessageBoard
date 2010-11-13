using System;

namespace Weblitz.MessageBoard.Core.Domain.Model
{
    public class AuditInfo
    {
        public virtual DateTime? CreatedOn { get; set; }

        public virtual string CreatedBy { get; set; }

        public virtual DateTime? ModifiedOn { get; set; }

        public virtual string ModifiedBy { get; set; }

        public virtual bool Equals(AuditInfo other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return other.CreatedOn.Equals(CreatedOn) && Equals(other.CreatedBy, CreatedBy) &&
                   other.ModifiedOn.Equals(ModifiedOn) && Equals(other.ModifiedBy, ModifiedBy);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            return obj.GetType() == typeof (AuditInfo) && Equals((AuditInfo) obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var result = (CreatedOn.HasValue ? CreatedOn.Value.GetHashCode() : 0);
                result = (result*397) ^ (CreatedBy != null ? CreatedBy.GetHashCode() : 0);
                result = (result*397) ^ (ModifiedOn.HasValue ? ModifiedOn.Value.GetHashCode() : 0);
                result = (result*397) ^ (ModifiedBy != null ? ModifiedBy.GetHashCode() : 0);
                return result;
            }
        }
    }
}