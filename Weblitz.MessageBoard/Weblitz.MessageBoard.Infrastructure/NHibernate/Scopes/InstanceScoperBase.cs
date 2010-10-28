using System;
using System.Collections;

namespace Weblitz.MessageBoard.Infrastructure.NHibernate.Scopes
{
    public abstract class InstanceScoperBase<T> : IInstanceScoper<T>
    {
        protected abstract IDictionary Dictionary { get; }

        public T GetScopedInstance(string key, Func<T> builder)
        {
            if (!Dictionary.Contains(key))
            {
                BuildInstance(key, builder);
            }
            return (T) Dictionary[key];
        }

        public void ClearScopedInstance(string key)
        {
            if (Dictionary.Contains(key))
            {
                ClearInstance(key);
            }
        }

        private void ClearInstance(string key)
        {
            lock (Dictionary.SyncRoot)
            {
                if (Dictionary.Contains(key))
                {
                    RemoveInstance(key);
                }
            }
        }

        private void RemoveInstance(string key)
        {
            Dictionary.Remove(key);
        }

        private void BuildInstance(string key, Func<T> builder)
        {
            lock (Dictionary.SyncRoot)
            {
                if (!Dictionary.Contains(key))
                {
                    AddInstance(key, builder);
                }
            }
        }

        private void AddInstance(string key, Func<T> builder)
        {
            T instance = builder.Invoke();
            Dictionary.Add(key, instance);
        }
    }
}