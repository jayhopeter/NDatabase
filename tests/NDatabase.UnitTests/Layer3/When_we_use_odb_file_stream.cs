using System.Text;
using NDatabase.Exceptions;
using NDatabase.IO;
using NUnit.Framework;

namespace NDatabase.UnitTests.Layer3
{
    internal class When_we_use_odb_file_stream : InstanceSpecification<IOdbStream>
    {
        private byte _testByte;
        private const string ExistingDbName = "odbfilestream.ndb";

        protected override void Establish_context()
        {
            OdbFactory.Delete(ExistingDbName);

            _testByte = 3;
        }

        protected override IOdbStream Create_subject_under_test()
        {
            return new OdbFileStream(ExistingDbName);
        }

        protected override void Because()
        {
            SubjectUnderTest.Write(_testByte);
        }

        protected override void Dispose_context()
        {
            SubjectUnderTest.Dispose();
        }

        [Test]
        public void It_should_has_length_equals_1_after_writing_byte_into_file()
        {
            Assert.That(SubjectUnderTest.Length, Is.EqualTo(1));
        }

        [Test]
        public void It_should_has_length_equals_2_after_writing_two_bytes_into_file()
        {
            SubjectUnderTest.Write(1);
            Assert.That(SubjectUnderTest.Length, Is.EqualTo(2));
        }

        [Test]
        public void It_should_be_able_to_read_written_byte()
        {
            SubjectUnderTest.SetPosition(0L);
            var value = SubjectUnderTest.Read();
            Assert.That(value, Is.EqualTo(_testByte));
        }

        [Test]
        public void It_should_throw_exception_when_reading_from_out_of_range_position()
        {
            Assert.That(() => SubjectUnderTest.Read(), Throws.Exception.TypeOf<OdbRuntimeException>());
        }

        [Test]
        public void It_should_be_able_to_read_both_written_bytes()
        {
            const byte val = 1;
            SubjectUnderTest.Write(val);

            SubjectUnderTest.SetPosition(0L);
            var value = SubjectUnderTest.Read();
            Assert.That(value, Is.EqualTo(_testByte));

            var secondValue = SubjectUnderTest.Read();
            Assert.That(secondValue, Is.EqualTo(val));
        }

        [Test]
        public void It_should_be_able_to_read_secondly_written_byte()
        {
            const byte val = 1;
            SubjectUnderTest.Write(val);

            SubjectUnderTest.SetPosition(1L);

            var secondValue = SubjectUnderTest.Read();
            Assert.That(secondValue, Is.EqualTo(val));
        }

        [Test]
        public void It_should_correctly_write_string_as_the_byte_array()
        {
            const string name = "Julia";
            var bytes = Encoding.GetEncoding("UTF-8").GetBytes(name);

            SubjectUnderTest.Write(bytes, bytes.Length);

            Assert.That(SubjectUnderTest.Length, Is.EqualTo(bytes.Length + 1));

            SubjectUnderTest.SetPosition(1L);
            var buffer = new byte[bytes.Length];
            var size = SubjectUnderTest.Read(buffer, bytes.Length);

            Assert.That(size, Is.EqualTo(bytes.Length));

            var value = Encoding.UTF8.GetString(buffer, 0, buffer.Length);

            Assert.That(value, Is.EqualTo(name));
        }
    }
}