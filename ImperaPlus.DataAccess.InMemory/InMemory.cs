using System;
using ImperaPlus.Domain.Repositories;
using Microsoft.EntityFrameworkCore;

namespace ImperaPlus.DataAccess.InMemory
{
    public class InMemory
    {
        public static IUnitOfWork GetInMemoryUnitOfWork()
        {
            var options = new DbContextOptionsBuilder<ImperaContext>()
                    .UseInMemoryDatabase(Guid.NewGuid().ToString())
                    .Options;

            var context = new ImperaContext(options);

            return new UnitOfWork(context);
        }
    }
}
