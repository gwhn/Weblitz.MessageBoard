using System;

namespace Weblitz.MessageBoard.Infrastructure.NHibernate.Scopes
{
    public interface IInstanceScoper<T>
    {
        T GetScopedInstance(string key, Func<T> builder);

        void ClearScopedInstance(string key);
    }
}