using System;
using System.Data.Common;
using System.Data.Entity.Infrastructure;
using Effort.Provider;

namespace ImperaPlus.TestSupport
{
    public class TestDbConnectionFactory : IDbConnectionFactory
    {
        public DbConnection CreateConnection(string nameOrConnectionString)
        {
            var connectionString =
                new EffortConnectionStringBuilder {InstanceId = Guid.NewGuid().ToString()};

            var connection = new EffortConnection { ConnectionString = connectionString.ConnectionString};

            return connection;
        }
    }
}