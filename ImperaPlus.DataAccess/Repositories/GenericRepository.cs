using System.Linq;
using ImperaPlus.Domain.Repositories;
using Microsoft.EntityFrameworkCore;

namespace ImperaPlus.DataAccess.Repositories
{
    public class GenericRepository<TEntity> : IGenericRepository<TEntity> where TEntity : class
    {
        private readonly DbContext context;

        public GenericRepository(DbContext context)
        {
            this.context = context;
        }

        protected virtual DbContext Context => context;

        protected virtual DbSet<TEntity> DbSet => context.Set<TEntity>();

        public void Add(TEntity item)
        {
            DbSet.Add(item);
        }

        public void Remove(TEntity item)
        {
            DbSet.Remove(item);
        }

        public IQueryable<TEntity> Query()
        {
            return DbSet;
        }
    }
}
