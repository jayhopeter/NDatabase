using System.Collections.Generic;
using NUnit.Framework;

namespace NDatabase.UnitTests.Bugs
{
    public class When_we_want_to_store_generic_types
    {
        private const string DbName = "dictionary.ndb";

        [Test]
        public void It_should_successfuly_store_generic_dictionary_and_list()
        {
            OdbFactory.Delete(DbName);

            var company = new Company();

            using (var odb = OdbFactory.Open(DbName))
            {
                odb.Store(company);
            }

            using (var odb = OdbFactory.OpenLast())
            {
                var query = odb.Query<Company>();
                var storedCompany = query.Execute<Company>().GetFirst();
                var dict = storedCompany.Dictionary;
                Assert.That((object) dict.Keys, Contains.Item("A"));
                Assert.That((object) dict.Keys, Contains.Item("B"));

                var list = storedCompany.List;
                Assert.That((object) list, Contains.Item(1234));
                Assert.That((object) list, Contains.Item(4321));
            }
        }

        #region Nested type: Company

        private class Company
        {
            private readonly IDictionary<string, Employee> _dictionary = new Dictionary<string, Employee>();
            private readonly IList<int> _list = new List<int>();

            public Company()
            {
                var employee1 = new Employee {Name = "A", Age = 1};
                var employee2 = new Employee {Name = "B", Age = 2};

                _dictionary.Add(employee1.Name, employee1);
                _dictionary.Add(employee2.Name, employee2);

                _list.Add(1234);
                _list.Add(4321);
            }

            public IDictionary<string, Employee> Dictionary
            {
                get { return _dictionary; }
            }

            public IList<int> List
            {
                get { return _list; }
            }
        }

        #endregion

        #region Nested type: Employee

        private class Employee
        {
            private static readonly IEqualityComparer<Employee> NameComparerInstance = new NameEqualityComparer();
            public string Name { get; set; }
            public int Age { get; set; }

            public static IEqualityComparer<Employee> NameComparer
            {
                get { return NameComparerInstance; }
            }

            #region Nested type: NameEqualityComparer

            private sealed class NameEqualityComparer : IEqualityComparer<Employee>
            {
                #region IEqualityComparer<Employee> Members

                public bool Equals(Employee x, Employee y)
                {
                    if (ReferenceEquals(x, y))
                        return true;
                    if (ReferenceEquals(x, null))
                        return false;
                    if (ReferenceEquals(y, null))
                        return false;
                    if (x.GetType() != y.GetType())
                        return false;
                    return string.Equals(x.Name, y.Name);
                }

                public int GetHashCode(Employee obj)
                {
                    return (obj.Name != null
                                ? obj.Name.GetHashCode()
                                : 0);
                }

                #endregion
            }

            #endregion
        }

        #endregion
    }
}
