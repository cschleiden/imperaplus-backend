using System.Collections.Generic;
using System.Linq;
using ImperaPlus.Domain.News;
using ImperaPlus.Domain.Repositories;
using Microsoft.EntityFrameworkCore;

namespace ImperaPlus.DataAccess.Repositories
{
    internal class NewsRepository : GenericRepository<NewsEntry>, INewsRepository
    {
        public NewsRepository(DbContext context)
            : base(context)
        {
        }

        public NewsEntry FindById(long id)
        {
            return this.DbSet.FirstOrDefault(x => x.Id == id);
        }

        public IEnumerable<NewsEntry> GetOrdered(int count)
        {
            return this.DbSet.OrderByDescending(x => x.CreatedAt).Take(count).Include(x => x.CreatedBy).Include(x => x.Content);
        }
    }
}