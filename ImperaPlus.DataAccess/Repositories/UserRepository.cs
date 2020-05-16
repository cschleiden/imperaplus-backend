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
            return this.DbSet
                .Include(x => x.Roles)
                .FirstOrDefault(x => x.Id == id);
        }

        public User FindByName(string name)
        {
            return this.DbSet.FirstOrDefault(x => x.UserName == name);
        }

        public IEnumerable<User> FindUsersToDelete(int days = -30)
        {
            var deletedCutoffDate = DateTime.UtcNow.AddDays(days);
            var cutoffDate = DateTime.UtcNow.AddDays(-90);
            return this.DbSet.Where(x =>
                // Delete deleted users sooner
                (x.IsDeleted && x.LastLogin <= deletedCutoffDate)
                // Delete users without logins in 90 days
                || (x.LastLogin < cutoffDate)
            );
        }
    }
}