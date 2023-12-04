using System.Data.Entity;
using System.Linq;

namespace Server.Domain
{
    public class Repository<T> : IRepository<T> where T : class
    {
        private readonly DbContext _context;

        public Repository(DbContext context)
        {
            _context = context;
        }

        public virtual DbSet<T> Table => _context.Set<T>();

        public virtual void Add(T entity)
        {
            Table.Add(entity);
        }

        public virtual void Update(T entity)
        {
            Table.Attach(entity);
        }

        public virtual void Delete(T entity)
        {
            Table.Remove(entity);
        }
    }
}