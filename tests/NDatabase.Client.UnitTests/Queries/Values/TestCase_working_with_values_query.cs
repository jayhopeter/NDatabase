using System.Collections;
using System.Text;
using NDatabase.Api;
using NUnit.Framework;

namespace NDatabase.Client.UnitTests.Queries.Values
{
    public class TestCase_working_with_values_query
    {
        private const string DbName = "values_query.ndb";
        private const int Limit = 10;

        [SetUp]
        public void SetUp()
        {
            OdbFactory.Delete(DbName);

            using (var odb = OdbFactory.Open(DbName))
            {
                for (var i = 1; i <= Limit; i++)
                    odb.Store(new Result("Result" + i, i, (i % 2 == 0) ? "EVEN" : "ODD"));
            }
        }

        [Test]
        public void using_sum_on_stored_objects()
        {
            IValues values;

            using (var odb = OdbFactory.Open(DbName))
            {
                values = odb.ValuesQuery<Result>().Sum("_value", "sum").Execute();
            }

            var objectValues = values.NextValues();
            Assert.That(objectValues.GetByAlias("sum"), Is.EqualTo(55m));
        }

        [Test]
        public void using_min_on_stored_objects()
        {
            IValues values;

            using (var odb = OdbFactory.Open(DbName))
            {
                values = odb.ValuesQuery<Result>().Min("_value", "min").Execute();
            }

            var objectValues = values.NextValues();
            Assert.That(objectValues.GetByAlias("min"), Is.EqualTo(1m));
        }

        [Test]
        public void using_max_on_stored_objects()
        {
            IValues values;

            using (var odb = OdbFactory.Open(DbName))
            {
                values = odb.ValuesQuery<Result>().Max("_value", "max").Execute();
            }

            var objectValues = values.NextValues();
            Assert.That(objectValues.GetByAlias("max"), Is.EqualTo(10m));
        }

        [Test]
        public void using_avg_on_stored_objects()
        {
            IValues values;

            using (var odb = OdbFactory.Open(DbName))
            {
                values = odb.ValuesQuery<Result>().Avg("_value", "avg").Execute();
            }

            var objectValues = values.NextValues();
            Assert.That(objectValues.GetByAlias("avg"), Is.EqualTo(5.5m));
        }

        [Test]
        public void using_count_on_stored_objects()
        {
            IValues values;

            using (var odb = OdbFactory.Open(DbName))
            {
                values = odb.ValuesQuery<Result>().Count("count").Execute();
            }

            var objectValues = values.NextValues();
            Assert.That(objectValues.GetByAlias("count"), Is.EqualTo(10m));
        }

        [Test]
        public void using_field_on_stored_objects()
        {
            IValues values;

            using (var odb = OdbFactory.Open(DbName))
            {
                values = odb.ValuesQuery<Result>().Field("_value", "field").Execute();
            }

            var objectValues = values.NextValues();
            Assert.That(objectValues.GetByAlias("field"), Is.EqualTo(1));

            objectValues = values.NextValues();
            Assert.That(objectValues.GetByAlias("field"), Is.EqualTo(2));

            objectValues = values.NextValues();
            Assert.That(objectValues.GetByAlias("field"), Is.EqualTo(3));

            objectValues = values.NextValues();
            Assert.That(objectValues.GetByAlias("field"), Is.EqualTo(4));

            objectValues = values.NextValues();
            Assert.That(objectValues.GetByAlias("field"), Is.EqualTo(5));

            objectValues = values.NextValues();
            Assert.That(objectValues.GetByAlias("field"), Is.EqualTo(6));

            objectValues = values.NextValues();
            Assert.That(objectValues.GetByAlias("field"), Is.EqualTo(7));

            objectValues = values.NextValues();
            Assert.That(objectValues.GetByAlias("field"), Is.EqualTo(8));

            objectValues = values.NextValues();
            Assert.That(objectValues.GetByAlias("field"), Is.EqualTo(9));

            objectValues = values.NextValues();
            Assert.That(objectValues.GetByAlias("field"), Is.EqualTo(10));

            objectValues = values.NextValues();
            Assert.That(objectValues, Is.Null);
        }

        [Test]
        public void using_field_with_filter_on_name_on_stored_objects()
        {
            IValues values;

            using (var odb = OdbFactory.Open(DbName))
            {
                var valuesQuery = odb.ValuesQuery<Result>();
                valuesQuery.Descend("_name").Constrain("Result4");
                values = valuesQuery.Field("_value", "field").Execute();
            }

            var objectValues = values.NextValues();
            Assert.That(objectValues.GetByAlias("field"), Is.EqualTo(4));

            objectValues = values.NextValues();
            Assert.That(objectValues, Is.Null);
        }

        [Test]
        public void using_sublist_on_stored_objects()
        {
            IValues values;

            using (var odb = OdbFactory.Open(DbName))
            {
                var valuesQuery = odb.ValuesQuery<Result>();
                values = valuesQuery.Sublist("_name", "sublist", 4, 3, true).Execute();
            }

            var objectValues = values.NextValues();
            var firstNameSublist = (IList)objectValues.GetByAlias("sublist");

            var stringBuilder = new StringBuilder();

            foreach (var value in firstNameSublist)
                stringBuilder.Append(value);

            Assert.That(stringBuilder.ToString(), Is.EqualTo("lt1"));
        }

        [Test]
        public void using_size_on_stored_objects()
        {
            IValues values;

            using (var odb = OdbFactory.Open(DbName))
            {
                values = odb.ValuesQuery<Result>().Size("_name", "size").Execute();
            }

            var objectValues = values.NextValues();
            Assert.That(objectValues.GetByAlias("size"), Is.EqualTo(7));
        }

        [Test]
        public void using_group_by_on_stored_objects()
        {
            IValues values;

            using (var odb = OdbFactory.Open(DbName))
            {
                values = odb.ValuesQuery<Result>().Sum("_value", "sum_group").GroupBy("_category").Execute();
            }

            //ODD
            var objectValues = values.NextValues();
            Assert.That(objectValues.GetByAlias("sum_group"), Is.EqualTo(25m));

            //EVEN
            objectValues = values.NextValues();
            Assert.That(objectValues.GetByAlias("sum_group"), Is.EqualTo(30m));
        }
    }

    public class Result
    {
        private readonly string _name;
        private readonly int _value;
        private readonly string _category;

        public Result(string name, int value, string category)
        {
            _name = name;
            _value = value;
            _category = category;
        }

        public string GetCategory()
        {
            return _category;
        }

        public string GetName()
        {
            return _name;
        }

        public int GetValue()
        {
            return _value;
        }
    }
}