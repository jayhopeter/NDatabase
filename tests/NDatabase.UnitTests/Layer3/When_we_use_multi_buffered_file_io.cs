using System.IO;
using System.Text;
using NDatabase.IO;
using NUnit.Framework;

namespace NDatabase.UnitTests.Layer3
{
    internal class When_we_use_multi_buffered_file_io : InstanceSpecification<IMultiBufferedFileIO>
    {
        private const long StartWritePosition = 10L;
        private string _fileName;

        protected override void Establish_context()
        {
            _fileName = "multibufferedfileio.ndb";
            
            OdbFactory.Delete(_fileName);
        }

        protected override IMultiBufferedFileIO Create_subject_under_test()
        {
            return new MultiBufferedFileIO(_fileName, MultiBuffer.DefaultBufferSizeForData);
        }

        protected override void Because()
        {
            SubjectUnderTest.SetCurrentWritePosition(StartWritePosition);
            SubjectUnderTest.WriteBytes(new byte[] {1, 2, 3, 4, 5});
        }

        protected override void Dispose_context()
        {
            SubjectUnderTest.Dispose();
        }

        [Test]
        public void It_should_create_or_open_existing_file_with_the_given_name()
        {
            Assert.That(File.Exists(_fileName), Is.True);
        }

        [Test]
        public void It_should_be_able_to_read_all_written_bytes()
        {
            SubjectUnderTest.SetCurrentReadPosition(StartWritePosition);
            Assert.That(SubjectUnderTest.CurrentPosition, Is.EqualTo(StartWritePosition));

            var bytes = SubjectUnderTest.ReadBytes(5);

            Assert.That(bytes[0], Is.EqualTo(1));
            Assert.That(bytes[1], Is.EqualTo(2));
            Assert.That(bytes[2], Is.EqualTo(3));
            Assert.That(bytes[3], Is.EqualTo(4));
            Assert.That(bytes[4], Is.EqualTo(5));
        }

        [Test]
        public void It_should_return_correct_length_of_the_file()
        {
            Assert.That(SubjectUnderTest.Length, Is.EqualTo(StartWritePosition + 5));
        }

        [Test]
        public void It_should_return_current_position_to_the_free_byte_space_after_last_write_position()
        {
            Assert.That(SubjectUnderTest.CurrentPosition, Is.EqualTo(StartWritePosition + 5));
        }

        [Test]
        public void It_should_be_able_to_write_another_byte_and_read_it()
        {
            const byte value = 16;
            SubjectUnderTest.WriteByte(value);

            SubjectUnderTest.SetCurrentReadPosition(StartWritePosition + 5);
            var result = SubjectUnderTest.ReadByte();
            Assert.That(result, Is.EqualTo(value));
        }

        [Test]
        public void It_should_be_able_to_write_and_read_more_bytes_than_one_buffer_has()
        {
            var sizeBiggerThanOneBufferSize = MultiBuffer.DefaultBufferSizeForData + 100;
            var bytes = new byte[sizeBiggerThanOneBufferSize];

            for (var i = 0; i < sizeBiggerThanOneBufferSize; i++)
                bytes[i] = (byte)(i % 256);

            using (var nonBufferedFileIO = new MultiBufferedFileIO("nonbufferedfileio.ndb", MultiBuffer.DefaultBufferSizeForData))
            {
                nonBufferedFileIO.SetCurrentWritePosition(0L);
                nonBufferedFileIO.WriteBytes(bytes);

                nonBufferedFileIO.SetCurrentReadPosition(0L);
                var readBytes = nonBufferedFileIO.ReadBytes(sizeBiggerThanOneBufferSize);

                CollectionAssert.AreEqual(readBytes, bytes);
            }
        }

        [Test]
        public void It_should_be_able_to_write_and_read_more_bytes_than_all_buffer_have()
        {
            var sizeBiggerThanAllBufferSize = MultiBuffer.DefaultBufferSizeForData * (MultiBuffer.NumberOfBuffers + 2);
            var bytes = new byte[sizeBiggerThanAllBufferSize];

            for (var i = 0; i < sizeBiggerThanAllBufferSize; i++)
                bytes[i] = (byte)(i % 256);

            using (var nonBufferedFileIO = new MultiBufferedFileIO("nonbufferedfileio.ndb", MultiBuffer.DefaultBufferSizeForData))
            {
                nonBufferedFileIO.SetCurrentWritePosition(0L);
                nonBufferedFileIO.WriteBytes(bytes);

                nonBufferedFileIO.SetCurrentReadPosition(0L);
                var readBytes = nonBufferedFileIO.ReadBytes(sizeBiggerThanAllBufferSize);

                CollectionAssert.AreEqual(readBytes, bytes);
            }
        }

        [Test]
        public void It_should_throw_exception_when_reading_from_out_of_range_position()
        {
            const long toBigPosition = 12345667L;
            Assert.That(() => SubjectUnderTest.SetCurrentReadPosition(toBigPosition), Throws.Exception);
        }

        [Test]
        public void It_should_display_meaningful_information_as_a_string_representation()
        {
            var representation = new StringBuilder();
            representation.Append("Buffers=currbuffer=1 : ")
                .Append("\n0:[0,0] : write=False - when=0")
                .Append("\n1:[10,16394] : write=True - when=0")
                .Append("\n2:[0,0] : write=False - when=0")
                .Append("\n3:[0,0] : write=False - when=0")
                .Append("\n4:[0,0] : write=False - when=0");

            Assert.That(SubjectUnderTest.ToString(), Is.EqualTo(representation.ToString()));
        }
    }
}