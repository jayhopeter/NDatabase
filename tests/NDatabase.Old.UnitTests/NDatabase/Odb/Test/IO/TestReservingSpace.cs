using NDatabase.Core;
using NDatabase.Core.Engine;
using NDatabase.IO;
using NUnit.Framework;

namespace Test.NDatabase.Odb.Test.IO
{
    [TestFixture]
    internal class TestReservingSpace : ODBTest
    {
        /// <exception cref="System.IO.IOException"></exception>
        public virtual void Write(IFileSystemInterface fsi, bool writeInTransaction)
        {
            fsi.WriteInt(1, writeInTransaction);
        }

        [Test]
        public virtual void TestSize()
        {
            DeleteBase("writing.ndb");
            DeleteBase("writing");
            DeleteBase("reserving.ndb");
            DeleteBase("reserving");
            var engine1 =
                (IStorageEngine) new StorageEngine(new FileIdentification("writing.ndb"));
            var engine2 =
                (IStorageEngine) new StorageEngine(new FileIdentification("reserving.ndb"));
            var writingFsi = engine1.GetObjectWriter().FileSystemProcessor.FileSystemInterface;
            var reservingFsi = engine2.GetObjectWriter().FileSystemProcessor.FileSystemInterface;
            AssertEquals(writingFsi.GetLength(), reservingFsi.GetLength());
            Write(writingFsi, false);
            Write(reservingFsi, true);
            AssertEquals(writingFsi.GetLength(), reservingFsi.GetLength());
            engine1.Commit();
            engine1.Close();
            engine2.Commit();
            engine2.Close();
            DeleteBase("writing.ndb");
            DeleteBase("reserving.ndb");
        }
    }
}
