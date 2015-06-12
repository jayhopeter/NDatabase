using System.Collections;

namespace Test.NDatabase.Odb.Test.Arraycollectionmap
{
    public class ObjectWithListOfInteger
    {
        private IList listOfIntegers;
        private string name;

        public ObjectWithListOfInteger(string name)
        {
            this.name = name;
            listOfIntegers = new ArrayList();
        }

        public virtual string GetName()
        {
            return name;
        }

        public virtual void SetName(string name)
        {
            this.name = name;
        }

        public virtual IList GetListOfIntegers()
        {
            return listOfIntegers;
        }

        public virtual void SetListOfIntegers(IList listOfIntegers)
        {
            this.listOfIntegers = listOfIntegers;
        }
    }
}
