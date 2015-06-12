using System.Collections;

namespace Test.NDatabase.Odb.Test.VO.Crawler.Jdk5
{
    /// <summary>
    ///   Created by IntelliJ IDEA.
    /// </summary>
    /// <remarks>
    ///   Created by IntelliJ IDEA. User: mayworm Date: Jan 22, 2006 Time: 3:49:54 PM
    ///   To change this template use File | Settings | File Templates.
    /// </remarks>
    public class Page
    {
        protected string content;

        protected int firstFetch;
        protected long id;
        protected IList metadata;

        protected int nextFetch;
        protected IList outputLinks;

        protected string path;
        protected float score;
        protected string url;

        // lista de urls existentes na pagina
        // Lista de metadados que foram obtidos, a partir do Dublin core
        // conteudo da pagina sem links e figuras
        // identificador unico gerado com o MD5 do conteudo
        // Url da pagina
        // data do primeiro download
        // data do proximo fecth
        // pontuacao da pagina a partir de um algotimo de rank
        // caminho para o arquivo fisicamente armazenado
        public virtual string GetPath()
        {
            return path;
        }

        public virtual void SetPath(string path)
        {
            this.path = path;
        }

        public virtual IList GetOutputLinks()
        {
            return outputLinks;
        }

        public virtual void SetOutputLinks(IList outputLinks)
        {
            this.outputLinks = outputLinks;
        }

        public virtual IList GetMetadata()
        {
            return metadata;
        }

        public virtual void SetMetadata(IList metadata)
        {
            this.metadata = metadata;
        }

        public virtual string GetContent()
        {
            return content;
        }

        public virtual void SetContent(string content)
        {
            this.content = content;
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

        public virtual int GetFirstFetch()
        {
            return firstFetch;
        }

        public virtual void SetFirstFetch(int firstFetch)
        {
            this.firstFetch = firstFetch;
        }

        public virtual int GetNextFetch()
        {
            return nextFetch;
        }

        public virtual void SetNextFetch(int nextFetch)
        {
            this.nextFetch = nextFetch;
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
