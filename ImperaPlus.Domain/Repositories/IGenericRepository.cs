using System.Linq;

namespace ImperaPlus.Domain.Repositories
{
    public interface IGenericRepository<TEntity> where TEntity : class
    {
        void Add(TEntity item);
        void Remove(TEntity item);
        IQueryable<TEntity> Query();
        TEntity FindById(params object[] keyValues);
    }
}