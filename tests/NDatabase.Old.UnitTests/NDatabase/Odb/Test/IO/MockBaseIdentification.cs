using System;
using NDatabase.Odb.Core.Layers.Layer3;

namespace Test.Odb.Test.IO
{
    public class MockBaseIdentification : IFileIdentification
    {
        #region IBaseIdentification Members

        public String GetIdentification()
        {
            return "mock";
        }

        public string Id { get { return "mock"; } }
        public string Directory { get { return string.Empty; } }
        public string FileName { get { return "mock"; } }

        public bool IsNew()
        {
            return false;
        }

        #endregion

        public bool IsLocal()
        {
            return false;
        }
    }
}
