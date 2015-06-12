using NDatabase.IO;
using NUnit.Framework;

namespace NDatabase.UnitTests.Layer3
{
    internal class When_we_use_multi_buffered_file_io_with_turned_off_buffer : InstanceSpecification<IMultiBufferedFileIO>
    {
        private const long StartWritePosition = 10L;
        private string _fileName;
        private byte[] _bytes;

        protected override void Establish_context()
        {
            _fileName = "multibufferedfileio.ndb";

            OdbFactory.Delete(_fileName);
        }

        protected override IMultiBufferedFileIO Create_subject_under_test()
        {
            return new MultiBufferedFileIO(MultiBuffer.DefaultBufferSizeForData);
        }

        protected override void Because()
        {
            SubjectUnderTest.SetCurrentWritePosition(StartWritePosition);
            _bytes = new byte[] { 1, 2, 3, 4, 5 };
            SubjectUnderTest.WriteBytes(_bytes);
        }

        protected override void Dispose_context()
        {
            SubjectUnderTest.Dispose();
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
        public void It_should_be_able_to_read_from_given_position()
        {
            using (var nonBufferedFileIO = new NonBufferedFileIO("nonbufferedfileio.ndb"))
            {
                nonBufferedFileIO.SetCurrentPosition(0L);
                nonBufferedFileIO.WriteBytes(_bytes, _bytes.Length);

                var buffer = new byte[3];
                var length = nonBufferedFileIO.Read(2L, buffer, buffer.Length);

                Assert.That(length, Is.EqualTo(buffer.Length));

                Assert.That(buffer[0], Is.EqualTo(3));
                Assert.That(buffer[1], Is.EqualTo(4));
                Assert.That(buffer[2], Is.EqualTo(5));
            }
        }

        [Test]
        public void It_should_be_able_to_write_and_read_more_bytes_than_one_buffer_has()
        {
            var sizeBiggerThanOneBufferSize = MultiBuffer.DefaultBufferSizeForData + 100;
            var bytes = new byte[sizeBiggerThanOneBufferSize];

            for (var i = 0; i < sizeBiggerThanOneBufferSize; i++)
                bytes[i] = (byte) (i % 256);

            using (var fileIO = new MultiBufferedFileIO("nonbufferedfileio.ndb", MultiBuffer.DefaultBufferSizeForData))
            {
                fileIO.SetCurrentWritePosition(0L);
                fileIO.WriteBytes(bytes);

                fileIO.SetCurrentReadPosition(0L);
                var readBytes = fileIO.ReadBytes(sizeBiggerThanOneBufferSize);

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

            using (var fileIO = new MultiBufferedFileIO("nonbufferedfileio.ndb", MultiBuffer.DefaultBufferSizeForData))
            {
                fileIO.SetCurrentWritePosition(0L);
                fileIO.WriteBytes(bytes);

                fileIO.SetCurrentReadPosition(0L);
                var readBytes = fileIO.ReadBytes(sizeBiggerThanAllBufferSize);

                CollectionAssert.AreEqual(readBytes, bytes);
            }
        }
    }
}