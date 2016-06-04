using System.Security.Cryptography.X509Certificates;
using ImperaPlus.Domain.Map;

namespace ImperaPlus.Domain.Repositories
{
    public interface IMapTemplateRepository : IGenericRepository<MapTemplate>
    {
        MapTemplate Get(string name);
    }
}