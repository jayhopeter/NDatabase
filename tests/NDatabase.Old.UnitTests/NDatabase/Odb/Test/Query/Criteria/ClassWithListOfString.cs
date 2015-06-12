using System.Collections.Generic;

namespace Test.NDatabase.Odb.Test.Query.Criteria
{
    /// <author>olivier</author>
    public class ClassWithListOfString
    {
        private string name;

        private IList<string> strings;

        public ClassWithListOfString(string name, IList<string> strings)
        {
            this.name = name;
            this.strings = strings;
        }

        public virtual string GetName()
        {
            return name;
        }

        public virtual void SetName(string name)
        {
            this.name = name;
        }

        public virtual IList<string> GetStrings()
        {
            return strings;
        }

        public virtual void SetStrings(IList<string> strings)
        {
            this.strings = strings;
        }
    }
}
