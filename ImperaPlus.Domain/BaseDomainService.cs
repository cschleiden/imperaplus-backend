using ImperaPlus.Domain.Exceptions;
using ImperaPlus.Domain.Repositories;
using ImperaPlus.Domain.Utilities;

namespace ImperaPlus.Domain
{
    public class BaseDomainService
    {
        protected readonly IUnitOfWork UnitOfWork;
        protected readonly IUserProvider userProvider;

        private User currentUser;

        public BaseDomainService(IUnitOfWork unitOfWork, IUserProvider userProvider)
        {
            this.UnitOfWork = unitOfWork;
            this.userProvider = userProvider;
        }

        protected User CurrentUser
        {
            get
            {
                return this.currentUser ?? (this.currentUser = this.UnitOfWork.Users.FindById(this.userProvider.GetCurrentUserId()));
            }
        }

        protected void CheckAdmin()
        {
            if (!this.userProvider.IsAdmin())
            {
                throw new Exceptions.DomainException(
                    ErrorCode.UserIsNotAllowedToPerformAction,
                    "User has to be admin to perform this action");
            }
        }

        protected User GetUser(string userId)
        {
            Require.NotNullOrEmpty(userId, nameof(userId));

            var user = this.UnitOfWork.Users.FindById(userId);
            if (user == null)
            {
                throw new DomainException(ErrorCode.UserDoesNotExist, "User does not exist");
            }

            return user;
        }
    }
}
