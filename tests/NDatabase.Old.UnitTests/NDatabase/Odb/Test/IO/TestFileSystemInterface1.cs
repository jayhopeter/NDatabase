using System;
using System.Globalization;
using NDatabase.Core;
using NDatabase.Core.Engine;
using NDatabase.IO;
using NUnit.Framework;

namespace Test.NDatabase.Odb.Test.IO
{
    [TestFixture]
    public class TestFileSystemInterface1 : ODBTest
    {
        [Test]
        public virtual void TestBigDecimal()
        {
            DeleteBase("testBigDecimal.ndb");
            var bd = Convert.ToDecimal("-128451.1234567899876543210", CultureInfo.InvariantCulture);

            IFileSystemInterface fsi = new FileSystemInterface(new FileIdentification("testBigDecimal.ndb"),
                                                               MultiBuffer.DefaultBufferSizeForData,
                                                               new MockSession("test"));
            fsi.SetWritePosition(0, false);
            fsi.WriteBigDecimal(bd, false);
            fsi.Close();

            fsi = new FileSystemInterface(new FileIdentification("testBigDecimal.ndb"),
                                          MultiBuffer.DefaultBufferSizeForData, new MockSession("test"));
            fsi.SetReadPosition(0);
            var bd2 = fsi.ReadBigDecimal();
            AssertEquals(bd, bd2);
            fsi.Close();
        }

        [Test]
        public virtual void TestBigInteger()
        {
            DeleteBase("testBigDecimal.ndb");
            var bd = Convert.ToDecimal("-128451");
            IFileSystemInterface fsi = new FileSystemInterface(new FileIdentification("testBigDecimal.ndb"),
                                                               MultiBuffer.DefaultBufferSizeForData,
                                                               new MockSession("test"));
            fsi.SetWritePosition(0, false);
            fsi.WriteBigDecimal(bd, false);
            fsi.Close();
            fsi = new FileSystemInterface(new FileIdentification("testBigDecimal.ndb"),
                                          MultiBuffer.DefaultBufferSizeForData, new MockSession("test"));
            fsi.SetReadPosition(0);
            var bd2 = fsi.ReadBigDecimal();
            AssertEquals(bd, bd2);
            fsi.Close();
        }

        [Test]
        public virtual void TestBoolean()
        {
            DeleteBase("testBoolean.ndb");

            const bool b1 = true;
            const bool b2 = false;
            IFileSystemInterface fsi = new FileSystemInterface(new FileIdentification("testBoolean.ndb"),
                                                               MultiBuffer.DefaultBufferSizeForData,
                                                               new MockSession("test"));
            fsi.SetWritePosition(0, false);
            fsi.WriteBoolean(b1, false);
            fsi.WriteBoolean(b2, false);
            fsi.Close();
            fsi = new FileSystemInterface(new FileIdentification("testBoolean.ndb"),
                                          MultiBuffer.DefaultBufferSizeForData, new MockSession("test"));
            fsi.SetReadPosition(0);
            var b11 = fsi.ReadBoolean();
            var b22 = fsi.ReadBoolean();
            AssertEquals(b1, b11);
            AssertEquals(b2, b22);
            fsi.Close();
        }

        [Test]
        public virtual void TestByte()
        {
            DeleteBase("testByte.ndb");

            const byte b = 127;
            IFileSystemInterface fsi = new FileSystemInterface(new FileIdentification("testByte.ndb"),
                                                               MultiBuffer.DefaultBufferSizeForData,
                                                               new MockSession("test"));
            fsi.SetWritePosition(0, false);
            fsi.WriteByte(b, false);
            fsi.Close();
            fsi = new FileSystemInterface(new FileIdentification("testByte.ndb"),
                                          MultiBuffer.DefaultBufferSizeForData, new MockSession("test"));
            fsi.SetReadPosition(0);
            var b2 = fsi.ReadByte();
            AssertEquals(b, b2);
            fsi.Close();
        }

        [Test]
        public virtual void TestChar()
        {
            DeleteBase("testChar.ndb");

            const char c = '\u00E1';
            IFileSystemInterface fsi = new FileSystemInterface(new FileIdentification("testChar.ndb"),
                                                               MultiBuffer.DefaultBufferSizeForData,
                                                               new MockSession("test"));
            fsi.SetWritePosition(0, false);
            fsi.WriteChar(c, false);
            fsi.Close();
            fsi = new FileSystemInterface(new FileIdentification("testChar.ndb"),
                                          MultiBuffer.DefaultBufferSizeForData, new MockSession("test"));
            fsi.SetReadPosition(0);
            var c2 = fsi.ReadChar();
            AssertEquals(c, c2);
            fsi.Close();
        }

        [Test]
        public virtual void TestFloat()
        {
            DeleteBase("testFloat.ndb");
            const float f = (float) 12544548.12454;
            IFileSystemInterface fsi = new FileSystemInterface(new FileIdentification("testFloat.ndb"),
                                                               MultiBuffer.DefaultBufferSizeForData,
                                                               new MockSession("test"));
            fsi.SetWritePosition(0, false);
            fsi.WriteFloat(f, false);
            fsi.Close();
            fsi = new FileSystemInterface(new FileIdentification("testFloat.ndb"),
                                          MultiBuffer.DefaultBufferSizeForData, new MockSession("test"));
            fsi.SetReadPosition(0);
            var f2 = fsi.ReadFloat();
            AssertTrue(f == f2);
            fsi.Close();
        }

        [Test]
        public virtual void TestInt()
        {
            DeleteBase("testInt.ndb");
            const int i = 259998;
            IFileSystemInterface fsi = new FileSystemInterface(new FileIdentification("testInt.ndb"),
                                                               MultiBuffer.DefaultBufferSizeForData,
                                                               new MockSession("test"));
            fsi.SetWritePosition(0, false);
            fsi.WriteInt(i, false);
            fsi.Close();
            fsi = new FileSystemInterface(new FileIdentification("testInt.ndb"),
                                          MultiBuffer.DefaultBufferSizeForData, new MockSession("test"));
            fsi.SetReadPosition(0);
            var i2 = fsi.ReadInt();
            AssertEquals(i, i2);
            fsi.Close();
        }

        [Test]
        public virtual void TestLong()
        {
            DeleteBase("testLong.ndb");
            const long i = 259999865;
            IFileSystemInterface fsi = new FileSystemInterface(new FileIdentification("testLong.ndb"),
                                                               MultiBuffer.DefaultBufferSizeForData,
                                                               new MockSession("test"));
            fsi.SetWritePosition(0, false);
            fsi.WriteLong(i, false);
            fsi.Close();
            fsi = new FileSystemInterface(new FileIdentification("testLong.ndb"),
                                          MultiBuffer.DefaultBufferSizeForData, new MockSession("test"));
            fsi.SetReadPosition(0);
            var i2 = fsi.ReadLong();
            AssertEquals(i, i2);
            fsi.Close();
        }

        [Test]
        public virtual void TestShort()
        {
            DeleteBase("testShort.ndb");
            const short s = 4598;
            IFileSystemInterface fsi = new FileSystemInterface(new FileIdentification("testShort.ndb"),
                                                               MultiBuffer.DefaultBufferSizeForData,
                                                               new MockSession("test"));
            fsi.SetWritePosition(0, false);
            fsi.WriteShort(s, false);
            fsi.Close();
            fsi = new FileSystemInterface(new FileIdentification("testShort.ndb"),
                                          MultiBuffer.DefaultBufferSizeForData, new MockSession("test"));
            fsi.SetReadPosition(0);
            var s2 = fsi.ReadShort();
            AssertEquals(s, s2);
            fsi.Close();
        }

        [Test]
        public virtual void TestString()
        {
            var baseName = GetBaseName();
            const string s = "ola chico, como voc√™ est√° ??? eu estou bem at√© amanh√£ de manh√£, √°√°√°√°'";
            IFileSystemInterface fsi = new FileSystemInterface(new FileIdentification(baseName),
                                                               MultiBuffer.DefaultBufferSizeForData,
                                                               new MockSession("test"));
            fsi.SetWritePosition(0, false);
            fsi.WriteString(s, false);
            fsi.Close();
            fsi = new FileSystemInterface(new FileIdentification(baseName), MultiBuffer.DefaultBufferSizeForData,
                                          new MockSession("test"));
            fsi.SetReadPosition(0);
            var s2 = fsi.ReadString();
            fsi.Close();
            AssertEquals(s, s2);
        }
    }
}
