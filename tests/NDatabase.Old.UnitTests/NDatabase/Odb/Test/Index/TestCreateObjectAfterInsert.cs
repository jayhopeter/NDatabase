using System;
using NDatabase.Api;
using NDatabase.Tool.Wrappers;
using NUnit.Framework;

namespace Test.NDatabase.Odb.Test.Index
{
    [TestFixture]
    public class TestCreateObjectAfterInsert : ODBTest
    {
        /// <summary>
        ///   Test the creation of an index after having created objects.
        /// </summary>
        /// <remarks>
        ///   Test the creation of an index after having created objects. In this case
        ///   ODB should creates the index and update it with already existing objects
        /// </remarks>
        /// <exception cref="System.Exception">System.Exception</exception>
        [Test]
        public virtual void Test1000Objects()
        {
            var OdbFileName = "index2.test1.odb";
            IOdb odb = null;
            var size = 1000;
            var start = OdbTime.GetCurrentTimeInMs();

            try
            {
                DeleteBase(OdbFileName);
                odb = Open(OdbFileName);
                for (var i = 0; i < size; i++)
                {
                    var io = new IndexedObject("name" + i, i, new DateTime());
                    odb.Store(io);
                }
                odb.Close();
                Println("\n\n END OF INSERT \n\n");
                odb = Open(OdbFileName);
                var names = new[] {"name"};
                odb.IndexManagerFor<IndexedObject>().AddUniqueIndexOn("index1", names);
                Println("\n\n after create index\n\n");
                var query = odb.Query<IndexedObject>();
                query.Descend("name").Constrain((object) "name0").Equal();
                var objects =
                    query.Execute<IndexedObject>(true);

                Println("\n\nafter get Objects\n\n");
                AssertEquals(1, objects.Count);
                var query2 = odb.Query<IndexedObject>();
                query2.Descend("duration").Constrain((object) 9).Equal();

                objects = query2.Execute<IndexedObject>(true);

                AssertEquals(1, objects.Count);
                objects = odb.Query<IndexedObject>().Execute<IndexedObject>(true);
                AssertEquals(size, objects.Count);
            }
            finally
            {
                var end = OdbTime.GetCurrentTimeInMs();
                Println((end - start) + "ms");
                
                odb.Close();
            }
        }

        /// <summary>
        ///   Test the creation of an index after having created objects.
        /// </summary>
        /// <remarks>
        ///   Test the creation of an index after having created objects. In this case
        ///   ODB should creates the index and update it with already existing objects
        /// </remarks>
        /// <exception cref="System.Exception">System.Exception</exception>
        [Test]
        public virtual void Test100ObjectsIntiNdex()
        {
            var OdbFileName = "index2.test2.odb";
            IOdb odb = null;
            var size = 100;
            var start = OdbTime.GetCurrentTimeInMs();
            
            try
            {
                DeleteBase(OdbFileName);
                odb = Open(OdbFileName);
                for (var i = 0; i < size; i++)
                {
                    var io = new IndexedObject("name" + i, i, new DateTime());
                    odb.Store(io);
                }
                odb.Close();
                Println("\n\n END OF INSERT \n\n");
                odb = Open(OdbFileName);
                var names = new[] {"duration"};
                odb.IndexManagerFor<IndexedObject>().AddUniqueIndexOn("index1", names);
                Println("\n\n after create index\n\n");
                var query = odb.Query<IndexedObject>();
                query.Descend("name").Constrain((object) "name0").Equal();

                var objects =
                    query.Execute<IndexedObject>(true);

                Println("\n\nafter get Objects\n\n");
                AssertEquals(1, objects.Count);
                var query2 = odb.Query<IndexedObject>();
                query2.Descend("duration").Constrain((object) 10).Equal();

                objects = query2.Execute<IndexedObject>(true);

                AssertEquals(1, objects.Count);
                objects = odb.Query<IndexedObject>().Execute<IndexedObject>(true);
                AssertEquals(size, objects.Count);
            }
            finally
            {
                var end = OdbTime.GetCurrentTimeInMs();
                Println((end - start) + "ms");
            }
        }

        /// <summary>
        ///   Test the creation of an index after having created objects.
        /// </summary>
        /// <remarks>
        ///   Test the creation of an index after having created objects. In this case
        ///   ODB should creates the index and update it with already existing objects
        /// </remarks>
        /// <exception cref="System.Exception">System.Exception</exception>
        [Test]
        public virtual void Test1Object()
        {
            const string odbFileName = "index2.test3.odb";

            DeleteBase(odbFileName);
            using (var odb = Open(odbFileName))
            {
                var io = new IndexedObject("name", 5, new DateTime());
                odb.Store(io);
            }

            using (var odb = Open(odbFileName))
            {
                var names = new[] {"name"};
                odb.IndexManagerFor<IndexedObject>().AddUniqueIndexOn("index1", names);
                var query = odb.Query<IndexedObject>();
                query.Descend("name").Constrain((object) "name").Equal();

                var objects =
                    query.Execute<IndexedObject>(true);

                AssertEquals(1, objects.Count);
            }
        }

        /// <summary>
        ///   Test the creation of an index after having created objects.
        /// </summary>
        /// <remarks>
        ///   Test the creation of an index after having created objects. In this case
        ///   ODB should creates the index and update it with already existing objects
        /// </remarks>
        /// <exception cref="System.Exception">System.Exception</exception>
        [Test]
        public virtual void Test2000Objects()
        {
            var OdbFileName = "index2.test4.odb";
            var start = OdbTime.GetCurrentTimeInMs();
            IOdb odb = null;
            var size = 2000;
            try
            {
                DeleteBase(OdbFileName);
                odb = Open(OdbFileName);
                for (var i = 0; i < size; i++)
                {
                    var io = new IndexedObject("name" + i, i, new DateTime());
                    odb.Store(io);
                }
                odb.Close();
                odb = Open(OdbFileName);
                var names = new[] {"name"};
                odb.IndexManagerFor<IndexedObject>().AddUniqueIndexOn("index1", names);
                var query = odb.Query<IndexedObject>();
                query.Descend("name").Constrain((object) "name0").Equal();

                var objects =
                    query.Execute<IndexedObject>(true);

                AssertEquals(1, objects.Count);
                objects = odb.Query<IndexedObject>().Execute<IndexedObject>(true);
                AssertEquals(size, objects.Count);
            }
            finally
            {
                if (odb != null)
                    odb.Close();
                var end = OdbTime.GetCurrentTimeInMs();
                Println((end - start) + "ms");
            }
        }
    }
}
