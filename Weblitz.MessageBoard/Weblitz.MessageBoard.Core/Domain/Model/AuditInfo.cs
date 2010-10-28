using System;

namespace Weblitz.MessageBoard.Core.Domain.Model
{
    public class AuditInfo
    {
        public virtual DateTime CreatedOn { get; set; }

        public virtual string CreatedBy { get; set; }

        public virtual DateTime? ModifiedOn { get; set; }

        public virtual string ModifiedBy { get; set; }
    }
}