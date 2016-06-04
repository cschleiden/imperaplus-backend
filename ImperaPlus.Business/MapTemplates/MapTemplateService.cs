using System.Linq;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using ImperaPlus.Domain.Repositories;
using ImperaPlus.DTO.Games.Map;
using ImperaPlus.Domain.Services;
using ImperaPlus.DataAccess;

namespace ImperaPlus.Application
{
    public interface IMapTemplateService
    {
        IQueryable<MapTemplateSummary> QuerySummary();

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

        public IQueryable<MapTemplateSummary> QuerySummary()
        {
            return this.UnitOfWork.MapTemplates.Query().Project().To<MapTemplateSummary>();
        }

        public MapTemplate Get(string name)
        {
            return Mapper.Map<MapTemplate>(this.mapTemplateProvider.GetTemplate(name));
        }
    }
}