using System;
using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;

namespace Test.NDatabase.Odb.Test.Performance
{
    [TestFixture]
    public class TestWeakReference : ODBTest
    {
        private SimpleObject GetSimpleObjectInstance(int i)
        {
            var so = new SimpleObject();
            so.SetDate(new DateTime());
            so.SetDuration(i);
            so.SetName("Bonjour, comment allez vous?" + i);
            return so;
        }

        [Test]
        public virtual void Test1()
        {
            var size = 200000;
            IDictionary map = new Dictionary<object, WeakReference>();
            IList l = new ArrayList();
            for (var i = 0; i < size; i++)
            {
                object o = GetSimpleObjectInstance(i);
                l.Add(o);
                if (i % 50000 == 0)
                    Println("i=" + i);
                map.Add(o, new WeakReference(o));
            }
            Println("Test 1 ok");
            Println("Map size " + map.Count);
        }

        [Test]
        public virtual void Test1WithoutWeak()
        {
            var size = 40000;
            IDictionary map = new Dictionary<object, object>();
            for (var i = 0; i < size; i++)
            {
                object o = GetSimpleObjectInstance(i);
                map.Add(o, o);
            }
            Println("Test 1 ok");
            Println("Map size " + map.Count);
        }

        [Test]
        public virtual void Test2()
        {
            var size = 20000;
            IDictionary map = new Dictionary<object, WeakReference>();
            for (var i = 0; i < size; i++)
            {
                object o = GetSimpleObjectInstance(i);
                if (i % 50000 == 0)
                    Println("i=" + i);
                map.Add(Convert.ToInt64(i), new WeakReference(o));
            }
            Println("Test 2 ok");
            Println("Map size " + map.Count);
        }
    }
}
