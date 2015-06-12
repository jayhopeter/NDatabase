using System;
using NUnit.Framework;
using Test.NDatabase.Odb.Test.VO.Login;

namespace Test.NDatabase.Odb.Test.Defragment
{
    [TestFixture]
    public class TestDefragment : ODBTest
    {
        /// <summary>
        ///   The name of the database file
        /// </summary>
        public static readonly string OdbFileName1 = "defrag3.ndb";

        public static readonly string OdbFileName2 = "defrag3-bis.ndb";

        [Test]
        public virtual void Test1()
        {
            DeleteBase(OdbFileName1);
            DeleteBase(OdbFileName2);
            var odb = Open(OdbFileName1);
            var user = new User("olivier", "user@ndatabase.net", null);
            odb.Store(user);
            odb.Close();

            odb = Open(OdbFileName1);
            odb.DefragmentTo(OdbFileName2);
            var newOdb = Open(OdbFileName2);

            Decimal nbUser = odb.Query<User>().Count();
            Decimal nbNewUser = odb.Query<User>().Count();
            AssertEquals(nbUser, nbNewUser);
            AssertEquals(odb.Query<Profile>().Count(),
                         odb.Query<Profile>().Count());
            odb.Close();
            newOdb.Close();
            DeleteBase(OdbFileName1);
            DeleteBase(OdbFileName2);
        }

        [Test]
        public virtual void Test2()
        {
            DeleteBase(OdbFileName1);
            DeleteBase(OdbFileName2);
            var odb = Open(OdbFileName1);
            var p = new Profile("profile");
            for (var i = 0; i < 500; i++)
            {
                var user = new User("olivier " + i, "user@ndatabase.net " + i, p);
                odb.Store(user);
            }
            odb.Close();
            odb = Open(OdbFileName1);
            odb.DefragmentTo(OdbFileName2);
            var newOdb = Open(OdbFileName2);
            AssertEquals(odb.Query<User>().Count(), odb.Query<User>().Count());
            AssertEquals(odb.Query<Profile>().Count(),
                         odb.Query<Profile>().Count());
            odb.Close();
            newOdb.Close();
            DeleteBase(OdbFileName1);
            DeleteBase(OdbFileName2);
        }

        [Test]
        public virtual void Test3()
        {
            DeleteBase(OdbFileName1);
            DeleteBase(OdbFileName2);
            
            var odb = Open(OdbFileName1);
            for (var i = 0; i < 1500; i++)
            {
                var user = new User("olivier " + i, "user@ndatabase.net " + i, new Profile("profile" + i));
                odb.Store(user);
            }
            odb.Close();
            odb = Open(OdbFileName1);
            odb.DefragmentTo(OdbFileName2);
            var newOdb = Open(OdbFileName2);
            AssertEquals(odb.Query<User>().Count(), odb.Query<User>().Count());
            odb.Close();
            newOdb.Close();
            DeleteBase(OdbFileName1);
            DeleteBase(OdbFileName2);
        }
    }
}
