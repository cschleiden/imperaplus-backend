using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ImperaPlus.Utils;

namespace ImperaPlus.Domain.Utilities
{
    public class SerializedCollection<T> : List<T>
    {        
        public string Serialized
        {
            get
            {
                using (TraceContext.Trace("Serialize collection"))
                {
                    return Jil.JSON.Serialize<List<T>>(this);
                }
            }

            set
            {
                if (string.IsNullOrEmpty(value))
                {
                    return;
                }

                this.Clear();
                using (TraceContext.Trace("Deserialize collection"))
                {
                    var items = Jil.JSON.Deserialize<List<T>>(value);
                    this.AddRange(items);
                }
            }
        }
    }
}
