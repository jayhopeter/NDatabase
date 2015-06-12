using System;
using System.Threading;
using NDatabase.Api;
using NUnit.Framework;

namespace Test.NDatabase.Odb.Test.Query.Criteria
{
    [TestFixture]
    public class TestCriteriaQuery : ODBTest
    {
        public void SetUp(string baseName)
        {
            base.SetUp();
            DeleteBase(baseName);
            var odb = Open(baseName);
            for (var i = 0; i < 50; i++)
                odb.Store(new VO.Login.Function("function " + i));
            odb.Close();
        }

        public void TearDown(string baseName)
        {
            DeleteBase(baseName);
        }

        [Test]
        public virtual void Test1()
        {
            var baseName = GetBaseName();
            SetUp(baseName);
            var odb = Open(baseName);
            var aq =
                odb.Query<VO.Login.Function>();

            (aq.Descend("name").Constrain("function 2").Equal()).Or(aq.Descend("name").Constrain("function 3").Equal());

            var l = aq.Execute<VO.Login.Function>(true, -1, -1);
            AssertEquals(2, l.Count);
            var f = l.GetFirst();
            AssertEquals("function 2", f.GetName());
            Println(l);
            odb.Close();
            Console.ReadLine();
        }

        [Test]
        public virtual void Test2()
        {
            var baseName = GetBaseName();
            SetUp(baseName);
            var odb = Open(baseName);
            var aq = odb.Query<VO.Login.Function>();
            (aq.Descend("name").Constrain("function 2").Equal()).Not();
            var l = aq.Execute<VO.Login.Function>(true, -1, -1);
            AssertEquals(49, l.Count);
            var f = l.GetFirst();
            AssertEquals("function 0", f.GetName());
            odb.Close();
        }

        [Test]
        public virtual void Test3()
        {
            var baseName = GetBaseName();
            SetUp(baseName);
            var odb = Open(baseName);
            var aq =
                odb.Query<VO.Login.Function>();

            (aq.Descend("name").Constrain("function 2").Equal()).Or(aq.Descend("name").Constrain("function 3").Equal()).
                Not();

            var l = aq.Execute<VO.Login.Function>(true, -1, -1);
            AssertEquals(48, l.Count);
            var f = l.GetFirst();
            AssertEquals("function 0", f.GetName());
            odb.Close();
        }

        [Test]
        public virtual void Test4Sort()
        {
            var baseName = GetBaseName();
            SetUp(baseName);
            var d = OdbConfiguration.GetIndexBTreeDegree();
            try
            {
                OdbConfiguration.SetIndexBTreeDegree(40);
                var odb = Open(baseName);
                var aq =
                    odb.Query<VO.Login.Function>();

                (aq.Descend("name").Constrain("function 2").Equal()).Or(
                    aq.Descend("name").Constrain("function 3").Equal()).Not();

                aq.Descend("name").OrderDescending();

                var l = aq.Execute<VO.Login.Function>(true, -1, -1);
                odb.Close();

                AssertEquals(48, l.Count);
                var f = l.GetFirst();
                AssertEquals("function 9", f.GetName());
            }
            finally
            {
                OdbConfiguration.SetIndexBTreeDegree(d);
            }
        }

        [Test]
        public virtual void TestDate1()
        {
            var baseName = GetBaseName();
            SetUp(baseName);
            var odb = Open(baseName);
            var myDates = new MyDates();
            var d1 = new DateTime();
            Thread.Sleep(100);

            var d2 = new DateTime();
            Thread.Sleep(100);
            var d3 = new DateTime();
            myDates.SetDate1(d1);
            myDates.SetDate2(d3);
            myDates.SetI(5);
            odb.Store(myDates);
            odb.Close();
            odb = Open(baseName);

            var query = odb.Query<MyDates>();
            query.Descend("date1").Constrain(d2).Smaller().Equal().And(
                query.Descend("date2").Constrain(d2).Greater().Equal()).And(
                    query.Descend("i").Constrain(5).Equal());
            var objects = query.Execute<MyDates>();
            AssertEquals(1, objects.Count);
            odb.Close();
        }

        [Test]
        public virtual void TestEqual2()
        {
            var baseName = GetBaseName();
            SetUp(baseName);
            var odb = Open(baseName);
            var aq = odb.Query<VO.Login.Function>();
            aq.Descend("name").Constrain("FuNcTiOn 1").Equal();
            aq.Descend("name").OrderDescending();
            var l = aq.Execute<VO.Login.Function>(true, -1, -1);
            AssertEquals(0, l.Count);
            odb.Close();
        }

        [Test]
        public virtual void TestILike()
        {
            var baseName = GetBaseName();
            SetUp(baseName);
            var odb = Open(baseName);
            var aq = odb.Query<VO.Login.Function>();
            aq.Descend("name").Constrain("FUNc%").InvariantLike();
            aq.Descend("name").OrderDescending();
            var l = aq.Execute<VO.Login.Function>(true, -1, -1);
            AssertEquals(50, l.Count);
            odb.Close();
        }

        [Test]
        public virtual void TestIequal()
        {
            var baseName = GetBaseName();
            SetUp(baseName);
            var odb = Open(baseName);
            var aq = odb.Query<VO.Login.Function>();
            aq.Descend("name").Constrain("FuNcTiOn 1").InvariantLike();
            aq.Descend("name").OrderDescending();
            var l = aq.Execute<VO.Login.Function>(true, -1, -1);
            AssertEquals(11, l.Count);
            odb.Close();
        }

        [Test]
        public virtual void TestLike()
        {
            var baseName = GetBaseName();
            SetUp(baseName);
            var odb = Open(baseName);
            var aq = odb.Query<VO.Login.Function>();
            aq.Descend("name").Constrain("func%").Like();
            aq.Descend("name").OrderDescending();
            var l = aq.Execute<VO.Login.Function>(true, -1, -1);
            AssertEquals(50, l.Count);
            odb.Close();
        }

        [Test]
        public virtual void TestLike2()
        {
            var baseName = GetBaseName();
            SetUp(baseName);
            var odb = Open(baseName);
            var aq = odb.Query<VO.Login.Function>();
            aq.Descend("name").Constrain("FuNc%").Like();
            aq.Descend("name").OrderDescending();
            var l = aq.Execute<VO.Login.Function>(true, -1, -1);
            AssertEquals(0, l.Count);
            odb.Close();
        }
    }
}