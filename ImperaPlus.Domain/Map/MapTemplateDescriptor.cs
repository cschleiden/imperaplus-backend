using System;
using System.ComponentModel.DataAnnotations;

namespace ImperaPlus.Domain.Map
{
    public class MapTemplateDescriptor : IChangeTrackedEntity
    {
        [Key]
        public string Name { get; set; }
        
        public DateTime CreatedAt { get; set; }

        public string CreatedBy { get; set; }

        public DateTime LastModifiedAt { get; set; }

        public bool IsActive { get; set; }
    }
}
