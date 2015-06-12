using System;
using Moq;
using NDatabase.Api;
using NDatabase.Api.Triggers;
using NDatabase.Core;
using NDatabase.Triggers;
using NDatabase.UnitTests.CodeSnippets.Data;
using NUnit.Framework;

namespace NDatabase.UnitTests.Triggers
{
    public class Triggers_test_case : InstanceSpecification<ITriggerManager>
    {
        private Mock<IStorageEngine> _storageEngineMock;

        protected override void Establish_context()
        {
            _storageEngineMock = new Mock<IStorageEngine>();
            _storageEngineMock.Setup(x => x.AddInsertTriggerFor(typeof (Warrior), It.IsAny<FakeInsertTrigger>()))
                              .Verifiable();
            _storageEngineMock.Setup(x => x.AddDeleteTriggerFor(typeof (Warrior), It.IsAny<FakeDeleteTrigger>()))
                              .Verifiable();
            _storageEngineMock.Setup(x => x.AddSelectTriggerFor(typeof (Warrior), It.IsAny<FakeSelectTrigger>()))
                              .Verifiable();
            _storageEngineMock.Setup(x => x.AddUpdateTriggerFor(typeof (Warrior), It.IsAny<FakeUpdateTrigger>()))
                              .Verifiable();
        }

        protected override ITriggerManager Create_subject_under_test()
        {
            return new TriggerManager<Warrior>(_storageEngineMock.Object);
        }

        protected override void Because()
        {
            SubjectUnderTest.AddDeleteTrigger(new FakeDeleteTrigger());
            SubjectUnderTest.AddInsertTrigger(new FakeInsertTrigger());
            SubjectUnderTest.AddUpdateTrigger(new FakeUpdateTrigger());
            SubjectUnderTest.AddSelectTrigger(new FakeSelectTrigger());
        }

        [Test]
        public void It_should_delegate_all_registrations_to_storage_engine()
        {
            _storageEngineMock.VerifyAll();
        }

        private class FakeDeleteTrigger : DeleteTrigger
        {
            public override bool BeforeDelete(object @object, OID oid)
            {
                throw new NotImplementedException();
            }

            public override void AfterDelete(object @object, OID oid)
            {
                throw new NotImplementedException();
            }
        }

        private class FakeInsertTrigger : InsertTrigger
        {
            public override bool BeforeInsert(object @object)
            {
                throw new NotImplementedException();
            }

            public override void AfterInsert(object @object, OID oid)
            {
                throw new NotImplementedException();
            }
        }

        private class FakeSelectTrigger : SelectTrigger
        {
            public override void AfterSelect(object @object, OID oid)
            {
                throw new NotImplementedException();
            }
        }

        private class FakeUpdateTrigger : UpdateTrigger
        {
            public override bool BeforeUpdate(IObjectRepresentation oldObjectRepresentation, object newObject, OID oid)
            {
                throw new NotImplementedException();
            }

            public override void AfterUpdate(IObjectRepresentation oldObjectRepresentation, object newObject, OID oid)
            {
                throw new NotImplementedException();
            }
        }
    }
}