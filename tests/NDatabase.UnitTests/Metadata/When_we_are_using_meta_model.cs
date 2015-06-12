using System.Collections.Generic;
using NDatabase.Meta;
using NDatabase.Oid;
using NUnit.Framework;

namespace NDatabase.UnitTests.Metadata
{
    public class A
    {
    }

    public class B : A
    {
    }

    internal class When_we_are_using_meta_model : InstanceSpecification<IMetaModel>
    {
        private IList<ClassInfo> _result;
        private ClassInfo _classInfoA;
        private ClassInfo _classInfoB;

        protected override void Establish_context()
        {
            _classInfoA = new ClassInfo(typeof(A)) {ClassInfoId = OIDFactory.BuildClassOID(1L)};
            _classInfoB = new ClassInfo(typeof(B)) {ClassInfoId = OIDFactory.BuildClassOID(2L)};
        }

        protected override IMetaModel Create_subject_under_test()
        {
            return new MetaModel();
        }

        protected override void Because()
        {
            SubjectUnderTest.AddClass(_classInfoA);
            SubjectUnderTest.AddClass(_classInfoB);
            _result = SubjectUnderTest.GetPersistentSubclassesOf(typeof (B));
        }

        [Test]
        public void It_should_properly_handle_inheritance()
        {
            Assert.That(_result, Has.Count.EqualTo(1));
        }
    }
}