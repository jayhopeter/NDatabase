using NUnit.Framework;
using Test.NDatabase.Odb.Test.VO.Login;

namespace Test.NDatabase.Odb.Test.Update
{
    [TestFixture]
    internal class TestUpdate2 : ODBTest
    {
        public static readonly string Name = "update.db";

        public static readonly string Name2 = "update2.db";

        public const int Size = 50;

        [Test]
        public virtual void Test10()
        {
            var updateTransaction = "update-transaction.ndb";
            DeleteBase(updateTransaction);
            var odb = Open(updateTransaction);
            var oid = odb.Store(new VO.Login.Function("function1"));
            odb.Close();
            odb = Open(updateTransaction);
            var f = (VO.Login.Function) odb.GetObjectFromId(oid);
            f.SetName("function2");
            odb.Store(f);
            odb.Rollback();
            odb.Close();
            odb = Open(updateTransaction);
            f = (VO.Login.Function) odb.GetObjectFromId(oid);
            odb.Close();
            AssertEquals("function1", f.GetName());
        }

        /// <summary>
        ///   Stores an object User that has a non null reference to a Profile.
        /// </summary>
        /// <remarks>
        ///   Stores an object User that has a non null reference to a Profile. Then
        ///   deletes the profile. Loads the user again and updates the user profile
        ///   with a new created profile. ODB did not detect the change Detected by
        ///   Olivier.
        /// </remarks>
        /// <exception cref="System.Exception">System.Exception</exception>
        [Test]
        public virtual void Test8()
        {
            DeleteBase(Name);
            var odb = Open(Name);
            var user = new User("name", "email", new Profile("p1", new VO.Login.Function("function")));
            odb.Store(user);
            odb.Close();
            odb = Open(Name);
            var query = odb.Query<Profile>();
            var p = query.Execute<Profile>().GetFirst();
            odb.Delete(p);
            odb.Close();
            odb = Open(Name);
            var query1 = odb.Query<User>();
            var user3 = query1.Execute<User>().GetFirst();
            AssertNull(user3.GetProfile());
            user3.SetProfile(new Profile("new profile", new VO.Login.Function("f1")));
            user3.SetEmail("email2");
            user3.SetName("name2");
            odb.Store(user3);
            odb.Close();
            odb = Open(Name);
            var query2 = odb.Query<User>();
            var user4 = query2.Execute<User>().GetFirst();
            odb.Close();
            DeleteBase(Name);
            AssertEquals("new profile", user4.GetProfile().GetName());
            AssertEquals("email2", user4.GetEmail());
            AssertEquals("name2", user4.GetName());
        }

        /// <summary>
        ///   Stores an object User that has a non null reference to a Profile.
        /// </summary>
        /// <remarks>
        ///   Stores an object User that has a non null reference to a Profile. Creates
        ///   a new profile.
        ///   Update the last profile and sets it a the new user profile.ODB detects
        ///   the reference change but does not update the profile Detected by Olivier.
        ///   22/05/2007
        /// </remarks>
        /// <exception cref="System.Exception">System.Exception</exception>
        [Test]
        public virtual void Test9()
        {
            DeleteBase(Name);
            var odb = Open(Name);
            var user = new User("name", "email", new Profile("p1", new VO.Login.Function("function")));
            odb.Store(user);
            odb.Store(new Profile("new profile"));
            odb.Close();
            odb = Open(Name);
            var query = odb.Query<Profile>();
            query.Descend("name").Constrain((object) "new profile").Equal();
            var p = query.Execute<Profile>().GetFirst();
            p.SetName("new profile2");
            var query2 = odb.Query<User>();
            var user2 = query2.Execute<User>().GetFirst();
            user2.SetProfile(p);
            odb.Store(user2);
            odb.Close();
            odb = Open(Name);
            var query1 = odb.Query<User>();
            var user3 = query1.Execute<User>().GetFirst();
            AssertNotNull(user3.GetProfile());
            odb.Close();
            DeleteBase(Name);
            AssertEquals("new profile2", user3.GetProfile().GetName());
        }
    }
}
