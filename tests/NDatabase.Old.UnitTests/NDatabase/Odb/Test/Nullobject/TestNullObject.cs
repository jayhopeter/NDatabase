using System;
using NUnit.Framework;
using Test.NDatabase.Odb.Test.VO.Login;

namespace Test.NDatabase.Odb.Test.Nullobject
{
    [TestFixture]
    public class TestNullObject : ODBTest
    {
        [Test]
        public virtual void Test1()
        {
            DeleteBase("null.ndb");
            var odb = Open("null.ndb");
            var user1 = new User("oli", "oli@sdsadf", null);
            var user2 = new User("karine", "karine@sdsadf", null);
            var user3 = new User(null, null, null);
            odb.Store(user1);
            odb.Store(user2);
            odb.Store(user3);
            odb.Close();
            odb = Open("null.ndb");
            var query = odb.Query<User>();
            var l = query.Execute<User>(true);
            AssertEquals(3, l.Count);
            user1 = l.Next();
            AssertEquals("oli", user1.GetName());
            AssertEquals("oli@sdsadf", user1.GetEmail());
            AssertEquals(null, user1.GetProfile());
            user2 = l.Next();
            AssertEquals("karine", user2.GetName());
            AssertEquals("karine@sdsadf", user2.GetEmail());
            AssertEquals(null, user2.GetProfile());
            user3 = l.Next();
            AssertEquals(null, user3.GetName());
            AssertEquals(null, user3.GetEmail());
            AssertEquals(null, user3.GetProfile());
            odb.Close();
            DeleteBase("null.ndb");
        }

        /// <summary>
        ///   Test generic attribute of type Object receving a native type
        /// </summary>
        /// <exception cref="System.Exception">System.Exception</exception>
        [Test]
        public virtual void Test2()
        {
            DeleteBase("nullo");
            var gc = new GenericClass(null);
            var odb = Open("nullo");
            odb.Store(gc);
            odb.Close();
            odb = Open("nullo");
            var query = odb.Query<GenericClass>();
            var objects = query.Execute<GenericClass>();
            var gc2 = objects.GetFirst();
            gc2.SetObject("Ola");
            odb.Store(gc2);
            odb.Close();
            odb = Open("nullo");
            var query1 = odb.Query<GenericClass>();
            objects = query1.Execute<GenericClass>();
            AssertEquals(1, objects.Count);
            var gc3 = objects.GetFirst();
            AssertEquals("Ola", gc3.GetObject());
            odb.Close();
        }

        [Test]
        public virtual void Test21()
        {
            DeleteBase("nullo");
            var gc = new GenericClass(null);
            var odb = Open("nullo");
            odb.Store(gc);
            odb.Close();
            odb = Open("nullo");
            var query = odb.Query<GenericClass>();
            var objects = query.Execute<GenericClass>();
            var gc2 = objects.GetFirst();
            object[] longs = {1, 2};
            gc2.SetObjects(longs);
            odb.Store(gc2);
            odb.Close();
            odb = Open("nullo");
            var query1 = odb.Query<GenericClass>();
            objects = query1.Execute<GenericClass>();
            AssertEquals(1, objects.Count);
            var gc3 = objects.GetFirst();
            var longs2 = gc3.GetObjects();
            AssertEquals(2, longs2.Length);
            AssertEquals(Convert.ToInt64(1), longs2[0]);
            AssertEquals(Convert.ToInt64(2), longs2[1]);
            odb.Close();
        }

        [Test]
        public virtual void Test22()
        {
            DeleteBase("nullo");
            var gc = new GenericClass(null);
            var odb = Open("nullo");
            odb.Store(gc);
            odb.Close();
            odb = Open("nullo");
            var query = odb.Query<GenericClass>();
            var objects = query.Execute<GenericClass>();
            var gc2 = objects.GetFirst();
            gc2.GetObjects()[0] = Convert.ToInt64(1);
            gc2.GetObjects()[1] = Convert.ToInt64(2);
            odb.Store(gc2);
            odb.Close();
            odb = Open("nullo");
            var query1 = odb.Query<GenericClass>();
            objects = query1.Execute<GenericClass>();
            AssertEquals(1, objects.Count);
            var gc3 = objects.GetFirst();
            var longs2 = gc3.GetObjects();
            AssertEquals(10, longs2.Length);
            AssertEquals(Convert.ToInt64(1), longs2[0]);
            AssertEquals(Convert.ToInt64(2), longs2[1]);
            odb.Close();
        }

        [Test]
        public virtual void Test23()
        {
            DeleteBase("nullo");
            var gc = new GenericClass(null);
            gc.GetObjects()[0] = new VO.Login.Function("f1");
            var odb = Open("nullo");
            odb.Store(gc);
            odb.Close();
            odb = Open("nullo");
            var query = odb.Query<GenericClass>();
            var objects = query.Execute<GenericClass>();
            var gc2 = objects.GetFirst();
            gc2.GetObjects()[0] = Convert.ToInt64(1);
            gc2.GetObjects()[1] = Convert.ToInt64(2);
            odb.Store(gc2);
            odb.Close();
            odb = Open("nullo");
            var query1 = odb.Query<GenericClass>();
            objects = query1.Execute<GenericClass>();
            AssertEquals(1, objects.Count);
            var gc3 = objects.GetFirst();
            var longs2 = gc3.GetObjects();
            AssertEquals(10, longs2.Length);
            AssertEquals(Convert.ToInt64(1), longs2[0]);
            AssertEquals(Convert.ToInt64(2), longs2[1]);
            odb.Close();
        }

        [Test]
        public virtual void Test3()
        {
            DeleteBase("nullo");
            var gc = new GenericClass(null);
            var strings = new[] {"OBJ1", "obj2"};
            gc.SetObjects(strings);
            var odb = Open("nullo");
            odb.Store(gc);
            odb.Close();
            odb = Open("nullo");
            var query = odb.Query<GenericClass>();
            var objects = query.Execute<GenericClass>();
            var gc2 = objects.GetFirst();
            gc2.SetObject("Ola");
            odb.Store(gc2);
            odb.Close();
        }

        [Test]
        public virtual void Test4()
        {
            DeleteBase("nullo");
            var gc = new GenericClass(null);
            var strings = new[] {"OBJ1", "obj2"};
            gc.SetObject(strings);
            var odb = Open("nullo");
            odb.Store(gc);
            odb.Close();
            odb = Open("nullo");
            var query = odb.Query<GenericClass>();
            var objects = query.Execute<GenericClass>();
            var gc2 = objects.GetFirst();
            gc2.SetObject("Ola");
            odb.Store(gc2);
            odb.Close();
        }

        [Test]
        public virtual void Test5()
        {
            DeleteBase("nullo");
            var f = new VO.Login.Function("a simple value");
            var odb = Open("nullo");
            odb.Store(f);
            odb.Close();
            odb = Open("nullo");
            var query = odb.Query<VO.Login.Function>();
            var objects = query.Execute<VO.Login.Function>();
            var f2 = objects.GetFirst();
            f2.SetName(null);
            odb.Store(f2);
            odb.Close();
            odb = Open("nullo");
            var query1 = odb.Query<VO.Login.Function>();
            objects = query1.Execute<VO.Login.Function>();
            f2 = objects.GetFirst();
            odb.Close();
            AssertEquals(null, f2.GetName());
        }
    }
}
