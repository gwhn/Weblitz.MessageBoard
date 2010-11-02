using System;
using System.Linq;
using NHibernate;
using Weblitz.MessageBoard.Core.Domain.Model;
using Weblitz.MessageBoard.Core.Domain.Repositories;

namespace Weblitz.MessageBoard.Infrastructure.NHibernate
{
    public class RepositoryBase<T> : IRepository<T> where T : Entity
    {
        protected ISession Session
        {
            get { return SessionManager.Instance.SessionFactory.GetCurrentSession(); }
        }

        public virtual T GetById(Guid id)
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