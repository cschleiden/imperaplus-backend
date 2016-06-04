using System;

namespace ImperaPlus.Domain
{
    public interface IChangeTrackedEntity
    {
        DateTime CreatedAt { get; set; }
        
        DateTime LastModifiedAt { get; set; }
    }
}