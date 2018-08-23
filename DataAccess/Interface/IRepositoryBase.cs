using System;

namespace DataAccess.Interface
{
    public interface IRepositoryBase : IDisposable
    {
        bool CanDispose { get; set; }
        void Dispose(bool force);
    }
}