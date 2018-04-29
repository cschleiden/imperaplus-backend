using System;
using System.Collections.Generic;
using System.Linq;
using ImperaPlus.Domain.Alliances;
using ImperaPlus.Domain.Repositories;
using Microsoft.EntityFrameworkCore;

namespace ImperaPlus.DataAccess.Repositories
{
    internal class AllianceRepository : GenericRepository<Alliance>, IAllianceRepository
    {
        public AllianceRepository(DbContext context)
            : base(context)
        {
        }

        public Alliance Get(Guid allianceId)
        {
            return this
                .WithMembers
                .Include(x => x.Requests)
                    .ThenInclude(x => x.RequestedByUser)
                .FirstOrDefault(x => x.Id == allianceId);
        }

        public Alliance FindByName(string name)
        {
            var lowerName = name.ToLower();
            return this.DbSet.FirstOrDefault(x => x.Name.ToLower() == lowerName);
        }

        public IEnumerable<Alliance> GetAll()
        {
            return this.WithMembers;
        }

        public IEnumerable<AllianceJoinRequest> GetRequestsForUser(string userId)
        {
            return this.Context.Set<AllianceJoinRequest>()
                .Where(x => x.RequestedByUserId == userId)
                .OrderBy(x => x.CreatedAt);
        }

        private IQueryable<Alliance> WithMembers
        {
            get
            {
                return this.DbSet
                    .Include(x => x.Members);
            }
        }
    }
}
