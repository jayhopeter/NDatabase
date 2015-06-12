using System;
using NDatabase;
using NDatabase.Btree;
using NDatabase.Core.BTree;
using NDatabase.Exceptions;
using NUnit.Framework;

namespace Test.NDatabase.Odb.Test.Btree.Odb
{
    /// <author>olivier</author>
    public class SingleValueBTree : ODBTest
    {
        [Test]
        public virtual void Test2SameKeySingleBTree()
        {
            const string singlevaluebtreeTest1DbName = "singlevaluebtree.test1.ndb";
            DeleteBase(singlevaluebtreeTest1DbName);

            using (var odb = OdbFactory.Open(singlevaluebtreeTest1DbName))
            {
                var size = 1000;
                IBTree tree = new OdbBtreeSingle(50, new LazyOdbBtreePersister(((global::NDatabase.Odb)odb).GetStorageEngine()));
                for (var i = 0; i < size; i++)
                {
                    if (i % 10000 == 0)
                        Println(i);
                    tree.Insert(i + 1, "value " + (i + 1));
                }
                try
                {
                    for (var i = 0; i < 10; i++)
                    {
                        if (i % 10000 == 0)
                            Println(i);
                        tree.Insert(100, "value " + (i + 1));
                        Fail("Single Value Btree should not accept duplcited key");
                    }
                }
                catch (DuplicatedKeyException ex)
                {
                    Console.WriteLine(ex.Message);
                    Assert.Pass();
                }
            }
        }
    }
}
