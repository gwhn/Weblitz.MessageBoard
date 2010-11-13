using System.Collections.Generic;

namespace Weblitz.MessageBoard.Core.Domain.Repositories
{
    public interface IRepository<TEntity> : IReadOnlyRepository<TEntity> where TEntity : class
    {
        void Save(TEntity entity);
        void Save(IEnumerable<TEntity> entities);
        void Delete(TEntity entity);
        void Delete(IEnumerable<TEntity> entities);
    }
}