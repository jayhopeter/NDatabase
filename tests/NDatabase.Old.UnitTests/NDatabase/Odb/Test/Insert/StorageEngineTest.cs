using System;
using System.Collections.Generic;
using NDatabase.Meta.Introspector;
using NUnit.Framework;
using Test.NDatabase.Odb.Test.VO.Attribute;
using Test.NDatabase.Odb.Test.VO.Login;

namespace Test.NDatabase.Odb.Test.Insert
{
    [TestFixture]
    public class StorageEngineTest : ODBTest
    {
        [Test]
        public virtual void TestComplexInstance()
        {
            DeleteBase("t-complex-instance.ndb");
            var odb = Open("t-complex-instance.ndb");
            var login = new VO.Login.Function("login");
            var logout = new VO.Login.Function("logout");
            var functions = new List<VO.Login.Function>();
            functions.Add(login);
            functions.Add(logout);
            var profile = new Profile("profile1", functions);
            var user = new User("oliver", "user@ndatabase.net", profile);
            var user22 = new User("oliver2", "olivier2@ndatabase.net", profile);
            odb.Store(user);
            odb.Store(user22);
            odb.Close();
            odb = Open("t-complex-instance.ndb");
            var query = odb.Query<User>();
            var l = query.Execute<User>(true);
            var user2 = l.GetFirst();
            // println("#### " + l.size() + " : " + l);
            AssertEquals(user.GetName(), user2.GetName());
            AssertEquals(user.GetEmail(), user2.GetEmail());
            AssertEquals(user.GetProfile().GetName(), user2.GetProfile().GetName());
            AssertEquals(user.GetProfile().GetFunctions()[0].ToString(), user2.GetProfile().GetFunctions()[0].ToString());
            odb.Close();
            DeleteBase("t-complex-instance.ndb");
        }

        [Test]
        public virtual void TestNonNativeAttributes()
        {
            var tc = new TestClass();
            var classInfo =
                ClassIntrospector.Introspect(tc.GetType(), true).
                    GetMainClassInfo();
            AssertEquals(0, classInfo.GetAllNonNativeAttributes().Count);
        }

        [Test]
        public virtual void TestSimpleInstance()
        {
            DeleteBase("t-simple-instance.ndb");
            var odb = Open("t-simple-instance.ndb");
            var tc1 = new TestClass();
            tc1.SetBigDecimal1(new Decimal(1.123456));
            tc1.SetBoolean1(true);
            tc1.SetChar1('d');
            tc1.SetDouble1(154.78998989);
            tc1.SetInt1(78964);
            tc1.SetString1("Ola chico como vc est\u00E1 ???");
            tc1.SetDate1(DateTime.Now);
            tc1.SetBoolean2(false);
            var tc2 = new TestClass();
            tc2.SetBigDecimal1(new Decimal(1.1234565454));
            tc2.SetBoolean1(false);
            tc2.SetChar1('c');
            tc2.SetDouble1(78454.8779);
            tc2.SetInt1(1254);
            tc2.SetString1("Ola chico como ca va ???");
            tc2.SetDate1(DateTime.Now);
            tc2.SetBoolean2(true);
            odb.Store(tc1);
            odb.Store(tc2);
            odb.Close();
            odb = Open("t-simple-instance.ndb");
            var query = odb.Query<TestClass>();
            var l = query.Execute<TestClass>(true);
            var tc12 = l.GetFirst();
            // println("#### " + l.size() + " : " + l);
            AssertEquals(tc1.GetBigDecimal1(), tc12.GetBigDecimal1());
            AssertEquals(tc1.GetString1(), tc12.GetString1());
            AssertEquals(tc1.GetChar1(), tc12.GetChar1());
            AssertEquals(tc1.GetDouble1(), tc12.GetDouble1());
            AssertEquals(tc1.GetInt1(), tc12.GetInt1());
            AssertEquals(tc1.IsBoolean1(), tc12.IsBoolean1());
            AssertEquals(false, tc12.GetBoolean2());
            Console.WriteLine(" Date is " + tc12.GetDate1());
            if (l.Count < 3)
                AssertEquals(tc1.GetDate1(), tc12.GetDate1());
            l.Next();
            var tc22 = l.Next();
            AssertEquals(tc2.GetBigDecimal1(), tc22.GetBigDecimal1());
            AssertEquals(tc2.GetString1(), tc22.GetString1());
            AssertEquals(tc2.GetChar1(), tc22.GetChar1());
            AssertEquals(tc2.GetDouble1(), tc22.GetDouble1());
            AssertEquals(tc2.GetInt1(), tc22.GetInt1());
            AssertEquals(tc2.IsBoolean1(), tc22.IsBoolean1());
            AssertEquals(true, tc2.GetBoolean2());
            if (l.Count < 3)
                AssertEquals(tc2.GetDate1(), tc22.GetDate1());
            odb.Close();
        }
    }
}
