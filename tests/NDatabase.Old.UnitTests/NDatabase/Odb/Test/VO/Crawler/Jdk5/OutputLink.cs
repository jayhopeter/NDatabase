using System;

namespace Test.NDatabase.Odb.Test.VO.Crawler.Jdk5
{
    public class OutputLink
    {
        private DateTime date;
        private long id;

        private string url;

        public OutputLink()
        {
        }

        public OutputLink(long id, DateTime date, string url)
        {
            // TODO Auto-generated constructor stub
            // TODO Auto-generated constructor stub
            this.id = id;
            this.date = date;
            this.url = url;
        }

        public virtual DateTime GetDate()
        {
            return date;
        }

        public virtual void SetDate(DateTime date)
        {
            this.date = date;
        }

        public virtual long GetId()
        {
            return id;
        }

        public virtual void SetId(long id)
        {
            this.id = id;
        }

        public virtual string GetUrl()
        {
            return url;
        }

        public virtual void SetUrl(string url)
        {
            this.url = url;
        }
    }
}
