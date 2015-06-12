using System;
using NUnit.Framework;

namespace NDatabase.Client.UnitTests.Types
{
    public class ClassWithEvents
    {
        private bool _wasInvoked;
        private bool _eventRised;

        public int One;

        public event EventHandler MyCustomEvent;

        public bool WasInvoked
        {
            get { return _wasInvoked; }
        }

        public bool EventWasRised
        {
            get { return _eventRised; }
        }

        public void OnMyCustomEvent()
        {
            var handler = MyCustomEvent;
            if (handler != null) handler(this, EventArgs.Empty);
        }

        public MyDelegate MyDelegateMember;

        public delegate void MyDelegate();

        public void Invoke()
        {
            _wasInvoked = true;
        }

        public void EventRised(object sender, EventArgs e)
        {
            _eventRised = true;
        }
    }


    public class When_we_store_type_with_event_or_delagete
    {
        private const string DbName = "Events.ndb";
        
        [Test]
        public void Test_storage_and_query()
        {
            var item = new ClassWithEvents {One = 1};
            item.MyDelegateMember = item.Invoke;
            item.MyCustomEvent += item.EventRised;

            OdbFactory.Delete(DbName);

            using (var odb = OdbFactory.Open(DbName))
            {
                odb.Store(item);
            }

            ClassWithEvents storedItem;
            using (var odb = OdbFactory.Open(DbName))
            {
                storedItem = odb.QueryAndExecute<ClassWithEvents>().GetFirst();
            }

            Assert.That(storedItem, Is.Not.Null);
            Assert.That(storedItem.One, Is.EqualTo(1));
//            storedItem.MyDelegateMember(); //-> will throw System.AccessViolationException
            Assert.That(item.WasInvoked, Is.False);
//            storedItem.OnMyCustomEvent(); // -> will throw System.AccessViolationException
            Assert.That(item.EventWasRised, Is.False);
        }
    }
}