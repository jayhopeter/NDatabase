using System;
using System.Threading;
using NDatabase;
using NUnit.Framework;

namespace Test.NDatabase.Odb.Test.Ext
{
    [TestFixture]
    public class TestExt : ODBTest
    {
        [Test]
        public virtual void TestGetObjectId()
        {
            DeleteBase("extb");
            var odb = OdbFactory.Open("extb");
            var f = new VO.Login.Function("Test Function");
            var oid = odb.Store(f);
            var extOid = odb.Ext().GetObjectExternalOID(f);
            AssertEquals(oid.ObjectId, extOid.ObjectId);
            AssertEquals(odb.Ext().GetDatabaseId(), extOid.GetDatabaseId());
            odb.Close();
            odb = Open("extb");
            // Getting object via external oid
            var f2 = (VO.Login.Function) odb.GetObjectFromId(extOid);
            var lastOid = odb.GetObjectId(f2);
            AssertEquals(oid, lastOid);
            AssertEquals(f.GetName(), f2.GetName());
            odb.Close();
        }

        [Test]
        public virtual void TestObjectVersion()
        {
            DeleteBase("extc");
            var odb = Open("extc");
            var size = 100;
            long updateDate = 0;
            long creationDate = 0;
            var oid = odb.Store(new VO.Login.Function("f"));
            odb.Close();
            Thread.Sleep(20);
            // LogUtil.allOn(true);
            for (var i = 0; i < size; i++)
            {
                odb = Open("extc");
                var f = (VO.Login.Function) odb.GetObjectFromId(oid);
                var version = odb.Ext().GetObjectVersion(oid);
                Console.WriteLine("i="+i + " - v="+ version+ " - oid="+oid);
                updateDate = odb.Ext().GetObjectUpdateDate(oid);
                creationDate = odb.Ext().GetObjectCreationDate(oid);
                f.SetName(f.GetName() + "-" + i);
                // update the object, should increase the version number
                odb.Store(f);
                odb.Close();
                AssertEquals(i + 1, version);
                Console.WriteLine(creationDate + " - "+ updateDate+ "- "+ TimeSpan.FromTicks(DateTime.Now.Ticks).TotalMilliseconds);
                
                // in first iteration, creation & update date may be equal
                if (i > 0)
                    AssertTrue(creationDate < updateDate);
            }
        }

        [Test]
        public virtual void TestObjectVersionWithoutClose()
        {
            DeleteBase("extd");
            var odb = Open("extd");
            var size = 100;
            var oid = odb.Store(new VO.Login.Function("f"));
            odb.Close();
            odb = Open("extd");
            Thread.Sleep(20);
            for (var i = 0; i < size; i++)
            {
                // odb = open("ext");
                var f = (VO.Login.Function) odb.GetObjectFromId(oid);
                var version = odb.Ext().GetObjectVersion(oid);
                // System.out.println("i="+i + " - v="+ version+ " - oid="+oid);
                AssertEquals(i + 1, version);
                f.SetName("f" + i);
                // update the object, should increase the version number
                odb.Store(f);
                odb.Commit();
            }
            odb.Close();
        }

        [Test]
        public virtual void TestObjectVersionWithoutClose2()
        {
            DeleteBase("ext2");
            var odb = Open("ext2");
            var size = 100;

            var oid = odb.Store(new VO.Login.Function("f"));
            odb.Close();
            odb = Open("ext2");
            Thread.Sleep(20);
            // LogUtil.allOn(true);
            for (var i = 0; i < size; i++)
            {
                // odb = open("ext");
                var f = (VO.Login.Function) odb.GetObjectFromId(oid);
                f.SetName("f" + i);
                odb.Store(f);
                odb.Commit();
            }
            odb.Close();
        }
    }
}
