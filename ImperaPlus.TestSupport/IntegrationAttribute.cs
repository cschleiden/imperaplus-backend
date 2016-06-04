using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ImperaPlus.TestSupport
{
    public class IntegrationAttribute : TestCategoryBaseAttribute
    {
        public override IList<string> TestCategories
        {
            get { return new[] {"Integration"}; }
        }
    }
}
