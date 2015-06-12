using System;
using System.Linq;
using NDatabase;
using NDatabase.Api.Query;
using NDatabase.Core.Query;
using NDatabase.Exceptions;
using NDatabase.Tool.Wrappers;
using NUnit.Framework;

namespace Test.NDatabase.Odb.Test.Index
{
    [TestFixture]
    public class TestIndex2 : ODBTest
    {
        /// <summary>
        ///   Test index creation without commit
        /// </summary>
        /// <exception cref="System.Exception">System.Exception</exception>
        [Test]
        public virtual void TestCreateIndexWithoutCommit()
        {
            var baseName = GetBaseName();
            DeleteBase(baseName);
            var odb = Open(baseName);
            var clazz = odb.IndexManagerFor<IndexedObject3>();
            var indexFields1 = new[] {"i1"};
            clazz.AddUniqueIndexOn("index1", indexFields1);
            for (var i = 0; i < 10; i++)
            {
                var io = new IndexedObject3(1 + i, 2, 3, "1" + i, "2", "3", new DateTime(2009, i + 1, 1), new DateTime(),
                                            new DateTime());
                odb.Store(io);
            }
            odb.Close();
            odb = Open(baseName);
            IQuery q = odb.Query<IndexedObject3>();
            q.Descend("i1").Constrain((object) 1).Equal();
            var iis = q.Execute<IndexedObject3>();
            odb.Close();
            AssertEquals(1, iis.Count);
            AssertTrue(((IInternalQuery)q).GetExecutionPlan().UseIndex());
            DeleteBase(baseName);
        }

        [Test]
        public void Test_if_odb_list_is_properly_stored()
        {
            IOdbList<string> list = new OdbList<string> {"one", "two"};
            const string dbName = "list.ndb";

            OdbFactory.Delete(dbName);

            using (var odb = OdbFactory.Open(dbName))
            {
                odb.Store(list);
            }
            
            using (var odb = OdbFactory.Open(dbName))
            {
                var items = odb.Query<IOdbList<string>>().Execute<IOdbList<string>>().First();
                CollectionAssert.AreEqual(items, list);

                var items2 = odb.Query<OdbList<string>>().Execute<OdbList<string>>().First();
                CollectionAssert.AreEqual(items2, list);
            }
        }

        /// <summary>
        ///   Create objects, then create index, then execute a select with index, then
        ///   rebuild index e execute
        /// </summary>
        /// <exception cref="System.Exception">System.Exception</exception>
        [Test]
        public virtual void TestDeleteIndex()
        {
            var baseName = GetBaseName();
            DeleteBase(baseName);
            var odb = Open(baseName);
            for (var i = 0; i < 250; i++)
            {
                var io = new IndexedObject3(1 + i, 2, 3, "1" + i, "2", "3", new DateTime(2009, (i % 12) + 1, 1),
                                            new DateTime(), new DateTime());
                odb.Store(io);
            }
            odb.Close();

            odb = Open(baseName);
            var clazz = odb.IndexManagerFor<IndexedObject3>();
            var indexFields1 = new[] {"i1", "i2", "i3"};
            clazz.AddUniqueIndexOn("index1", indexFields1);
            odb.Close();

            odb = Open(baseName);

            IQuery q =
                odb.Query<IndexedObject3>();

            ((IConstraint) q.Descend("i1").Constrain((object) 10).Equal()).And(q.Descend("i2").Constrain((object) 2).Equal()).And(q.Descend("i3").Constrain((object) 3).Equal());

            var objects = q.Execute<IndexedObject3>();
            AssertEquals(true, ((IInternalQuery)q).GetExecutionPlan().UseIndex());
            odb.IndexManagerFor<IndexedObject3>().DeleteIndex("index1");
            odb.Close();

            odb = Open(baseName);
            q = odb.Query<IndexedObject3>();
            ((IConstraint) q.Descend("i1").Constrain((object) 10).Equal()).And(q.Descend("i2").Constrain((object) 2).Equal()).And(q.Descend("i3").Constrain((object) 3).Equal());
            objects = q.Execute<IndexedObject3>();

            AssertEquals(false, ((IInternalQuery)q).GetExecutionPlan().UseIndex());
            odb.Close();
            DeleteBase(baseName);
        }

        [Test]
        public virtual void TestIndexFail()
        {
            var baseName = GetBaseName();
            DeleteBase(baseName);
            var @base = Open(baseName);
            var indexName = "index1";
            var clazz = @base.IndexManagerFor<IndexedObject3>();
            var indexFields1 = new[] {"i1", "i2", "i3"};
            clazz.AddUniqueIndexOn(indexName, indexFields1);
            @base.Close();
            @base = Open(baseName);
            var io = new IndexedObject3(1, 2, 3, "1", "2", "3", new DateTime(), new DateTime(), new DateTime());
            @base.Store(io);
            try
            {
                var io2 = new IndexedObject3(1, 2, 3, "1", "2", "3", new DateTime(), new DateTime(), new DateTime());
                @base.Store(io2);
            }
            catch (DuplicatedKeyException e)
            {
                Console.WriteLine(e.Message);
                Assert.Pass();
            }
            // println(e.getMessage());
            @base.Close();
            @base = Open(baseName);
            var query = @base.Query<IndexedObject3>();
            var oo3 = query.Execute<IndexedObject3>();
            @base.Close();
            AssertEquals(0, oo3.Count);
            DeleteBase(baseName);
        }

        /// <summary>
        ///   Create objects, then create index, then execute a select with index, then
        ///   rebuild index e execute
        /// </summary>
        /// <exception cref="System.Exception">System.Exception</exception>
        [Test]
        public virtual void TestRebuildIndex()
        {
            var baseName = GetBaseName();
            DeleteBase(baseName);
            var odb = Open(baseName);
            for (var i = 0; i < 250; i++)
            {
                var io = new IndexedObject3(1 + i, 2, 3, "1" + i, "2", "3", new DateTime(2009, (i % 12) + 1, 1),
                                            new DateTime(), new DateTime());
                odb.Store(io);
            }
            odb.Close();

            odb = Open(baseName);
            var clazz = odb.IndexManagerFor<IndexedObject3>();
            var indexFields1 = new[] {"i1", "i2", "i3"};
            clazz.AddUniqueIndexOn("index1", indexFields1);
            odb.Close();

            odb = Open(baseName);

            IQuery q =
                odb.Query<IndexedObject3>();

            ((IConstraint) q.Descend("i1").Constrain((object) 10).Equal()).And(q.Descend("i2").Constrain((object) 2).Equal()).And(q.Descend("i3").Constrain((object) 3).Equal());

            var objects = q.Execute<IndexedObject3>();
            AssertEquals(true, ((IInternalQuery)q).GetExecutionPlan().UseIndex());
            odb.IndexManagerFor<IndexedObject3>().RebuildIndex("index1");
            odb.Close();

            odb = Open(baseName);
            q = odb.Query<IndexedObject3>();

            ((IConstraint) q.Descend("i1").Constrain((object) 10).Equal()).And(q.Descend("i2").Constrain((object) 2).Equal()).And(q.Descend("i3").Constrain((object) 3).Equal());
            objects = q.Execute<IndexedObject3>();
            AssertEquals(true, ((IInternalQuery)q).GetExecutionPlan().UseIndex());
            odb.Close();
            DeleteBase(baseName);
        }

        [Test]
        public virtual void TestSaveIndex()
        {
            var baseName = GetBaseName();
            DeleteBase(baseName);
            var @base = Open(baseName);
            var clazz = @base.IndexManagerFor<IndexedObject3>();
            var indexFields1 = new[] {"i1", "i2", "i3"};
            clazz.AddUniqueIndexOn("index1", indexFields1);
            var indexFields2 = new[] {"s1", "s2", "s3"};
            clazz.AddUniqueIndexOn("index2", indexFields2);
            var indexFields3 = new[] {"dt1", "dt2", "dt3"};
            clazz.AddUniqueIndexOn("index3", indexFields3);
            var indexFields4 = new[] {"i1", "i2", "i3", "s1", "s2", "s3", "dt1", "dt2", "dt3"};
            clazz.AddUniqueIndexOn("index4", indexFields4);
            @base.Close();
            @base = Open(baseName);
            
            @base.Close();
            @base = Open(baseName);
            for (var i = 0; i < 10; i++)
            {
                var io = new IndexedObject3(1 + i, 2, 3, "1" + i, "2", "3", new DateTime(2009, i + 1, 1), new DateTime(),
                                            new DateTime());
                @base.Store(io);
            }
            @base.Close();
            DeleteBase(baseName);
        }
    }
}
