using Weblitz.MessageBoard.Core.Domain.Model;

namespace Weblitz.MessageBoard.Core
{
    public interface IRepository<T> where T : Entity
    {
        T GetById(object id);

        void Save(T entity);
        
        T[] GetAll();
        
        void Delete(T entity);
    }
}