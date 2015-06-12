using NDatabase.Core;
using NDatabase.Core.Engine;
using NDatabase.IO;
using NDatabase.Tool.Wrappers;
using NUnit.Framework;

namespace Test.NDatabase.Odb.Test.IO
{
    [TestFixture]
    public class TestFileSystemInterface2 : ODBTest
    {
        [Test]
        public virtual void TestReadWrite()
        {
            DeleteBase("testReadWrite.ndb");
            IFileSystemInterface fsi = new FileSystemInterface(new FileIdentification("testReadWrite.ndb"),
                                                               MultiBuffer.DefaultBufferSizeForData,
                                                               new MockSession("test"));
            fsi.SetWritePosition(fsi.GetLength(), false);
            for (var i = 0; i < 10000; i++)
            {
                fsi.WriteInt(i, false);
                var currentPosition = fsi.GetPosition();
                if (i == 8000)
                {
                    currentPosition = fsi.GetPosition();
                    
                    // Using the for transaction method to avoid protected area
                    // verification, check the setWritePosition method
                    fsi.SetWritePositionNoVerification(4, false);
                    AssertEquals(1, fsi.ReadInt());
                    
                    fsi.SetWritePositionNoVerification(currentPosition, false);
                }
                if (i == 9000)
                {
                    currentPosition = fsi.GetPosition();
                    
                    fsi.SetWritePositionNoVerification(8, false);
                    fsi.WriteInt(12, false);
                    
                    fsi.SetWritePositionNoVerification(currentPosition, false);
                }
            }
            fsi.SetReadPosition(0);
            for (var i = 0; i < 10000; i++)
            {
                var j = fsi.ReadInt();
                if (i == 2)
                    AssertEquals(12, j);
                else
                    AssertEquals(i, j);
            }
            fsi.Close();
            DeleteBase("testReadWrite.ndb");
        }

        [Test]
        public virtual void TestStringGetBytesWithoutEncoding()
        {
            var test = "How are you my friend?";
            var size = 1000000;
            var t0 = OdbTime.GetCurrentTimeInMs();
            // Execute with encoding
            for (var i = 0; i < size; i++)
                ByteArrayConverter.StringToByteArray(test, -1);
            var t1 = OdbTime.GetCurrentTimeInMs();
            Println("With Encoding=" + (t1 - t0));
        }
    }
}
