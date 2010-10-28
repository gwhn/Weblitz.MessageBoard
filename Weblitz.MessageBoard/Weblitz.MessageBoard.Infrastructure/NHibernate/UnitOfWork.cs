using System;
using NHibernate;
using Weblitz.MessageBoard.Core;

namespace Weblitz.MessageBoard.Infrastructure.NHibernate
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ISessionBuilder _builder;

        public UnitOfWork(ISessionBuilder builder)
        {
            _builder = builder;
        }

        public UnitOfWork() : this(new SessionBuilder())
        {
        }

        public ISession Session
        {
            get { return _builder.Construct(); }
        }

        private ITransaction Transaction
        {
            get { return Session.Transaction; }
        }

        public void Begin()
        {
            if (Transaction.IsActive || Transaction.WasCommitted || Transaction.WasRolledBack)
            {
                Transaction.Dispose();
            }
            Session.BeginTransaction();
        }

        public void Commit()
        {
            if (!Transaction.IsActive)
            {
                throw new InvalidOperationException("Must call Start() on the unit of work before committing");                
            }
            Transaction.Commit();
        }

        public void RollBack()
        {
            if (Transaction.IsActive)
            {
                Transaction.Rollback();
            }
        }

        public void Dispose()
        {
            Session.Dispose();
        }
    }
}