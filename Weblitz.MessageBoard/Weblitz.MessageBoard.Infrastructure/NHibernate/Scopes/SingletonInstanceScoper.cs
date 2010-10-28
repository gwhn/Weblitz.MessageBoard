using System.Collections;
using System.Collections.Generic;

namespace Weblitz.MessageBoard.Infrastructure.NHibernate.Scopes
{
    public class SingletonInstanceScoper<T> : InstanceScoperBase<T>
    {
        private static readonly IDictionary _dictionary = new Dictionary<string, T>();

        protected override IDictionary Dictionary
        {
            get { return _dictionary; }
        }
    }
}