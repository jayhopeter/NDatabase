using System;
using NDatabase;
using NDatabase.Api;
using NDatabase.Oid;
using NDatabase.Tool.Wrappers;
using NUnit.Framework;
using Test.NDatabase.Tool;

namespace Test.NDatabase.Odb.Test.Oid
{
    [TestFixture]
    public class TestOid : ODBTest
    {
        [Test]
        public virtual void TestAndy1()
        {
            DeleteBase("test-oid");

            var odb = Open("test-oid");
            var b1 = new B("b");
            var a1 = new A("a", b1);
            odb.Store(a1);
            var oida = odb.GetObjectId(a1);
            var oidb = odb.GetObjectId(b1);
            odb.Close();
            odb = Open("test-oid");
            var a2 = (A) odb.GetObjectFromId(oida);
            var b2 = (B) odb.GetObjectFromId(oidb);
            odb.Close();
            AssertNotNull(a2);
            AssertNotNull(b2);
            AssertNotNull(a2.GetB());
        }

        [Test]
        public virtual void TestAndy2()
        {
            DeleteBase("test-oid");

            // LogUtil.allOn(true);
            var odb = Open("test-oid");
            var b1 = new B("b");
            var a1 = new A("a", b1);
            odb.Store(a1);
            var oida = (odb.GetObjectId(a1)).ObjectId;
            var oidb = (odb.GetObjectId(b1)).ObjectId;
            odb.Close();
            odb = Open("test-oid");
            var a2 = (A) odb.GetObjectFromId(new ObjectOID(oida));
            var b2 = (B) odb.GetObjectFromId(new ObjectOID(oidb));
            odb.Close();
            AssertNotNull(a2);
            AssertNotNull(b2);
            AssertNotNull(a2.GetB());
        }

        [Test]
        public virtual void TestAndy3()
        {
            DeleteBase("test-oid");

            var odb = Open("test-oid");
            try
            {
                var a2 = (A) odb.GetObjectFromId(new ObjectOID(34));
                Fail("Should have thrown Exception");
            }
            catch (Exception)
            {
            }
        }

        [Test]
        public virtual void TestEquals()
        {
            var oid1 = OIDFactory.BuildObjectOID(1);
            var oid2 = OIDFactory.BuildObjectOID(1);
            AssertEquals(oid1, oid2);
        }

        [Test]
        public virtual void TestOIdInMap()
        {
            var oid1 = OIDFactory.BuildObjectOID(1);
            var oid2 = OIDFactory.BuildObjectOID(1);
            var map = new OdbHashMap<OID, string>();
            map.Add(oid1, "oid1");
            AssertNotNull(map[oid2]);
        }

        /// <summary>
        ///   Performance test.
        /// </summary>
        /// <remarks>
        ///   Performance test. Using ID or long in hash map
        /// </remarks>
        [Test]
        public virtual void TestPerformanceOid()
        {
            var size = 300000;
            var oidMap = new OdbHashMap<OID, string>();

            // OID
            var timeOidMapCreation = new StopWatch();
            timeOidMapCreation.Start();
            // Creates a hashmap with 100000 Longs
            for (var i = 0; i < size; i++)
                oidMap.Add(OIDFactory.BuildObjectOID(i), i.ToString());
            timeOidMapCreation.End();
            var timeOidMapGet = new StopWatch();
            timeOidMapGet.Start();
            // get all map elements
            for (var i = 0; i < size; i++)
                AssertNotNull(oidMap[OIDFactory.BuildObjectOID(i)]);
            timeOidMapGet.End();
            Println(size + " objects : OID Map creation=" + timeOidMapCreation.GetDurationInMiliseconds() + " - get=" +
                    timeOidMapGet.GetDurationInMiliseconds());
        }

        // ok must enter the catch block
    }
}
