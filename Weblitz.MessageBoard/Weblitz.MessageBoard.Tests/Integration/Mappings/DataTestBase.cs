using System.Collections.Generic;
using NUnit.Framework;
using Weblitz.MessageBoard.Core.Domain.Model;

namespace Weblitz.MessageBoard.Tests.Integration.Mappings
{
    [TestFixture]
    public class DataTestBase : IntegrationTestBase
    {
        protected void Persist(Entity entity)
        {
            using (var session = Session)
            {
                session.SaveOrUpdate(entity);
                session.Flush();
            }
        }

        protected void Persist(IEnumerable<Entity> entities)
        {
            using (var session = Session)
            {
                foreach (var entity in entities)
                {
                    session.SaveOrUpdate(entity);
                }
                session.Flush();
            }
        }

        protected T Load<T>(object id) where T : Entity
        {
            using (var session = Session)
            {
                return session.Load<T>(id);
            }
        }

        protected void Reload<T>(ref T entity) where T : Entity
        {
            using (var session = Session)
            {
                if (session.Contains(entity))
                {
                    session.Evict(entity);
                }
                entity = session.Get<T>(entity.Id);
            }
        }
        
    }
}