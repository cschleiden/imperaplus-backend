using AutoMapper;
using ImperaPlus.Application.Exceptions;
using ImperaPlus.Application.Visibility;
using ImperaPlus.DataAccess;
using ImperaPlus.Domain.Games;
using ImperaPlus.Domain.Repositories;

namespace ImperaPlus.Application
{
    public class BaseService
    {
        protected readonly IUnitOfWork UnitOfWork;
        protected readonly IUserProvider userProvider;

        private Domain.User currentUser;

        public BaseService(IUnitOfWork unitOfWork, IUserProvider userProvider)
        {
            this.UnitOfWork = unitOfWork;
            this.userProvider = userProvider;
        }

        protected Game GetGame(long gameId)
        {
            var game = this.UnitOfWork.Games.Find(gameId);
            if (game == null)
            {
                throw new ApplicationException("Cannot find game", ErrorCode.CannotFindGame);
            }

            return game;
        }

        protected Game GetGameWithHistory(long gameId)
        {
            var game = this.UnitOfWork.Games.FindWithHistory(gameId);
            if (game == null)
            {
                throw new ApplicationException("Cannot find game", ErrorCode.CannotFindGame);
            }

            return game;
        }

        protected Game GetGameMessages(long gameId)
        {
            var game = this.UnitOfWork.Games.FindWithMessages(gameId);
            if (game == null)
            {
                throw new ApplicationException("Cannot find game", ErrorCode.CannotFindGame);
            }

            return game;
        }

        protected Domain.User CurrentUser
        {
            get
            {
                return this.currentUser ?? (this.currentUser = this.UnitOfWork.Users.FindById(this.userProvider.GetCurrentUserId()));
            }
        }
    }
}