using System.Collections;
using System.Web;

namespace Weblitz.MessageBoard.Infrastructure.NHibernate.Scopes
{
    public class HttpContextInstanceScoper<T> : InstanceScoperBase<T>
    {
        public bool IsEnabled
        {
            get { return HttpContext.Current != null; }
        }

        protected override IDictionary Dictionary
        {
            get { return HttpContext.Current.Items; }
        }
    }
}