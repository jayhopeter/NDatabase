using NDatabase.Api;
using NUnit.Framework;

namespace Test.NDatabase.Odb.Test.Trigger
{
    [TestFixture]
    public class TestAutoIncrementTrigger : ODBTest
    {
        public static readonly string Base = "trigger-auto-increment.ndb";

        /// <exception cref="System.Exception"></exception>
        [Test]
        public virtual void Test1()
        {
            IOdb odb = null;
            DeleteBase(Base);
            try
            {
                odb = Open(Base);
                odb.TriggerManagerFor<ObjectWithAutoIncrementId>().AddInsertTrigger(new LocalAutoIncrementTrigger());
                var o = new ObjectWithAutoIncrementId("Object 1");
                odb.Store(o);
                AssertEquals(1, o.GetId());
            }
            finally
            {
                if (odb != null)
                    odb.Close();
            }
        }

        /// <exception cref="System.Exception"></exception>
        [Test]
        public virtual void Test1000Objects()
        {
            IOdb odb = null;
            DeleteBase(Base);
            try
            {
                odb = Open(Base);
                odb.TriggerManagerFor<ObjectWithAutoIncrementId>().AddInsertTrigger(new LocalAutoIncrementTrigger());
                for (var i = 0; i < 1000; i++)
                {
                    var o = new ObjectWithAutoIncrementId("Object " + (i + 1));
                    odb.Store(o);
                    AssertEquals(i + 1, o.GetId());
                }
                odb.Close();
                odb = Open(Base);
                odb.TriggerManagerFor<ObjectWithAutoIncrementId>().AddInsertTrigger(new LocalAutoIncrementTrigger());
                for (var i = 0; i < 1000; i++)
                {
                    var o = new ObjectWithAutoIncrementId("Object - bis - " + (i + 1));
                    odb.Store(o);
                    AssertEquals(1000 + i + 1, o.GetId());
                }
                odb.Close();
            }
            finally
            {
            }
        }

        /// <exception cref="System.Exception"></exception>
        [Test]
        public virtual void Test2Objects()
        {
            IOdb odb = null;
            DeleteBase(Base);
            try
            {
                odb = Open(Base);
                odb.TriggerManagerFor<ObjectWithAutoIncrementId>().AddInsertTrigger(new LocalAutoIncrementTrigger());
                var o = new ObjectWithAutoIncrementId("Object 1");
                odb.Store(o);
                AssertEquals(1, o.GetId());
                odb.Close();
                odb = Open(Base);
                odb.TriggerManagerFor<ObjectWithAutoIncrementId>().AddInsertTrigger(new LocalAutoIncrementTrigger());
                o = new ObjectWithAutoIncrementId("Object 2");
                odb.Store(o);
                AssertEquals(2, o.GetId());
            }
            finally
            {
                if (odb != null)
                    odb.Close();
            }
        }
    }
}
