using System.Linq;
using AutoMapper;
using AutoMapper.QueryableExtensions;
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

        public MapTemplateService(IUnitOfWork unitOfWork, IUserProvider userProvider, IMapTemplateProvider mapTemplateProvider) 
            : base(unitOfWork, userProvider)
        {
            this.mapTemplateProvider = mapTemplateProvider;
        }

        public IQueryable<MapTemplateDescriptor> QuerySummary()
        {
            return this.UnitOfWork.MapTemplateDescriptors.Query().Project().To<MapTemplateDescriptor>();
        }

        public MapTemplate Get(string name)
        {
            return Mapper.Map<MapTemplate>(this.mapTemplateProvider.GetTemplate(name));
        }
    }
}