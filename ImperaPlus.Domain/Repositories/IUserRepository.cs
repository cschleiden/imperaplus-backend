namespace ImperaPlus.Domain.Repositories
{
    public interface IUserRepository : IGenericRepository<User>
    {
        User FindById(string id);

        User FindByName(string name);

        User FindByIdWithRoles(string userId);
    }
}