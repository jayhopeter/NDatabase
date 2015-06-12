using System;
using NDatabase.Core;
using NDatabase.Core.Engine;
using NDatabase.IO;
using NDatabase.Meta;
using NDatabase.Transaction;
using NUnit.Framework;

namespace NDatabase.UnitTests.Transaction
{
    internal class When_we_use_write_action_to_save_data : InstanceSpecification<WriteAction>
    {
        private byte[] _data;
        private IFileSystemInterface _fsi;
        private long _position;

        protected override void Establish_context()
        {
            _position = 3L;
            _data = new byte[] {1, 2, 3};

            _fsi = new FileSystemInterfaceFake();
        }

        protected override WriteAction Create_subject_under_test()
        {
            return new WriteAction(_position, _data);
        }

        [Test]
        public void It_should_contain_clear_string_representation()
        {
            Assert.That(SubjectUnderTest.ToString(), Is.EqualTo("position=3 | bytes=[1 2 3 ] & size=3"));
        }

        [Test]
        public void It_should_push_single_data_set_to_persisted_instance()
        {
            SubjectUnderTest.PersistMeTo(_fsi);

            var savedBytes = ((FileSystemInterfaceFake) _fsi).Bytes;
            var savedWriteInTransaction = ((FileSystemInterfaceFake) _fsi).WriteInTransaction;
            
            Assert.That(savedWriteInTransaction, Is.False);

            var position = ByteArrayConverter.ByteArrayToLong(savedBytes);
            Assert.That(position, Is.EqualTo(_position));

            var size = ByteArrayConverter.ByteArrayToInt(savedBytes, 8);
            Assert.That(size, Is.EqualTo(_data.Length));

            Assert.That(savedBytes[12], Is.EqualTo(_data[0]));
            Assert.That(savedBytes[13], Is.EqualTo(_data[1]));
            Assert.That(savedBytes[14], Is.EqualTo(_data[2]));
        }

        [Test]
        public void It_should_push_extended_data_set_to_persisted_instance()
        {
            var newData = new byte[] {4, 5, 6};

            SubjectUnderTest.AddBytes(newData);

            Assert.That(SubjectUnderTest.ToString(), Is.EqualTo("position=3 | bytes=[1 2 3 4 5 6 ] & size=6"));

            SubjectUnderTest.PersistMeTo(_fsi);

            var savedBytes = ((FileSystemInterfaceFake)_fsi).Bytes;

            var position = ByteArrayConverter.ByteArrayToLong(savedBytes);
            Assert.That(position, Is.EqualTo(_position));

            var size = ByteArrayConverter.ByteArrayToInt(savedBytes, 8);
            Assert.That(size, Is.EqualTo(_data.Length + newData.Length));

            Assert.That(savedBytes[12], Is.EqualTo(_data[0]));
            Assert.That(savedBytes[13], Is.EqualTo(_data[1]));
            Assert.That(savedBytes[14], Is.EqualTo(_data[2]));
            Assert.That(savedBytes[15], Is.EqualTo(newData[0]));
            Assert.That(savedBytes[16], Is.EqualTo(newData[1]));
            Assert.That(savedBytes[17], Is.EqualTo(newData[2]));
        }

        [Test]
        public void It_should_allow_to_save_data_from_persisted_instance()
        {
            SubjectUnderTest.ApplyTo(_fsi);

            var savedBytes = ((FileSystemInterfaceFake)_fsi).Bytes;
            var savedWriteInTransaction = ((FileSystemInterfaceFake)_fsi).WriteInTransaction;
            var savedPosition = ((FileSystemInterfaceFake)_fsi).Position;

            Assert.That(savedPosition, Is.EqualTo(_position));
            Assert.That(savedWriteInTransaction, Is.False);

            Assert.That(savedBytes[0], Is.EqualTo(_data[0]));
            Assert.That(savedBytes[1], Is.EqualTo(_data[1]));
            Assert.That(savedBytes[2], Is.EqualTo(_data[2]));
        }

        [Test]
        public void It_should_allow_you_to_clean_it()
        {
            Assert.That(SubjectUnderTest.IsEmpty(), Is.False);

            SubjectUnderTest.Clear();

            Assert.That(SubjectUnderTest.IsEmpty(), Is.True);
            Assert.That(SubjectUnderTest.ToString(), Is.EqualTo("position=3 | bytes=null & size=3"));
        }

        [Test]
        public void It_should_allow_you_to_create_empty_write_action_and_then_add_data_set_to_persisted_instance()
        {
            var writeAction = new WriteAction(_position);

            Assert.That(writeAction.ToString(), Is.EqualTo("position=3 | bytes=[] & size=0"));

            writeAction.AddBytes(_data);

            writeAction.PersistMeTo(_fsi);

            var savedBytes = ((FileSystemInterfaceFake)_fsi).Bytes;
            var savedWriteInTransaction = ((FileSystemInterfaceFake)_fsi).WriteInTransaction;
            
            Assert.That(savedWriteInTransaction, Is.False);

            var position = ByteArrayConverter.ByteArrayToLong(savedBytes);
            Assert.That(position, Is.EqualTo(_position));

            var size = ByteArrayConverter.ByteArrayToInt(savedBytes, 8);
            Assert.That(size, Is.EqualTo(_data.Length));

            Assert.That(savedBytes[12], Is.EqualTo(_data[0]));
            Assert.That(savedBytes[13], Is.EqualTo(_data[1]));
            Assert.That(savedBytes[14], Is.EqualTo(_data[2]));
        }

        #region Nested type: FileSystemInterfaceFake

        private class FileSystemInterfaceFake : IFileSystemInterface
        {
            public bool WriteInTransaction { get; private set; }
            public byte[] Bytes { get; private set; }

            #region IFileSystemInterface Members

            public void Dispose()
            {
                throw new NotImplementedException();
            }

            public void Flush()
            {
                throw new NotImplementedException();
            }

            public long GetPosition()
            {
                throw new NotImplementedException();
            }

            public long GetLength()
            {
                throw new NotImplementedException();
            }

            public void SetWritePositionNoVerification(long position, bool writeInTransacation)
            {
                throw new NotImplementedException();
            }

            public void SetWritePosition(long position, bool writeInTransacation)
            {
                Position = position;
            }

            public long Position { get; private set; }

            public void SetReadPosition(long position)
            {
                throw new NotImplementedException();
            }

            public long GetAvailablePosition()
            {
                throw new NotImplementedException();
            }

            public void EnsureSpaceFor(OdbType type)
            {
                throw new NotImplementedException();
            }

            public void WriteByte(byte i, bool writeInTransaction)
            {
                throw new NotImplementedException();
            }

            public byte ReadByte()
            {
                throw new NotImplementedException();
            }

            public void WriteBytes(byte[] bytes, bool writeInTransaction)
            {
                Bytes = bytes;
                WriteInTransaction = writeInTransaction;
            }

            public byte[] ReadBytes(int length)
            {
                throw new NotImplementedException();
            }

            public void WriteChar(char c, bool writeInTransaction)
            {
                throw new NotImplementedException();
            }

            public char ReadChar()
            {
                throw new NotImplementedException();
            }

            public void WriteShort(short s, bool writeInTransaction)
            {
                throw new NotImplementedException();
            }

            public short ReadShort()
            {
                throw new NotImplementedException();
            }

            public void WriteInt(int i, bool writeInTransaction)
            {
                throw new NotImplementedException();
            }

            public int ReadInt()
            {
                throw new NotImplementedException();
            }

            public void WriteLong(long i, bool writeInTransaction)
            {
                throw new NotImplementedException();
            }

            public long ReadLong()
            {
                throw new NotImplementedException();
            }

            public void WriteFloat(float f, bool writeInTransaction)
            {
                throw new NotImplementedException();
            }

            public float ReadFloat()
            {
                throw new NotImplementedException();
            }

            public void WriteDouble(double d, bool writeInTransaction)
            {
                throw new NotImplementedException();
            }

            public double ReadDouble()
            {
                throw new NotImplementedException();
            }

            public void WriteBigDecimal(decimal d, bool writeInTransaction)
            {
                throw new NotImplementedException();
            }

            public decimal ReadBigDecimal()
            {
                throw new NotImplementedException();
            }

            public void WriteDate(DateTime d, bool writeInTransaction)
            {
                throw new NotImplementedException();
            }

            public DateTime ReadDate()
            {
                throw new NotImplementedException();
            }

            public void WriteString(string s, bool writeInTransaction)
            {
                throw new NotImplementedException();
            }

            public void WriteString(string s, bool writeInTransaction, int totalSpace)
            {
                throw new NotImplementedException();
            }

            public string ReadString()
            {
                throw new NotImplementedException();
            }

            public void WriteBoolean(bool b, bool writeInTransaction)
            {
                throw new NotImplementedException();
            }

            public bool ReadBoolean()
            {
                throw new NotImplementedException();
            }

            public void Close()
            {
                throw new NotImplementedException();
            }

            public IDbIdentification GetFileIdentification()
            {
                throw new NotImplementedException();
            }

            public void WriteUShort(ushort s, bool writeInTransaction)
            {
                throw new NotImplementedException();
            }

            public ushort ReadUShort()
            {
                throw new NotImplementedException();
            }

            public void WriteUInt(uint i, bool writeInTransaction)
            {
                throw new NotImplementedException();
            }

            public uint ReadUInt()
            {
                throw new NotImplementedException();
            }

            public void WriteULong(ulong i, bool writeInTransaction)
            {
                throw new NotImplementedException();
            }

            public ulong ReadULong()
            {
                throw new NotImplementedException();
            }

            public void WriteSByte(sbyte i, bool writeInTransaction)
            {
                throw new NotImplementedException();
            }

            public sbyte ReadSByte()
            {
                throw new NotImplementedException();
            }

            #endregion
        }

        #endregion
    }
}
