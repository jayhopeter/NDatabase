using System.Collections;
using System.Collections.Generic;
using NDatabase.Api;
using NDatabase.Tool.Wrappers;
using NUnit.Framework;

namespace Test.NDatabase.Odb.Test.Arraycollectionmap
{
    [TestFixture]
    public class TestMapContainingCollection : ODBTest
    {
        [Test]
        public virtual void Test1()
        {
            DeleteBase("map-with-collections");
            IOdb odb = null;
            odb = Open("map-with-collections");
            var o = new MyMapObject("test");
            IList<string> c = new List<string>();
            c.Add("ola");
            o.GetMap().Add("c", c);
            odb.Store(o);
            odb.Close();
            odb = Open("map-with-collections");
            var query = odb.Query<MyMapObject>();
            var os = query.Execute<MyMapObject>();
            var mmo = os.GetFirst();
            odb.Close();
            DeleteBase("map-with-collections");
            AssertEquals(o.GetName(), mmo.GetName());
            AssertEquals(o.GetMap().Count, mmo.GetMap().Count);
            AssertEquals(o.GetMap()["c"], mmo.GetMap()["c"]);
        }

        [Test]
        public virtual void Test2()
        {
            DeleteBase("map-with-collections");
            IOdb odb = null;
            odb = Open("map-with-collections");
            var o = new MyMapObject("test");
            IList<MyMapObject> c = new List<MyMapObject>();
            c.Add(o);
            o.GetMap().Add("c", c);
            odb.Store(o);
            odb.Close();
            odb = Open("map-with-collections");
            var query = odb.Query<MyMapObject>();
            var os = query.Execute<MyMapObject>();
            var mmo = os.GetFirst();
            odb.Close();
            DeleteBase("map-with-collections");
            AssertEquals(o.GetName(), mmo.GetName());
            AssertEquals(o.GetMap().Count, mmo.GetMap().Count);
            var c1 = (ICollection) o.GetMap()["c"];
            var c2 = (ICollection) mmo.GetMap()["c"];
            AssertEquals(c1.Count, c2.Count);
            var enumerator = c2.GetEnumerator();
            enumerator.MoveNext();
            AssertEquals(mmo, enumerator.Current);
        }

        [Test]
        public virtual void Test3()
        {
            // LogUtil.objectReaderOn(true);
            DeleteBase("map-with-collections");
            IOdb odb = null;
            odb = Open("map-with-collections");
            var o = new MyMapObject("test");
            IList<MyMapObject> c = new List<MyMapObject>();
            c.Add(o);
            var f1 = new VO.Login.Function("function1");
            o.GetMap().Add("a", c);
            var size = 1;
            for (var i = 0; i < size; i++)
                o.GetMap().Add("A" + i, f1);
            o.GetMap().Add("c", f1);
            Println("RealMap" + o.GetMap());
            odb.Store(o);
            odb.Close();
            odb = Open("map-with-collections");
            var query = odb.Query<MyMapObject>();
            var os = query.Execute<MyMapObject>();
            var mmo = os.GetFirst();
            odb.Close();
            DeleteBase("map-with-collections");
            AssertEquals(o.GetName(), mmo.GetName());
            AssertEquals(size + 2, mmo.GetMap().Count);

            var enumerator = ((ICollection) mmo.GetMap()["a"]).GetEnumerator();
            enumerator.MoveNext();
            AssertEquals(mmo, enumerator.Current);
            AssertEquals("function1", mmo.GetMap()["c"].ToString());
        }
    }

    internal class MyMapObject
    {
        private OdbHashMap<object, object> map;
        private string name;

        public MyMapObject(string name)
        {
            this.name = name;
            map = new OdbHashMap<object, object>();
        }

        public virtual string GetName()
        {
            return name;
        }

        public virtual void SetName(string name)
        {
            this.name = name;
        }

        public virtual OdbHashMap<object, object> GetMap()
        {
            return map;
        }

        public virtual void SetMap(OdbHashMap<object, object> map)
        {
            this.map = map;
        }
    }
}
