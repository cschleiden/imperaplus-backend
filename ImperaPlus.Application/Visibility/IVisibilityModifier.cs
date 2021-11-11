using ImperaPlus.Domain.Services;
using ImperaPlus.DTO.Games;
using ImperaPlus.DTO.Games.History;
using System.Collections.Generic;

namespace ImperaPlus.Application.Visibility
{
    public interface IVisibilityModifier
    {
        void Apply(Domain.User user, Game game);

        void Apply(Domain.User user, HistoryTurn historyTurn, DTO.Games.Map.Map previousHistoryTurn);

        void Expand(Domain.User user, Domain.Games.Game game, List<Domain.Games.Country> changedCountries);
    }

    public abstract class BaseVisibilityModifier : IVisibilityModifier
    {
        protected IMapTemplateProvider MapTemplateProvider { get; set; }

        public BaseVisibilityModifier(IMapTemplateProvider mapTemplateProvider)
        {
            MapTemplateProvider = mapTemplateProvider;
        }

        public abstract void Apply(Domain.User user, Game game);

        public abstract void Apply(Domain.User user, HistoryTurn historyTurn, DTO.Games.Map.Map previousMap);

        public abstract void Expand(Domain.User user, Domain.Games.Game game,
            List<Domain.Games.Country> changedCountries);
    }

    public class VisibilityModifierFactory : IVisibilityModifierFactory
    {
        private IMapTemplateProvider mapTemplateProvider;

        public VisibilityModifierFactory(IMapTemplateProvider mapTemplateProvider)
        {
            this.mapTemplateProvider = mapTemplateProvider;
        }

        public IVisibilityModifier Construct(Domain.Enums.VisibilityModifierType visibilityModifierType)
        {
            switch (visibilityModifierType)
            {
                case Domain.Enums.VisibilityModifierType.Fog:
                    return new FogVisibilityModifier(mapTemplateProvider);

                default:
                    return new DefaultVisibilityModifier(mapTemplateProvider);
            }
        }
    }
}
