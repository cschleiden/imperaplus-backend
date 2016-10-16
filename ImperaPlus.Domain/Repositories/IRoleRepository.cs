using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace ImperaPlus.Domain.Repositories
{
    public interface IRoleRepository : IGenericRepository<IdentityRole>
    {
    }
}