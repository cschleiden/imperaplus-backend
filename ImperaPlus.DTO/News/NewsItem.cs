using System;

namespace ImperaPlus.DTO.News
{
    public class NewsItem
    {
        public DateTime DateTime { get; set; }        

        public string PostedBy { get; set; }

        public NewsContent[] Content { get; set; }
    }
}
