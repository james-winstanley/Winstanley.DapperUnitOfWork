using System;

namespace Winstanley.UnitOfWork.Interfaces
{
    public interface IUnitOfWorkBase : IDisposable
    {
        /// <summary>
        /// Commits this instance transaction.
        /// </summary>
        void Commit();
    }
}
