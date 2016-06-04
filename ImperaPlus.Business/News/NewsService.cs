using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using ImperaPlus.Domain.News;
using ImperaPlus.Domain.Repositories;
using ImperaPlus.DTO.News;
using ImperaPlus.DataAccess;
using System;

namespace ImperaPlus.Application.News
{
    public interface INewsService
    {
        void PostNews(DTO.News.NewsContent[] newsContent);

        void Delete(long id);

        IEnumerable<NewsItem> GetNews();
    }

    public class NewsService : BaseService, INewsService
    {
        public NewsService(
            IUnitOfWork unitOfWork,
            IUserProvider userProvider)
            : base(unitOfWork, userProvider)
        {            
        }

        public void PostNews(DTO.News.NewsContent[] newsContents)
        {
            var newsEntry = NewsEntry.Create();

            newsEntry.CreatedById = this.userProvider.GetCurrentUserId();
            newsEntry.CreatedAt = DateTime.UtcNow;

            foreach(var newsContent in newsContents)
            {
                newsEntry.AddContent(newsContent.Language, newsContent.Title, newsContent.Text);
            }

            this.UnitOfWork.News.Add(newsEntry);

            this.UnitOfWork.Commit();
        }

        public void Delete(long id)
        {
            var newsEntry = this.UnitOfWork.News.FindById(id);
            if (newsEntry != null)
            {
                this.UnitOfWork.News.Remove(newsEntry);                
                this.UnitOfWork.Commit();
            }
        }

        public IEnumerable<NewsItem> GetNews()
        {
            return Mapper.Map<NewsEntry[], IEnumerable<NewsItem>>(this.UnitOfWork.News.GetOrdered(10).ToArray());
        }
    }
}