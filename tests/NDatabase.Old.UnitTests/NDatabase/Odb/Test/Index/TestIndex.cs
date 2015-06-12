using System;
using NDatabase;
using NDatabase.Api;
using NDatabase.Api.Query;
using NDatabase.Core.Query;
using NDatabase.Exceptions;
using NDatabase.Tool.Wrappers;
using NUnit.Framework;

namespace Test.NDatabase.Odb.Test.Index
{
    [TestFixture]
    public class TestIndex : ODBTest
    {
        [Test]
        public void SimpleUniqueIndex()
        {
            var baseName = GetBaseName();
            DeleteBase(baseName);
            var odb = Open(baseName);
            var clazz = odb.IndexManagerFor<VO.Login.Function>();
            var indexFields = new[] {"name"};
            clazz.AddUniqueIndexOn("index", indexFields);
            odb.Close();
            odb = Open(baseName);
            // inserting 3 objects with 3 different index keys
            odb.Store(new VO.Login.Function("function1"));
            odb.Store(new VO.Login.Function("function2"));
            odb.Store(new VO.Login.Function("function3"));
            odb.Close();
            odb = Open(baseName);
            try
            {
                // Tries to store another function with name function1 => send an
                // exception because of duplicated keys
                odb.Store(new VO.Login.Function("function1"));
                Fail("Should have thrown Exception");
            }
            catch (DuplicatedKeyException)
            {
                Assert.Pass();
                odb.Close();
                DeleteBase(baseName);
            }
        }

        [Test]
        public void TestIndexExist1()
        {
            var baseName = GetBaseName();
            DeleteBase(baseName);
            var odb = Open(baseName);
            var clazz = odb.IndexManagerFor<VO.Login.Function>();
            var indexFields = new[] {"name"};
            clazz.AddUniqueIndexOn("my-index", indexFields);
            odb.Store(new VO.Login.Function("test"));
            odb.Close();
            odb = Open(baseName);
            AssertTrue(odb.IndexManagerFor<VO.Login.Function>().ExistIndex("my-index"));
            AssertFalse(odb.IndexManagerFor<VO.Login.Function>().ExistIndex("my-indexdhfdjkfhdjkhj"));
            odb.Close();
        }

        [Test]
        public void TestIndexExist2()
        {
            var baseName = GetBaseName();
            DeleteBase(baseName);
            var odb = Open(baseName);
            var clazz = odb.IndexManagerFor<VO.Login.Function>();
            var indexFields = new[] {"name"};
            clazz.AddUniqueIndexOn("my-index", indexFields);
            odb.Close();
            odb = Open(baseName);
            AssertTrue(odb.IndexManagerFor<VO.Login.Function>().ExistIndex("my-index"));
            AssertFalse(odb.IndexManagerFor<VO.Login.Function>().ExistIndex("my-indexdhfdjkfhdjkhj"));
            odb.Close();
        }

        [Test]
        public void TestIndexWithOneFieldAndQueryWithTwoFields()
        {
            var baseName = GetBaseName();
            DeleteBase(baseName);
            var odb = Open(baseName);
            var clazz = odb.IndexManagerFor<IndexedObject>();
            var indexFields = new[] {"name"};
            clazz.AddUniqueIndexOn("index1", indexFields);
            odb.Close();
            odb = Open(baseName);
            var io1 = new IndexedObject("olivier", 15, new DateTime());
            odb.Store(io1);
            odb.Close();
            odb = Open(baseName);
            IQuery q = odb.Query<IndexedObject>();
            
            var constraintOnName = (IConstraint) q.Descend("name").Constrain((object) "olivier").Equal();
            var constraintOnDuration = (IConstraint) q.Descend("duration").Constrain((object) 15).Equal();
            constraintOnName.And(constraintOnDuration);

            var objects = q.Execute<IndexedObject>(true);
            odb.Close();
            Println(((IInternalQuery)q).GetExecutionPlan().ToString());
            AssertEquals(false, ((IInternalQuery)q).GetExecutionPlan().UseIndex());
            AssertEquals(1, objects.Count);
            DeleteBase(baseName);
        }

        [Test]
        public void TestInsertAndDeleteWithIndex()
        {
            var baseName = GetBaseName();
            DeleteBase(baseName);
            var odb = Open(baseName);
            // base.store(new IndexedObject());
            var clazz = odb.IndexManagerFor<IndexedObject>();
            var indexFields = new[] {"name"};
            clazz.AddUniqueIndexOn("index1", indexFields);
            odb.Close();
            odb = Open(baseName);
            var size = 1000;
            var start0 = OdbTime.GetCurrentTimeInMs();
            for (var i = 0; i < size; i++)
            {
                var io1 = new IndexedObject("olivier" + (i + 1), 15 + i, new DateTime());
                odb.Store(io1);
                if (i % 10 == 0)
                    Println(i);
            }
            var tt0 = OdbTime.GetCurrentTimeInMs();
            odb.Close();
            var tt1 = OdbTime.GetCurrentTimeInMs();
            var end0 = OdbTime.GetCurrentTimeInMs();
            
            Println("inserting time with index=" + (end0 - start0));
            Println("commit time=" + (tt1 - tt0));
            
            odb = Open(baseName);
            long totalTime = 0;
            long maxTime = 0;
            long minTime = 100000;
            for (var i = 0; i < size; i++)
            {
                IQuery query = odb.Query<IndexedObject>();
                query.Descend("name").Constrain("olivier" + (i + 1)).Equal();
                var start = OdbTime.GetCurrentTimeInMs();
                var objects = query.Execute<IndexedObject>(true);
                var end = OdbTime.GetCurrentTimeInMs();
                AssertEquals(1, objects.Count);
                var io2 = objects.GetFirst();
                AssertEquals("olivier" + (i + 1), io2.GetName());
                AssertEquals(15 + i, io2.GetDuration());
                var d = end - start;
                totalTime += d;
                if (d > maxTime)
                    maxTime = d;
                if (d < minTime)
                    minTime = d;
                odb.Delete(io2);
            }
            odb.Close();
            odb = Open(baseName);
            IQuery q = odb.Query<IndexedObject>();
            var oos = q.Execute<IndexedObject>(true);
            for (var i = 0; i < size; i++)
            {
                q = odb.Query<IndexedObject>();
                q.Descend("name").Constrain((object) ("olivier" + (i + 1))).Equal();
                oos = q.Execute<IndexedObject>(true);
                AssertEquals(0, oos.Count);
            }
            odb.Close();
            DeleteBase(baseName);
            Println("total duration=" + totalTime + " / " + (double) totalTime / size);
            Println("duration max=" + maxTime + " / min=" + minTime);
            if (testPerformance)
            {
                AssertTrue(totalTime / size < 0.9);

                AssertTrue(maxTime < 20);
                AssertTrue(minTime == 0);
            }
        }

        [Test]
        public void TestInsertAndDeleteWithIndex1()
        {
            var baseName = GetBaseName();
            DeleteBase(baseName);
            var odb = Open(baseName);
            // base.store(new IndexedObject());
            var clazz = odb.IndexManagerFor<IndexedObject>();
            var indexFields = new[] {"name"};
            clazz.AddUniqueIndexOn("index1", indexFields);
            odb.Close();
            odb = Open(baseName);
            var size = 1400;
            for (var i = 0; i < size; i++)
            {
                var io1 = new IndexedObject("olivier" + (i + 1), 15 + i, new DateTime());
                odb.Store(io1);
            }
            odb.Close();
            Console.Out.WriteLine("----ola");
            odb = Open(baseName);
            IQuery q = odb.Query<IndexedObject>();
            var objects = q.Execute<IndexedObject>();
            while (objects.HasNext())
            {
                var io = objects.Next();
                Println(io);
                odb.Delete(io);
            }
            odb.Close();
        }

        [Test]
        public void TestInsertAndDeleteWithIndexWith1000()
        {
            var baseName = GetBaseName();

            DeleteBase(baseName);
            var odb = Open(baseName);
            // base.store(new IndexedObject());
            var clazz = odb.IndexManagerFor<IndexedObject>();
            var indexFields = new[] {"name"};
            clazz.AddUniqueIndexOn("index1", indexFields);
            odb.Close();
            odb = Open(baseName);
            var size = 1000;
            var start0 = OdbTime.GetCurrentTimeInMs();
            for (var i = 0; i < size; i++)
            {
                var io1 = new IndexedObject("olivier" + (i + 1), 15 + i, new DateTime());
                odb.Store(io1);
                if (i % 100 == 0)
                    Println(i);
            }
            var tt0 = OdbTime.GetCurrentTimeInMs();
            odb.Close();
            var tt1 = OdbTime.GetCurrentTimeInMs();
            var end0 = OdbTime.GetCurrentTimeInMs();
            
            Println("inserting time with index=" + (end0 - start0));
            Println("commit time=" + (tt1 - tt0));
            
            odb = Open(baseName);
            long totalSelectTime = 0;
            long maxTime = 0;
            long minTime = 100000;
            var t0 = OdbTime.GetCurrentTimeInMs();
            long t1 = 0;
            long ta1 = 0;
            long ta2 = 0;
            long totalTimeDelete = 0;
            long totalTimeSelect = 0;
            for (var j = 0; j < size; j++)
            {
                IQuery q = odb.Query<IndexedObject>();
                q.Descend("name").Constrain((object) ("olivier" + (j + 1))).Equal();
                var start = OdbTime.GetCurrentTimeInMs();
                var objects = q.Execute<IndexedObject>(true);
                var end = OdbTime.GetCurrentTimeInMs();
                AssertEquals(1, objects.Count);
                var io2 = objects.GetFirst();
                AssertEquals("olivier" + (j + 1), io2.GetName());
                AssertEquals(15 + j, io2.GetDuration());
                var d = end - start;
                totalSelectTime += d;
                if (d > maxTime)
                    maxTime = d;
                if (d < minTime)
                    minTime = d;
                ta1 = OdbTime.GetCurrentTimeInMs();
                odb.Delete(io2);
                ta2 = OdbTime.GetCurrentTimeInMs();
                totalTimeDelete += (ta2 - ta1);
                totalTimeSelect += (end - start);
                if (j % 100 == 0 && j > 0)
                {
                    t1 = OdbTime.GetCurrentTimeInMs();
                    Println(j + " - t= " + (t1 - t0) + " - delete=" + (totalTimeDelete / j) + " / select=" +
                            (totalTimeSelect / j));
                    
                    t0 = t1;
                }
            }
            odb.Close();
            Println("total select=" + totalSelectTime + " / " + (double) totalSelectTime / size);
            Println("total delete=" + totalTimeDelete + " / " + (double) totalTimeDelete / size);
            Println("duration max=" + maxTime + " / min=" + minTime);
            odb = Open(baseName);
            for (var i = 0; i < size; i++)
            {
                IQuery q = odb.Query<IndexedObject>();
                q.Descend("name").Constrain((object) ("olivier" + (i + 1))).Equal();
                var start = OdbTime.GetCurrentTimeInMs();
                var objects = q.Execute<IndexedObject>(true);
                var end = OdbTime.GetCurrentTimeInMs();
                AssertEquals(0, objects.Count);
                if (i % 100 == 0)
                    Println(i);
            }
            odb.Close();
            DeleteBase(baseName);
            var timePerObject = totalSelectTime / (float) size;
            Println("Time per object = " + timePerObject);
            if (timePerObject > 1)
                Println("Time per object = " + timePerObject);
            AssertTrue(timePerObject < 1);
            // TODO Try to get maxTime < 10!
            AssertTrue(maxTime < 250);
            AssertTrue(minTime < 1);
        }

        [Test]
        public void TestInsertAndDeleteWithIndexWith40Elements()
        {
            var baseName = GetBaseName();
            OdbConfiguration.SetIndexBTreeDegree(3);
            DeleteBase(baseName);
            var odb = Open(baseName);
            // base.store(new IndexedObject());
            var clazz = odb.IndexManagerFor<IndexedObject>();
            var indexFields = new[] {"name"};
            clazz.AddUniqueIndexOn("index1", indexFields);
            odb.Close();
            odb = Open(baseName);
            var size = 6;
            var start0 = OdbTime.GetCurrentTimeInMs();
            for (var i = 0; i < size; i++)
            {
                var io1 = new IndexedObject("olivier" + (i + 1), 15 + i, new DateTime());
                odb.Store(io1);
                if (i % 1000 == 0)
                    Println(i);
            }
            var tt0 = OdbTime.GetCurrentTimeInMs();
            odb.Close();
            var tt1 = OdbTime.GetCurrentTimeInMs();
            var end0 = OdbTime.GetCurrentTimeInMs();
            
            Println("inserting time with index=" + (end0 - start0));
            Println("commit time=" + (tt1 - tt0));
            
            odb = Open(baseName);
            long totalTime = 0;
            long maxTime = 0;
            long minTime = 100000;
            var t0 = OdbTime.GetCurrentTimeInMs();
            long t1 = 0;
            for (var i = 0; i < size; i++)
            {
                IQuery q = odb.Query<IndexedObject>();
                q.Descend("name").Constrain((object) ("olivier" + (i + 1))).Equal();
                var start = OdbTime.GetCurrentTimeInMs();
                var objects = q.Execute<IndexedObject>(true);
                var end = OdbTime.GetCurrentTimeInMs();
                AssertEquals(1, objects.Count);
                var io2 = objects.GetFirst();
                AssertEquals("olivier" + (i + 1), io2.GetName());
                AssertEquals(15 + i, io2.GetDuration());
                var d = end - start;
                totalTime += d;
                if (d > maxTime)
                    maxTime = d;
                if (d < minTime)
                    minTime = d;
                odb.Delete(io2);
                if (i % 100 == 0)
                {
                    t1 = OdbTime.GetCurrentTimeInMs();
                    Println(i + " - t= " + (t1 - t0));
                    t0 = t1;
                }
            }
            // println(new BTreeDisplay().build(cii.getBTree(), true));
            odb.Close();
            odb = Open(baseName);
            for (var i = 0; i < size; i++)
            {
                IQuery q = odb.Query<IndexedObject>();
                q.Descend("name").Constrain((object) ("olivier" + (i + 1))).Equal();
                var start = OdbTime.GetCurrentTimeInMs();
                var objects = q.Execute<IndexedObject>(true);
                var end = OdbTime.GetCurrentTimeInMs();
                AssertEquals(0, objects.Count);
                if (i % 100 == 0)
                    Println(i);
            }
            var unitTime = (double) totalTime / size;
            Println("total duration=" + totalTime + " / " + unitTime);
            Println("duration max=" + maxTime + " / min=" + minTime);
            odb.Close();
            DeleteBase(baseName);
            AssertTrue(unitTime < 10);
            // TODO Try to get maxTime < 10!
            if (testPerformance)
            {
                AssertTrue(maxTime < 250);
                AssertTrue(minTime <= 1);
            }
            OdbConfiguration.SetIndexBTreeDegree(20);
        }

        [Test]
        public void TestInsertAndDeleteWithIndexWith4Elements()
        {
            var baseName = GetBaseName();
            DeleteBase(baseName);
            var odb = Open(baseName);
            // base.store(new IndexedObject());
            var clazz = odb.IndexManagerFor<IndexedObject>();
            var indexFields = new[] {"name"};
            clazz.AddUniqueIndexOn("index1", indexFields);
            odb.Close();
            odb = Open(baseName);
            var size = 4;
            var start0 = OdbTime.GetCurrentTimeInMs();
            for (var i = 0; i < size; i++)
            {
                var io1 = new IndexedObject("olivier" + (i + 1), 15 + i, new DateTime());
                odb.Store(io1);
                if (i % 1000 == 0)
                    Println(i);
            }
            var tt0 = OdbTime.GetCurrentTimeInMs();
            odb.Close();
            var tt1 = OdbTime.GetCurrentTimeInMs();
            var end0 = OdbTime.GetCurrentTimeInMs();
            
            Println("inserting time with index=" + (end0 - start0));
            Println("commit time=" + (tt1 - tt0));
            
            odb = Open(baseName);
            long totalTime = 0;
            long maxTime = 0;
            long minTime = 100000;
            var t0 = OdbTime.GetCurrentTimeInMs();
            long t1 = 0;
            for (var i = 0; i < size; i++)
            {
                IQuery q = odb.Query<IndexedObject>();
                q.Descend("name").Constrain((object) ("olivier" + (i + 1))).Equal();
                var start = OdbTime.GetCurrentTimeInMs();
                var objects = q.Execute<IndexedObject>(true);
                var end = OdbTime.GetCurrentTimeInMs();
                AssertEquals(1, objects.Count);
                var io2 = objects.GetFirst();
                AssertEquals("olivier" + (i + 1), io2.GetName());
                AssertEquals(15 + i, io2.GetDuration());
                var d = end - start;
                totalTime += d;
                if (d > maxTime)
                    maxTime = d;
                if (d < minTime)
                    minTime = d;
                odb.Delete(io2);
                if (i % 100 == 0)
                {
                    t1 = OdbTime.GetCurrentTimeInMs();
                    Println(i + " - t= " + (t1 - t0));
                    t0 = t1;
                }
            }
            odb.Close();
            odb = Open(baseName);
            for (var i = 0; i < size; i++)
            {
                IQuery q = odb.Query<IndexedObject>();
                q.Descend("name").Constrain((object) ("olivier" + (i + 1))).Equal();
                var start = OdbTime.GetCurrentTimeInMs();
                var objects = q.Execute<IndexedObject>(true);
                var end = OdbTime.GetCurrentTimeInMs();
                AssertEquals(0, objects.Count);
                if (i % 100 == 0)
                    Println(i);
            }
            odb.Close();
            DeleteBase(baseName);
            var unitTime = (double) totalTime / size;
            Println("total duration=" + totalTime + " / " + (double) totalTime / size);
            Println("duration max=" + maxTime + " / min=" + minTime);
            AssertTrue(unitTime < 10);

            if (testPerformance)
            {
                AssertTrue(maxTime < 250);
                AssertTrue(minTime <= 1);
            }
        }

        /// <summary>
        ///   Test with two key index
        /// </summary>
        [Test]
        public void TestInsertWith3Indexes()
        {
            var baseName = GetBaseName();
            DeleteBase(baseName);
            var odb = Open(baseName);
            // Configuration.setUseLazyCache(true);
            // base.store(new IndexedObject());
            var clazz = odb.IndexManagerFor<IndexedObject>();
            var indexFields3 = new[] {"name"};
            clazz.AddUniqueIndexOn("index3", indexFields3);
            var indexFields2 = new[] {"name", "creation"};
            clazz.AddUniqueIndexOn("index2", indexFields2);
            var indexField4 = new[] {"duration", "creation"};
            clazz.AddUniqueIndexOn("inde3", indexField4);
            odb.Close();
            odb = Open(baseName);
            var size = 1000;
            var start0 = OdbTime.GetCurrentTimeInMs();
            var dates = new DateTime[size];
            for (var i = 0; i < size; i++)
            {
                // println(i);
                dates[i] = new DateTime();
                var io1 = new IndexedObject("olivier" + (i + 1), i, dates[i]);
                odb.Store(io1);
                if (i % 100 == 0)
                    Println(i);
            }
            odb.Close();
            var end0 = OdbTime.GetCurrentTimeInMs();
            
            Println("inserting time with index=" + (end0 - start0));
            odb = Open(baseName);
            var start = OdbTime.GetCurrentTimeInMs();
            for (var i = 0; i < size; i++)
            {
                IQuery q =
                    odb.Query<IndexedObject>();

                var constraintOnDuration = (IConstraint) q.Descend("duration").Constrain((object) i).Equal();
                var constraintOnCreation = (IConstraint) q.Descend("creation").Constrain((object) dates[i]).Equal();
                constraintOnDuration.And(constraintOnCreation);

                var objects = q.Execute<IndexedObject>(true);
                AssertEquals(1, objects.Count);
                AssertTrue(((IInternalQuery)q).GetExecutionPlan().UseIndex());
            }
            var end = OdbTime.GetCurrentTimeInMs();
            double duration = (end - start);
            duration = duration / size;
            Println("duration=" + duration);
            odb.Close();
            DeleteBase(baseName);

            Println(duration);
            var d = 2;

            if (duration > d)
                Fail("Time of search in index is greater than " + d + " ms : " + duration);
        }

        /// <summary>
        ///   Test with 3 indexes
        /// </summary>
        [Test]
        public void TestInsertWith3IndexesCheckAll()
        {
            var baseName = GetBaseName();
            // LogUtil.logOn(LazyOdbBtreePersister.LOG_ID, true);
            DeleteBase(baseName);
            var odb = Open(baseName);

            // base.store(new IndexedObject());
            var clazz = odb.IndexManagerFor<IndexedObject>();
            var indexFields = new[] {"duration"};
            clazz.AddIndexOn("index1", indexFields);
            var indexFields2 = new[] {"creation"};
            clazz.AddIndexOn("index2", indexFields2);
            var indexFields3 = new[] {"name"};
            clazz.AddIndexOn("index3", indexFields3);
            odb.Close();
            odb = Open(baseName);
            var size = 130;
            var commitInterval = 10;
            var start0 = OdbTime.GetCurrentTimeInMs();
            for (var i = 0; i < size; i++)
            {
                var io1 = new IndexedObject("olivier" + (i + 1), i, new DateTime());
                odb.Store(io1);
                if (i % commitInterval == 0)
                {
                    odb.Commit();
                    Println(i + " : commit / " + size);
                }
            }
            odb.Close();
            var end0 = OdbTime.GetCurrentTimeInMs();
            
            // ObjectWriter.getNbNormalUpdates());
            // println("inserting time with index=" + (end0 - start0));
            odb = Open(baseName);
            var start = OdbTime.GetCurrentTimeInMs();
            for (var i = 0; i < size; i++)
            {
                IQuery q = odb.Query<IndexedObject>();
                q.Descend("duration").Constrain((object) i).Equal();
                var objects = q.Execute<IndexedObject>(false);
                // println("olivier" + (i+1));
                AssertEquals(1, objects.Count);
            }
            var end = OdbTime.GetCurrentTimeInMs();
            try
            {
                var duration = (end - start) / (float) size;
                Println(duration);
                var d = 0.144;

                if (testPerformance && duration > d)
                    Fail("Time of search in index is greater than " + d + " ms : " + duration);
            }
            finally
            {
                odb.Close();
                DeleteBase(baseName);
            }
        }

        /// <summary>
        ///   Test index with 3 keys .
        /// </summary>
        /// <remarks>
        ///   Test index with 3 keys .
        ///   Select using only one field to verify that query does not use index, then
        ///   execute a query with the 3 fields and checks than index is used
        /// </remarks>
        [Test]
        public void TestInsertWith3Keys()
        {
            var baseName = GetBaseName();
            DeleteBase(baseName);
            var odb = Open(baseName);
            // base.store(new IndexedObject());
            var clazz = odb.IndexManagerFor<IndexedObject>();
            var indexFields = new[] {"name", "duration", "creation"};
            clazz.AddUniqueIndexOn("index", indexFields);
            odb.Close();
            odb = Open(baseName);
            var size = 500;
            var commitInterval = 10000;
            var start0 = OdbTime.GetCurrentTimeInMs();
            for (var i = 0; i < size; i++)
            {
                var io2 = new IndexedObject("olivier" + (i + 1), i + 15 + size, new DateTime());
                odb.Store(io2);
                if (i % commitInterval == 0)
                {
                    var t0 = OdbTime.GetCurrentTimeInMs();
                    odb.Commit();
                    var t1 = OdbTime.GetCurrentTimeInMs();
                    Println(i + " : commit - ctime " + (t1 - t0) + " -ttime=");
                }
            }
            var theDate = new DateTime();
            var theName = "name indexed";
            var theDuration = 45;
            var io1 = new IndexedObject(theName, theDuration, theDate);
            odb.Store(io1);
            odb.Close();

            odb = Open(baseName);
            // first search without index
            IQuery q = odb.Query<IndexedObject>();
            q.Descend("name").Constrain((object) theName).Equal();
            var objects = q.Execute<IndexedObject>(true);
            AssertFalse(((IInternalQuery)q).GetExecutionPlan().UseIndex());
            Println(((IInternalQuery)q).GetExecutionPlan().GetDetails());
            AssertEquals(1, objects.Count);
            var io3 = objects.GetFirst();
            AssertEquals(theName, io3.GetName());
            AssertEquals(theDuration, io3.GetDuration());
            AssertEquals(theDate, io3.GetCreation());
            odb.Close();

            odb = Open(baseName);
            // Then search usin index
            q = odb.Query<IndexedObject>();

            var constraintOnName = (IConstraint) q.Descend("name").Constrain((object) theName).Equal();
            var constraintOnCreation = (IConstraint) q.Descend("creation").Constrain((object) theDate).Equal();
            var constraintOnDuration = (IConstraint) q.Descend("duration").Constrain((object) theDuration).Equal();

            constraintOnName.And(constraintOnCreation).And(constraintOnDuration);

            objects = q.Execute<IndexedObject>(true);
            AssertTrue(((IInternalQuery)q).GetExecutionPlan().UseIndex());
            AssertEquals("index", ((IInternalQuery)q).GetExecutionPlan().GetIndex().Name);
            Println(((IInternalQuery)q).GetExecutionPlan().GetDetails());
            AssertEquals(1, objects.Count);
            io3 = objects.GetFirst();
            AssertEquals(theName, io3.GetName());
            AssertEquals(theDuration, io3.GetDuration());
            AssertEquals(theDate, io3.GetCreation());
            odb.Close();
        }

        /// <summary>
        ///   Test with two key index
        /// </summary>
        [Test]
        public void TestInsertWith4IndexesAndCommits()
        {
            var baseName = GetBaseName();

            DeleteBase(baseName);
            var odb = Open(baseName);
            // Configuration.setUseLazyCache(true);
            // base.store(new IndexedObject());
            var clazz = odb.IndexManagerFor<IndexedObject>();
            var indexField1 = new[] {"duration"};
            clazz.AddUniqueIndexOn("inde1", indexField1);
            var indexFields3 = new[] {"name"};
            clazz.AddUniqueIndexOn("index3", indexFields3);
            var indexFields2 = new[] {"name", "creation"};
            clazz.AddUniqueIndexOn("index2", indexFields2);
            var indexField4 = new[] {"duration", "creation"};
            clazz.AddUniqueIndexOn("inde4", indexField4);
            odb.Close();
            odb = Open(baseName);
            var size = 100;
            var commitInterval = 10;
            var start0 = OdbTime.GetCurrentTimeInMs();
            for (var i = 0; i < size; i++)
            {
                // println(i);
                var io1 = new IndexedObject("olivier" + (i + 1), i, new DateTime());
                odb.Store(io1);
                if (i % 10 == 0)
                    Println(i);
                if (i % commitInterval == 0)
                    odb.Commit();
            }
            odb.Close();
            var end0 = OdbTime.GetCurrentTimeInMs();
            
            Println("inserting time with index=" + (end0 - start0));
            odb = Open(baseName);
            var start = OdbTime.GetCurrentTimeInMs();
            for (var i = 0; i < size; i++)
            {
                IQuery q = odb.Query<IndexedObject>();
                q.Descend("duration").Constrain((object) i).Equal();
                var objects = q.Execute<IndexedObject>(false);
                // println("olivier" + (i+1));
                AssertEquals(1, objects.Count);
            }
            var end = OdbTime.GetCurrentTimeInMs();
            var duration = end - start;
            Println("duration=" + duration);
            odb.Close();
            DeleteBase(baseName);

            if (testPerformance && duration > 111)
                Fail("Time of search in index : " + duration + ", should be less than 111");
        }

        /// <summary>
        ///   Test with one key index
        /// </summary>
        [Test]
        public void TestInsertWithDateIndex3CheckAll()
        {
            var baseName = GetBaseName();
            // LogUtil.logOn(LazyOdbBtreePersister.LOG_ID, true);
            DeleteBase(baseName);
            var odb = Open(baseName);

            // base.store(new IndexedObject());
            var clazz = odb.IndexManagerFor<IndexedObject>();
            var indexFields = new[] {"creation"};
            clazz.AddUniqueIndexOn("index1", indexFields);
            odb.Close();
            odb = Open(baseName);
            var size = 1300;
            var commitInterval = 1000;
            var start0 = OdbTime.GetCurrentTimeInMs();
            for (var i = 0; i < size; i++)
            {
                var io1 = new IndexedObject("olivier" + (i + 1), i, new DateTime(start0 + i));
                odb.Store(io1);
                if (i % commitInterval == 0)
                    odb.Commit();
            }
            // println(i+" : commit / " + size);
            odb.Close();
            var end0 = OdbTime.GetCurrentTimeInMs();
            
            // ObjectWriter.getNbNormalUpdates());
            // println("inserting time with index=" + (end0 - start0));
            odb = Open(baseName);
            var start = OdbTime.GetCurrentTimeInMs();
            for (var i = 0; i < size; i++)
            {
                IQuery q = odb.Query<IndexedObject>();
                q.Descend("creation").Constrain((object) new DateTime(start0 + i)).Equal();
                var objects = q.Execute<IndexedObject>(false);
                // println("olivier" + (i+1));
                AssertEquals(1, objects.Count);
            }
            var end = OdbTime.GetCurrentTimeInMs();
            try
            {
                var duration = (end - start) / (float) size;
                Println(duration);
                var d = 0.144;

                if (testPerformance && duration > d)
                    Fail("Time of search in index is greater than " + d + " ms : " + duration);
            }
            finally
            {
                odb.Close();
                DeleteBase(baseName);
            }
        }

        [Test]
        public void TestInsertWithIndex()
        {
            var baseName = GetBaseName();
            DeleteBase(baseName);
            var odb = Open(baseName);
            var clazz = odb.IndexManagerFor<IndexedObject>();
            var indexFields = new[] {"name", "duration"};
            clazz.AddUniqueIndexOn("index1", indexFields);
            odb.Close();
            odb = Open(baseName);
            var io1 = new IndexedObject("olivier", 15, new DateTime());
            odb.Store(io1);
            odb.Close();
            odb = Open(baseName);
            IQuery q = odb.Query<IndexedObject>();
            q.Descend("name").Constrain(null).Equal().Not();
            var objects = q.Execute<IndexedObject>(true);
            odb.Close();
            AssertEquals(1, objects.Count);
            var io2 = objects.GetFirst();
            AssertEquals("olivier", io2.GetName());
            AssertEquals(15, io2.GetDuration());
            AssertFalse(((IInternalQuery)q).GetExecutionPlan().GetDetails().IndexOf("index1") != -1);
        }

        [Test]
        public void TestInsertWithIndex1()
        {
            var baseName = GetBaseName();
            DeleteBase(baseName);
            var odb = Open(baseName);
            // base.store(new IndexedObject());
            var clazz = odb.IndexManagerFor<IndexedObject>();
            var indexFields = new[] {"name"};
            clazz.AddUniqueIndexOn("index1", indexFields);
            odb.Close();
            odb = Open(baseName);
            var size = 1000;
            var start0 = OdbTime.GetCurrentTimeInMs();
            for (var i = 0; i < size; i++)
            {
                var io1 = new IndexedObject("olivier" + (i + 1), 15 + i, new DateTime());
                odb.Store(io1);
                if (i % 100 == 0)
                    Println(i);
            }
            var tt0 = OdbTime.GetCurrentTimeInMs();
            odb.Close();
            var tt1 = OdbTime.GetCurrentTimeInMs();
            var end0 = OdbTime.GetCurrentTimeInMs();
            
            Println("inserting time with index=" + (end0 - start0));
            Println("commit time=" + (tt1 - tt0));
            
            odb = Open(baseName);
            long totalTime = 0;
            long maxTime = 0;
            long minTime = 100000;
            for (var i = 0; i < size; i++)
            {
                IQuery q = odb.Query<IndexedObject>();
                q.Descend("name").Constrain((object) ("olivier" + (i + 1))).Equal();
                var start = OdbTime.GetCurrentTimeInMs();
                var objects = q.Execute<IndexedObject>(true);
                var end = OdbTime.GetCurrentTimeInMs();
                AssertEquals(1, objects.Count);
                var io2 = objects.GetFirst();
                AssertEquals("olivier" + (i + 1), io2.GetName());
                AssertEquals(15 + i, io2.GetDuration());
                var d = end - start;
                totalTime += d;
                if (d > maxTime)
                    maxTime = d;
                if (d < minTime)
                    minTime = d;
            }
            odb.Close();
            DeleteBase(baseName);
            Println("total duration=" + totalTime + " / " + (double) totalTime / size);
            Println("duration max=" + maxTime + " / min=" + minTime);
            if (testPerformance && totalTime / size > 2)
                Fail("Total/size is > than 2 : " + totalTime);
            if (testPerformance)
            {
                // TODO Try to get maxTime < 10!
                AssertTrue(maxTime < 100);
                AssertTrue(minTime < 1);
            }
        }

        [Test]
        public void TestInsertWithIndex2()
        {
            var baseName = GetBaseName();

            DeleteBase(baseName);
            var odb = Open(baseName);
            // base.store(new IndexedObject());
            var clazz = odb.IndexManagerFor<IndexedObject>();
            var indexFields = new[] {"name"};
            clazz.AddUniqueIndexOn("index1", indexFields);
            var size = 1000;
            var start0 = OdbTime.GetCurrentTimeInMs();
            for (var i = 0; i < size; i++)
            {
                var io1 = new IndexedObject("olivier" + (i + 1), 15 + i, new DateTime());
                odb.Store(io1);
                if (i % 100 == 0)
                    Println(i);
            }
            var tt0 = OdbTime.GetCurrentTimeInMs();
            odb.Close();
            var tt1 = OdbTime.GetCurrentTimeInMs();
            var end0 = OdbTime.GetCurrentTimeInMs();
            
            Println("inserting time with index=" + (end0 - start0));
            Println("commit time=" + (tt1 - tt0));
            
            odb = Open(baseName);
            long totalTime = 0;
            long maxTime = 0;
            long minTime = 100000;
            var t0 = OdbTime.GetCurrentTimeInMs();
            long t1 = 0;
            for (var i = 0; i < size; i++)
            {
                IQuery q = odb.Query<IndexedObject>();
                q.Descend("name").Constrain((object) ("olivier" + (i + 1))).Equal();
                var start = OdbTime.GetCurrentTimeInMs();
                var objects = q.Execute<IndexedObject>(true);
                var end = OdbTime.GetCurrentTimeInMs();
                AssertEquals(1, objects.Count);
                var io2 = objects.GetFirst();
                AssertEquals("olivier" + (i + 1), io2.GetName());
                AssertEquals(15 + i, io2.GetDuration());
                var d = end - start;
                totalTime += d;
                if (d > maxTime)
                    maxTime = d;
                if (d < minTime)
                    minTime = d;
                if (i % 100 == 0)
                {
                    t1 = OdbTime.GetCurrentTimeInMs();
                    Println("i=" + i + " - time=" + (t1 - t0));
                    t0 = t1;
                }
            }
            // /
            odb.Close();
            DeleteBase(baseName);
            // println("total duration=" + totalTime + " / " + (double) totalTime /
            // size);
            // println("duration max=" + maxTime + " / min=" + minTime);
            if (totalTime / size > 1)
                Fail("Total/size is > than 1 : " + (totalTime / (float) size));
            Println("Max time=" + maxTime);
            Println("Min time=" + minTime);
            // TODO Try to get maxTime < 10!
            AssertTrue(maxTime < 250);
            AssertTrue(minTime < 1);
        }

        /// <summary>
        ///   Test with on e key index
        /// </summary>
        [Test]
        public void TestInsertWithIndex3()
        {
            var baseName = GetBaseName();
            // LogUtil.logOn(LazyOdbBtreePersister.LOG_ID, true);
            DeleteBase(baseName);
            var odb = Open(baseName);

            // base.store(new IndexedObject());
            var clazz = odb.IndexManagerFor<IndexedObject>();
            var indexFields = new[] {"name"};
            clazz.AddUniqueIndexOn("index1", indexFields);
            odb.Close();
            odb = Open(baseName);
            var size = 1300;
            var commitInterval = 10;
            var start0 = OdbTime.GetCurrentTimeInMs();
            var engine = ((global::NDatabase.Odb)odb).GetStorageEngine();
            for (var i = 0; i < size; i++)
            {
                var io1 = new IndexedObject("olivier" + (i + 1), 15 + size, new DateTime());
                odb.Store(io1);
                if (i % commitInterval == 0)
                {
                    odb.Commit();
                    odb.Close();
                    odb = Open(baseName);
                    engine = ((global::NDatabase.Odb)odb).GetStorageEngine();
                }
                if (io1.GetName().Equals("olivier" + size))
                    Println("Ola chico");
            }
            engine = ((global::NDatabase.Odb)odb).GetStorageEngine();
            // println(new
            // BTreeDisplay().build(engine.getSession(true).getMetaModel().getClassInfo(IndexedObject.class.Name,
            // true).getIndex(0).getBTree(), true));
            odb.Close();
            var end0 = OdbTime.GetCurrentTimeInMs();
            
            // ObjectWriter.getNbNormalUpdates());
            Console.WriteLine("inserting time with index=" + (end0 - start0));
            odb = Open(baseName);
            
            IQuery q = odb.Query<IndexedObject>();
            q.Descend("name").Constrain((object) ("olivier" + size)).Equal();
            var start = OdbTime.GetCurrentTimeInMs();
            var objects = q.Execute<IndexedObject>(false);
            var end = OdbTime.GetCurrentTimeInMs();
            try
            {
                AssertEquals(1, objects.Count);
                var io2 = objects.GetFirst();
                AssertEquals("olivier" + size, io2.GetName());
                AssertEquals(15 + size, io2.GetDuration());
                var duration = end - start;
                Println("duration=" + duration);

                if (testPerformance)
                {
                    if (duration > 2)
                        Fail("Time of search in index is greater than 2ms : " + duration);
                }
            }
            finally
            {
                odb.Close();
                DeleteBase(baseName);
            }
        }

        [Test]
        public void TestInsertWithIndex3Part1()
        {
            var baseName = "index.ndb";
            // LogUtil.logOn(LazyOdbBtreePersister.LOG_ID, true);
            DeleteBase(baseName);
            var @base = Open(baseName);

            // base.store(new IndexedObject());
            var clazz = @base.IndexManagerFor<IndexedObject>();
            var indexFields = new[] {"name"};
            clazz.AddUniqueIndexOn("index1", indexFields);
            @base.Close();
            @base = Open(baseName);
            var size = 1300;
            var commitInterval = 10;
            var start0 = OdbTime.GetCurrentTimeInMs();
            var engine = ((global::NDatabase.Odb)@base).GetStorageEngine();
            for (var i = 0; i < size; i++)
            {
                var io1 = new IndexedObject("olivier" + (i + 1), 15 + size, new DateTime());
                @base.Store(io1);
                if (i % commitInterval == 0)
                {
                    @base.Commit();
                    @base.Close();
                    @base = Open(baseName);
                    engine = ((global::NDatabase.Odb)@base).GetStorageEngine();
                }
                if (io1.GetName().Equals("olivier" + size))
                    Println("Ola chico");
            }
            engine = ((global::NDatabase.Odb)@base).GetStorageEngine();
            // println(new
            // BTreeDisplay().build(engine.getSession(true).getMetaModel().getClassInfo(IndexedObject.class.Name,
            // true).getIndex(0).getBTree(), true));
            @base.Close();
            var end0 = OdbTime.GetCurrentTimeInMs();
        }

        [Test]
        public void TestInsertWithIndex3Part2()
        {
            var baseName = "index.ndb";
            var size = 1300;
            var odb = Open(baseName);
            IQuery q = odb.Query<IndexedObject>();
            q.Descend("name").Constrain((object) ("olivier" + size)).Equal();
            var start = OdbTime.GetCurrentTimeInMs();
            var objects = q.Execute<IndexedObject>(false);
            var end = OdbTime.GetCurrentTimeInMs();
            try
            {
                AssertEquals(1, objects.Count);
                var io2 = objects.GetFirst();
                AssertEquals("olivier" + size, io2.GetName());
                AssertEquals(15 + size, io2.GetDuration());
                var duration = end - start;
                Println("duration=" + duration);

                if (testPerformance)
                {
                    if (duration > 2)
                        Fail("Time of search in index is greater than 2ms : " + duration);
                }
            }
            finally
            {
                odb.Close();
            }
        }

        /// <summary>
        ///   Test with two key index
        /// </summary>
        [Test]
        public void TestInsertWithIndex4()
        {
            var baseName = GetBaseName();
            DeleteBase(baseName);
            var odb = Open(baseName);
            // base.store(new IndexedObject());
            var clazz = odb.IndexManagerFor<IndexedObject>();
            var indexFields3 = new[] {"name"};
            clazz.AddUniqueIndexOn("index3", indexFields3);
            var indexFields2 = new[] {"name", "creation"};
            clazz.AddUniqueIndexOn("index2", indexFields2);
            var indexField4 = new[] {"duration", "creation"};
            clazz.AddUniqueIndexOn("inde3", indexField4);
            odb.Close();
            odb = Open(baseName);
            var size = 500;
            var commitInterval = 1000;
            var start0 = OdbTime.GetCurrentTimeInMs();
            for (var i = 0; i < size; i++)
            {
                // println(i);
                var ioio = new IndexedObject("olivier" + (i + 1), i + 15 + size, new DateTime());
                odb.Store(ioio);
                if (i % commitInterval == 0)
                {
                    var t0 = OdbTime.GetCurrentTimeInMs();
                    odb.Commit();
                    var t1 = OdbTime.GetCurrentTimeInMs();
                    Println(i + " : commit - ctime " + (t1 - t0) + " -ttime=");
                }
            }
            var theDate = new DateTime();
            var theName = "name indexed";
            var io1 = new IndexedObject(theName, 45, theDate);
            odb.Store(io1);
            odb.Close();
            var end0 = OdbTime.GetCurrentTimeInMs();
            
            Println("inserting time with index=" + (end0 - start0));
            odb = Open(baseName);
            
            IQuery q = odb.Query<IndexedObject>();
            q.Descend("name").Constrain((object) theName).Equal();
            var start = OdbTime.GetCurrentTimeInMs();
            var objects = q.Execute<IndexedObject>(true);
            var end = OdbTime.GetCurrentTimeInMs();
            AssertEquals("index3", ((IInternalQuery)q).GetExecutionPlan().GetIndex().Name);
            AssertEquals(1, objects.Count);
            var io2 = objects.GetFirst();
            AssertEquals(theName, io2.GetName());
            AssertEquals(45, io2.GetDuration());
            AssertEquals(theDate, io2.GetCreation());
            var duration = end - start;
            Println("duration=" + duration);
            odb.Close();
            DeleteBase(baseName);

            if (testPerformance && duration > 1)
                Fail("Time of search in index > 1 : " + duration);
        }

        // deleteBase(baseName);
        /// <summary>
        ///   Test with one key index
        /// </summary>
        [Test]
        public void TestInsertWithIntIndex3CheckAll()
        {
            var baseName = GetBaseName();
            // LogUtil.logOn(LazyOdbBtreePersister.LOG_ID, true);
            DeleteBase(baseName);
            var odb = Open(baseName);

            // base.store(new IndexedObject());
            var clazz = odb.IndexManagerFor<IndexedObject>();
            var indexFields = new[] {"duration"};
            clazz.AddUniqueIndexOn("index1", indexFields);
            odb.Close();
            odb = Open(baseName);
            var size = 1300;
            var commitInterval = 10;
            var start0 = OdbTime.GetCurrentTimeInMs();
            for (var i = 0; i < size; i++)
            {
                var io1 = new IndexedObject("olivier" + (i + 1), i, new DateTime());
                odb.Store(io1);
                if (i % commitInterval == 0)
                    odb.Commit();
            }
            // println(i+" : commit / " + size);
            odb.Close();
            var end0 = OdbTime.GetCurrentTimeInMs();
            
            Console.WriteLine("inserting time with index=" + (end0 - start0));
            odb = Open(baseName);
            var start = OdbTime.GetCurrentTimeInMs();
            for (var i = 0; i < size; i++)
            {
                IQuery q = odb.Query<IndexedObject>();
                q.Descend("duration").Constrain((object) i).Equal();
                var objects = q.Execute<IndexedObject>(false);
                
                AssertEquals(1, objects.Count);
            }
            var end = OdbTime.GetCurrentTimeInMs();
            try
            {
                var duration = (end - start) / (float) size;
                if (testPerformance && duration > 2)
                    Fail("Time of search in index is greater than 2ms : " + duration);
            }
            finally
            {
                odb.Close();
                DeleteBase(baseName);
            }
        }

        /// <summary>
        ///   Test with on e key index
        /// </summary>
        [Test]
        public void TestInsertWithoutIndex3()
        {
            var baseName = GetBaseName();

            DeleteBase(baseName);
            var odb = Open(baseName);
            var size = 3000;
            var commitInterval = 1000;
            var start0 = OdbTime.GetCurrentTimeInMs();
            for (var i = 0; i < size; i++)
            {
                var io1 = new IndexedObject("olivier" + (i + 1), 15 + size, new DateTime());
                odb.Store(io1);
                if (i % commitInterval == 0)
                    odb.Commit();
            }
            // println(i+" : commit");
            odb.Close();
            var end0 = OdbTime.GetCurrentTimeInMs();
            
            Println("inserting time with index=" + (end0 - start0));
            odb = Open(baseName);
            IQuery q = odb.Query<IndexedObject>();
            q.Descend("name").Constrain((object) ("olivier" + size)).Equal();
            var start = OdbTime.GetCurrentTimeInMs();
            var objects = q.Execute<IndexedObject>(false);
            var end = OdbTime.GetCurrentTimeInMs();
            AssertEquals(1, objects.Count);
            var io2 = objects.GetFirst();
            AssertEquals("olivier" + size, io2.GetName());
            AssertEquals(15 + size, io2.GetDuration());
            var duration = end - start;
            Println("duration=" + duration);
            odb.Close();
            DeleteBase(baseName);

            Println(duration);
            double d = 500;

            if (duration > d)
                Fail("Time of search in index is greater than " + d + " ms : " + duration);
        }

        [Test]
        public void TestSaveIndex()
        {
            var baseName = GetBaseName();
            DeleteBase(baseName);
            var @base = Open(baseName);
            var clazz = @base.IndexManagerFor<IndexedObject>();
            var indexFields = new[] {"name", "duration"};
            clazz.AddUniqueIndexOn("index1", indexFields);
            var indexFields2 = new[] {"name", "creation"};
            clazz.AddUniqueIndexOn("index2", indexFields2);
            var indexFields3 = new[] {"duration", "creation"};
            clazz.AddUniqueIndexOn("index3", indexFields3);
            @base.Close();
            DeleteBase(baseName);
        }

        [Test]
        public void TestSizeBTree()
        {
            var baseName = GetBaseName();
            DeleteBase(baseName);
            var @base = Open(baseName);
            // base.store(new IndexedObject());
            var clazz = @base.IndexManagerFor<IndexedObject>();
            var indexFields = new[] {"name"};
            clazz.AddUniqueIndexOn("index1", indexFields);
            @base.Close();
            @base = Open(baseName);
            var size = 4;
            for (var i = 0; i < size; i++)
            {
                var io1 = new IndexedObject("olivier" + (i + 1), 15 + i, new DateTime());
                @base.Store(io1);
                if (i % 1000 == 0)
                    Println(i);
            }
            @base.Close();
            
            DeleteBase(baseName);
        }

        /// <summary>
        ///   Test index.
        /// </summary>
        /// <remarks>
        ///   Test index. Creates 1000 objects. Take 10 objects to update 10000 times.
        ///   Then check if all objects are ok
        /// </remarks>
        [Test]
        public void TestXUpdatesWithIndex()
        {
            var baseName = GetBaseName();
            DeleteBase(baseName);
            var odb = Open(baseName);
            // base.store(new IndexedObject());
            var clazz = odb.IndexManagerFor<IndexedObject>();
            var indexFields = new[] {"name"};
            clazz.AddUniqueIndexOn("index", indexFields);
            odb.Close();
            odb = Open(baseName);
            var start = OdbTime.GetCurrentTimeInMs();
            var size = 100;
            var nbObjects = 10;
            var nbUpdates = 10;
            for (var i = 0; i < size; i++)
            {
                var io1 = new IndexedObject("IO-" + i + "-0", i + 15 + size, new DateTime());
                odb.Store(io1);
            }
            odb.Close();
            Println("Time of insert " + size + " objects = " + size);
            var indexes = new[]
                {
                    "IO-0-0", "IO-10-0", "IO-20-0", "IO-30-0", "IO-40-0", "IO-50-0", "IO-60-0", "IO-70-0", "IO-80-0",
                    "IO-90-0"
                };
            long t1 = 0;
            long t2 = 0;
            long t3 = 0;
            long t4 = 0;
            long t5 = 0;
            long t6 = 0;
            for (var i = 0; i < nbUpdates; i++)
            {
                start = OdbTime.GetCurrentTimeInMs();
                for (var j = 0; j < nbObjects; j++)
                {
                    t1 = OdbTime.GetCurrentTimeInMs();
                    odb = Open(baseName);
                    t2 = OdbTime.GetCurrentTimeInMs();
                    var q = odb.Query<IndexedObject>();
                    q.Descend("name").Constrain((object) indexes[j]).Equal();
                    var os = q.Execute<IndexedObject>();
                    t3 = OdbTime.GetCurrentTimeInMs();
                    AssertTrue(((IInternalQuery)q).GetExecutionPlan().UseIndex());
                    AssertEquals(1, os.Count);
                    // check if index has been used
                    AssertTrue(((IInternalQuery)q).GetExecutionPlan().UseIndex());
                    var io = os.GetFirst();
                    if (i > 0)
                        AssertTrue(io.GetName().EndsWith(("-" + (i - 1))));
                    io.SetName(io.GetName() + "-updated-" + i);
                    odb.Store(io);
                    t4 = OdbTime.GetCurrentTimeInMs();
                    
                    indexes[j] = io.GetName();
                    AssertEquals(new Decimal(size), odb.Query<IndexedObject>().Count());
                    t5 = OdbTime.GetCurrentTimeInMs();
                    odb.Commit();
                    odb.Close();
                    t6 = OdbTime.GetCurrentTimeInMs();
                }
                var end = OdbTime.GetCurrentTimeInMs();
                Console.Out.WriteLine("Nb Updates of " + nbObjects + " =" + i + " - " + (end - start) + "ms  -- open=" +
                                      (t2 - t1) + " - getObjects=" + (t3 - t2) + " - update=" + (t4 - t3) + " - count=" +
                                      (t5 - t4) + " - close=" + (t6 - t5));
            }
        }
    }
}
