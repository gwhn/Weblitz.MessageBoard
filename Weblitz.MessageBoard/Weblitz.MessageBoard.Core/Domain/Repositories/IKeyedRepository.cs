using Weblitz.MessageBoard.Core.Domain.Model;

namespace Weblitz.MessageBoard.Core.Domain.Repositories
{
    public interface IKeyedRepository<in TKey>
    {
        Entity FindBy(TKey id);
    }

    public interface IKeyedRepository<TEntity, in TKey> : IRepository<TEntity>, IKeyedRepository<TKey> where TEntity : class
    {
        new TEntity FindBy(TKey id);
    }
}