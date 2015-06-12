using NDatabase.Meta;
using NDatabase.Meta.Introspector;
using NUnit.Framework;

namespace NDatabase.UnitTests.Metadata
{
    internal class When_we_are_inspecting_null : InstanceSpecification<NullNativeObjectInfo>
    {
        protected override NullNativeObjectInfo Create_subject_under_test()
        {
            var introspector = (IObjectIntrospector)new ObjectIntrospector(null);
            return
                introspector.GetMetaRepresentation(null, true, null, new DefaultInstrumentationCallback()) as
                NullNativeObjectInfo;
        }

        [Test]
        public void It_should_be_represented_by_enum_native_object_info()
        {
            Assert.That(SubjectUnderTest, Is.Not.Null);
            Assert.That(SubjectUnderTest.IsNull(), Is.True);
        }
    }
}