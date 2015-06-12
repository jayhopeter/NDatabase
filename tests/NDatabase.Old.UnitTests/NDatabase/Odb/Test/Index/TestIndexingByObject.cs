using System;
using System.Linq;
using NDatabase.Api;
using NDatabase.Api.Query;
using NDatabase.Core.Query;
using NDatabase.Meta;
using NDatabase.Tool.Wrappers;
using NUnit.Framework;

namespace Test.NDatabase.Odb.Test.Index
{
    /// <summary>
    ///   Nunit to test indexing an object when the index field is an object and not a
    ///   native attribute
    /// </summary>
    [TestFixture]
    public class TestIndexingByObject : ODBTest
    {
        [Test]
        public virtual void Test1()
        {
            Println("************START OF TEST1***************");
            
            DeleteBase("index-object");
            IndexedObject2 o1;
            using (var odb = Open("index-object"))
            {
                var fields = new[] {"object"};
                odb.IndexManagerFor<IndexedObject2>().AddUniqueIndexOn("index1", fields);
                o1 = new IndexedObject2("Object1", new IndexedObject("Inner Object 1", 10, new DateTime()));
                odb.Store(o1);

                Assert.That(odb.IndexManagerFor<IndexedObject2>().ExistIndex("index1"), Is.True);

                var indexes = odb.QueryAndExecute<ClassInfoIndex>().ToList();
                Assert.That(indexes.Count, Is.EqualTo(1));
            }

            using (var odb = Open("index-object"))
            {
                var indexes = odb.QueryAndExecute<ClassInfoIndex>().ToList();
                Assert.That(indexes.Count, Is.EqualTo(1));

                Assert.That(odb.IndexManagerFor<IndexedObject2>().ExistIndex("index1"), Is.True);

                var query = odb.Query<IndexedObject>();
                var objects = query.Execute<IndexedObject>();
                var io = objects.GetFirst();

                IQuery q = odb.Query<IndexedObject2>();
                q.Descend("object").Constrain(io).Identity();

                var objects2 = q.Execute<IndexedObject2>();
                IndexedObject2 o2 = objects2.GetFirst();

                AssertEquals(o1.GetName(), o2.GetName());
                Println(((IInternalQuery)q).GetExecutionPlan().GetDetails());
                AssertFalse(((IInternalQuery)q).GetExecutionPlan().GetDetails().IndexOf("index1") == -1);
            }

            DeleteBase("index-object");
            Println("************END OF TEST1***************");
        }

        [Test]
        public virtual void Test2()
        {
            Println("************START OF TEST2***************");
            DeleteBase("index-object");
            var odb = Open("index-object");
            var fields = new[] {"object"};
            odb.IndexManagerFor<IndexedObject2>().AddUniqueIndexOn("index1", fields);
            var size = 500;
            for (var i = 0; i < size; i++)
                odb.Store(new IndexedObject2("Object " + i, new IndexedObject("Inner Object " + i, i, new DateTime())));
            odb.Close();
            odb = Open("index-object");
            IQuery q = odb.Query<IndexedObject>();
            q.Descend("name").Constrain((object) ("Inner Object " + (size - 1))).Equal();
            // First get the object used to index, the last one. There is no index
            // on the class and field
            var start0 = OdbTime.GetCurrentTimeInMs();
            var objects = q.Execute<IndexedObject>();
            var end0 = OdbTime.GetCurrentTimeInMs();
            var io = objects.GetFirst();
            Println("d0=" + (end0 - start0));
            Println(((IInternalQuery)q).GetExecutionPlan().GetDetails());
            q = odb.Query<IndexedObject2>();
            q.Descend("object").Constrain(io).Identity();
            var start = OdbTime.GetCurrentTimeInMs();

            var objects2 = q.Execute<IndexedObject2>();
            var end = OdbTime.GetCurrentTimeInMs();
            Println("d=" + (end - start));
            var o2 = objects2.GetFirst();
            odb.Close();
            AssertEquals("Object " + (size - 1), o2.GetName());
            Println(((IInternalQuery)q).GetExecutionPlan().GetDetails());
            AssertTrue(((IInternalQuery)q).GetExecutionPlan().UseIndex());
            DeleteBase("index-object");
            Println("************END OF TEST2***************");
        }

        [Test]
        public virtual void Test3_BadAttributeInIndex()
        {
            var baseName = GetBaseName();
            IOdb odb = null;
            var fieldName = "fkjdsfkjdhfjkdhjkdsh";
            try
            {
                odb = Open(baseName);
                var fields = new[] {fieldName};
                odb.IndexManagerFor<IndexedObject2>().AddUniqueIndexOn("index1", fields);
                Fail("Should have thrown an exception because the field " + fieldName + " does not exist");
            }
            catch (Exception)
            {
            }
            finally
            {
                // normal
                odb.Close();
                DeleteBase(baseName);
            }
        }

        [Test]
        public virtual void Test4()
        {
            var baseName = GetBaseName();
            var odb = Open(baseName);
            var fields = new[] {"object"};
            odb.IndexManagerFor<IndexedObject2>().AddUniqueIndexOn("index1", fields);
            var fields2 = new[] {"name"};
            odb.IndexManagerFor<IndexedObject>().AddUniqueIndexOn("index2", fields2);
            var size = 500;
            for (var i = 0; i < size; i++)
                odb.Store(new IndexedObject2("Object " + i, new IndexedObject("Inner Object " + i, i, new DateTime())));
            odb.Close();
            odb = Open(baseName);
            IQuery q = odb.Query<IndexedObject>();
            q.Descend("name").Constrain((object) ("Inner Object " + (size - 1))).Equal();
            // First get the object used to index, the last one. There is no index
            // on the class and field
            var start0 = OdbTime.GetCurrentTimeInMs();
            var objects = q.Execute<IndexedObject>();
            var end0 = OdbTime.GetCurrentTimeInMs();
            // check if index has been used
            AssertTrue(((IInternalQuery)q).GetExecutionPlan().UseIndex());
            var io = objects.GetFirst();
            Println("d0=" + (end0 - start0));
            Println(((IInternalQuery)q).GetExecutionPlan().GetDetails());
            q = odb.Query<IndexedObject2>();
            q.Descend("object").Constrain(io).Identity();
            var start = OdbTime.GetCurrentTimeInMs();

            var objects2 = q.Execute<IndexedObject2>();
            var end = OdbTime.GetCurrentTimeInMs();
            Println("d=" + (end - start));
            var o2 = objects2.GetFirst();
            odb.Close();
            AssertEquals("Object " + (size - 1), o2.GetName());
            Println(((IInternalQuery)q).GetExecutionPlan().GetDetails());
            AssertTrue(((IInternalQuery)q).GetExecutionPlan().UseIndex());
            DeleteBase(baseName);
        }
    }
}
