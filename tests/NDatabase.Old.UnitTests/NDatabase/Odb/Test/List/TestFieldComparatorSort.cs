using System;
using NDatabase.Api;
using NDatabase.Tool.Wrappers;
using NUnit.Framework;

namespace Test.NDatabase.Odb.Test.List
{
    [TestFixture]
    public class TestFieldComparatorSort : ODBTest
    {
        [Test]
        public virtual void Test1()
        {
            var baseName = GetBaseName();
            IOdb odb = null;
            var k = 10;
            var t1 = OdbTime.GetCurrentTimeInMs();
            odb = Open(baseName);
            for (var i = 0; i < k; i++)
            {
                odb.Store(new User("john" + (k - i), "john@ibm.com", "ny 875", k - i + 1, new DateTime(t1 - i * 1000),
                                   i % 2 == 0));
                odb.Store(new User("john" + (k - i), "john@ibm.com", "ny 875", k - i, new DateTime(t1 - i * 1000),
                                   (i + 1) % 2 == 0));
            }
            odb.Close();
            odb = Open(baseName);
            var q = odb.Query<User>();
            q.Descend("name").OrderAscending();
            q.Descend("id").OrderAscending();
            var users = q.Execute<User>();
            odb.Close();
            if (k < 11)
            {
                //NDatabase.Tool.DisplayUtility.Display("test1", users);
            }
            var user = users.GetFirst();
            AssertTrue(user.GetName().StartsWith("john1"));
            AssertEquals(1, user.GetId());
        }

        [Test]
        public virtual void Test1_2()
        {
            var baseName = GetBaseName();
            IOdb odb = null;
            var k = 10;
            var t1 = OdbTime.GetCurrentTimeInMs();
            odb = Open(baseName);
            for (var i = 0; i < k; i++)
            {
                odb.Store(new User("john" + (k - i), "john@ibm.com", "ny 875", k - i + 1, new DateTime(t1 - i * 1000),
                                   i % 2 == 0));
                odb.Store(new User("john" + (k - i), "john@ibm.com", "ny 875", k - i, new DateTime(t1 - i * 1000),
                                   (i + 1) % 2 == 0));
            }
            odb.Close();
            odb = Open(baseName);
            var q = odb.Query<User>();
            q.Descend("name").OrderDescending();
            q.Descend("id").OrderDescending();
            var users = q.Execute<User>();
            odb.Close();
            if (k < 11)
            {
                //NDatabase.Tool.DisplayUtility.Display("test1", users);
            }
            var user = users.GetFirst();
            AssertTrue(user.GetName().StartsWith("john9"));
            AssertEquals(10, user.GetId());
        }

        [Test]
        public virtual void Test2()
        {
            var baseName = GetBaseName();
            IOdb odb = null;
            var k = 10;
            var t1 = OdbTime.GetCurrentTimeInMs();
            var fields = new[] {"ok", "id", "name"};
            odb = Open(baseName);
            for (var i = 0; i < k; i++)
            {
                odb.Store(new User("john" + (k - i), "john@ibm.com", "ny 875", k - i + 1, new DateTime(t1 - i * 1000),
                                   i % 2 == 0));
                odb.Store(new User("john" + (k - i), "john@ibm.com", "ny 875", k - i, new DateTime(t1 - i * 1000),
                                   (i + 1) % 2 == 0));
            }
            odb.Close();
            odb = Open(baseName);
            var q = odb.Query<User>();
            q.Descend("ok").OrderAscending();
            q.Descend("id").OrderAscending();
            q.Descend("name").OrderAscending();
            var users = q.Execute<User>();
            odb.Close();
            if (k < 11)
            {
                //NDatabase.Tool.DisplayUtility.Display("test1", users);
            }
            var user = users.GetFirst();
            AssertTrue(user.GetName().StartsWith("john1"));
            AssertEquals(2, user.GetId());
        }

        [Test]
        public virtual void Test2_2()
        {
            var baseName = GetBaseName();
            IOdb odb = null;
            var k = 10;
            var t1 = OdbTime.GetCurrentTimeInMs();
            var fields = new[] {"ok", "id", "name"};
            odb = Open(baseName);
            for (var i = 0; i < k; i++)
            {
                odb.Store(new User("john" + (k - i), "john@ibm.com", "ny 875", k - i + 1, new DateTime(t1 - i * 1000),
                                   i % 2 == 0));
                odb.Store(new User("john" + (k - i), "john@ibm.com", "ny 875", k - i, new DateTime(t1 - i * 1000),
                                   (i + 1) % 2 == 0));
            }
            odb.Close();
            odb = Open(baseName);
            var q = odb.Query<User>();
            q.Descend("ok").OrderDescending();
            q.Descend("id").OrderDescending();
            q.Descend("name").OrderDescending();
            var users = q.Execute<User>();
            odb.Close();
            if (k < 11)
            {
                //NDatabase.Tool.DisplayUtility.Display("test1", users);
            }
            var user = users.GetFirst();
            AssertTrue(user.GetName().StartsWith("john10"));
            AssertEquals(11, user.GetId());
        }
    }
}
