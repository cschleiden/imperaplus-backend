namespace ImperaPlus.Domain.Repositories
{
    public interface IUserRepository : IGenericRepository<User>
    {
        User FindByName(string name);
    }
}