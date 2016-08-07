using System.Security.Cryptography.X509Certificates;
using ImperaPlus.Domain.Map;

namespace ImperaPlus.Domain.Repositories
{
    public interface IMapTemplateDescriptorRepository : IGenericRepository<MapTemplateDescriptor>
    {
        MapTemplateDescriptor Get(string name);
    }
}