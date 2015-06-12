using System;
using System.Linq;
using NDatabase.Api;
using NDatabase.Meta.Introspector;
using NUnit.Framework;

namespace NDatabase.UnitTests.Layer1
{
    public class When_we_use_class_introspector
    {
        private class EuCountry : Country
        {
            private static readonly string ContinentName = "Europe";

            private readonly string _currency;

            public EuCountry(CountryName name, int population, string currency)
                : base(name, population, ContinentName)
            {
                _currency = currency;
            }
        }

        private class Country
        {
            public class CountryName
            {
                private readonly string _name;

                public CountryName(string name)
                {
                    _name = name;
                }

                public string Value
                {
                    get { return _name; }
                }
            }

            private readonly CountryName _name;
            
            [NonSerialized]
            private string _nonSerializedProperty;

            [NonPersistent]
            private string _nonPersistentProperty;

            public Country(CountryName name, int population, string continent)
            {
                _name = name;
                Population = population;
                Continent = continent;
            }

            public CountryName Name
            {
                get { return _name; }
            }

            public int Population { get; set; }

            public virtual string Continent { get; private set; }

            public string NonPersistentProperty
            {
                get { return _nonPersistentProperty; }
                set { _nonPersistentProperty = value; }
            }

            public string NonSerializedProperty
            {
                get { return _nonSerializedProperty; }
                set { _nonSerializedProperty = value; }
            }

            public IntPtr Pointer { get; set; }
        }

        [Test]
        public void It_should_allow_on_getting_all_fields_based_on_class_name()
        {
            var countryFields = ClassIntrospector.GetAllFieldsFrom(typeof (EuCountry));

            Assert.That(countryFields, Has.Count.EqualTo(4));

            var fieldNames = countryFields.Select(field => field.Name);

            Assert.That(fieldNames, Contains.Item("_name"));
            Assert.That(fieldNames, Contains.Item("_currency"));
            Assert.That(fieldNames, Contains.Item("<Population>k__BackingField"));
            Assert.That(fieldNames, Contains.Item("<Continent>k__BackingField"));
        }

        [Test]
        public void It_should_introspect_recursively_given_type()
        {
            var classInfoList = ClassIntrospector.Introspect(typeof (EuCountry), true);

            Assert.That(classInfoList.GetClassInfos(), Has.Count.EqualTo(2));

            Assert.That(classInfoList.GetMainClassInfo().FullClassName,
                        Is.StringStarting(
                            "NDatabase.UnitTests.Layer1.When_we_use_class_introspector+EuCountry"));
            Assert.That(classInfoList.GetClassInfos().Skip(1).First().FullClassName,
                        Is.StringStarting(
                            "NDatabase.UnitTests.Layer1.When_we_use_class_introspector+Country+CountryName"));
        }
    }
}