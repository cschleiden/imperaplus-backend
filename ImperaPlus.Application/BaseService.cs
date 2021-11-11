using AutoMapper;
using ImperaPlus.Domain;
using ImperaPlus.Domain.Repositories;

namespace ImperaPlus.Application
{
    public class BaseService
    {
        protected readonly IUnitOfWork UnitOfWork;
        protected readonly IMapper Mapper;
        protected readonly IUserProvider userProvider;

        private User currentUser;

        public BaseService(IUnitOfWork unitOfWork, IMapper mapper, IUserProvider userProvider)
        {
            UnitOfWork = unitOfWork;
            Mapper = mapper;
            this.userProvider = userProvider;
        }

        protected User CurrentUser =>
            currentUser ?? (currentUser = UnitOfWork.Users.FindById(userProvider.GetCurrentUserId()));

        protected void CheckAdmin()
        {
            if (!userProvider.IsAdmin())
            {
                throw new Exceptions.ApplicationException(
                    "User has to be admin to perform this action",
                    ErrorCode.UserIsNotAllowedToPerformAction);
            }
        }
    }
}
