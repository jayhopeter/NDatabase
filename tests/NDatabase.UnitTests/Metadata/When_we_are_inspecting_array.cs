using System;
using Moq;
using NDatabase.Core;
using NDatabase.Core.Session;
using NDatabase.Meta;
using NDatabase.Meta.Introspector;
using NUnit.Framework;

namespace NDatabase.UnitTests.Metadata
{
    internal class When_we_are_inspecting_array : InstanceSpecification<ArrayObjectInfo>
    {
        private DayOfWeek[] _days;

        private Mock<IObjectWriter> _objectWriterMock;
        private Mock<IMetaModel> _metaModelMock;
        private Mock<ISession> _session;

        private IObjectIntrospectionDataProvider _classInfoProvider;

        protected override void Establish_context()
        {
            _days = new[] {DayOfWeek.Saturday, DayOfWeek.Sunday};

            _objectWriterMock = new Mock<IObjectWriter>();
            _objectWriterMock.Setup(x => x.AddClasses(It.IsAny<ClassInfoList>())).Verifiable();

            _metaModelMock = new Mock<IMetaModel>();
            _metaModelMock.Setup(x => x.ExistClass(typeof(DayOfWeek))).Returns(false).Verifiable();

            _session = new Mock<ISession>();
            _session.Setup(x => x.GetMetaModel()).Returns(_metaModelMock.Object).Verifiable();
            _session.Setup(x => x.GetObjectWriter()).Returns(_objectWriterMock.Object).Verifiable();

            _classInfoProvider = new SessionDataProvider(_session.Object);
        }

        protected override ArrayObjectInfo Create_subject_under_test()
        {
            var introspector = (IObjectIntrospector)new ObjectIntrospector(_classInfoProvider);
            return
                introspector.GetMetaRepresentation(_days, true, null, new DefaultInstrumentationCallback()) as
                ArrayObjectInfo;
        }

        [Test]
        public void It_should_be_represented_by_enum_native_object_info()
        {
            Assert.That(SubjectUnderTest, Is.Not.Null);
            Assert.That(SubjectUnderTest.IsArrayObject(), Is.True);
            Assert.That(((EnumNativeObjectInfo) SubjectUnderTest.GetArray()[0]).GetEnumValue(), Is.EqualTo(DayOfWeek.Saturday.ToString()));
            Assert.That(((EnumNativeObjectInfo) SubjectUnderTest.GetArray()[1]).GetEnumValue(), Is.EqualTo(DayOfWeek.Sunday.ToString()));

            var fullName = OdbClassNameResolver.GetFullName(typeof(DayOfWeek));
            Assert.That(SubjectUnderTest.GetRealArrayComponentClassName(), Is.EqualTo(fullName));
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
