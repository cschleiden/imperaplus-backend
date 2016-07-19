using System.Data.Entity;
using System.Linq;
using ImperaPlus.Domain.Map;
using ImperaPlus.Domain.Repositories;

namespace ImperaPlus.DataAccess.Repositories
{
    class MapTemplateRepository : GenericRepository<MapTemplate>, IMapTemplateDescriptorRepository
    {
        public MapTemplateRepository(DbContext context) 
            : base(context)
        {
        }

        public MapTemplate Get(string name)
        {
            return
                this.DbSet
                    .Include(x => x.Countries)
                    .Include(x => x.Connections)
                    .Include(x => x.Continents)
                    .Include(x => x.Continents.Select(c => c.Countries))
                    .FirstOrDefault(x => x.Name == name);
        }
    }
}