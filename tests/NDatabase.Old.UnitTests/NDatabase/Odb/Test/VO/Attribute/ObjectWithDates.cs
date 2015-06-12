using System;

namespace Test.NDatabase.Odb.Test.VO.Attribute
{
    /// <author>olivier</author>
    public class ObjectWithDates
    {
        private DateTime javaSqlDte;
        private DateTime javaUtilDate;
        private string name;

        private DateTime timestamp;

        public ObjectWithDates(string name, DateTime javaUtilDate, DateTime javaSqlDte, DateTime timestamp)
        {
            this.name = name;
            this.javaUtilDate = javaUtilDate;
            this.javaSqlDte = javaSqlDte;
            this.timestamp = timestamp;
        }

        public virtual string GetName()
        {
            return name;
        }

        public virtual void SetName(string name)
        {
            this.name = name;
        }

        public virtual DateTime GetJavaUtilDate()
        {
            return javaUtilDate;
        }

        public virtual void SetJavaUtilDate(DateTime javaUtilDate)
        {
            this.javaUtilDate = javaUtilDate;
        }

        public virtual DateTime GetJavaSqlDte()
        {
            return javaSqlDte;
        }

        public virtual void SetJavaSqlDte(DateTime javaSqlDte)
        {
            this.javaSqlDte = javaSqlDte;
        }

        public virtual DateTime GetTimestamp()
        {
            return timestamp;
        }

        public virtual void SetTimestamp(DateTime timestamp)
        {
            this.timestamp = timestamp;
        }
    }
}
