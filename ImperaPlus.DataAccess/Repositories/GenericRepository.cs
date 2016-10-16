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

        protected virtual DbContext Context
        {
            get
            {
                return this.context;
            }
        }

        protected virtual DbSet<TEntity> DbSet
        {
            get
            {
                return this.context.Set<TEntity>();
            }
        }

        public void Add(TEntity item)
        {
            this.DbSet.Add(item);
        }

        public void Remove(TEntity item)
        {
            this.DbSet.Remove(item);
        }

        public IQueryable<TEntity> Query()
        {
            return this.DbSet;
        }
    }
}