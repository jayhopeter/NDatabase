using System;
using Moq;
using NDatabase.Api;
using NDatabase.Core;
using NDatabase.Meta;
using NDatabase.Meta.Introspector;
using NUnit.Framework;

namespace NDatabase.UnitTests.Refactor
{
    public class When_we_are_using_refactor_manager_to_rename_class : InstanceSpecification<IRefactorManager>
    {
        private Type _type;
        private ClassInfo _classInfo;
        private Mock<IMetaModel> _metaModelMock;
        private Mock<IObjectWriter> _objectWriterMock;

        protected override void Establish_context()
        {
            _type = typeof(TypeToRefactor);

            _metaModelMock = new Mock<IMetaModel>();
            _classInfo = ClassIntrospector.Introspect(typeof(TypeToRefactor), true).GetMainClassInfo();
            var fullClassName = OdbClassNameResolver.GetFullName(_type);
            _metaModelMock.Setup(x => x.GetClassInfo(fullClassName, true)).Returns(_classInfo).Verifiable();

            _objectWriterMock = new Mock<IObjectWriter>();
            _objectWriterMock.Setup(x => x.UpdateClassInfo(_classInfo, true)).Verifiable();
        }

        protected override IRefactorManager Create_subject_under_test()
        {
            return new RefactorManager(_metaModelMock.Object, _objectWriterMock.Object);
        }

        protected override void Because()
        {
            var fullClassName = OdbClassNameResolver.GetFullName(_type);
            SubjectUnderTest.RenameClass(fullClassName, typeof(RenamedType));
        }

        [Test]
        public void It_should_update_class_info_with_new_attribute_name()
        {
            var fullClassName = OdbClassNameResolver.GetFullName(typeof(RenamedType));
            Assert.That(_classInfo.FullClassName, Is.EqualTo(fullClassName));
            Assert.That(_classInfo.UnderlyingType, Is.EqualTo(typeof(RenamedType)));
        }

        [Test]
        public void It_should_delegate_creation_of_class_info_to_meta_model()
        {
            _metaModelMock.Verify();
        }

        [Test]
        public void It_should_delegate_updating_of_class_info_to_object_writer()
        {
            _objectWriterMock.Verify();
        }
    }
}