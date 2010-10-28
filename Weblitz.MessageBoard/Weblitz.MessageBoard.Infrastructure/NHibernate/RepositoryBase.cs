using System.Linq;
using NHibernate;
using Weblitz.MessageBoard.Core;
using Weblitz.MessageBoard.Core.Domain.Model;
using Weblitz.MessageBoard.Infrastructure.NHibernate.Builders;

namespace Weblitz.MessageBoard.Infrastructure.NHibernate
{
    public class RepositoryBase<T> : IRepository<T> where T : Entity
    {
        protected ISession Session
        {
            get { return new SessionBuilder().Construct(); }
        }

        public virtual T GetById(object id)
        {
            return Session.Get<T>(id);
        }

        public virtual void Save(T entity)
        {
            Session.SaveOrUpdate(entity);
        }

        public virtual T[] GetAll()
        {
            return Session.CreateCriteria(typeof(T)).List<T>().ToArray();
        }

        public virtual void Delete(T entity)
        {
            Session.Delete(entity);
        }
    }
}