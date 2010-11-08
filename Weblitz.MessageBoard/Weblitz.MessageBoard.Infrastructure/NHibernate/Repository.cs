using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using NHibernate;
using NHibernate.Linq;
using Weblitz.MessageBoard.Core.Domain.Model;
using Weblitz.MessageBoard.Core.Domain.Repositories;

namespace Weblitz.MessageBoard.Infrastructure.NHibernate
{
    public class Repository<TEntity> : IKeyedRepository<TEntity, Guid> where TEntity : Entity
    {
        protected ISession Session
        {
            get { return SessionManager.Instance.SessionFactory.GetCurrentSession(); }
        }

        public IQueryable<TEntity> All()
        {
            return Session.Query<TEntity>();
        }

        public TEntity FindBy(Expression<Func<TEntity, bool>> expression)
        {
            return FilterBy(expression).SingleOrDefault();
        }

        public IQueryable<TEntity> FilterBy(Expression<Func<TEntity, bool>> expression)
        {
            return All().Where(expression).AsQueryable();
        }

        public void Save(TEntity entity)
        {
            Session.SaveOrUpdate(entity);
        }

        public void Save(IEnumerable<TEntity> entities)
        {
            foreach (var entity in entities)
            {
                Save(entity);
            }
        }

        public void Delete(TEntity entity)
        {
            Session.Delete(entity);
        }

        public void Delete(IEnumerable<TEntity> entities)
        {
            foreach (var entity in entities)
            {
                Delete(entity);
            }
        }

        public TEntity FindBy(Guid id)
        {
            return Session.Get<TEntity>(id);
        }
    }
}