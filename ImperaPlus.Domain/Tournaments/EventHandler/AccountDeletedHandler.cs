using ImperaPlus.Domain.Events;
using ImperaPlus.Domain.Exceptions;
using ImperaPlus.Domain.Repositories;
using ImperaPlus.Domain.Users;
using NLog.Fluent;
using System;

namespace ImperaPlus.Domain.Tournaments.EventHandler
{
    public class AccountDeletedHandler : IEventHandler<AccountDeleted>
    {
        private IUnitOfWork unitOfWork;

        public AccountDeletedHandler(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        public void Handle(AccountDeleted evt)
        {
            // Try to leave open tournaments
            var openTournaments = unitOfWork.Tournaments.Get(TournamentState.Open);
            foreach (var tournament in openTournaments)
            {
                try
                {
                    tournament.LeaveUser(evt.User);
                }
                catch (DomainException domainException) when (domainException.ErrorCode ==
                                                              ErrorCode.TournamentUserNoParticipant)
                {
                    // Ignore tournament
                }
                catch (DomainException domainException) when (domainException.ErrorCode ==
                                                              ErrorCode.TournamentTeamCreatorHasToDelete)
                {
                    var ownedTeam = tournament.GetOwnedTeamForUser(evt.User);
                    tournament.DeleteTeam(evt.User, ownedTeam);
                }
                catch (Exception)
                {
                    Log.Error().Message("Could not leave tournament {1} for user {0}", evt.User.Id, tournament.Id)
                        .Write();
                }
            }
        }
    }
}
