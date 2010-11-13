namespace Weblitz.MessageBoard.Core.Domain.Repositories
{
    public interface IKeyedRepository<TEntity, in TKey> : IRepository<TEntity> where TEntity : class
    {
        TEntity FindBy(TKey id);
    }
}