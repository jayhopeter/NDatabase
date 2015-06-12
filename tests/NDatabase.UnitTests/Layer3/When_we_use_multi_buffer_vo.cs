using NDatabase.IO;
using NDatabase.Tool.Wrappers;
using NUnit.Framework;

namespace NDatabase.UnitTests.Layer3
{
    internal class When_we_use_multi_buffer_vo : InstanceSpecification<IMultiBuffer>
    {
        private long _buffer1StartPosition;
        private long _buffer1EndPosition;

        private long _buffer2StartPosition;
        private long _buffer2EndPosition;

        private long _buffer3StartPosition;
        private long _buffer3EndPosition;

        private long _buffer4StartPosition;
        private long _buffer4EndPosition;

        private long _buffer5StartPosition;
        private long _buffer5EndPosition;
        
        private long _creationDate1;
        private long _creationDate2;
        private long _creationDate3;
        private long _creationDate4;
        private long _creationDate5;

        protected override void Establish_context()
        {
            _buffer1StartPosition = 1L;
            _buffer1EndPosition = 10L;
            _creationDate1 = OdbTime.GetCurrentTimeInTicks() + 1;

            _buffer2StartPosition = 20L;
            _buffer2EndPosition = 200L;
            _creationDate2 = OdbTime.GetCurrentTimeInTicks() + 2;

            _buffer3StartPosition = 3000L;
            _buffer3EndPosition = 3400L;
            _creationDate3 = OdbTime.GetCurrentTimeInTicks() + 3;

            _buffer4StartPosition = 4000L;
            _buffer4EndPosition = 4300L;
            _creationDate4 = OdbTime.GetCurrentTimeInTicks() + 4;

            _buffer5StartPosition = 5000L;
            _buffer5EndPosition = 5400L;
            _creationDate5 = OdbTime.GetCurrentTimeInTicks() + 5;
        }

        protected override IMultiBuffer Create_subject_under_test()
        {
            return new MultiBuffer(MultiBuffer.DefaultBufferSizeForData);
        }

        protected override void Because()
        {
            SubjectUnderTest.SetPositions(0, _buffer1StartPosition, _buffer1EndPosition);
            SubjectUnderTest.SetPositions(1, _buffer2StartPosition, _buffer2EndPosition);
            SubjectUnderTest.SetPositions(2, _buffer3StartPosition, _buffer3EndPosition);
            SubjectUnderTest.SetPositions(3, _buffer4StartPosition, _buffer4EndPosition);
            SubjectUnderTest.SetPositions(4, _buffer5StartPosition, _buffer5EndPosition);

            SubjectUnderTest.SetCreationDate(0, _creationDate1);
            SubjectUnderTest.SetCreationDate(1, _creationDate2);
            SubjectUnderTest.SetCreationDate(2, _creationDate3);
            SubjectUnderTest.SetCreationDate(3, _creationDate4);
            SubjectUnderTest.SetCreationDate(4, _creationDate5);
        }

        [Test]
        public void It_should_have_proper_start_positions()
        {
            Assert.That(SubjectUnderTest.BufferPositions[0].Start, Is.EqualTo(_buffer1StartPosition));
            Assert.That(SubjectUnderTest.BufferPositions[1].Start, Is.EqualTo(_buffer2StartPosition));
            Assert.That(SubjectUnderTest.BufferPositions[2].Start, Is.EqualTo(_buffer3StartPosition));
            Assert.That(SubjectUnderTest.BufferPositions[3].Start, Is.EqualTo(_buffer4StartPosition));
            Assert.That(SubjectUnderTest.BufferPositions[4].Start, Is.EqualTo(_buffer5StartPosition));
        }

        [Test]
        public void It_should_have_proper_end_positions()
        {
            Assert.That(SubjectUnderTest.BufferPositions[0].End, Is.EqualTo(_buffer1EndPosition));
            Assert.That(SubjectUnderTest.BufferPositions[1].End, Is.EqualTo(_buffer2EndPosition));
            Assert.That(SubjectUnderTest.BufferPositions[2].End, Is.EqualTo(_buffer3EndPosition));
            Assert.That(SubjectUnderTest.BufferPositions[3].End, Is.EqualTo(_buffer4EndPosition));
            Assert.That(SubjectUnderTest.BufferPositions[4].End, Is.EqualTo(_buffer5EndPosition));
        }

        [Test]
        public void It_should_have_correct_creation_dates()
        {
            Assert.That(SubjectUnderTest.GetCreationDate(0), Is.EqualTo(_creationDate1));
            Assert.That(SubjectUnderTest.GetCreationDate(1), Is.EqualTo(_creationDate2));
            Assert.That(SubjectUnderTest.GetCreationDate(2), Is.EqualTo(_creationDate3));
            Assert.That(SubjectUnderTest.GetCreationDate(3), Is.EqualTo(_creationDate4));
            Assert.That(SubjectUnderTest.GetCreationDate(4), Is.EqualTo(_creationDate5));
        }

        [Test]
        public void It_should_have_all_max_positions_set_to_zero_on_start()
        {
            for (var i = 0; i < 5; i++)
                Assert.That(SubjectUnderTest.MaxPositionInBuffer[i], Is.EqualTo(0));
        }


        [Test]
        public void It_should_have_all_buffers_clean_on_start()
        {
            for (var i = 0; i < 5; i++)
                Assert.That(SubjectUnderTest.HasBeenUsedForWrite(i), Is.False);
        }

        [Test]
        public void It_should_have_all_dictionaries_set_to_null_after_clear_invocation()
        {
            SubjectUnderTest.Clear();
            Assert.That(SubjectUnderTest.MaxPositionInBuffer, Is.Null);
            Assert.That(SubjectUnderTest.BufferPositions, Is.Null);
            Assert.That(SubjectUnderTest.Buffers, Is.Null);
        }

        [Test]
        public void It_should_handle_properly_the_written_byte_to_the_given_buffer()
        {
            const byte value = 2;
            const int bufferIndex = 3;
            const int possitionInBuffer = 1;

            SubjectUnderTest.SetByte(bufferIndex, possitionInBuffer, value);

            Assert.That(SubjectUnderTest.HasBeenUsedForWrite(bufferIndex), Is.True);

            Assert.That(SubjectUnderTest.Buffers[bufferIndex][possitionInBuffer], Is.EqualTo(value));

            Assert.That(SubjectUnderTest.MaxPositionInBuffer[bufferIndex], Is.EqualTo(possitionInBuffer));
        }

        [Test]
        public void It_should_return_correct_position_when_checking_the_buffor()
        {
            var bufferIndex1 = SubjectUnderTest.GetBufferIndexForPosition(_buffer1StartPosition, (int) (_buffer1EndPosition - _buffer1StartPosition));
            Assert.That(bufferIndex1, Is.EqualTo(0));


            var bufferIndex3 = SubjectUnderTest.GetBufferIndexForPosition(_buffer3StartPosition, (int)(_buffer3EndPosition - _buffer3StartPosition));
            Assert.That(bufferIndex3, Is.EqualTo(2));
        }

        [Test]
        public void It_should_return_minus_1_when_requested_position_is_out_of_range()
        {
            const int startOutOfRange = 2000;

            var bufferIndex = SubjectUnderTest.GetBufferIndexForPosition(startOutOfRange, 400);
            Assert.That(bufferIndex, Is.EqualTo(-1));
        }

        [Test]
        public void It_should_have_empty_buffer_after_cleaning_it()
        {
            const byte value = 2;
            const int bufferIndex = 3;
            const int possitionInBuffer = 1;

            SubjectUnderTest.SetByte(bufferIndex, possitionInBuffer, value);
            Assert.That(SubjectUnderTest.Buffers[bufferIndex][possitionInBuffer], Is.EqualTo(value));

            var maxPosition = SubjectUnderTest.MaxPositionInBuffer[bufferIndex];
            SubjectUnderTest.ClearBuffer(bufferIndex);

            for (var i = 0; i < maxPosition; i++)
                Assert.That(SubjectUnderTest.Buffers[bufferIndex][i], Is.EqualTo(0));

            Assert.That(SubjectUnderTest.MaxPositionInBuffer[bufferIndex], Is.EqualTo(0));
            Assert.That(SubjectUnderTest.BufferPositions[bufferIndex].Start, Is.EqualTo(0));
            Assert.That(SubjectUnderTest.BufferPositions[bufferIndex].End, Is.EqualTo(0));
            Assert.That(SubjectUnderTest.HasBeenUsedForWrite(bufferIndex), Is.False);
        }

        [Test]
        public void It_should_properly_write_array_of_bytes()
        {
            const int bufferIndex = 1;
            byte[] bytes = {1, 2, 3};

            SubjectUnderTest.WriteBytes(bufferIndex, bytes, 0, 10, bytes.Length);

            Assert.That(SubjectUnderTest.Buffers[bufferIndex][10], Is.EqualTo(1));
            Assert.That(SubjectUnderTest.Buffers[bufferIndex][10 + 1], Is.EqualTo(2));
            Assert.That(SubjectUnderTest.Buffers[bufferIndex][10 + 2], Is.EqualTo(3));
        }
    }
}