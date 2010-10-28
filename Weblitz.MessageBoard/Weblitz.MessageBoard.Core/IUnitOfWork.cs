using System;

namespace Weblitz.MessageBoard.Core
{
    public interface IUnitOfWork : IDisposable
    {
        void Begin();
        void Commit();
        void RollBack();
    }
}