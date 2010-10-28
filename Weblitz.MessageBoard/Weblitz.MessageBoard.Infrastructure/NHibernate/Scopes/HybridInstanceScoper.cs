using System;

namespace Weblitz.MessageBoard.Infrastructure.NHibernate.Scopes
{
    public class HybridInstanceScoper<T> : IInstanceScoper<T>
    {
        private readonly ThreadStaticInstanceScoper<T> _threadStaticInstanceScoper;
        private readonly HttpContextInstanceScoper<T> _httpContextInstanceScoper;

        public HybridInstanceScoper()
        {
            _threadStaticInstanceScoper = new ThreadStaticInstanceScoper<T>();
            _httpContextInstanceScoper = new HttpContextInstanceScoper<T>();
        }

        public T GetScopedInstance(string key, Func<T> builder)
        {
            return Scoper.GetScopedInstance(key, builder);
        }

        public void ClearScopedInstance(string key)
        {
            Scoper.ClearScopedInstance(key);
        }

        private IInstanceScoper<T> Scoper
        {
            get
            {
                if (_httpContextInstanceScoper.IsEnabled)
                {
                    return _httpContextInstanceScoper;
                }
                return _threadStaticInstanceScoper;
            }
        }
    }
}