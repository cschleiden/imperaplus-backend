using System.Linq;
using AutoMapper;
using ImperaPlus.Domain;
using ImperaPlus.Domain.Repositories;
using ImperaPlus.Domain.Services;
using ImperaPlus.DTO.Games.Map;

namespace ImperaPlus.Application
{
    public interface IMapTemplateService
    {
        IQueryable<MapTemplateDescriptor> QuerySummary();

        MapTemplate Get(string name);
    }

    public class MapTemplateService : BaseService, IMapTemplateService
    {
        private IMapTemplateProvider mapTemplateProvider;

        public MapTemplateService(IUnitOfWork unitOfWork, IMapper mapper, IUserProvider userProvider, IMapTemplateProvider mapTemplateProvider) 
            : base(unitOfWork, mapper, userProvider)
        {
            this.mapTemplateProvider = mapTemplateProvider;
        }

        public IQueryable<MapTemplateDescriptor> QuerySummary()
        {
            return this.Mapper.ProjectTo<MapTemplateDescriptor>(this.UnitOfWork.MapTemplateDescriptors.Query());
        }

        public MapTemplate Get(string name)
        {
            return Mapper.Map<MapTemplate>(this.mapTemplateProvider.GetTemplate(name));
        }
    }
}