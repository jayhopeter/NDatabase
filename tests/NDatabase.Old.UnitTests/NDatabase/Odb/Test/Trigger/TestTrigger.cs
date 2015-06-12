using NDatabase.Api;
using NUnit.Framework;
using Test.NDatabase.Odb.Test.VO.Login;

namespace Test.NDatabase.Odb.Test.Trigger
{
    [TestFixture]
    public class TestTrigger : ODBTest
    {
        /// <exception cref="System.Exception"></exception>
        [Test]
        public virtual void Test1()
        {
            IOdb odb = null;
            DeleteBase("trigger.ndb");
            var myTrigger = new MyTrigger();
            try
            {
                odb = Open("trigger.ndb");
                odb.TriggerManagerFor<VO.Login.Function>().AddInsertTrigger(myTrigger);
                var f1 = new VO.Login.Function("function1");
                var f2 = new VO.Login.Function("function2");
                var profile = new Profile("profile1", f1);
                var user = new User("oli", "oli@ndatabase.net", profile);
                odb.Store(user);
            }
            finally
            {
                if (odb != null)
                    odb.Close();
            }
            odb = Open("trigger.ndb");
            odb.Close();
            DeleteBase("trigger.ndb");
            AssertEquals(1, myTrigger.nbInsertsBefore);
            AssertEquals(1, myTrigger.nbInsertsAfter);
        }

        // To test if triggers are called on recursive objects
        /// <exception cref="System.Exception"></exception>
        [Test]
        public virtual void Test2()
        {
            IOdb odb = null;
            DeleteBase("trigger.ndb");
            var myTrigger = new MyTrigger();
            try
            {
                odb = Open("trigger.ndb");
                odb.TriggerManagerFor<VO.Login.Function>().AddInsertTrigger(myTrigger);
                var f1 = new VO.Login.Function("function1");
                var f2 = new VO.Login.Function("function2");
                var profile = new Profile("profile1", f1);
                var user = new User("oli", "oli@ndatabase.net", profile);
                odb.Store(user);
                odb.Store(f2);
            }
            finally
            {
                if (odb != null)
                    odb.Close();
            }
            odb = Open("trigger.ndb");
            odb.Close();
            DeleteBase("trigger.ndb");
            AssertEquals(2, myTrigger.nbInsertsBefore);
            AssertEquals(2, myTrigger.nbInsertsAfter);
        }

        // To test select triggers
        /// <exception cref="System.Exception"></exception>
        [Test]
        public virtual void TestSelectTrigger()
        {
            IOdb odb = null;
            DeleteBase("trigger.ndb");
            var myTrigger = new MySelectTrigger();
            try
            {
                odb = Open("trigger.ndb");
                var f1 = new VO.Login.Function("function1");
                var f2 = new VO.Login.Function("function2");
                var profile = new Profile("profile1", f1);
                var user = new User("oli", "oli@ndatabase.net", profile);
                odb.Store(user);
                odb.Store(f2);
            }
            finally
            {
                if (odb != null)
                    odb.Close();
            }
            odb = Open("trigger.ndb");
            odb.TriggerManagerFor<VO.Login.Function>().AddSelectTrigger(myTrigger);
            var query = odb.Query<VO.Login.Function>();
            var functions = query.Execute<VO.Login.Function>();
            odb.Close();
            DeleteBase("trigger.ndb");
            AssertEquals(2, functions.Count);
            AssertEquals(2, myTrigger.nbCalls);
        }
    }
}
