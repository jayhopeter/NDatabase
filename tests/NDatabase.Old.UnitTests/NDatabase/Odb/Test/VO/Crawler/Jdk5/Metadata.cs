namespace Test.NDatabase.Odb.Test.VO.Crawler.Jdk5
{
    /// <summary>
    ///   Created by IntelliJ IDEA.
    /// </summary>
    /// <remarks>
    ///   Created by IntelliJ IDEA. User: mayworm Date: Jan 22, 2006 Time: 3:53:16 PM
    ///   To change this template use File | Settings | File Templates.
    /// </remarks>
    public class Metadata
    {
        private string id;
        private string property;

        private string value;

        public virtual string GetProperty()
        {
            return property;
        }

        public virtual void SetProperty(string property)
        {
            this.property = property;
        }

        public virtual string GetValue()
        {
            return value;
        }

        public virtual void SetValue(string value)
        {
            this.value = value;
        }

        public virtual string GetId()
        {
            return id;
        }

        public virtual void SetId(string id)
        {
            this.id = id;
        }
    }
}
