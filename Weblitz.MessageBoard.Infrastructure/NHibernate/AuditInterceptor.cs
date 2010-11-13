using System;
using System.Threading;
using NHibernate;
using NHibernate.Type;
using Weblitz.MessageBoard.Core.Domain.Model;

namespace Weblitz.MessageBoard.Infrastructure.NHibernate
{
    public class AuditInterceptor : EmptyInterceptor
    {
        public override bool OnSave(object entity, object id, object[] state, string[] propertyNames, IType[] types)
        {
            if (entity is IAuditable)
            {
                for (var i = 0; i < propertyNames.Length; i++)
                {
                    if (propertyNames[i] != "AuditInfo") continue;
                    var auditInfo = state[i] as AuditInfo;
                    if (auditInfo == null) continue;
                    auditInfo.CreatedOn = DateTime.Now;
                    auditInfo.CreatedBy = Thread.CurrentPrincipal.Identity.Name;
                }
            }
            return false;
        }

        public override bool OnFlushDirty(object entity, object id, object[] currentState, object[] previousState,
                                          string[] propertyNames, IType[] types)
        {
            if (entity is IAuditable)
            {
                for (var i = 0; i < propertyNames.Length; i++)
                {
                    if (propertyNames[i] != "AuditInfo") continue;
                    var auditInfo = currentState[i] as AuditInfo;
                    if (auditInfo == null) continue;
                    auditInfo.ModifiedOn = DateTime.Now;
                    auditInfo.ModifiedBy = Thread.CurrentPrincipal.Identity.Name;
                }
            }
            return false;
        }
    }
}