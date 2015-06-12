using System.Collections.Generic;
using NDatabase;
using NDatabase.Btree;
using NDatabase.Core.BTree;
using NUnit.Framework;

namespace Test.NDatabase.Odb.Test.Btree.Odb
{
    [TestFixture]
    internal class TestPersister : ODBTest
    {
        [Test]
        public virtual void Test1()
        {
            DeleteBase("btree45.ndb");
            var odb = Open("btree45.ndb");
            var storageEngine = ((global::NDatabase.Odb)odb).GetStorageEngine();
            var persister = new LazyOdbBtreePersister(storageEngine);
            IBTreeMultipleValuesPerKey tree = new OdbBtreeMultiple(3, persister);
            tree.Insert(1, new MyObject("Value 1"));
            tree.Insert(20, new MyObject("Value 20"));
            tree.Insert(25, new MyObject("Value 25"));
            tree.Insert(29, new MyObject("Value 29"));
            tree.Insert(21, new MyObject("Value 21"));
            AssertEquals(5, tree.GetRoot().GetNbKeys());
            AssertEquals(0, tree.GetRoot().GetNbChildren());
            AssertEquals(21, tree.GetRoot().GetMedian().GetKey());
            var values = tree.GetRoot().GetMedian().GetValue() as List<object>;
            AssertEquals("Value 21", values[0].ToString());
            AssertEquals(0, tree.GetRoot().GetNbChildren());
            
            tree.Insert(45, new MyObject("Value 45"));
            AssertEquals(2, tree.GetRoot().GetNbChildren());
            AssertEquals(1, tree.GetRoot().GetNbKeys());
            AssertEquals(21, tree.GetRoot().GetKeyAt(0));
            var valuesAsObjectAt = tree.GetRoot().GetValueAsObjectAt(0) as List<object>;
            AssertEquals("Value 21", valuesAsObjectAt[0].ToString());
            persister.Close();
            odb = Open("btree45.ndb");
            storageEngine = ((global::NDatabase.Odb)odb).GetStorageEngine();
            persister = new LazyOdbBtreePersister(storageEngine);
            tree = (IBTreeMultipleValuesPerKey) persister.LoadBTree(tree.GetId());
            AssertEquals(6, tree.GetSize());
            // println(tree.getRoot());
            var o = (MyObject) tree.Search(20)[0];
            AssertEquals("Value 20", o.GetName());
            o = (MyObject) tree.Search(29)[0];
            AssertEquals("Value 29", o.GetName());
            o = (MyObject) tree.Search(45)[0];
            AssertEquals("Value 45", o.GetName());
            odb.Close();
            DeleteBase("btree45.ndb");
        }

        [Test]
        public virtual void TestDirectSave()
        {
            DeleteBase("btree46.ndb");
            var odb = Open("btree46.ndb");
            IBTree tree = new OdbBtreeMultiple(3, new InMemoryPersister());
            IBTreeNodeMultipleValuesPerKey node = new OdbBtreeNodeMultiple(tree);
            odb.Store(node);
            for (var i = 0; i < 4; i++)
            {
                node.SetKeyAndValueAt(new KeyAndValue(i + 1, "String" + (i + 1)), i);
                odb.Store(node);
            }
            odb.Close();
            DeleteBase("btree46.ndb");
        }
    }

    internal class MyObject
    {
        private readonly string name;

        public MyObject(string name)
        {
            this.name = name;
        }

        public virtual string GetName()
        {
            return name;
        }

        public override string ToString()
        {
            return name;
        }
    }
}
