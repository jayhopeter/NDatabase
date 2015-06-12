using Moq;
using NDatabase.Api;
using NDatabase.Api.Triggers;
using NDatabase.Core;
using NDatabase.Core.Engine;
using NDatabase.Oid;
using NDatabase.Triggers;
using NDatabase.UnitTests.CodeSnippets.Data;
using NUnit.Framework;

namespace NDatabase.UnitTests.Triggers
{
    internal class Internal_trigger_manager_test_case : InstanceSpecification<IInternalTriggerManager>
    {
        private readonly FakeDeleteTrigger _fakeDeleteTrigger = new FakeDeleteTrigger();
        private readonly FakeInsertTrigger _fakeInsertTrigger = new FakeInsertTrigger();
        private readonly FakeSelectTrigger _fakeSelectTrigger = new FakeSelectTrigger();
        private readonly FakeUpdateTrigger _fakeUpdateTrigger = new FakeUpdateTrigger();
        private Mock<IStorageEngine> _storageEngineMock;

        protected override void Establish_context()
        {
            _storageEngineMock = new Mock<IStorageEngine>();
        }

        protected override IInternalTriggerManager Create_subject_under_test()
        {
            return new InternalTriggerManager(_storageEngineMock.Object);
        }

        protected override void Because()
        {
            SubjectUnderTest.AddDeleteTriggerFor(typeof (object), _fakeDeleteTrigger);
            SubjectUnderTest.AddInsertTriggerFor(typeof (Warrior), _fakeInsertTrigger);
            SubjectUnderTest.AddUpdateTriggerFor(typeof (Warrior), _fakeUpdateTrigger);
            SubjectUnderTest.AddSelectTriggerFor(typeof (Warrior), _fakeSelectTrigger);
        }

        [Test]
        public void It_should_properly_handle_delete_before_trigger()
        {
            const string value = "delete_before";
            var oid = OIDFactory.BuildObjectOID(1L);
            SubjectUnderTest.ManageDeleteTriggerBefore(typeof (Warrior), value, oid);

            Assert.That(value, Is.EqualTo(_fakeDeleteTrigger.GetObject()));
            Assert.That(oid, Is.EqualTo(_fakeDeleteTrigger.GetOid()));
        }

        [Test]
        public void It_should_properly_handle_delete_after_trigger()
        {
            const string value = "delete_after";
            var oid = OIDFactory.BuildObjectOID(1L);
            SubjectUnderTest.ManageDeleteTriggerAfter(typeof(Warrior), value, oid);

            Assert.That(value, Is.EqualTo(_fakeDeleteTrigger.GetObject()));
            Assert.That(oid, Is.EqualTo(_fakeDeleteTrigger.GetOid()));
        }

        [Test]
        public void It_should_properly_handle_insert_before_trigger()
        {
            const string value = "insert_before";
            SubjectUnderTest.ManageInsertTriggerBefore(typeof(Warrior), value);

            Assert.That(value, Is.EqualTo(_fakeInsertTrigger.GetObject()));
            Assert.That(_fakeInsertTrigger.GetOid(), Is.Null);
        }

        [Test]
        public void It_should_properly_handle_insert_after_trigger()
        {
            const string value = "insert_after";
            var oid = OIDFactory.BuildObjectOID(1L);
            SubjectUnderTest.ManageInsertTriggerAfter(typeof(Warrior), value, oid);

            Assert.That(value, Is.EqualTo(_fakeInsertTrigger.GetObject()));
            Assert.That(oid, Is.EqualTo(_fakeInsertTrigger.GetOid()));
        }

        [Test]
        public void It_should_properly_handle_update_before_trigger()
        {
            const string value = "update_before";
            var oid = OIDFactory.BuildObjectOID(1L);
            SubjectUnderTest.ManageUpdateTriggerBefore(typeof(Warrior), null, value, oid);

            Assert.That(value, Is.EqualTo(_fakeUpdateTrigger.GetObject()));
            Assert.That(oid, Is.EqualTo(_fakeUpdateTrigger.GetOid()));
        }

        [Test]
        public void It_should_properly_handle_update_after_trigger()
        {
            const string value = "insert_after";
            var oid = OIDFactory.BuildObjectOID(1L);
            SubjectUnderTest.ManageUpdateTriggerAfter(typeof(Warrior), null, value, oid);

            Assert.That(value, Is.EqualTo(_fakeUpdateTrigger.GetObject()));
            Assert.That(oid, Is.EqualTo(_fakeUpdateTrigger.GetOid()));
        }

        [Test]
        public void It_should_properly_handle_select_trigger()
        {
            const string value = "select";
            var oid = OIDFactory.BuildObjectOID(1L);
            SubjectUnderTest.ManageSelectTriggerAfter(typeof(Warrior), value, oid);

            Assert.That(value, Is.EqualTo(_fakeSelectTrigger.GetObject()));
            Assert.That(oid, Is.EqualTo(_fakeSelectTrigger.GetOid()));
        }

        private class FakeDeleteTrigger : DeleteTrigger
        {
            private object _object;
            private OID _oid;

            public override bool BeforeDelete(object @object, OID oid)
            {
                _oid = oid;
                _object = @object;

                return true;
            }

            public override void AfterDelete(object @object, OID oid)
            {
                _oid = oid;
                _object = @object;
            }

            public OID GetOid()
            {
                return _oid;
            }

            public object GetObject()
            {
                return _object;
            }
        }

        private class FakeInsertTrigger : InsertTrigger
        {
            private object _object;
            private OID _oid;

            public override bool BeforeInsert(object @object)
            {
                _object = @object;
                _oid = null;

                return true;
            }

            public override void AfterInsert(object @object, OID oid)
            {
                _oid = oid;
                _object = @object;
            }

            public object GetObject()
            {
                return _object;
            }

            public OID GetOid()
            {
                return _oid;
            }
        }

        private class FakeSelectTrigger : SelectTrigger
        {
            private object _object;
            private OID _oid;

            public override void AfterSelect(object @object, OID oid)
            {
                _oid = oid;
                _object = @object;
            }

            public object GetObject()
            {
                return _object;
            }

            public OID GetOid()
            {
                return _oid;
            }
        }

        private class FakeUpdateTrigger : UpdateTrigger
        {
            private object _object;
            private OID _oid;

            public override bool BeforeUpdate(IObjectRepresentation oldObjectRepresentation, object newObject, OID oid)
            {
                _object = newObject;
                _oid = oid;
                
                return true;
            }

            public override void AfterUpdate(IObjectRepresentation oldObjectRepresentation, object newObject, OID oid)
            {
                _object = newObject;
                _oid = oid;
            }

            public object GetObject()
            {
                return _object;
            }

            public OID GetOid()
            {
                return _oid;
            }
        }
    }
}