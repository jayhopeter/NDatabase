using NDatabase.Tool.Wrappers;

namespace Test.NDatabase.Odb.Test.VO.Crawler.Jdk5
{
    /// <summary>
    ///   Created by IntelliJ IDEA.
    /// </summary>
    /// <remarks>
    ///   Created by IntelliJ IDEA. User: olivier s Date: 04/12/2005 Time: 19:19:19 To
    ///   change this template use File | Settings | File Templates.
    /// </remarks>
    public class SearchElement
    {
        private byte fetchInterval = 30;
        private long id;

        private long nextFetch = OdbTime.GetCurrentTimeInTicks();

        private int numOutlinks;
        private byte retries;

        private float score = 1.0f;
        private string url;
        private byte version;

        // private static final byte DEFAULT_INTERVAL =
        // (byte)CrawlerConf.get().getInt("default.fetch.interval", 30);
        public virtual long GetId()
        {
            return id;
        }

        public virtual void SetId(long id)
        {
            this.id = id;
        }

        public virtual byte GetVersion()
        {
            return version;
        }

        public virtual void SetVersion(byte version)
        {
            this.version = version;
        }

        public virtual string GetUrl()
        {
            return url;
        }

        public virtual void SetUrl(string url)
        {
            this.url = url;
        }

        public virtual long GetNextFetch()
        {
            return nextFetch;
        }

        public virtual void SetNextFetch(long nextFetch)
        {
            this.nextFetch = nextFetch;
        }

        public virtual byte GetRetries()
        {
            return retries;
        }

        public virtual void SetRetries(byte retries)
        {
            this.retries = retries;
        }

        public virtual byte GetFetchInterval()
        {
            return fetchInterval;
        }

        public virtual void SetFetchInterval(byte fetchInterval)
        {
            this.fetchInterval = fetchInterval;
        }

        public virtual int GetNumOutlinks()
        {
            return numOutlinks;
        }

        public virtual void SetNumOutlinks(int numOutlinks)
        {
            this.numOutlinks = numOutlinks;
        }

        public virtual float GetScore()
        {
            return score;
        }

        public virtual void SetScore(float score)
        {
            this.score = score;
        }
    }
}
