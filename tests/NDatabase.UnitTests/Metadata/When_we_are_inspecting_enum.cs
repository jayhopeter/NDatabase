using System;
using Moq;
using NDatabase.Core;
using NDatabase.Core.Session;
using NDatabase.Meta;
using NDatabase.Meta.Introspector;
using NUnit.Framework;

namespace NDatabase.UnitTests.Metadata
{
    internal class When_we_are_inspecting_enum : InstanceSpecification<EnumNativeObjectInfo>
    {
        private DayOfWeek _day;

        private Mock<IObjectWriter> _objectWriterMock;
        private Mock<IMetaModel> _metaModelMock;
        private Mock<ISession> _session;

        private IObjectIntrospectionDataProvider _classInfoProvider;

        protected override void Establish_context()
        {
            _day = DayOfWeek.Saturday;

            _objectWriterMock = new Mock<IObjectWriter>();
            _objectWriterMock.Setup(x => x.AddClasses(It.IsAny<ClassInfoList>())).Verifiable();

            _metaModelMock = new Mock<IMetaModel>();
            _metaModelMock.Setup(x => x.ExistClass(typeof(DayOfWeek))).Returns(false).Verifiable();

            _session = new Mock<ISession>();
            _session.Setup(x => x.GetMetaModel()).Returns(_metaModelMock.Object).Verifiable();
            _session.Setup(x => x.GetObjectWriter()).Returns(_objectWriterMock.Object).Verifiable();

            _classInfoProvider = new SessionDataProvider(_session.Object);
        }

        protected override EnumNativeObjectInfo Create_subject_under_test()
        {
            var introspector = (IObjectIntrospector)new ObjectIntrospector(_classInfoProvider);
            return
                introspector.GetMetaRepresentation(_day, true, null, new DefaultInstrumentationCallback()) as
                EnumNativeObjectInfo;
        }

        [Test]
        public void It_should_be_represented_by_enum_native_object_info()
        {
            Assert.That(SubjectUnderTest, Is.Not.Null);
            Assert.That(SubjectUnderTest.IsEnumObject(), Is.True);
            Assert.That(SubjectUnderTest.GetEnumValue(), Is.EqualTo(DayOfWeek.Saturday.ToString()));

            var enumClassInfo = SubjectUnderTest.GetEnumClassInfo();
            var fullName = OdbClassNameResolver.GetFullName(typeof(DayOfWeek));
            Assert.That(enumClassInfo.FullClassName, Is.EqualTo(fullName));
            Assert.That(enumClassInfo.Attributes, Has.Count.EqualTo(1));
        }

        [Test]
        public void It_should_invoke_all_delegations()
        {
            _session.Verify();
            _metaModelMock.Verify();
            _objectWriterMock.Verify();
        }
    }
}