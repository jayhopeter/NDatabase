using NDatabase.Api;
using NUnit.Framework;

namespace Test.NDatabase.Odb.Test.Trigger
{
    [TestFixture]
    public class TestTriggerBefore : ODBTest
    {
        // fails when the trigger is called after the object introspection (1.9
        // beta2)
        /// <exception cref="System.Exception"></exception>
        [Test]
        public virtual void Test1()
        {
            IOdb odb = null;
            var baseName = GetBaseName();
            DeleteBase(baseName);
            var myTrigger = new MyTriggerBefore();
            try
            {
                odb = Open(baseName);
                odb.TriggerManagerFor<SimpleObject>().AddInsertTrigger(myTrigger);
                var so = new SimpleObject(5);
                var oid = odb.Store(so);
                AssertEquals(6, so.GetId());
                odb.Close();
                odb = Open(baseName);
                var so2 = (SimpleObject) odb.GetObjectFromId(oid);
                AssertEquals(6, so2.GetId());
            }
            finally
            {
                if (odb != null)
                    odb.Close();
            }
            DeleteBase(baseName);
        }

        /// <exception cref="System.Exception"></exception>
        [Test]
        public virtual void Test2UpdateTriggers()
        {
            if (!testNewFeature)
                return;
            IOdb odb = null;
            var baseName = GetBaseName();
            DeleteBase(baseName);
            var myTrigger = new MyUpdateTriggerBefore();
            try
            {
                odb = Open(baseName);
                var so = new SimpleObject(5);
                var oid = odb.Store(so);
                AssertEquals(5, so.GetId());
                odb.Close();
                odb = Open(baseName);
                odb.TriggerManagerFor<SimpleObject>().AddUpdateTrigger(myTrigger);
                var so2 = (SimpleObject) odb.GetObjectFromId(oid);
                AssertEquals(5, so2.GetId());
                odb.Store(so2);
                odb.Close();
                AssertEquals(6, so2.GetId());
                odb = Open(baseName);
                so2 = (SimpleObject) odb.GetObjectFromId(oid);
                AssertEquals(6, so2.GetId());
            }
            finally
            {
                if (odb != null && !odb.IsClosed())
                    odb.Close();
            }
            DeleteBase(baseName);
        }
    }
}
