using NDatabase.IO;

namespace Test.NDatabase.Odb.Test.IO
{
    internal class MockFileIdentification : IDbIdentification
    {
        #region IFileIdentification Members

        public string Id
        {
            get { return "mock"; }
        }

        public string FileName
        {
            get { return "mock"; }
        }

        public bool IsNew()
        {
            return false;
        }

        public void EnsureDirectories()
        {
            // mock
        }

        public IMultiBufferedFileIO GetIO(int bufferSize)
        {
            return new MultiBufferedFileIO(bufferSize);
        }

        public IDbIdentification GetTransactionIdentification(long creationDateTime, string sessionId)
        {
            return new InMemoryIdentification();
        }

        public string Directory
        {
            get { return string.Empty; }
        }

        #endregion

        public bool IsLocal()
        {
            return false;
        }
    }
}
