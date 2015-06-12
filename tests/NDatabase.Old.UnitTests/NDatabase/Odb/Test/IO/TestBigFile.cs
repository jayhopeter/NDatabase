using System;
using System.Collections.Generic;
using System.IO;
using NDatabase;
using NDatabase.Core.Query;
using NDatabase.Tool.Wrappers;
using NUnit.Framework;
using Test.NDatabase.Odb.Test.VO.Login;

namespace Test.NDatabase.Odb.Test.IO
{
    [TestFixture]
    public class TestBigFile : ODBTest
    {
        private object GetUserInstance(int i)
        {
            var login = new VO.Login.Function("login" + i);
            var logout = new VO.Login.Function("logout" + i);
            IList<VO.Login.Function> list = new List<VO.Login.Function>();
            list.Add(login);
            list.Add(logout);
            var profile = new Profile("operator" + i, list);
            var user = new User("olivier smadja" + i, "user@ndatabase.net", profile);
            return user;
        }

        [Test]
        public void T2estBigFileWithOdbSelect()
        {
            const int size1 = 100;
            const int size2 = 10;

            const string dbName = "big-file.ndb";
            OdbFactory.Delete(dbName);

            using (Open(dbName))
            {
            }

            var z = 0;
            for (var i = 0; i < size1; i++)
            {
                using (var odb = Open(dbName))
                {
                    for (var j = 0; j < size2; j++)
                    {
                        odb.Store(GetUserInstance(j));
                        z++;
                    }
                    Println(i + "/" + size1 + " " + z + " objects");
                }
            }

            var start = OdbTime.GetCurrentTimeInMs();
            using (var odb = Open(dbName))
            {
                var q = odb.Query<VO.Login.Function>();
                q.Descend("name").Constrain((object) "login100").Equal();
                var functions = q.Execute<VO.Login.Function>(true, 0, 1);
                Console.Out.WriteLine(((IInternalQuery) q).GetExecutionPlan().GetDetails());
                Console.Out.WriteLine(functions.Count);
                Println(OdbTime.GetCurrentTimeInMs() - start + "ms");
            }
        }

        /// <exception cref="System.IO.IOException"></exception>
        [Test]
        public virtual void Test1()
        {
            var raf = new FileStream("testBigFile", FileMode.OpenOrCreate);
            long l = 2 * 1024000;
            Println(l);
            raf.Seek(l, SeekOrigin.Begin);
            for (var i = 0; i < 1024000; i++)
                raf.Write(new byte[] {0}, 0, 1);
            raf.Write(new byte[] {0}, 0, 1);
            raf.Close();
        }
    }
}
