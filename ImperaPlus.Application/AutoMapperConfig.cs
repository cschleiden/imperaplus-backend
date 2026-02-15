using System;
using System.Linq;
using AutoMapper;

namespace ImperaPlus.Application
{
    //public class VictoryConditionsResolver : IMemberValueResolver<Domain.Games.GameOptions, DTO.Games.GameOptions, SerializedCollection<Domain.Enums.VictoryConditionType>, DTO.Games.VictoryConditionType[]>
    //{
    //    public DTO.Games.VictoryConditionType[] Resolve(Domain.Games.GameOptions source, DTO.Games.GameOptions destination, SerializedCollection<Domain.Enums.VictoryConditionType> sourceMember, DTO.Games.VictoryConditionType[] destMember, ResolutionContext context)
    //    {
    //        return sourceMember.Select(x => context.Mapper.Map<DTO.Games.VictoryConditionType>(x)).ToArray();
    //    }
    //}

    //public class VisibilityModifierResolver : IMemberValueResolver<Domain.Games.GameOptions, DTO.Games.GameOptions, SerializedCollection<Domain.Enums.VisibilityModifierType>, DTO.Games.VisibilityModifierType[]>
    //{
    //    public DTO.Games.VisibilityModifierType[] Resolve(Domain.Games.GameOptions source, DTO.Games.GameOptions destination, SerializedCollection<Domain.Enums.VisibilityModifierType> sourceMember, DTO.Games.VisibilityModifierType[] destMember, ResolutionContext context)
    //    {
    //        return sourceMember.Select(x => context.Mapper.Map<DTO.Games.VisibilityModifierType>(x)).ToArray();
    //    }
    //}

    //public class DomainVictoryConditionsResolver : IMemberValueResolver<DTO.Games.GameOptions, Domain.Games.GameOptions, IEnumerable<DTO.Games.VictoryConditionType>, SerializedCollection<Domain.Enums.VictoryConditionType>>
    //{
    //    public SerializedCollection<Domain.Enums.VictoryConditionType> Resolve(DTO.Games.GameOptions source, Domain.Games.GameOptions destination, IEnumerable<DTO.Games.VictoryConditionType> sourceMember, SerializedCollection<Domain.Enums.VictoryConditionType> destMember, ResolutionContext context)
    //    {
    //        return new SerializedCollection<Domain.Enums.VictoryConditionType>(
    //            sourceMember.Select(victoryCondition => context.Mapper.Map<Domain.Enums.VictoryConditionType>(victoryCondition)));
    //    }
    //}

    //public class DomainVisibilityModifierResolver : IMemberValueResolver<DTO.Games.GameOptions, Domain.Games.GameOptions, IEnumerable<DTO.Games.VisibilityModifierType>, SerializedCollection<Domain.Enums.VisibilityModifierType>>
    //{
    //    public SerializedCollection<Domain.Enums.VisibilityModifierType> Resolve(DTO.Games.GameOptions source, Domain.Games.GameOptions destination, IEnumerable<DTO.Games.VisibilityModifierType> sourceMember, SerializedCollection<Domain.Enums.VisibilityModifierType> destMember, ResolutionContext context)
    //    {
    //        return new SerializedCollection<Domain.Enums.VisibilityModifierType>(
    //            sourceMember.Select(victoryCondition => context.Mapper.Map<Domain.Enums.VisibilityModifierType>(victoryCondition)));
    //    }
    //}

    //public class SerializedConverter<T> : ITypeConverter<SerializedCollection<T>, IEnumerable<T>>
    //{
    //    public IEnumerable<T> Convert(SerializedCollection<T> source, IEnumerable<T> destination, ResolutionContext context)
    //    {
    //        return source;
    //    }
    //}

    public class AutoMapperConfig : Profile
    {
        public AutoMapperConfig()
        {
            CreateMap<Domain.Games.Game, DTO.Games.GameSummary>()
                .ForMember(x => x.Name, c => c.MapFrom(x => x.Name))
                .ForMember(x => x.HasPassword, c => c.MapFrom(x => x.IsPasswordProtected))
                .ForMember(x => x.Type, c => c.MapFrom(x => (DTO.Games.GameType)x.Type))
                .ForMember(x => x.State, c => c.MapFrom(x => (DTO.Games.GameState)x.State))
                .ForMember(x => x.MapTemplate, c => c.MapFrom(x => x.MapTemplateName))
                .ForMember(x => x.LastActionAt, c => c.MapFrom(x => x.LastModifiedAt))
                .ForMember(x => x.StartedAt, c => c.MapFrom(x => x.StartedAt))
                .ForMember(x => x.CreatedByUserId, c => c.MapFrom(x => x.CreatedById))
                .ForMember(x => x.CreatedByName, c => c.MapFrom(x => x.CreatedBy.UserName))
                .ForMember(x => x.TurnCounter, c => c.MapFrom(x => x.TurnCounter))
                .ForMember(
                    x => x.TimeoutSecondsLeft,
                    c => c.MapFrom(
                        x => x.Options.TimeoutInSeconds - (int)(DateTime.UtcNow - x.LastTurnStartedAt).TotalSeconds))
                // Ladder
                .ForMember(x => x.LadderId, c => c.MapFrom(x => x.LadderId))
                .ForMember(x => x.LadderName, c => c.MapFrom(x => x.Ladder != null ? x.Ladder.Name : null));

            CreateMap<Domain.Games.Game, DTO.Games.GameActionResult>()
                .ForMember(x => x.Id, c => c.MapFrom(x => x.Id))
                .ForMember(x => x.State, c => c.MapFrom(x => (DTO.Games.GameState)x.State))
                .ForMember(x => x.PlayState, c => c.MapFrom(x => (DTO.Games.PlayState)x.PlayState))
                .ForMember(x => x.CountryUpdates, c => c.Ignore())
                .ForMember(x => x.ActionResult, c => c.Ignore())
                .ForMember(x => x.TurnCounter, c => c.MapFrom(x => x.TurnCounter))
                .ForMember(x => x.Cards, c => c.MapFrom(x => x.CurrentPlayer.Cards))
                .ForMember(x => x.AttacksInCurrentTurn, c => c.MapFrom(x => x.AttacksInCurrentTurn))
                .ForMember(x => x.MovesInCurrentTurn, c => c.MapFrom(x => x.MovesInCurrentTurn))
                .ForMember(x => x.CurrentPlayer, c => c.MapFrom(x => x.CurrentPlayer))
                .ForMember(x => x.UnitsToPlace, c => c.Ignore()); // Manually mapped

            CreateMap<Domain.Games.Game, DTO.Games.Game>()
                .ForMember(x => x.Id, c => c.MapFrom(x => x.Id))
                .ForMember(x => x.Name, c => c.MapFrom(x => x.Name))
                .ForMember(x => x.HasPassword, c => c.MapFrom(x => x.IsPasswordProtected))
                .ForMember(x => x.Type, c => c.MapFrom(x => (DTO.Games.GameType)x.Type))
                .ForMember(x => x.State, c => c.MapFrom(x => (DTO.Games.GameState)x.State))
                .ForMember(x => x.MapTemplate, c => c.MapFrom(x => x.MapTemplateName))
                .ForMember(x => x.Map, c => c.MapFrom(x => x.Map))
                .ForMember(x => x.Teams, c => c.MapFrom(x => x.Teams))
                .ForMember(x => x.TurnCounter, c => c.MapFrom(x => x.TurnCounter))
                .ForMember(x => x.UnitsToPlace, c => c.Ignore()) // Manually mapped
                .ForMember(x => x.AttacksInCurrentTurn, c => c.MapFrom(x => x.AttacksInCurrentTurn))
                .ForMember(x => x.MovesInCurrentTurn, c => c.MapFrom(x => x.MovesInCurrentTurn))
                .ForMember(x => x.LastModifiedAt, c => c.MapFrom(x => x.LastModifiedAt))
                .ForMember(
                    x => x.TimeoutSecondsLeft,
                    c => c.MapFrom(
                        x => x.Options.TimeoutInSeconds - (int)(DateTime.UtcNow - x.LastTurnStartedAt).TotalSeconds));

            CreateMap<Domain.Games.Chat.GameChatMessage, DTO.Games.Chat.GameChatMessage>()
                .ForMember(x => x.DateTime, c => c.MapFrom(x => x.DateTime))
                .ForMember(x => x.Id, c => c.MapFrom(x => x.Id))
                .ForMember(x => x.TeamId, c => c.MapFrom(x => x.TeamId))
                .ForMember(x => x.Text, c => c.MapFrom(x => x.Text))
                .ForMember(x => x.User, c => c.MapFrom(x => x.User));

            // History
            CreateMap<Domain.Games.History.HistoryGameTurn, DTO.Games.History.HistoryTurn>()
                .ForMember(x => x.TurnId, c => c.MapFrom(x => x.TurnNo))
                .ForMember(x => x.Game, c => c.MapFrom(x => x.Game))
                .ForMember(x => x.Actions, c => c.MapFrom(x => x.Actions));
            CreateMap<Domain.Games.History.HistoryEntry, DTO.Games.History.HistoryEntry>();
            CreateMap<Domain.Games.History.HistoryAction, DTO.Games.History.HistoryAction>();

            CreateMap<Domain.Games.GameOptions, DTO.Games.GameOptions>()
                .ForMember(x => x.MapDistribution, c => c.MapFrom(x => (DTO.Games.MapDistribution)x.MapDistribution))
                .ForMember(x => x.VictoryConditions, c => c.MapFrom(x => x.VictoryConditions))
                .ForMember(x => x.VisibilityModifier, c => c.MapFrom(x => x.VisibilityModifier));

            CreateMap<DTO.Games.GameOptions, Domain.Games.GameOptions>()
                .ForMember(x => x.Id, c => c.Ignore())
                .ForMember(x => x.SerializedVictoryConditions, c => c.Ignore())
                .ForMember(x => x.SerializedVisibilityModifier, c => c.Ignore())
                .ForMember(x => x.MapDistribution, c => c.MapFrom(x => (Domain.Enums.MapDistribution)x.MapDistribution))
                .ForMember(x => x.VictoryConditions, c => c.MapFrom(x => x.VictoryConditions))
                .ForMember(x => x.VisibilityModifier, c => c.MapFrom(x => x.VisibilityModifier));

            CreateMap<Domain.Games.Map, DTO.Games.Map.Map>();

            CreateMap<Domain.Games.Country, DTO.Games.Map.Country>()
                .ForMember(x => x.Units, c => c.MapFrom(x => x.Units))
                .ForMember(x => x.Identifier, c => c.MapFrom(x => x.CountryIdentifier))
                .ForMember(x => x.PlayerId, c => c.MapFrom(x => x.PlayerId))
                .ForMember(x => x.TeamId, c => c.MapFrom(x => x.TeamId))
                .ForMember(x => x.Flags, c => c.MapFrom(x => (DTO.Games.CountryFlags)x.Flags));

            CreateMap<Domain.Games.Player, DTO.Games.Player>()
                .ForMember(x => x.UserId, c => c.MapFrom(x => x.UserId))
                .ForMember(x => x.Name, c => c.MapFrom(x => x.User.UserName))
                .ForMember(x => x.Cards, c => c.Condition((player, destinationModel, source, destination, context) =>
                {
                    // Map BonusCards only for current user
                    var userId = context.Items["userId"] as string;
                    return player != null && player.UserId == userId;
                }))
                .ForMember(x => x.State, c => c.MapFrom(x => (DTO.Games.PlayerState)x.State))
                .ForMember(x => x.Outcome, c => c.MapFrom(x => (DTO.Games.PlayerOutcome)x.Outcome))
                .ForMember(x => x.Timeouts, c => c.MapFrom(x => x.Timeouts))
                .ForMember(x => x.NumberOfCountries, c => c.MapFrom(x => x.Countries.Count()))
                .ForMember(x => x.NumberOfUnits, c => c.MapFrom(x => x.Countries.Sum(y => y.Units)));

            CreateMap<Domain.Games.Player, DTO.Games.PlayerSummary>()
                .ForMember(x => x.UserId, c => c.MapFrom(x => x.UserId))
                .ForMember(x => x.Name, c => c.MapFrom(x => x.User.UserName))
                .ForMember(x => x.State, c => c.MapFrom(x => (DTO.Games.PlayerState)x.State))
                .ForMember(x => x.Outcome, c => c.MapFrom(x => (DTO.Games.PlayerOutcome)x.Outcome))
                .ForMember(x => x.Timeouts, c => c.MapFrom(x => x.Timeouts));

            CreateMap<Domain.Games.Team, DTO.Games.Team>();
            CreateMap<Domain.Games.Team, DTO.Games.TeamSummary>();

            CreateMap<Domain.Map.MapTemplate, DTO.Games.Map.MapTemplate>();
            CreateMap<Domain.Map.MapTemplate, DTO.Games.Map.MapTemplateDescriptor>()
                .ForMember(x => x.IsActive, c => c.MapFrom(_ => true));
            CreateMap<Domain.Map.MapTemplateDescriptor, DTO.Games.Map.MapTemplateDescriptor>();

            CreateMap<Domain.Enums.VictoryConditionType, DTO.Games.VictoryConditionType>();
            CreateMap<Domain.Enums.VictoryConditionType, DTO.Games.VictoryConditionType>().ReverseMap();
            CreateMap<Domain.Enums.VisibilityModifierType, DTO.Games.VisibilityModifierType>();
            CreateMap<Domain.Enums.VisibilityModifierType, DTO.Games.VisibilityModifierType>().ReverseMap();

            CreateMap<Domain.Map.CountryTemplate, DTO.Games.Map.CountryTemplate>();
            CreateMap<Domain.Map.CountryTemplate, string>()
                .ConvertUsing(x => x.Identifier.ToString());
            CreateMap<Domain.Map.Connection, DTO.Games.Map.Connection>();
            CreateMap<Domain.Map.Continent, DTO.Games.Map.Continent>();

            CreateMap<Domain.Chat.ChatMessage, DTO.Chat.ChatMessage>()
                .ForMember(x => x.ChannelIdentifier, x => x.MapFrom(ci => ci.ChannelId))
                .ForMember(x => x.DateTime, x => x.MapFrom(ci => ci.CreatedAt))
                .ForMember(x => x.UserName, x => x.MapFrom(ci => ci.CreatedBy.UserName))
                .ForMember(x => x.Text, x => x.MapFrom(ci => ci.Text));

            CreateMap<Domain.Chat.Channel, DTO.Chat.ChannelInformation>()
                .ForMember(x => x.Identifier, x => x.MapFrom(ci => ci.Id))
                .ForMember(x => x.Persistant, x => x.MapFrom(_ => true))
                .ForMember(x => x.Title, x => x.MapFrom(ci => ci.Name))
                .ForMember(x => x.Messages, x => x.MapFrom(ci => ci.RecentMessages))
                .ForMember(x => x.Users, cfg => cfg.Ignore());

            CreateMap<Domain.News.NewsContent, DTO.News.NewsContent>();

            CreateMap<Domain.News.NewsEntry, DTO.News.NewsItem>()
                .ForMember(x => x.DateTime, x => x.MapFrom(ne => ne.CreatedAt))
                .ForMember(x => x.PostedBy, x => x.MapFrom(ne => ne.CreatedBy.UserName));

            CreateLadderMapping();
            CreateTournamentMapping();
            CreateMessageMapping();
            CreateUserMapping();
            CreateAllianceMapping();
        }

        private void CreateAllianceMapping()
        {
            CreateMap<Domain.Alliances.Alliance, DTO.Alliances.AllianceSummary>()
                .ForMember(x => x.NumberOfMembers, x => x.MapFrom(a => a.Members.Count()))
                .ForMember(x => x.Admins, x => x.MapFrom(a => a.Administrators));

            CreateMap<Domain.Alliances.Alliance, DTO.Alliances.Alliance>()
                .ForMember(x => x.NumberOfMembers, x => x.MapFrom(a => a.Members.Count()))
                .ForMember(x => x.Admins, x => x.MapFrom(a => a.Administrators));

            CreateMap<Domain.Alliances.AllianceJoinRequest, DTO.Alliances.AllianceJoinRequest>();
            CreateMap<Domain.Alliances.AllianceJoinRequestState, DTO.Alliances.AllianceJoinRequestState>();
        }

        private void CreateLadderMapping()
        {
            // Standing is mapped separately
            CreateMap<Domain.Ladders.Ladder, DTO.Ladder.LadderSummary>()
                .ForMember(x => x.Standing, x => x.Ignore())
                .ForMember(x => x.IsQueued, x => x.Ignore())
                .ForMember(x => x.QueueCount, x => x.MapFrom(l => l.Queue.Count()));


            CreateMap<Domain.Ladders.Ladder, DTO.Ladder.Ladder>()
                .ForMember(x => x.Standing, x => x.Ignore())
                .ForMember(x => x.Standings, x => x.Ignore())
                .ForMember(x => x.IsQueued, x => x.Ignore())
                .ForMember(x => x.QueueCount, x => x.Ignore());
        }

        private void CreateTournamentMapping()
        {
            CreateMap<Domain.Tournaments.Tournament, DTO.Tournaments.TournamentSummary>();
            CreateMap<Domain.Tournaments.Tournament, DTO.Tournaments.Tournament>()
                .ForMember(x => x.Password, x => x.Ignore());

            CreateMap<Domain.Tournaments.TournamentTeam, DTO.Tournaments.TournamentTeam>()
                .ForMember(x => x.RequiresPassword, x => x.MapFrom(t => !string.IsNullOrWhiteSpace(t.Password)));
            CreateMap<Domain.Tournaments.TournamentTeam, DTO.Tournaments.TournamentTeamSummary>()
                .ForMember(x => x.RequiresPassword, x => x.MapFrom(t => !string.IsNullOrWhiteSpace(t.Password)));

            CreateMap<Domain.Tournaments.TournamentGroup, DTO.Tournaments.TournamentGroup>();
            CreateMap<Domain.Tournaments.TournamentParticipant, DTO.Users.UserReference>()
                .ForMember(x => x.Id, x => x.MapFrom(p => p.UserId))
                .ForMember(x => x.Name, x => x.MapFrom(p => p.User.UserName));

            CreateMap<Domain.Tournaments.TournamentPairing, DTO.Tournaments.TournamentPairing>();

            CreateMap<Domain.Tournaments.TournamentState, DTO.Tournaments.TournamentState>();
            CreateMap<Domain.Tournaments.TournamentTeamState, DTO.Tournaments.TournamentTeamState>();
        }

        private void CreateMessageMapping()
        {
            CreateMap<Domain.Messages.Message, DTO.Messages.Message>()
                .ForMember(x => x.SentAt, x => x.MapFrom(c => c.CreatedAt))
                .ForMember(x => x.From, x => x.MapFrom(c => c.From))
                .ForMember(x => x.To, x => x.MapFrom(c => c.Recipient))
                .ForMember(x => x.IsRead, x => x.MapFrom(c => c.IsRead));
        }

        private void CreateUserMapping()
        {
            CreateMap<Domain.User, DTO.Users.UserReference>()
                .ForMember(x => x.Id, x => x.MapFrom(c => c.Id))
                .ForMember(x => x.Name, x => x.MapFrom(c => c.UserName));
        }
    }
}
