using System;
using System.Data.Entity;
using System.Linq;
using ImperaPlus.Domain;
using ImperaPlus.Domain.Repositories;

namespace ImperaPlus.DataAccess.Repositories
{
    internal class UserRepository : GenericRepository<User>, IUserRepository
    {
        public UserRepository(DbContext context) : base(context)
        {
        }

        public User FindByName(string name)
        {
            return this.DbSet.FirstOrDefault(x => x.UserName == name);
        }
    }
}