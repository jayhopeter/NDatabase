using System;
using NDatabase;
using NUnit.Framework;

namespace Test.NDatabase.Odb.Test.Cache
{
    [TestFixture]
    public class TestHashCode : ODBTest
    {
        /// <summary>
        ///   a problem reported by glsender - 1875544
        /// </summary>
        [Test]
        public virtual void Test1()
        {
            var baseName = GetBaseName();
            var odb = Open(baseName);
            MyObjectWithMyHashCode my = null;
            // creates 1000 objects
            for (var i = 0; i < 1000; i++)
            {
                my = new MyObjectWithMyHashCode(Convert.ToInt64(1000));
                odb.Store(my);
            }
            odb.Close();
            odb = Open(baseName);
            var query1 = odb.Query<MyObjectWithMyHashCode>();
            var objects = query1.Execute<MyObjectWithMyHashCode>();
            AssertEquals(1000, objects.Count);
            while (objects.HasNext())
            {
                my = objects.Next();
                odb.Delete(my);
            }
            odb.Close();
            odb = Open(baseName);
            var query = odb.Query<MyObjectWithMyHashCode>();
            objects = query.Execute<MyObjectWithMyHashCode>();
            odb.Close();
            OdbFactory.Delete(baseName);
            AssertEquals(0, objects.Count);
        }

        /// <summary>
        ///   a problem reported by glsender
        /// </summary>
        [Test]
        public virtual void Test2()
        {
            var baseName = GetBaseName();
            var odb = Open(baseName);
            MyObjectWithMyHashCode2 my = null;
            // creates 1000 objects
            for (var i = 0; i < 1000; i++)
            {
                my = new MyObjectWithMyHashCode2(Convert.ToInt64(1000));
                odb.Store(my);
            }
            odb.Close();
            odb = Open(baseName);
            var query1 = odb.Query<MyObjectWithMyHashCode2>();
            var objects = query1.Execute<MyObjectWithMyHashCode2>();
            AssertEquals(1000, objects.Count);
            while (objects.HasNext())
            {
                my = objects.Next();
                odb.Delete(my);
            }
            odb.Close();
            odb = Open(baseName);
            var query = odb.Query<MyObjectWithMyHashCode2>();
            objects = query.Execute<MyObjectWithMyHashCode2>();
            odb.Close();
            OdbFactory.Delete(baseName);
            AssertEquals(0, objects.Count);
        }
    }
}
