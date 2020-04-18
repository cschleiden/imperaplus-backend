namespace ImperaPlus.Domain.News
{
    public class NewsContent : IIdentifiableEntity
    {
        protected NewsContent()
        {
        }

        internal NewsContent(string language, string title, string text)
        {
            this.Language = language;
            this.Title = title;
            this.Text = text;
        }

        public long Id { get; set; }

        public string Language { get; set; }

        public string Title { get; set; }

        public string Text { get; set; }
    }
}