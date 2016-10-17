namespace ImperaPlus.Domain
{
    public interface ISerializedEntity
    {
        /// <summary>
        /// Update all backing fields
        /// </summary>
        void Serialize();
    }
}