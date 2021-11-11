using System.Collections.Generic;

namespace ImperaPlus.Domain.Utilities
{
    public class SerializedCollection<T> : List<T>
    {
        public SerializedCollection()
        {
        }

        public SerializedCollection(string input)
            : base(Jil.JSON.Deserialize<IEnumerable<T>>(input ?? "[]"))
        {
        }

        public SerializedCollection(IEnumerable<T> data)
            : base(data)
        {
        }

        public string Serialize()
        {
            return Jil.JSON.Serialize<IEnumerable<T>>(this);
        }
    }
}
