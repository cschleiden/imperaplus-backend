using ImperaPlus.Domain.Events;
using System.ComponentModel.DataAnnotations.Schema;

namespace ImperaPlus.Domain
{
    public class Entity
    {
        [NotMapped]
        public IEventAggregator EventAggregator { get; set; }
    }
}
