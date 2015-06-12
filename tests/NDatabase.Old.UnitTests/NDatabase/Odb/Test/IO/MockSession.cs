using System;
using NDatabase.Core;
using NDatabase.Core.Engine;
using NDatabase.Core.Session;

namespace Test.NDatabase.Odb.Test.IO
{
    internal class MockSession : global::NDatabase.Transaction.Session
    {
        public MockSession(String baseIdentification) : base("mock", baseIdentification)
        {
        }

        public override void Commit()
        {
        }

        public override IStorageEngine GetStorageEngine()
        {
            try
            {
                return new StorageEngine(new MockFileIdentification());
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
            return null;
        }

        public override ITransaction GetTransaction()
        {
            return null;
        }

        public override void SetFileSystemInterfaceToApplyTransaction(IFileSystemInterface fsi)
        {
        }

        public override IObjectWriter GetObjectWriter()
        {
            return null;
        }

        public override bool TransactionIsPending()
        {
            return false;
        }
    }
}
