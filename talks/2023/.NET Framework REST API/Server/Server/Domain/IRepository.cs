using System;
using System.Data.Entity;
using System.Linq;

namespace Server.Domain
{
    internal interface IRepository<T> where T : class
    {
        DbSet<T> Table { get; }

        void Add(T entity);
        void Update(T entity);
        void Delete(T entity);
    }
}
