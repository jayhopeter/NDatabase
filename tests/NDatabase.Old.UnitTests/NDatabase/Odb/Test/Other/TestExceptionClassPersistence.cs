using System;
using NUnit.Framework;

namespace Test.NDatabase.Odb.Test.Other
{
    public class TestExceptionClassPersistence : ODBTest
    {
        public static readonly string DbName = "class.ndb";

        [Test]
        public void Test1()
        {
            DeleteBase(DbName);
            var odb = Open(DbName);
            odb.Store(new Exception("test"));
            odb.Close();
            odb = Open(DbName);
            var query = odb.Query<Exception>();
            var l = query.Execute<Exception>();
            odb.Close();
            DeleteBase(DbName);
        }
    }
}
