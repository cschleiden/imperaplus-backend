﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics;
using System.Linq;
using ImperaPlus.Domain.Exceptions;
using ImperaPlus.Domain.Games;
using ImperaPlus.Domain.Services;
using ImperaPlus.Domain.Utilities;

namespace ImperaPlus.Domain.Tournaments
{
    [Table("Tournament")]
    public class Tournament : Entity, IChangeTrackedEntity
    {
        public const int GroupSize = 4;

        public static readonly TournamentState[] ActiveStates = { TournamentState.Groups, TournamentState.Knockout };

        /// <summary>
        /// Only use to support automatic form generation
        /// </summary>
        /// <returns>Empty tournament instance</returns>
        public static Tournament CreateEmpty()
        {
            return new Tournament() { Options = new GameOptions() };
        }

        protected Tournament()
        {
            Id = Guid.NewGuid();

            MapTemplates = new SerializedCollection<string>();

            Groups = new HashSet<TournamentGroup>();
            Teams = new HashSet<TournamentTeam>();
            Pairings = new HashSet<TournamentPairing>();

            Phase = 0;
        }

        public Tournament(
            string name,
            int numberOfTeams,
            int numberOfGroupGames,
            int numberOfKnockoutGames,
            int numberOfFinalGames,
            DateTime startOfRegistration,
            DateTime startOfTournament,
            GameOptions options)
            : this()
        {
            Require.NotNullOrEmpty(name, nameof(name));
            Require.NotNull(options, nameof(options));

            Name = name;

            if (numberOfTeams <= 0 || Math.Pow(2, Math.Log(numberOfTeams, 2)) != numberOfTeams)
            {
                throw new DomainException(
                    ErrorCode.TournamentInvalidOption,
                    "Number of teams has to be a power of two and greater than zero");
            }

            NumberOfTeams = numberOfTeams;

            if (numberOfGroupGames < 0 || numberOfGroupGames > 0 && numberOfGroupGames % 2 == 0)
            {
                throw new DomainException(ErrorCode.TournamentInvalidOption,
                    "Number of group games has to be odd and 0 or more");
            }

            NumberOfGroupGames = numberOfGroupGames;

            if (NumberOfGroupGames > 0)
            {
                if (numberOfTeams < 8)
                {
                    throw new DomainException(ErrorCode.TournamentInvalidOption,
                        "At least 8 teams required for group mode");
                }
            }

            if (numberOfKnockoutGames <= 0 || numberOfKnockoutGames % 2 == 0)
            {
                throw new DomainException(ErrorCode.TournamentInvalidOption,
                    "Number of knockout games has to be odd and greater than zero");
            }

            NumberOfKnockoutGames = numberOfKnockoutGames;

            if (numberOfFinalGames <= 0 || numberOfFinalGames % 2 == 0)
            {
                throw new DomainException(ErrorCode.TournamentInvalidOption,
                    "Number of final games has to be odd and greater than zero");
            }

            NumberOfFinalGames = numberOfFinalGames;

            OptionsId = options.Id;
            Options = options;

            StartOfRegistration = startOfRegistration;
            StartOfTournament = startOfTournament;
        }

        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public Guid Id { get; private set; }

        public string Name { get; private set; }

        /// <summary>
        /// Number of teams allowed in tournament. Has to be power of 2
        /// </summary>
        public int NumberOfTeams { get; protected set; }

        public long OptionsId { get; protected set; }

        public virtual GameOptions Options { get; protected set; }

        public virtual ICollection<TournamentGroup> Groups { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime LastModifiedAt { get; set; }

        public virtual ICollection<TournamentTeam> Teams { get; private set; }

        public virtual ICollection<TournamentPairing> Pairings { get; set; }

        /// <summary>
        /// How many group games should be played, has to be odd
        /// </summary>
        public int NumberOfGroupGames { get; protected set; }

        /// <summary>
        /// How many group games should be played, has to be odd
        /// </summary>
        public int NumberOfKnockoutGames { get; protected set; }

        /// <summary>
        /// How many group games should be played, has to be odd
        /// </summary>
        public int NumberOfFinalGames { get; protected set; }

        /// <summary>
        /// Start of registration period
        /// </summary>
        public DateTime StartOfRegistration { get; protected set; }

        /// <summary>
        /// Start of tournament
        /// </summary>
        public DateTime StartOfTournament { get; protected set; }

        /// <summary>
        /// End of tournament
        /// </summary>
        public DateTime? EndOfTournament { get; protected set; }

        public virtual TournamentTeam Winner { get; protected set; }

        public Guid? WinnerId { get; protected set; }

        /// <summary>
        /// Current phase of knockout games
        /// </summary>
        public int Phase { get; set; }

        public TournamentState State { get; set; }

        [NotMapped] public SerializedCollection<string> MapTemplates { get; private set; }

        public string SerializedMapTemplates
        {
            get => MapTemplates.Serialize();

            set => MapTemplates = new SerializedCollection<string>(value);
        }

        /// <summary>
        /// Completion of tournament in percent
        /// </summary>
        [NotMapped]
        public int Completion
        {
            get
            {
                if (State == TournamentState.Open)
                {
                    return 0;
                }

                if (State == TournamentState.Closed)
                {
                    return 100;
                }

                return ActiveTeams.Count() / Teams.Count() * 100;
            }
        }

        /// <summary>
        /// Value indicating whether tournament is for single player teams
        /// </summary>
        [NotMapped]
        public bool IsSinglePlayerTournament => Options.NumberOfPlayersPerTeam == 1;

        /// <summary>
        /// Gets value indicating whether players can register
        /// </summary>
        [NotMapped]
        public bool CanRegister =>
            State == TournamentState.Open
            && DateTime.UtcNow >= StartOfRegistration
            && DateTime.UtcNow < StartOfTournament
            && Teams.SelectMany(x => x.Participants).Count() <= NumberOfTeams * Options.NumberOfPlayersPerTeam;

        /// <summary>
        /// Gets value indicating whether tournament can be started
        /// </summary>
        [NotMapped]
        public bool CanStart =>
            State == TournamentState.Open
            && Teams.SelectMany(x => x.Participants).Count() == NumberOfTeams * Options.NumberOfPlayersPerTeam
            && DateTime.UtcNow >= StartOfTournament;

        /// <summary>
        /// Gets value indicating whether the tournament has a group phase or only knockout
        /// </summary>
        [NotMapped]
        public bool HasGroupPhase => NumberOfGroupGames > 0;

        [NotMapped] public bool CanStartNextRound => Pairings.All(p => p.CanWinnerBeDetermined) && !CanEnd;

        [NotMapped] public bool CanEnd => ActiveTeams.Count() == 1;

        [NotMapped]
        public IEnumerable<TournamentTeam> ActiveTeams => Teams.Where(t => t.State != TournamentTeamState.InActive);

        [NotMapped] public bool CanChangeTeams => State == TournamentState.Open;

        public string GetMapTemplateForGame(IRandomGen random)
        {
            if (!MapTemplates.Any())
            {
                throw new DomainException(ErrorCode.TournamentNoMapTemplates, "No map templates set for tournament");
            }

            return MapTemplates.Shuffle(random).First();
        }

        /// <summary>
        /// Add user to tournament. Team will be automatically created
        /// </summary>
        /// <param name="user">User to join</param>
        /// <returns>Team the user was added to</returns>
        public TournamentTeam AddUser(User user)
        {
            Require.NotNull(user, nameof(user));

            if (!IsSinglePlayerTournament)
            {
                throw new DomainException(
                    ErrorCode.TournamentRequiresExplicitTeam,
                    "For team tournaments, joining a team explicitly is required");
            }

            return AddUser(user, null);
        }

        /// <summary>
        /// Add user to tournament and given team
        /// </summary>
        /// <param name="user">User to join</param>
        /// <param name="team">Team to join</param>
        /// <param name="password">Optional password if team is protected</param>
        /// <returns>Team the user was added to</returns>
        public TournamentTeam AddUser(User user, TournamentTeam team, string password = null)
        {
            Require.NotNull(user, nameof(user));

            if (!CanChangeTeams)
            {
                throw new DomainException(ErrorCode.TournamentCannotJoinLeave, "Cannot leave team");
            }

            if (team == null)
            {
                if (Teams.Count() >= NumberOfTeams)
                {
                    throw new DomainException(ErrorCode.TournamentTooManyTeams, "Too many teams already");
                }

                team = new TournamentTeam(this);
                team.Name = user.UserName;
                team.CreatedBy = user;
                team.CreatedById = user.Id;

                Teams.Add(team);
            }
            else
            {
                if (!team.PasswordMatch(password))
                {
                    throw new DomainException(ErrorCode.TournamentIncorrectPassword,
                        "Cannot join team, password incorrect");
                }
            }

            team.AddUser(user);

            return team;
        }

        public TournamentParticipant LeaveUser(User user)
        {
            Require.NotNull(user, nameof(user));

            if (!CanChangeTeams)
            {
                throw new DomainException(ErrorCode.TournamentCannotJoinLeave, "Cannot leave team");
            }

            var currentTeam = Teams.FirstOrDefault(t => t.Participants.Any(p => p.UserId == user.Id));
            if (currentTeam == null)
            {
                throw new DomainException(ErrorCode.TournamentUserNoParticipant,
                    "User is no participant in tournament");
            }

            if (currentTeam.CreatedById == user.Id)
            {
                throw new DomainException(ErrorCode.TournamentTeamCreatorHasToDelete,
                    "Creator of team has to delete team");
            }

            var participant = currentTeam.Participants.FirstOrDefault(p => p.UserId == user.Id);
            currentTeam.Participants.Remove(participant);

            return participant;
        }

        /// <summary>
        /// Create new team in tournament
        /// </summary>
        /// <param name="user">User creating team</param>
        /// <param name="name">Name of new team</param>
        /// <param name="password">Optional password to protect team membership</param>
        public TournamentTeam CreateTeam(User user, string name, string password = null)
        {
            Require.NotNull(user, nameof(user));
            Require.NotNullOrEmpty(name, nameof(name));

            if (!CanChangeTeams)
            {
                throw new DomainException(ErrorCode.TournamentCannotJoinLeave, "Cannot create team");
            }

            if (Teams.Count() >= NumberOfTeams)
            {
                throw new DomainException(ErrorCode.TournamentTooManyTeams, "Too many teams already");
            }

            if (Teams.Any(t => string.Equals(t.Name, name, StringComparison.OrdinalIgnoreCase)))
            {
                throw new DomainException(ErrorCode.TournamentDuplicateTeamName, "Team with name already exists");
            }

            var team = new TournamentTeam(this)
            {
                CreatedBy = user, CreatedById = user.Id, Name = name, Password = password
            };

            team.AddUser(user);

            Teams.Add(team);

            return team;
        }

        /// <summary>
        /// Gets a team in the current tournament that the given user has created
        /// </summary>
        /// <param name="user">User to find team</param>
        public TournamentTeam GetOwnedTeamForUser(User user)
        {
            Require.NotNull(user, nameof(user));

            var ownedTeam = Teams.FirstOrDefault(t => t.CreatedById == user.Id);

            return ownedTeam;
        }

        public void DeleteTeam(User user, TournamentTeam team)
        {
            Require.NotNull(user, nameof(user));
            Require.NotNull(team, nameof(team));

            if (!CanChangeTeams)
            {
                throw new DomainException(ErrorCode.TournamentCannotJoinLeave, "Cannot delete team");
            }

            if (user.Id != team.CreatedById)
            {
                throw new DomainException(ErrorCode.TournamentTeamDeleteNoPermission,
                    "User does not have permission to delete team");
            }

            Teams.Remove(team);

            // TODO: Generate domain events for all players in team
        }

        /// <summary>
        /// Start tournament
        /// </summary>
        public void Start(IRandomGen random)
        {
            if (State != TournamentState.Open)
            {
                throw new DomainException(ErrorCode.TournamentStart, "Cannot start tournament");
            }

            if (!CanStart)
            {
                throw new DomainException(ErrorCode.TournamentStart, "Cannot start tournament");
            }

            if (HasGroupPhase)
            {
                State = TournamentState.Groups;

                CreateGroupPairings(random);
            }
            else
            {
                State = TournamentState.Knockout;

                CreateNextRoundPairings(Teams);
            }

            StartOfTournament = DateTime.UtcNow;
        }

        internal void End()
        {
            Debug.Assert(CanEnd, "Cannot end tournament");
            Debug.Assert(ActiveTeams.Count() == 1, "There should be only one active team");

            State = TournamentState.Closed;
            EndOfTournament = DateTime.UtcNow;

            Winner = ActiveTeams.First();
            Winner.State = TournamentTeamState.InActive;

            // TODO: Generate domain event
        }

        public void StartNextRound(IRandomGen random, ILogger log)
        {
            ++Phase;

            if (State == TournamentState.Groups)
            {
                log.Log(LogLevel.Info, "Switch from Group to KO phase for tournament {0}", Name);

                State = TournamentState.Knockout;

                // Update losers
                var losingTeams = Groups.SelectMany(g => g.Losers);
                foreach (var losingTeam in losingTeams)
                {
                    log.Log(LogLevel.Info, "Team {0} has lost", losingTeam.Name);
                    losingTeam.State = TournamentTeamState.InActive;
                }

                // Create pairings for KO phase
                var koTeams = Groups.SelectMany(g => g.Winners).Shuffle(random);

                log.Log(LogLevel.Info, "Teams for KO phase: {0}", string.Join(", ", koTeams.Select(t => t.Name)));

                CreateNextRoundPairings(koTeams);
            }
            else
            {
                // Knockout
                log.Log(LogLevel.Info, "Creating knockout pairings");
                CreateNextRoundPairings(Pairings
                    .Where(p => p.Phase == Phase - 1)
                    .OrderBy(p => p.Order)
                    .Select(p => p.Winner)
                );
            }
        }

        /// <summary>
        /// Create pairings for next knockout round
        /// </summary>
        /// <param name="teams">List of teams to create pairings for</param>
        private void CreateNextRoundPairings(IEnumerable<TournamentTeam> teams)
        {
            var teamArray = teams.ToArray();

            for (var i = 0; i < teamArray.Length; i += 2)
            {
                var teamA = teamArray[i];
                var teamB = teamArray[i + 1];

                var numberOfGames = NumberOfKnockoutGames;
                if (teamArray.Length == 2)
                {
                    numberOfGames = NumberOfFinalGames;
                }

                CreatePairing(i / 2, teamA, teamB, numberOfGames);
            }
        }

        /// <summary>
        /// Create all pairings and games for all groups
        /// </summary>
        private void CreateGroupPairings(IRandomGen random)
        {
            // Create groups
            var shuffledTeams = Teams.Shuffle(random);
            var teamIterator = shuffledTeams.GetEnumerator();

            for (var i = 0; i < NumberOfTeams / GroupSize; ++i)
            {
                var group = new TournamentGroup(this, i + 1);

                for (var j = 0; j < GroupSize; ++j)
                {
                    teamIterator.MoveNext();
                    group.Teams.Add(teamIterator.Current);
                }

                Groups.Add(group);
            }

            // Create pairings so that each team plays against all others
            var order = 0;

            foreach (var group in Groups)
            {
                for (var i = 0; i < group.Teams.Count(); ++i)
                {
                    for (var j = i + 1; j < group.Teams.Count(); ++j)
                    {
                        var teamA = group.Teams.ElementAt(i);
                        var teamB = group.Teams.ElementAt(j);

                        var pairing = CreatePairing(order++, teamA, teamB, NumberOfGroupGames);

                        pairing.Group = group;
                        pairing.GroupId = group.Id;

                        group.Pairings.Add(pairing);
                    }
                }
            }
        }

        private TournamentPairing CreatePairing(int order, TournamentTeam teamA, TournamentTeam teamB,
            int numberOfGames)
        {
            var pairing = new TournamentPairing(this, Phase, order, teamA, teamB, numberOfGames);
            Pairings.Add(pairing);
            return pairing;
        }
    }
}
