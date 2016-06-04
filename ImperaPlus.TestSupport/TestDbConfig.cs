using System.Data.Entity;

namespace ImperaPlus.TestSupport
{
    public class TestDbConfig : DbConfiguration
    {
        public TestDbConfig()
        {
            this.SetDefaultConnectionFactory(new TestDbConnectionFactory());
        }
    }
}