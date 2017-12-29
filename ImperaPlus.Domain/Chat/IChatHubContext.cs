namespace ImperaPlus.Domain.Chat
{
    public interface IChatHubContext
    {
        void DeleteMessage(long messageId);
    }
}
