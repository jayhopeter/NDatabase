using NUnit.Framework;

namespace Test.NDatabase.Odb.Test.Inheritance
{
    [TestFixture]
    public class TestInheritance : ODBTest
    {
        private static readonly string Name = "inheritance.ndb";

        /// <summary>
        ///   Test persistence of attributes declared by an interface
        /// </summary>
        /// <exception cref="System.Exception">System.Exception</exception>
        [Test]
        public virtual void TestInterface()
        {
            DeleteBase(Name);
            var class1 = new Class1("olivier");
            var class2 = new Class2(10, class1);
            var odb = Open(Name);
            odb.Store(class2);
            odb.Close();
            odb = Open(Name);
            var query = odb.Query<Class2>();
            var c2 = query.Execute<Class2>().GetFirst();
            AssertEquals(class2.GetNb(), c2.GetNb());
            AssertEquals(class2.GetInterface1().GetName(), c2.GetInterface1().GetName());
            odb.Close();
        }

        /// <summary>
        ///   Test persistence of attributes declared by an interface
        /// </summary>
        /// <exception cref="System.Exception">System.Exception</exception>
        [Test]
        public virtual void TestSubClass()
        {
            DeleteBase(Name);
            Class1 class1 = new SubClassOfClass1("olivier", 78);
            var class3 = new Class3(10, class1);
            var odb = Open(Name);
            odb.Store(class3);
            odb.Close();
            odb = Open(Name);
            var query = odb.Query<Class3>();
            var c3 = query.Execute<Class3>().GetFirst();
            AssertEquals(class3.GetNb(), c3.GetNb());
            AssertEquals(class3.GetClass1().GetName(), c3.GetClass1().GetName());
            odb.Close();
        }
    }
}
