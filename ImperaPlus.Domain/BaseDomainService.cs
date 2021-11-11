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
            UnitOfWork = unitOfWork;
            this.userProvider = userProvider;
        }

        protected User CurrentUser =>
            currentUser ?? (currentUser = UnitOfWork.Users.FindById(userProvider.GetCurrentUserId()));

        protected void CheckAdmin()
        {
            if (!userProvider.IsAdmin())
            {
                throw new DomainException(
                    ErrorCode.UserIsNotAllowedToPerformAction,
                    "User has to be admin to perform this action");
            }
        }

        protected User GetUser(string userId)
        {
            Require.NotNullOrEmpty(userId, nameof(userId));

            var user = UnitOfWork.Users.FindById(userId);
            if (user == null)
            {
                throw new DomainException(ErrorCode.UserDoesNotExist, "User does not exist");
            }

            return user;
        }
    }
}
