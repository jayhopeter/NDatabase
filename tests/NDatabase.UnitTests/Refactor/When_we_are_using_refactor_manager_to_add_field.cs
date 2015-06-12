using System;
using Moq;
using NDatabase.Api;
using NDatabase.Core;
using NDatabase.Meta;
using NDatabase.Meta.Introspector;
using NUnit.Framework;

namespace NDatabase.UnitTests.Refactor
{
    public class When_we_are_using_refactor_manager_to_add_field : InstanceSpecification<IRefactorManager>
    {
        private Type _type;
        private Type _fieldType;
        private string _fieldName;
        private ClassInfo _classInfo;
        private Mock<IMetaModel> _metaModelMock;
        private Mock<IObjectWriter> _objectWriterMock;

        protected override void Establish_context()
        {
            _type = typeof (TypeToRefactor);
            
            _fieldType = typeof (string);
            _fieldName = "NewField";

            _metaModelMock = new Mock<IMetaModel>();
            _classInfo = ClassIntrospector.Introspect(typeof (TypeToRefactor), true).GetMainClassInfo();
            _metaModelMock.Setup(x => x.GetClassInfo(_type, true)).Returns(_classInfo).Verifiable();
            
            _objectWriterMock = new Mock<IObjectWriter>();
            _objectWriterMock.Setup(x => x.UpdateClassInfo(_classInfo, true)).Verifiable();
        }

        protected override IRefactorManager Create_subject_under_test()
        {
            return new RefactorManager(_metaModelMock.Object, _objectWriterMock.Object);
        }

        protected override void Because()
        {
            SubjectUnderTest.AddField(_type, _fieldType, _fieldName);
        }

        [Test]
        public void It_should_extend_class_info_with_new_attribute_info()
        {
            Assert.That(_classInfo.Attributes.Count, Is.EqualTo(3));

            var attributeInfo = _classInfo.GetAttributeInfoFromName(_fieldName);

            Assert.That(attributeInfo, Is.Not.Null);
            Assert.That(attributeInfo.GetName(), Is.EqualTo(_fieldName));
            Assert.That(attributeInfo.GetAttributeType(), Is.EqualTo(OdbType.GetFromClass(_fieldType)));
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