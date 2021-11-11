﻿using ImperaPlus.Domain.Events;
using ImperaPlus.Domain.Repositories;
using ImperaPlus.Domain.Users;
using ImperaPlus.Domain.Utilities;

namespace ImperaPlus.Domain.Services
{
    public interface IUserService
    {
        void DeleteAccount(User user, bool force);

        void ConfirmEmail(User user);
    }

    public class UserService : IUserService
    {
        private IEventAggregator eventAggregator;
        private IUnitOfWork unitOfWork;
        private IGameService gameService;

        public UserService(IEventAggregator eventAggregator, IUnitOfWork unitOfWork, IGameService gameService)
        {
            this.eventAggregator = eventAggregator;
            this.unitOfWork = unitOfWork;
            this.gameService = gameService;
        }

        public void DeleteAccount(User user, bool force)
        {
            Require.NotNull(user, nameof(user));

            // Let sub-systems react to this
            eventAggregator.Raise(new AccountDeleted(user, force));

            // Mark as deleted, will be cleaned up by a job
            user.IsDeleted = true;
        }

        public void ConfirmEmail(User user)
        {
            Require.NotNull(user, nameof(user));

            user.EmailConfirmed = true;
        }
    }
}
