using System;
using System.Collections;
using NDatabase;
using NDatabase.Api;
using NDatabase.Btree;
using NDatabase.Core.BTree;
using NUnit.Framework;

namespace Test.NDatabase.Odb.Test.Btree.Odb
{
    public class BigBTree : ODBTest
    {
        [Test]
        public virtual void Test1()
        {
            const string bigbtreeTest1DbName = "bigbtree.test1.ndb";
            DeleteBase(bigbtreeTest1DbName);

            using (var odb = OdbFactory.Open(bigbtreeTest1DbName))
            {
                var size = 10000;
                IBTree tree = new OdbBtreeSingle(50, new LazyOdbBtreePersister(((global::NDatabase.Odb)odb).GetStorageEngine()));
                for (var i = 0; i < size; i++)
                {
                    if (i % 1000 == 0)
                        Println(i);
                    tree.Insert(i + 1, "value " + (i + 1));
                }
                AssertEquals(size, tree.GetSize());
                IEnumerator iterator = new BTreeIteratorSingleValuePerKey<object>(tree, OrderByConstants.OrderByAsc);
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
        }

        [Test]
        public virtual void Test2()
        {
            var size = 10000;
            IBTree tree = new InMemoryBTreeMultipleValuesPerKey(50);
            for (var i = 0; i < size; i++)
            {
                if (i % 1000 == 0)
                    Println(i);
                tree.Insert(i + 1, "value " + (i + 1));
            }
            AssertEquals(size, tree.GetSize());
            IEnumerator iterator = new BTreeIteratorMultipleValuesPerKey<object>(tree, OrderByConstants.OrderByAsc);

            var j = 0;
            while (iterator.MoveNext())
            {
                var o = iterator.Current;
                Console.WriteLine(o);
                j++;
                if (j == size)
                    AssertEquals("value " + size, o);
            }
        }

        [Test]
        public virtual void Test3()
        {
            var size = 10;
            IBTree tree = new InMemoryBTreeMultipleValuesPerKey(5);
            for (var i = 0; i < size; i++)
            {
                Println(i);
                tree.Insert(i + 1, "value " + (i + 1));
            }
            AssertEquals(size, tree.GetSize());
            IEnumerator iterator = new BTreeIteratorMultipleValuesPerKey<object>(tree, OrderByConstants.OrderByAsc);

            var j = 0;
            while (iterator.MoveNext())
            {
                var o = iterator.Current;
                Console.WriteLine(o);
                j++;
                if (j == size)
                    AssertEquals("value " + size, o);
            }
        }
    }
}
