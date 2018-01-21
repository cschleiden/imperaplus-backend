using System;
using System.Linq;
using System.Collections.Generic;
using AutoMapper;
using ImperaPlus.DTO.Games;
using ImperaPlus.Domain.Utilities;

namespace ImperaPlus.Application
{
    public class ConditionalBonusCardResolver : ValueResolver<Domain.Games.BonusCard, DTO.Games.BonusCard>
    {
        protected override DTO.Games.BonusCard ResolveCore(Domain.Games.BonusCard source)
        {
            throw new NotImplementedException();
        }
    }

    public class VictoryConditionsResolver : ValueResolver<SerializedCollection<Domain.Enums.VictoryConditionType>, IEnumerable<DTO.Games.VictoryConditionType>>
    {
        protected override IEnumerable<VictoryConditionType> ResolveCore(SerializedCollection<Domain.Enums.VictoryConditionType> source)
        {
            foreach(var victoryCondition in source)
            {
                yield return Mapper.Map<VictoryConditionType>(victoryCondition);
            }
        }
    }

    public class VisibilityModifierResolver : ValueResolver<SerializedCollection<Domain.Enums.VisibilityModifierType>, IEnumerable<DTO.Games.VisibilityModifierType>>
    {
        protected override IEnumerable<VisibilityModifierType> ResolveCore(SerializedCollection<Domain.Enums.VisibilityModifierType> source)
        {
            foreach (var visibilityModifier in source)
            {
                yield return Mapper.Map<VisibilityModifierType>(visibilityModifier);
            }
        }
    }

    public class DomainVictoryConditionsResolver : ValueResolver<IEnumerable<DTO.Games.VictoryConditionType>, SerializedCollection<Domain.Enums.VictoryConditionType>>
    {
        protected override SerializedCollection<Domain.Enums.VictoryConditionType> ResolveCore(IEnumerable<VictoryConditionType> source)
        {
            return new SerializedCollection<Domain.Enums.VictoryConditionType>(
                source.Select(victoryCondition => Mapper.Map<Domain.Enums.VictoryConditionType>(victoryCondition)));
        }
    }

    public class DomainVisibilityModifierResolver : ValueResolver<IEnumerable<DTO.Games.VisibilityModifierType>, SerializedCollection<Domain.Enums.VisibilityModifierType>>
    {
        protected override SerializedCollection<Domain.Enums.VisibilityModifierType> ResolveCore(IEnumerable<VisibilityModifierType> source)
        {
            return new SerializedCollection<Domain.Enums.VisibilityModifierType>(
                source.Select(victoryCondition => Mapper.Map<Domain.Enums.VisibilityModifierType>(victoryCondition)));
        }
    }

    public class SerializedConverter<T> : ValueResolver<SerializedCollection<T>, IEnumerable<T>>
    {
        protected override IEnumerable<T> ResolveCore(SerializedCollection<T> source)
        {
            return source;
        }
    }

    public static class AutoMapperConfig       
    {
        public static void Configure()
        {
            Mapper.Reset();

            Mapper.CreateMap(typeof(SerializedCollection<>), typeof(IEnumerable<>)); //.ConvertUsing(typeof(SerializedConverter<>));

            Mapper.CreateMap<Domain.Games.Game, DTO.Games.GameSummary>()
                .ForMember(x => x.Name, c => c.MapFrom(x => x.Name))
                .ForMember(x => x.HasPassword, c => c.MapFrom(x => x.IsPasswordProtected))
                .ForMember(x => x.Type, c => c.MapFrom(x => (GameType)x.Type))
                .ForMember(x => x.State, c => c.MapFrom(x => (GameState)x.State))
                .ForMember(x => x.MapTemplate, c => c.MapFrom(x => x.MapTemplateName))
                .ForMember(x => x.LastActionAt, c => c.MapFrom(x => x.LastModifiedAt))
                .ForMember(x => x.StartedAt, c => c.MapFrom(x => x.StartedAt))
                .ForMember(x => x.CreatedByUserId, c => c.MapFrom(x => x.CreatedById))
                .ForMember(x => x.CreatedByName, c => c.MapFrom(x => x.CreatedBy.UserName))
                .ForMember(
                    x => x.TimeoutSecondsLeft, 
                    c => c.MapFrom(
                        x => x.Options.TimeoutInSeconds - (int)((DateTime.UtcNow - x.LastTurnStartedAt).TotalSeconds)))
                // Ladder
                .ForMember(x => x.LadderId, c => c.MapFrom(x => x.LadderId))
                .ForMember(x => x.LadderName, c => c.MapFrom(x => x.Ladder != null ? x.Ladder.Name : null));

            Mapper.CreateMap<Domain.Games.Game, DTO.Games.GameActionResult>()
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
           
            Mapper.CreateMap<Domain.Games.Game, DTO.Games.Game>()
                .ForMember(x => x.Id, c => c.MapFrom(x => x.Id))
                .ForMember(x => x.Name, c => c.MapFrom(x => x.Name))
                .ForMember(x => x.HasPassword, c => c.MapFrom(x => x.IsPasswordProtected))
                .ForMember(x => x.Type, c => c.MapFrom(x => (GameType)x.Type))
                .ForMember(x => x.State, c => c.MapFrom(x => (GameState)x.State))
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
                        x => x.Options.TimeoutInSeconds - (int)((DateTime.UtcNow - x.LastTurnStartedAt).TotalSeconds)));

            Mapper.CreateMap<Domain.Games.Chat.GameChatMessage, DTO.Games.Chat.GameChatMessage>()
                .ForMember(x => x.DateTime, c => c.MapFrom(x => x.DateTime))
                .ForMember(x => x.Id, c => c.MapFrom(x => x.Id))
                .ForMember(x => x.TeamId, c => c.MapFrom(x => x.TeamId))
                .ForMember(x => x.Text, c => c.MapFrom(x => x.Text))
                .ForMember(x => x.User, c => c.MapFrom(x => x.User));

            // History
            Mapper.CreateMap<Domain.Games.History.HistoryGameTurn, DTO.Games.History.HistoryTurn>()
                .ForMember(x => x.TurnId, c => c.MapFrom(x => x.TurnNo))
                .ForMember(x => x.Game, c => c.MapFrom(x => x.Game))
                .ForMember(x => x.Actions, c => c.MapFrom(x => x.Actions));
            Mapper.CreateMap<Domain.Games.History.HistoryEntry, DTO.Games.History.HistoryEntry>();
            Mapper.CreateMap<Domain.Games.History.HistoryAction, DTO.Games.History.HistoryAction>();

            Mapper.CreateMap<Domain.Games.GameOptions, DTO.Games.GameOptions>()
                .ForMember(x => x.MapDistribution, c => c.MapFrom(x => (MapDistribution)x.MapDistribution))
                .ForMember(x => x.VictoryConditions, c => c.ResolveUsing<VictoryConditionsResolver>().FromMember(x => x.VictoryConditions))
                .ForMember(x => x.VisibilityModifier, c => c.ResolveUsing<VisibilityModifierResolver>().FromMember(x => x.VisibilityModifier));

            Mapper.CreateMap<DTO.Games.GameOptions, Domain.Games.GameOptions>()
                .ForMember(x => x.Id, c => c.Ignore())
                .ForMember(x => x.SerializedVictoryConditions, c => c.Ignore())
                .ForMember(x => x.SerializedVisibilityModifier, c => c.Ignore())
                .ForMember(x => x.MapDistribution, c => c.MapFrom(x => (Domain.Enums.MapDistribution)x.MapDistribution))
                .ForMember(x => x.VictoryConditions, c => c.ResolveUsing<DomainVictoryConditionsResolver>().FromMember(x => x.VictoryConditions))
                .ForMember(x => x.VisibilityModifier, c => c.ResolveUsing<DomainVisibilityModifierResolver>().FromMember(x => x.VisibilityModifier));
            
            Mapper.CreateMap<Domain.Games.Map, DTO.Games.Map.Map>();

            Mapper.CreateMap<Domain.Games.Country, DTO.Games.Map.Country>()
                .ForMember(x => x.Units, c => c.MapFrom(x => x.Units))
                .ForMember(x => x.Identifier, c => c.MapFrom(x => x.CountryIdentifier))                
                .ForMember(x => x.PlayerId, c => c.MapFrom(x => x.PlayerId))
                .ForMember(x => x.TeamId, c => c.MapFrom(x => x.TeamId));

            Mapper.CreateMap<Domain.Games.Player, DTO.Games.Player>()
                .ForMember(x => x.UserId, c => c.MapFrom(x => x.UserId))
                .ForMember(x => x.Name, c => c.MapFrom(x => x.User.UserName))
                .ForMember(x => x.Cards, c => c.Condition((ResolutionContext context) =>
                {
                    // Map BonusCards only for current user
                    var userId = context.Options.Items["userId"] as string;
                    var player = context.Parent.SourceValue as Domain.Games.Player;

                    return player != null && player.UserId == userId;
                }))
                .ForMember(x => x.State, c => c.MapFrom(x => (PlayerState)x.State))
                .ForMember(x => x.Outcome, c => c.MapFrom(x => (PlayerOutcome)x.Outcome))
                .ForMember(x => x.Timeouts, c => c.MapFrom(x => x.Timeouts))
                .ForMember(x => x.NumberOfCountries, c => c.MapFrom(x => x.Countries.Count()))
                .ForMember(x => x.NumberOfUnits, c => c.MapFrom(x => x.Countries.Sum(y => y.Units)));

            Mapper.CreateMap<Domain.Games.Player, DTO.Games.PlayerSummary>()
                .ForMember(x => x.UserId, c => c.MapFrom(x => x.UserId))
                .ForMember(x => x.Name, c => c.MapFrom(x => x.User.UserName))
                .ForMember(x => x.State, c => c.MapFrom(x => (PlayerState)x.State))
                .ForMember(x => x.Outcome, c => c.MapFrom(x => (PlayerOutcome)x.Outcome))
                .ForMember(x => x.Timeouts, c => c.MapFrom(x => x.Timeouts));

            Mapper.CreateMap<Domain.Games.Team, DTO.Games.Team>();
            Mapper.CreateMap<Domain.Games.Team, DTO.Games.TeamSummary>();

            Mapper.CreateMap<Domain.Map.MapTemplate, DTO.Games.Map.MapTemplate>();
            Mapper.CreateMap<Domain.Map.MapTemplate, DTO.Games.Map.MapTemplateDescriptor>()
                .ForMember(x => x.IsActive, c => c.UseValue(true));
            Mapper.CreateMap<Domain.Map.MapTemplateDescriptor, DTO.Games.Map.MapTemplateDescriptor>();

            Mapper.CreateMap<Domain.Enums.VictoryConditionType, DTO.Games.VictoryConditionType>().ReverseMap();
            Mapper.CreateMap<Domain.Enums.VisibilityModifierType, DTO.Games.VisibilityModifierType>().ReverseMap();

            Mapper.CreateMap<Domain.Map.CountryTemplate, DTO.Games.Map.CountryTemplate>();
            Mapper.CreateMap<Domain.Map.CountryTemplate, string>()
                .ConvertUsing(x => x.Identifier.ToString());
            Mapper.CreateMap<Domain.Map.Connection, DTO.Games.Map.Connection>();
            Mapper.CreateMap<Domain.Map.Continent, DTO.Games.Map.Continent>();

            Mapper.CreateMap<Domain.Chat.ChatMessage, DTO.Chat.ChatMessage>()
                .ForMember(x => x.ChannelIdentifier, x => x.MapFrom(ci => ci.ChannelId))
                .ForMember(x => x.DateTime, x => x.MapFrom(ci => ci.CreatedAt))
                .ForMember(x => x.UserName, x => x.MapFrom(ci => ci.CreatedBy.UserName))
                .ForMember(x => x.Text, x => x.MapFrom(ci => ci.Text));
            
            Mapper.CreateMap<Domain.Chat.Channel, DTO.Chat.ChannelInformation>()
                .ForMember(x => x.Identifier, x => x.MapFrom(ci => ci.Id))
                .ForMember(x => x.Persistant, x => x.UseValue(true))
                .ForMember(x => x.Title, x => x.MapFrom(ci => ci.Name))
                .ForMember(x => x.Messages, x => x.MapFrom(ci => ci.RecentMessages))
                .ForMember(x => x.Users, config => config.Ignore());

            Mapper.CreateMap<Domain.News.NewsContent, DTO.News.NewsContent>();

            Mapper.CreateMap<Domain.News.NewsEntry, DTO.News.NewsItem>()
                .ForMember(x => x.DateTime, x => x.MapFrom(ne => ne.CreatedAt))
                .ForMember(x => x.PostedBy, x => x.MapFrom(ne => ne.CreatedBy.UserName));

            CreateLadderMapping();
            CreateTournamentMapping();
            CreateMessageMapping();
            CreateUserMapping();

            Mapper.AssertConfigurationIsValid();
        }

        private static void CreateLadderMapping()
        {
            // Standing is mapped separately
            Mapper.CreateMap<Domain.Ladders.Ladder, DTO.Ladder.LadderSummary>()
                .ForMember(x => x.Standing, x => x.Ignore())
                .ForMember(x => x.IsQueued, x => x.Ignore())
                .ForMember(x => x.QueueCount, x => x.MapFrom(l => l.Queue.Count()));


            Mapper.CreateMap<Domain.Ladders.Ladder, DTO.Ladder.Ladder>()
                .ForMember(x => x.Standing, x => x.Ignore())
                .ForMember(x => x.Standings, x => x.Ignore())
                .ForMember(x => x.IsQueued, x => x.Ignore())
                .ForMember(x => x.QueueCount, x => x.Ignore());
        }

        private static void CreateTournamentMapping()
        {
            Mapper.CreateMap<Domain.Tournaments.Tournament, DTO.Tournaments.TournamentSummary>();
            Mapper.CreateMap<Domain.Tournaments.Tournament, DTO.Tournaments.Tournament>();

            Mapper.CreateMap<Domain.Tournaments.TournamentTeam, DTO.Tournaments.TournamentTeam>();
            Mapper.CreateMap<Domain.Tournaments.TournamentTeam, DTO.Tournaments.TournamentTeamSummary>();

            Mapper.CreateMap<Domain.Tournaments.TournamentGroup, DTO.Tournaments.TournamentGroup>();
            Mapper.CreateMap<Domain.Tournaments.TournamentParticipant, DTO.Users.UserReference>()
                .ForMember(x => x.Id, x => x.MapFrom(p => p.UserId))
                .ForMember(x => x.Name, x => x.MapFrom(p => p.User.UserName));

            Mapper.CreateMap<Domain.Tournaments.TournamentPairing, DTO.Tournaments.TournamentPairing>();

            Mapper.CreateMap<Domain.Tournaments.TournamentState, DTO.Tournaments.TournamentState>();
            Mapper.CreateMap<Domain.Tournaments.TournamentTeamState, DTO.Tournaments.TournamentTeamState>();
        }

        private static void CreateMessageMapping()
        {
            Mapper.CreateMap<Domain.Messages.Message, DTO.Messages.Message>()
                .ForMember(x => x.SentAt, x => x.MapFrom(c => c.CreatedAt))
                .ForMember(x => x.From, x => x.MapFrom(c => c.From))
                .ForMember(x => x.To, x => x.MapFrom(c => c.Recipient))
                .ForMember(x => x.IsRead, x => x.MapFrom(c => c.IsRead));
        }

        private static void CreateUserMapping()
        {
            Mapper.CreateMap<Domain.User, DTO.Users.UserReference>()
                .ForMember(x => x.Id, x => x.MapFrom(c => c.Id))
                .ForMember(x => x.Name, x => x.MapFrom(c => c.UserName));
        }
    }
}