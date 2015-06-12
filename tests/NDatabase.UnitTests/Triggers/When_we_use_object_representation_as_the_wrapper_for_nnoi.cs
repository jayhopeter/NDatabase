using Moq;
using NDatabase.Api;
using NDatabase.Cache;
using NDatabase.Core;
using NDatabase.Core.Session;
using NDatabase.Meta;
using NDatabase.Meta.Introspector;
using NDatabase.Oid;
using NDatabase.Triggers;
using NDatabase.UnitTests.TestData;
using NUnit.Framework;

namespace NDatabase.UnitTests.Triggers
{
    public class When_we_use_object_representation_as_the_wrapper_for_nnoi : InstanceSpecification<IObjectRepresentation>
    {
        private readonly OID _sampleOid = OIDFactory.BuildObjectOID(1L);

        private Person _person;
        private NonNativeObjectInfo _nnoi;

        private string _objectClassName;
        private OID _oid;

        private Mock<ISession> _session;
        private Mock<IMetaModel> _metaModelMock;
        private Mock<IObjectWriter> _objectWriterMock;
        private Mock<IOdbCache> _cacheMock;
        private IObjectIntrospectionDataProvider _classInfoProvider;

        protected override void Establish_context()
        {
            _person = new Person("Julia", 3);

            _objectWriterMock = new Mock<IObjectWriter>();
            _objectWriterMock.Setup(x => x.AddClasses(It.IsAny<ClassInfoList>())).Verifiable();

            _metaModelMock = new Mock<IMetaModel>();
            _metaModelMock.Setup(x => x.ExistClass(typeof(Person))).Returns(false).Verifiable();

            var objectInfoHeader = new ObjectInfoHeader(1, null, null, null, null, null);

            _cacheMock = new Mock<IOdbCache>();
            _cacheMock.Setup(x => x.GetOid(_person)).Returns(_sampleOid).Verifiable();
            _cacheMock.Setup(x => x.GetObjectInfoHeaderByOid(_sampleOid, true)).Returns(objectInfoHeader).Verifiable();

            _session = new Mock<ISession>();
            _session.Setup(x => x.GetMetaModel()).Returns(_metaModelMock.Object).Verifiable();
            _session.Setup(x => x.GetObjectWriter()).Returns(_objectWriterMock.Object).Verifiable();
            _session.Setup(x => x.GetCache()).Returns(_cacheMock.Object).Verifiable();

            _classInfoProvider = new SessionDataProvider(_session.Object);

            var introspector = (IObjectIntrospector)new ObjectIntrospector(_classInfoProvider);
            _nnoi = introspector.GetMetaRepresentation(_person, true, null, new DefaultInstrumentationCallback()) as NonNativeObjectInfo;
        }

        protected override IObjectRepresentation Create_subject_under_test()
        {
            return new ObjectRepresentation(_nnoi, _classInfoProvider);
        }

        protected override void Because()
        {
            _objectClassName = SubjectUnderTest.GetObjectClassName();
            _oid = SubjectUnderTest.GetOid();
        }

        [Test]
        public void It_should_have_set_correct_base_attributes()
        {
            var className = OdbClassNameResolver.GetFullName(typeof (Person));
            Assert.That(_objectClassName, Is.EqualTo(className));

            Assert.That(_oid, Is.EqualTo(_sampleOid));
        }

        [Test]
        public void It_should_go_through_all_required_delegations()
        {
            _session.Verify();
            _cacheMock.Verify();
            _metaModelMock.Verify();
            _objectWriterMock.Verify();
        }

        [Test]
        public void It_should_have_correct_property_values()
        {
            Assert.That(SubjectUnderTest.GetValueOf("Name"), Is.EqualTo("Julia"));
            Assert.That(SubjectUnderTest.GetValueOf("Age"), Is.EqualTo(3));
        }

        [Test]
        public void It_should_allow_on_changing_property_values()
        {
            SubjectUnderTest.SetValueOf("Name", "Madzia");
            SubjectUnderTest.SetValueOf("Age", 24);

            Assert.That(SubjectUnderTest.GetValueOf("Name"), Is.EqualTo("Madzia"));
            Assert.That(SubjectUnderTest.GetValueOf("Age"), Is.EqualTo(24));
        }
    }
}