using ImperaPlus.Domain.Repositories;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace ImperaPlus.DataAccess.Repositories
{
    internal class RoleRepository : GenericRepository<IdentityRole>, IRoleRepository
    {
        public RoleRepository(DbContext context) : base(context)
        {
        }
    }
}
