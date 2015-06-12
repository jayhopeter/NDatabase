using System;
using NDatabase.Api;
using NUnit.Framework;
using Test.NDatabase.Odb.Test.VO.Login;

namespace Test.NDatabase.Odb.Test.Acid
{
    [TestFixture]
    public class TestStopEngineWithoutCommit : ODBTest
    {
        private bool simpleObject;

        private readonly ODBTest test = new ODBTest();

        [Test]
        public virtual void T1estA1()
        {
            test.DeleteBase("acid1");

            using (var odb = test.Open("acid1"))
                odb.Store(GetInstance("f1"));

            using (var odb = test.Open("acid1"))
            {
                var query = odb.Query<VO.Login.Function>();
                AssertEquals(2, query.Execute<VO.Login.Function>().Count);
            }
        }

        [Test]
        public virtual void T1estB1()
        {
            test.DeleteBase("acid1");

            using (var odb = test.Open("acid1"))
            {
                odb.Store(GetInstance("f1"));
                odb.Commit();
            }

            int size;

            using (var odb = test.Open("acid1"))
            {
                var query = odb.Query<User>();
                size = query.Execute<User>().Count;
            }

            AssertEquals(1, size);
        }

        [Test]
        public virtual void T1estC1()
        {
            test.DeleteBase("acid1");

            simpleObject = true;

            using (var odb = test.Open("acid1"))
            {
                var size = 5;
                var oids = new OID[size];
                for (var i = 0; i < size; i++)
                    oids[i] = odb.Store(GetInstance("f" + i));
                for (var i = 0; i < size; i++)
                    odb.DeleteObjectWithId(oids[i]);
            }

            using (var odb = test.Open("acid1"))
            {
                var query = odb.Query<VO.Login.Function>();
                AssertEquals(0, query.Execute<VO.Login.Function>().Count);
            }
        }

        [Test]
        public virtual void T1estD1()
        {
            test.DeleteBase("acid1");

            simpleObject = true;

            using (var odb = test.Open("acid1"))
            {
                var size = 1000;
                var oids = new OID[size];
                for (var i = 0; i < size; i++)
                    oids[i] = odb.Store(GetInstance("f" + i));
                for (var i = 0; i < size; i++)
                    odb.DeleteObjectWithId(oids[i]);
            }

            using (var odb = test.Open("acid1"))
            {
                var query = odb.Query<VO.Login.Function>();
                AssertEquals(0, query.Execute<VO.Login.Function>().Count);
            }
        }

        [Test]
        public virtual void T1estE1()
        {
            test.DeleteBase("acid1");
            using (var odb = test.Open("acid1"))
            {
                var size = 1000;
                var oids = new OID[size];
                for (var i = 0; i < size; i++)
                {
                    oids[i] = odb.Store(GetInstance("f" + i));
                    if (simpleObject)
                    {
                        var f = (VO.Login.Function)odb.GetObjectFromId(oids[i]);
                        f.SetName("function " + i);
                        odb.Store(f);
                    }
                    else
                    {
                        var f = (User)odb.GetObjectFromId(oids[i]);
                        f.SetName("function " + i);
                        odb.Store(f);
                    }
                    odb.DeleteObjectWithId(oids[i]);
                }
            }

            using (var odb = test.Open("acid1"))
            {
                var query = odb.Query<VO.Login.Function>();
                AssertEquals(0, query.Execute<VO.Login.Function>().Count);
            }
        }

        [Test]
        public virtual void T1estF1()
        {
            test.DeleteBase("acid1");
            using (var odb = test.Open("acid1"))
            {
                var size = 1000;
                var oids = new OID[size];
                for (var i = 0; i < size; i++)
                {
                    oids[i] = odb.Store(GetInstance("f" + i));
                    if (simpleObject)
                    {
                        var f = (VO.Login.Function)odb.GetObjectFromId(oids[i]);
                        f.SetName("function " + i);
                        odb.Store(f);
                        odb.Store(f);
                        odb.Store(f);
                        odb.Store(f);
                    }
                    else
                    {
                        var f = (User)odb.GetObjectFromId(oids[i]);
                        f.SetName("function " + i);
                        odb.Store(f);
                        odb.Store(f);
                        odb.Store(f);
                        odb.Store(f);
                    }
                }
                for (var i = 0; i < size; i++)
                {
                    var o = odb.GetObjectFromId(oids[i]);
                    odb.Delete(o);
                }
            }

            using (var odb = test.Open("acid1"))
            {
                if (simpleObject)
                {
                    var query = odb.Query<VO.Login.Function>();
                    AssertEquals(0, query.Execute<VO.Login.Function>().Count);
                }
                else
                {
                    var query1 = odb.Query<User>();
                    AssertEquals(0, query1.Execute<User>().Count);
                }
            }
        }

        [Test]
        public virtual void T1estG1()
        {
            test.DeleteBase("acid1");

            simpleObject = false;

            using (var odb = test.Open("acid1"))
            {
                var size = 1000;
                var oids = new OID[size];
                for (var i = 0; i < size; i++)
                {
                    oids[i] = odb.Store(GetInstance("f" + i));

                    var f = (User)odb.GetObjectFromId(oids[i]);
                    f.SetName("function " + i);
                    odb.Store(f);
                    odb.Store(f);
                    odb.Store(f);
                    odb.Store(f);

                }
                odb.Commit();
            }

            using (var odb = test.Open("acid1"))
            {
                var query = odb.Query<User>();
                AssertEquals(1000, query.Execute<User>().Count);
            }

            using (var odb = test.Open("acid1"))
            {
                var size = 1000;
                var oids = new OID[size];
                for (var i = 0; i < size; i++)
                {
                    oids[i] = odb.Store(GetInstance("f" + i));
                    var f = (User) odb.GetObjectFromId(oids[i]);
                    f.SetName("function " + i);
                    odb.Store(f);
                    odb.Store(f);
                    odb.Store(f);
                    odb.Store(f);
                }
                for (var i = 0; i < size; i++)
                {
                    object o = null;
                    o = odb.GetObjectFromId(oids[i]);
                    odb.Delete(o);
                }
            }

            using (var odb = test.Open("acid1"))
            {
                var query = odb.Query<User>();
                AssertEquals(1000, query.Execute<User>().Count);
            }
        }

        [Test]
        public virtual void T1estH1()
        {
            test.DeleteBase("acid1");
            using (var odb = test.Open("acid1"))
            {
                var size = 1000;
                var oids = new OID[size];
                for (var i = 0; i < size; i++)
                {
                    oids[i] = odb.Store(GetInstance("f" + i));
                    if (simpleObject)
                    {
                        var f = (VO.Login.Function)odb.GetObjectFromId(oids[i]);
                        f.SetName("function " + i);
                        odb.Store(f);
                        odb.Delete(f);
                        oids[i] = odb.Store(f);
                        odb.Delete(f);
                        oids[i] = odb.Store(f);
                        odb.Delete(f);
                        oids[i] = odb.Store(f);
                    }
                    else
                    {
                        var f = (User)odb.GetObjectFromId(oids[i]);
                        f.SetName("function " + i);
                        odb.Store(f);
                        odb.Delete(f);
                        oids[i] = odb.Store(f);
                        odb.Delete(f);
                        oids[i] = odb.Store(f);
                        odb.Delete(f);
                        oids[i] = odb.Store(f);
                    }
                }
                for (var i = 0; i < size; i++)
                {
                    var o = odb.GetObjectFromId(oids[i]);
                    odb.Delete(o);
                }
            }

            using (var odb = test.Open("acid1"))
            {
                if (simpleObject)
                {
                    var query = odb.Query<VO.Login.Function>();
                    AssertEquals(0, query.Execute<VO.Login.Function>().Count);
                }
                else
                {
                    var query1 = odb.Query<User>();
                    AssertEquals(0, query1.Execute<User>().Count);
                }
            }
        }

        [Test]
        public virtual void T1estI1()
        {
            test.DeleteBase("acid1");
            using (var odb = test.Open("acid1"))
            {
                odb.Store(GetInstance("f1"));
                odb.Store(GetInstance("f2"));
                odb.Store(GetInstance("f3"));
            }

            using (var odb = test.Open("acid1"))
            {
                var o = GetInstance("f4");
                odb.Store(o);
                odb.Delete(o);
            }

            using (var odb = test.Open("acid1"))
            {
                if (simpleObject)
                {
                    var query = odb.Query<VO.Login.Function>();
                    AssertEquals(3, query.Execute<VO.Login.Function>().Count);
                }
                else
                {
                    var query1 = odb.Query<User>();
                    AssertEquals(3, query1.Execute<User>().Count);
                }
            }

        }

        [Test]
        public virtual void T1estJ1()
        {
            test.DeleteBase("acid1");
            using (var odb = test.Open("acid1"))
            {
                odb.Store(GetInstance("f1"));
                odb.Store(GetInstance("f2"));
                odb.Store(GetInstance("f3"));
                odb.Commit();
                var o = GetInstance("f4");
                odb.Store(o);
                odb.Delete(o);
            }

            using (var odb = test.Open("acid1"))
            {
                if (simpleObject)
                {
                    var query = odb.Query<VO.Login.Function>();
                    AssertEquals(3, query.Execute<VO.Login.Function>().Count);
                }
                else
                {
                    var query1 = odb.Query<User>();
                    AssertEquals(3, query1.Execute<User>().Count);
                }
            }
        }

        [Test]
        public virtual void T1estK1()
        {
            test.DeleteBase("acid1");
            using (var odb = test.Open("acid1"))
            {
                odb.Store(GetInstance("f1"));
                odb.Store(GetInstance("f2"));
                var oid = odb.Store(GetInstance("f3"));
                odb.Commit();
                var o = odb.GetObjectFromId(oid);
                odb.Delete(o);
                odb.Rollback();
            }

            using (var odb = test.Open("acid1"))
            {
                if (simpleObject)
                {
                    var query = odb.Query<VO.Login.Function>();
                    AssertEquals(3, query.Execute<VO.Login.Function>().Count);
                }
                else
                {
                    var query1 = odb.Query<User>();
                    AssertEquals(3, query1.Execute<User>().Count);
                }
            }
        }

        [Test]
        public virtual void T1estL1()
        {
            test.DeleteBase("acid1");
            using (var odb = test.Open("acid1"))
            {
                odb.Store(GetInstance("f1"));
                odb.Store(GetInstance("f2"));
                var oid = odb.Store(GetInstance("f3"));
                odb.Commit();
                var o = odb.GetObjectFromId(oid);
                if (simpleObject)
                {
                    var f = (VO.Login.Function)o;
                    f.SetName("flksjdfjs;dfsljflsjflksjfksjfklsdjfksjfkalsjfklsdjflskd");
                    odb.Store(f);
                }
                else
                {
                    var f = (User)o;
                    f.SetName("flksjdfjs;dfsljflsjflksjfksjfklsdjfksjfkalsjfklsdjflskd");
                    odb.Store(f);
                }
                odb.Rollback();
            }

            using (var odb = test.Open("acid1"))
            {
                if (simpleObject)
                {
                    var query = odb.Query<VO.Login.Function>();
                    AssertEquals(3, query.Execute<VO.Login.Function>().Count);
                }
                else
                {
                    var query1 = odb.Query<User>();
                    AssertEquals(3, query1.Execute<User>().Count);
                }
            }
        }

        [Test]
        public virtual void T1estM1()
        {
            test.DeleteBase("acid1");
            using (var odb = test.Open("acid1"))
            {
                var size = 1;
                var oids = new OID[size];
                for (var i = 0; i < size; i++)
                    oids[i] = odb.Store(GetInstance("f" + i));
                for (var i = 0; i < size; i++)
                    odb.DeleteObjectWithId(oids[i]);
                odb.Rollback();
            }

            using (var odb = test.Open("acid1"))
            {
                if (simpleObject)
                {
                    var query = odb.Query<VO.Login.Function>();
                    AssertEquals(0, query.Execute<VO.Login.Function>().Count);
                }
                else
                {
                    var query1 = odb.Query<User>();
                    AssertEquals(0, query1.Execute<User>().Count);
                }
            }
        }

        [Test]
        public virtual void T1estN1()
        {
            test.DeleteBase("acid1");
            using (var odb = test.Open("acid1"))
            {
                for (var i = 0; i < 10; i++)
                    odb.Store(GetInstance("f" + i));
            }

            using (var odb = test.Open("acid1"))
            {
                odb.Store(GetInstance("f1000"));
                odb.Commit();
            }
        }

        [Test]
        public virtual void T1estN2()
        {
            using (var odb = test.Open("acid1"))
            {
                if (simpleObject)
                {
                    var query = odb.Query<VO.Login.Function>();
                    query.Descend("name").Constrain((object) "f1000").Equal();
                    var objects = query.Execute<VO.Login.Function>();

                    var f = objects.GetFirst();
                    f.SetName("new name");
                    odb.Store(f);
                }
                else
                {
                    var query = odb.Query<User>();
                    query.Descend("name").Constrain((object) "f1000").Equal();
                    var objects = query.Execute<User>();
                    var f = objects.GetFirst();
                    f.SetName("new name");
                    odb.Store(f);
                }
                odb.Commit();
            }
        }

        [Test]
        public virtual void T1estN3()
        {
            using (var odb = test.Open("acid1"))
            {
                if (simpleObject)
                {
                    var query = odb.Query<VO.Login.Function>();
                    query.Descend("name").Constrain((object) "new name").Equal();
                    var objects = query.Execute<VO.Login.Function>();
                    odb.Delete(objects.GetFirst());
                }
                else
                {
                    var query = odb.Query<User>();
                    query.Descend("name").Constrain((object) "new name").Equal();
                    var objects = query.Execute<User>();
                    odb.Delete(objects.GetFirst());
                }
                odb.Commit();
            }
        }

        [Test]
        public virtual void T1estN4()
        {
            int nb;
            using (var odb = test.Open("acid1"))
            {
                nb = 0;
                if (simpleObject)
                {
                    var query = odb.Query<VO.Login.Function>();
                    query.Descend("name").Constrain((object) "f1000").Equal();
                    var objects = query.Execute<VO.Login.Function>();
                    nb = objects.Count;
                }
                else
                {
                    var query = odb.Query<User>();
                    query.Descend("name").Constrain((object) "f1000").Equal();
                    var objects = query.Execute<User>();
                    nb = objects.Count;
                }
            }
            if (nb != 0)
                throw new Exception("Object f1000 still exist :-(");
        }

        private object GetInstance(string @string)
        {
            if (simpleObject)
                return new VO.Login.Function(@string);

            var p = new Profile(@string);
            p.AddFunction(new VO.Login.Function("function " + @string + "1"));
            p.AddFunction(new VO.Login.Function("function " + @string + "2"));

            return new User(@string, "email" + @string, p);
        }
    }
}
