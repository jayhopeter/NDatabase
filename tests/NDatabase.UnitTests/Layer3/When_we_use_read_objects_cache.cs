using NDatabase.Cache;
using NDatabase.Meta;
using NDatabase.Meta.Introspector;
using NDatabase.Oid;
using NUnit.Framework;

namespace NDatabase.UnitTests.Layer3
{
    internal class When_we_use_read_objects_cache : InstanceSpecification<IReadObjectsCache>
    {
        private NonNativeObjectInfo _objectInfo;
        private ObjectOID _oid;

        protected override void Establish_context()
        {
            var employee = new Employee { Name = "Object" };
            _oid = new ObjectOID(1234L);

            var classInfoList = ClassIntrospector.Introspect(typeof(Employee), true);

            var mainClassInfo = classInfoList.GetMainClassInfo();
            mainClassInfo.ClassInfoId = new ClassOID(12345L);
            _objectInfo = new NonNativeObjectInfo(employee, mainClassInfo);
            _objectInfo.SetOid(_oid);
        }

        protected override IReadObjectsCache Create_subject_under_test()
        {
            return new ReadObjectsCache();
        }

        protected override void Because()
        {
            SubjectUnderTest.StartReadingObjectInfoWithOid(_oid, _objectInfo);
        }

        [Test]
        public void It_should_mark_object_as_already_read()
        {
            Assert.That(SubjectUnderTest.IsReadingObjectInfoWithOid(_oid), Is.True);
        }

        [Test]
        public void It_should_allow_on_reading_already_saved_object_info()
        {
            Assert.That(SubjectUnderTest.GetObjectInfoByOid(_oid), Is.EqualTo(_objectInfo));
        }

        [Test]
        public void It_should_allow_on_cleaning_data()
        {
            SubjectUnderTest.ClearObjectInfos();
            Assert.That(SubjectUnderTest.GetObjectInfoByOid(_oid), Is.EqualTo(null));
        }

        #region Nested type: Employee

        private class Employee
        {
            public string Name { get; set; }
        }

        #endregion
    }
}