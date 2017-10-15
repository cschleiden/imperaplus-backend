using System;
using ImperaPlus.Domain.Alliances;

namespace ImperaPlus.Domain.Repositories
{
    public interface IAllianceRepository : IGenericRepository<Alliance>
    {
        Alliance Get(Guid allianceId);

        Alliance GetWithMembers(Guid allianceId);

        Alliance FindByName(string name);
    }
}
