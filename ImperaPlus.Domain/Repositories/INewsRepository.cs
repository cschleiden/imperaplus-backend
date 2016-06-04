using System.Collections.Generic;
using ImperaPlus.Domain.News;

namespace ImperaPlus.Domain.Repositories
{
    public interface INewsRepository : IGenericRepository<NewsEntry>
    {
        IEnumerable<NewsEntry> GetOrdered(int count);
    }
}