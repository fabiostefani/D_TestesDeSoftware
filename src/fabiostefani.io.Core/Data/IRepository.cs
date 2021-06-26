using System;
using fabiostefani.io.Core.DomainObjects;

namespace fabiostefani.io.Core.Data
{
    public interface IRepository<T> : IDisposable where T : IAggregateRoot
    {
        IUnitOfWork UnitOfWork { get; }
    }
}