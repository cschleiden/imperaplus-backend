using System.Data.Entity;
using ImperaPlus.Domain.Repositories;
using Microsoft.AspNet.Identity.EntityFramework;

namespace ImperaPlus.DataAccess.Repositories
{
    class RoleRepository : GenericRepository<IdentityRole>, IRoleRepository
    {
        public RoleRepository(DbContext context) : base(context)
        {
        }
    }
}