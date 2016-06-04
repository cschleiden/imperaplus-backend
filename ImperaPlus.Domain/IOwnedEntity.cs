namespace ImperaPlus.Domain
{
    public interface IOwnedEntity
    {
        string CreatedById { get; set; }
        User CreatedBy { get; set; }
    }
}