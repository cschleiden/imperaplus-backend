namespace ImperaPlus.Domain.News
{
    public class NewsContent : IIdentifiableEntity
    {
        protected NewsContent()
        {
        }

        internal NewsContent(string language, string title, string text)
        {
            Language = language;
            Title = title;
            Text = text;
        }

        public long Id { get; set; }

        public string Language { get; set; }

        public string Title { get; set; }

        public string Text { get; set; }
    }
}
