using NDatabase.Meta;
using NDatabase.Oid;
using NUnit.Framework;

namespace NDatabase.UnitTests.Layer2
{
    internal class When_we_use_class_info_zone_info : InstanceSpecification<CIZoneInfo>
    {
        private ObjectOID _firstOid;
        private long _numberOfObjects;
        private ObjectOID _secondOid;

        protected override void Establish_context()
        {
            _firstOid = new ObjectOID(1L);
            _secondOid = new ObjectOID(2L);
            _numberOfObjects = 4L;
        }

        protected override CIZoneInfo Create_subject_under_test()
        {
            return new CIZoneInfo();
        }

        protected override void Because()
        {
            SubjectUnderTest.SetNbObjects(_numberOfObjects);

            SubjectUnderTest.First = _firstOid;
            SubjectUnderTest.Last = _secondOid;
        }

        [Test]
        public void It_should_return_correct_number_of_objects()
        {
            Assert.That(SubjectUnderTest.GetNumberbOfObjects(), Is.EqualTo(_numberOfObjects));
            Assert.That(SubjectUnderTest.HasObjects(), Is.True);
        }

        [Test]
        public void It_should_has_representative_string_form()
        {
            Assert.That(SubjectUnderTest.ToString(), Is.EqualTo("(first=1,last=2,nb=4)"));
        }

        [Test]
        public void It_should_allow_on_increasing_number_of_objects()
        {
            SubjectUnderTest.IncreaseNbObjects();

            Assert.That(SubjectUnderTest.GetNumberbOfObjects(), Is.EqualTo(_numberOfObjects + 1));
        }

        [Test]
        public void It_should_allow_on_decreasing_number_of_objects()
        {
            SubjectUnderTest.DecreaseNbObjects();

            Assert.That(SubjectUnderTest.GetNumberbOfObjects(), Is.EqualTo(_numberOfObjects - 1));
        }

        [Test]
        public void It_should_allow_on_reset_of_content()
        {
            SubjectUnderTest.Reset();

            Assert.That(SubjectUnderTest.HasObjects(), Is.False);
            Assert.That(SubjectUnderTest.GetNumberbOfObjects(), Is.EqualTo(0));
            Assert.That(SubjectUnderTest.First, Is.Null);
            Assert.That(SubjectUnderTest.Last, Is.Null);
        }

        [Test]
        public void It_should_allow_on_setting_content_based_on_class_info_sent_as_argument()
        {
            var zoneInfo = new CIZoneInfo {First = null};
            zoneInfo.SetNbObjects(10L);

            SubjectUnderTest.SetBasedOn(zoneInfo);

            Assert.That(SubjectUnderTest.HasObjects(), Is.EqualTo(zoneInfo.HasObjects()));
            Assert.That(SubjectUnderTest.GetNumberbOfObjects(), Is.EqualTo(zoneInfo.GetNumberbOfObjects()));
            Assert.That(SubjectUnderTest.First, Is.EqualTo(zoneInfo.First));
            Assert.That(SubjectUnderTest.Last, Is.EqualTo(zoneInfo.Last));
        }
    }
}
