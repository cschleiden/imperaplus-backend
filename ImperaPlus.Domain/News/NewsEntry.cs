using System;
using System.Collections.Generic;
using System.Linq;
using ImperaPlus.Domain.Exceptions;

namespace ImperaPlus.Domain.News
{
    public class NewsEntry : IOwnedEntity, IChangeTrackedEntity, IIdentifiableEntity
    {
        protected NewsEntry()
        {
            Content = new HashSet<NewsContent>();
        }

        public long Id { get; set; }

        public string CreatedById { get; set; }
        public virtual User CreatedBy { get; set; }

        public virtual ICollection<NewsContent> Content { get; private set; }

        public DateTime CreatedAt { get; set; }
        public DateTime LastModifiedAt { get; set; }

        public static NewsEntry Create()
        {
            return new NewsEntry();
        }

        public NewsContent AddContent(string language, string title, string text)
        {
            if (Content.Any(x => x.Language == language))
            {
                throw new DomainException(ErrorCode.DuplicateNewsContent,
                    "There is already news content for the given language");
            }

            var newsContent = new NewsContent(language, title, text);

            Content.Add(newsContent);

            return newsContent;
        }

        public NewsContent GetContentForLanguage(string language)
        {
            var content = Content.FirstOrDefault(x => x.Language == language);

            if (content == null)
            {
                throw new DomainException(ErrorCode.NewsContentNotFound, "No news content for the given language");
            }

            return content;
        }
    }
}
