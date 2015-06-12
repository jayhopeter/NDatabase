using System.Collections.Generic;

namespace Test.NDatabase.Odb.Test.Arraycollectionmap
{
    public class MyList : List<object>
    {
        public virtual object MyGet(int i)
        {
            return this[i];
        }
    }
}
