﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using ImperaPlus.Domain;
using ImperaPlus.Domain.Repositories;
using ImperaPlus.Domain.Utilities;
using ImperaPlus.DTO.Games;
using ImperaPlus.DTO.Tournaments;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace ImperaPlus.Application.Tournaments
{
    public interface ITournamentService
    {
        IEnumerable<TournamentSummary> GetAll();

        IEnumerable<TournamentSummary> GetAll(TournamentState state);

        IEnumerable<Tournament> GetAllFull();

        IEnumerable<Tournament> GetRecentFull();

        Tournament Get(Guid tournamentId, bool includeGames = false, bool readOnly = false);

        IEnumerable<TournamentTeam> GetTeams(Guid tournamentId);

        TournamentTeam CreateTeam(Guid tournamentId, string name, string password);

        TournamentTeam Join(Guid tournamentId);

        TournamentTeam JoinTeam(Guid tournamentId, Guid teamId, string password);

        void DeleteTeam(Guid tournamentId, Guid teamId);

        void Leave(Guid tournamentId);

        Task<Guid> Create(Tournament tournament);

        Task Delete(Guid id);

        IEnumerable<GameSummary> GetGamesForPairing(Guid pairingId);
    }

    public class TournamentService : BaseService, ITournamentService
    {
        private RoleManager<IdentityRole> roleManager;
        private Domain.Services.ITournamentService tournamentService;

        public TournamentService(IUnitOfWork unitOfWork, IMapper mapper, IUserProvider userProvider,
            Domain.Services.ITournamentService tournamentService, RoleManager<IdentityRole> roleManager)
            : base(unitOfWork, mapper, userProvider)
        {
            this.tournamentService = tournamentService;
            this.roleManager = roleManager;
        }

        public TournamentTeam CreateTeam(Guid tournamentId, string name, string password)
        {
            var tournament = GetTournament(tournamentId);
            var team = tournament.CreateTeam(CurrentUser, name, password);

            UnitOfWork.Commit();

            return Mapper.Map<TournamentTeam>(team);
        }

        public void DeleteTeam(Guid tournamentId, Guid teamId)
        {
            Require.NotEmpty(tournamentId, nameof(tournamentId));
            Require.NotEmpty(teamId, nameof(teamId));

            var tournament = GetTournament(tournamentId);
            var team = GetTeam(tournament, teamId);

            tournament.DeleteTeam(CurrentUser, team);

            UnitOfWork.GetGenericRepository<Domain.Tournaments.TournamentParticipant>()
                .Remove(team.Participants.First());
            UnitOfWork.GetGenericRepository<Domain.Tournaments.TournamentTeam>().Remove(team);

            UnitOfWork.Commit();
        }

        public Tournament Get(Guid tournamentId, bool includeGames = false, bool readOnly = false)
        {
            Require.NotEmpty(tournamentId, nameof(tournamentId));

            var tournament = GetTournament(tournamentId, includeGames, readOnly);

            return Mapper.Map<Tournament>(tournament);
        }

        public IEnumerable<TournamentSummary> GetAll(TournamentState state)
        {
            var domainState = Mapper.Map<Domain.Tournaments.TournamentState>(state);

            return Mapper.Map<IEnumerable<TournamentSummary>>(UnitOfWork.Tournaments.Get(domainState));
        }

        public IEnumerable<TournamentSummary> GetAll()
        {
            return Mapper.Map<IEnumerable<TournamentSummary>>(UnitOfWork.Tournaments.Get());
        }

        public IEnumerable<TournamentTeam> GetTeams(Guid tournamentId)
        {
            Require.NotEmpty(tournamentId, nameof(tournamentId));

            var tournament = GetTournament(tournamentId);

            return Mapper.Map<IEnumerable<TournamentTeam>>(tournament.Teams);
        }

        public TournamentTeam Join(Guid tournamentId)
        {
            Require.NotEmpty(tournamentId, nameof(tournamentId));

            var tournament = GetTournament(tournamentId);
            var team = tournament.AddUser(CurrentUser);

            UnitOfWork.Commit();

            return Mapper.Map<TournamentTeam>(team);
        }

        public TournamentTeam JoinTeam(Guid tournamentId, Guid teamId, string password)
        {
            Require.NotEmpty(tournamentId, nameof(tournamentId));

            var tournament = GetTournament(tournamentId);
            var team = GetTeam(tournament, teamId);

            tournament.AddUser(CurrentUser, team, password);

            UnitOfWork.Commit();

            return Mapper.Map<TournamentTeam>(team);
        }

        public void Leave(Guid tournamentId)
        {
            Require.NotEmpty(tournamentId, nameof(tournamentId));

            var tournament = GetTournament(tournamentId);

            // TODO: CS: Move this to domain
            var participantToRemove = tournament.LeaveUser(CurrentUser);
            UnitOfWork.GetGenericRepository<Domain.Tournaments.TournamentParticipant>().Remove(participantToRemove);

            UnitOfWork.Commit();
        }

        private Domain.Tournaments.Tournament GetTournament(Guid tournamentId, bool includeGames = false,
            bool readOnly = false)
        {
            Require.NotEmpty(tournamentId, nameof(tournamentId));

            var tournament = UnitOfWork.Tournaments.GetById(tournamentId, includeGames);
            if (tournament == null)
            {
                throw new Exceptions.ApplicationException("Cannot find tournament", ErrorCode.TournamentNotFound);
            }

            return tournament;
        }

        private Domain.Tournaments.TournamentTeam GetTeam(Domain.Tournaments.Tournament tournament, Guid teamId)
        {
            var team = tournament.Teams.FirstOrDefault(x => x.Id == teamId);
            if (team == null)
            {
                throw new Exceptions.ApplicationException("Cannot find team", ErrorCode.TournamentTeamNotFound);
            }

            return team;
        }

        public Task<Guid> Create(Tournament tournament)
        {
            CheckAdmin();

            if (tournament.MapTemplates == null || tournament.MapTemplates.Count() == 0)
            {
                throw new Exceptions.ApplicationException("No map templates", ErrorCode.MapTemplatesRequired);
            }

            var gameOptions = Mapper.Map<Domain.Games.GameOptions>(tournament.Options);

            var newTournament = new Domain.Tournaments.Tournament(
                tournament.Name, tournament.NumberOfTeams, tournament.NumberOfGroupGames,
                tournament.NumberOfKnockoutGames,
                tournament.NumberOfFinalGames, tournament.StartOfRegistration, tournament.StartOfTournament,
                gameOptions);

            foreach (var mapTemplateName in tournament.MapTemplates)
            {
                // Ensure MapTemplates exist
                if (UnitOfWork.MapTemplateDescriptors.Get(mapTemplateName) == null)
                {
                    throw new Exceptions.ApplicationException("Cannot find map template",
                        ErrorCode.CannotFindMapTemplate);
                }
            }

            newTournament.MapTemplates.Clear();
            newTournament.MapTemplates.AddRange(tournament.MapTemplates);

            if (UnitOfWork.Tournaments.ExistsWithName(tournament.Name))
            {
                throw new Exceptions.ApplicationException("Name is already taken", ErrorCode.CannotCreateTournament);
            }

            UnitOfWork.Tournaments.Add(newTournament);
            UnitOfWork.Commit();

            return Task.FromResult(newTournament.Id);
        }

        public IEnumerable<Tournament> GetAllFull()
        {
            var tournaments = UnitOfWork.Tournaments.GetAllFull();

            return Mapper.Map<IEnumerable<Tournament>>(tournaments);
        }

        public IEnumerable<Tournament> GetRecentFull()
        {
            var tournaments = UnitOfWork.Tournaments.GetRecentFull();

            return Mapper.Map<IEnumerable<Tournament>>(tournaments);
        }

        public Task Delete(Guid tournamentId)
        {
            CheckAdmin();

            var tournament = GetTournament(tournamentId);

            UnitOfWork.Tournaments.Remove(tournament);
            UnitOfWork.Commit();

            return Task.FromResult(0);
        }

        public IEnumerable<GameSummary> GetGamesForPairing(Guid pairingId)
        {
            var games = tournamentService.GetGamesForPairing(pairingId);
            return Mapper.Map<IEnumerable<GameSummary>>(games);
        }
    }
}
