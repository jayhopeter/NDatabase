using System.Collections.Generic;
using NUnit.Framework;
using Test.NDatabase.Odb.Test.VO.Login;

namespace Test.NDatabase.Odb.Test.Query.Criteria
{
    [TestFixture]
    public class TestCriteriaQuery6 : ODBTest
    {
        /// <exception cref="System.Exception"></exception>
        [Test]
        public virtual void Test1()
        {
            var baseName = GetBaseName();
            var odb = Open(baseName);
            IList<Profile> profiles = new List<Profile>();
            profiles.Add(new Profile("p1", new VO.Login.Function("f1")));
            profiles.Add(new Profile("p2", new VO.Login.Function("f2")));
            var cb = new ClassB("name", profiles);
            odb.Store(cb);
            odb.Close();
            odb = Open(baseName);
            // this object is not known y NDatabase so the query will not return anything
            var p = new Profile("p1", (IList<VO.Login.Function>) null);
            var query = odb.Query<ClassB>();
            query.Descend("profiles").Constrain(p).Contains();
            var l = query.Execute<ClassB>();
            odb.Close();
            AssertEquals(0, l.Count);
        }

        /// <exception cref="System.Exception"></exception>
        [Test]
        public virtual void Test2()
        {
            var baseName = GetBaseName();
            var odb = Open(baseName);
            IList<Profile> profiles = new List<Profile>();
            profiles.Add(new Profile("p1", new VO.Login.Function("f1")));
            profiles.Add(new Profile("p2", new VO.Login.Function("f2")));
            var cb = new ClassB("name", profiles);
            odb.Store(cb);
            odb.Close();
            odb = Open(baseName);
            var query1 = odb.Query<Profile>();
            var p = query1.Execute<Profile>().GetFirst();
            var query = odb.Query<ClassB>();
            query.Descend("profiles").Constrain(p).Contains();
            var l = query.Execute<ClassB>();
            odb.Close();
            AssertEquals(1, l.Count);
        }

        /// <exception cref="System.Exception"></exception>
        [Test]
        public virtual void TestReuse()
        {
            var baseName = GetBaseName();
            var odb = Open(baseName);
            IList<Profile> profiles = new List<Profile>();
            profiles.Add(new Profile("p1", new VO.Login.Function("f1")));
            profiles.Add(new Profile("p2", new VO.Login.Function("f2")));
            var cb = new ClassB("name", profiles);
            odb.Store(cb);
            odb.Close();
            odb = Open(baseName);
            var query = odb.Query<ClassB>();
            query.Descend("name").Constrain((object) "name").Equal();
            var l = query.Execute<ClassB>();
            odb.Close();
            AssertEquals(1, l.Count);
        }
    }
}
