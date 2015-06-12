using System.Collections;
using NDatabase.Api;
using NDatabase.Btree;
using NUnit.Framework;

namespace Test.NDatabase.Odb.Test.Btree.Odb
{
    public class MultipleKeyBTree : ODBTest
    {
        [Test]
        public virtual void Test1()
        {
            var size = 100000;
            IBTree tree = new InMemoryBTreeMultipleValuesPerKey(50);
            for (var i = 0; i < size; i++)
            {
                if (i % 10000 == 0)
                    Println(i);
                tree.Insert(i + 1, "value " + (i + 1));
            }
            AssertEquals(size, tree.GetSize());
            IEnumerator iterator = new BTreeIteratorMultipleValuesPerKey<object>(tree, OrderByConstants.OrderByAsc);
            var j = 0;
            while (iterator.MoveNext())
            {
                var o = iterator.Current;
                // println(o);
                j++;
                if (j == size)
                    AssertEquals("value " + size, o);
            }
        }

        [Test]
        public virtual void Test2SameKey()
        {
            var size = 1000;
            var size2 = 100;
            IBTree tree = new InMemoryBTreeMultipleValuesPerKey(50);
            for (var i = 0; i < size; i++)
            {
                if (i % 10000 == 0)
                    Println(i);
                tree.Insert(i + 1, "value " + (i + 1));
            }
            for (var i = 0; i < size2; i++)
                tree.Insert(100, "value " + (i + 1));
            AssertEquals(size + size2, tree.GetSize());
        }
    }
}
