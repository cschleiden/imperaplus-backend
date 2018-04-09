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

        public Alliance GetWithMembers(Guid allianceId)
        {
            return this.WithMembers.FirstOrDefault(x => x.Id == allianceId);
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
