using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ImperaPlus.TestSupport
{
    public class LayerAttribute : TestCategoryBaseAttribute
    {
        private readonly string layer;

        public LayerAttribute(string layer)
        {
            this.layer = layer;
        }

        public override IList<string> TestCategories
        {
            get { return new[] { this.layer }; }
        }
    }
}