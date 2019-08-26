using System;
using System.Collections.Generic;
using System.Linq;
using ImperaPlus.Domain;
using ImperaPlus.Domain.Repositories;
using Microsoft.EntityFrameworkCore;

namespace ImperaPlus.DataAccess.Repositories
{
    internal class UserRepository : GenericRepository<User>, IUserRepository
    {
        public UserRepository(DbContext context) : base(context)
        {
        }

        public User FindById(string id)
        {
            return this.DbSet.FirstOrDefault(x => x.Id == id);
        }

        public User FindByIdWithRoles(string id)
        {
            // TODO: FIX!
            return this.DbSet
                //.Include(x => x.Roles)
                .FirstOrDefault(x => x.Id == id);
        }

        public User FindByName(string name)
        {
            return this.DbSet.FirstOrDefault(x => x.UserName == name);
        }

        public IEnumerable<User> FindUsersToDelete(int days = -30)
        {
            var cutoffDate = DateTime.UtcNow.AddDays(days);
            return this.DbSet.Where(x => x.IsDeleted && x.LastLogin <= cutoffDate);
        }
    }
}