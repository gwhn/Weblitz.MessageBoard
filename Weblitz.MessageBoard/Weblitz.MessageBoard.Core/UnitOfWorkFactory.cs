using System;

namespace Weblitz.MessageBoard.Core
{
    public class UnitOfWorkFactory : AbstractFactoryBase<IUnitOfWork>
    {
        public static Func<IUnitOfWork> GetDefault = DefaultUnconfiguredState;
    }
}