using System.Collections.Generic;
using ImperaPlus.Domain.News;

namespace ImperaPlus.Domain.Repositories
{
    public interface INewsRepository : IGenericRepository<NewsEntry>
    {
        NewsEntry FindById(long id);

        IEnumerable<NewsEntry> GetOrdered(int count);
    }
}