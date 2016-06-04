using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using ImperaPlus.DataAccess;
using ImperaPlus.Domain.Repositories;
using ImperaPlus.DTO.Users;

namespace ImperaPlus.Application.Users
{
    public interface IUserService
    {
        IEnumerable<DTO.Users.UserReference> FindUsers(string query);
    }

    public class UserService : BaseService, IUserService
    {
        public const int MaxResult = 5;

        public UserService(IUnitOfWork unitOfWork, IUserProvider userProvider)
            : base(unitOfWork, userProvider)
        {
        }

        public IEnumerable<UserReference> FindUsers(string query)
        {
            return Mapper.Map<IEnumerable<UserReference>>(this.UnitOfWork.Users
                .Query()
                .Where(x => x.UserName.StartsWith(query))
                .OrderBy(x => x.UserName)
                .Take(MaxResult));
        }
    }
}
