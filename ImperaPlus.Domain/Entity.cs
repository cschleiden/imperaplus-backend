using System.ComponentModel.DataAnnotations.Schema;
using ImperaPlus.Domain.Events;

namespace ImperaPlus.Domain
{
    public class Entity
    {
        [NotMapped]
        public EventQueue EventQueue = new EventQueue();
    }
}
