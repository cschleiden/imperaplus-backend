using AutoMapper;
using ImperaPlus.Domain;
using ImperaPlus.Domain.Repositories;
using ImperaPlus.DTO.Users;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ImperaPlus.Application.Users
{
    public interface IUserService
    {
        IEnumerable<UserReference> FindUsers(string query);

        void TrackLogin(User user);

        void DeleteAccount();

        void DeleteAccount(User user, bool force = false);

        void SetLanguage(User user, string language);
    }

    public class UserService : BaseService, IUserService
    {
        public const int MaxResult = 5;

        private Domain.Services.IUserService userService;

        public UserService(IUnitOfWork unitOfWork, IMapper mapper, IUserProvider userProvider, Domain.Services.IUserService userService)
            : base(unitOfWork, mapper, userProvider)
        {
            this.userService = userService;
        }

        public void TrackLogin(User user)
        {
            user.LastLogin = DateTime.UtcNow;
            this.UnitOfWork.Commit();
        }

        public void DeleteAccount()
        {
            this.DeleteAccount(this.CurrentUser);
        }

        public void DeleteAccount(User user, bool force = false)
        {
            this.userService.DeleteAccount(user, force);
            this.UnitOfWork.Commit();
        }

        public void SetLanguage(User user, string language) 
        {
            user.Language = language;
            this.UnitOfWork.Commit();
        }

        public IEnumerable<UserReference> FindUsers(string query)
        {
            return Mapper.Map<IEnumerable<UserReference>>(this.UnitOfWork.Users
                .Query()
                .Where(x => !x.IsDeleted)
                .Where(x => x.UserName.StartsWith(query))
                .OrderBy(x => x.UserName)
                .Take(MaxResult));
        }
    }
}
