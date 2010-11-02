using System;
using Weblitz.MessageBoard.Core.Domain.Model;

namespace Weblitz.MessageBoard.Core.Domain.Repositories
{
    public interface IRepository<T> where T : Entity
    {
        T GetById(Guid id);

        void Save(T entity);
        
        T[] GetAll();
        
        void Delete(T entity);
    }
}