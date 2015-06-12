using System;
using System.Collections.Generic;
using NUnit.Framework;
using Test.NDatabase.Odb.Test.VO.Attribute;
using Test.NDatabase.Odb.Test.VO.Login;

namespace Test.NDatabase.Odb.Test.Acid
{
    [TestFixture]
    public class TestConsistency : ODBTest
    {
        public static string OdbFileName = "consistency.ndb";

        public virtual void CreateInconsistentFile()
        {
            var odb = Open(OdbFileName);
            for (var i = 0; i < 10; i++)
            {
                var o = GetUserInstance();
                odb.Store(o);
            }
            odb.Close();
            odb = Open(OdbFileName);
            for (var i = 0; i < 10; i++)
            {
                var o = GetUserInstance();
                odb.Store(o);
            }
        }

        private TestClass GetTestClassInstance()
        {
            var tc = new TestClass();
            tc.SetBigDecimal1(new Decimal(1.123456789));
            tc.SetBoolean1(true);
            tc.SetChar1('d');
            tc.SetDouble1(154.78998989);
            tc.SetInt1(78964);
            tc.SetString1("Ola chico como vc est√° ???");
            tc.SetDate1(new DateTime());
            return tc;
        }

        private object GetUserInstance()
        {
            var login = new VO.Login.Function("login");
            var logout = new VO.Login.Function("logout");
            IList<VO.Login.Function> list = new List<VO.Login.Function>();
            list.Add(login);
            list.Add(logout);
            var profile = new Profile("operator", list);
            var user = new User("olivier smadja", "user@ndatabase.net", profile);
            return user;
        }

        public static void Main2(string[] args)
        {
            new TestConsistency().CreateInconsistentFile();
        }

        [Test]
        public virtual void Test1()
        {
            AssertTrue(true);
        }

        // new TestConsistency().openFile();
    }
}
