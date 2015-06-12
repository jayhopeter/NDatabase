using NDatabase.Api;
using NUnit.Framework;

namespace NDatabase.UnitTests.Utils
{
    public class Test_order_by_constants
    {
        [Test]
        public void It_should_have_none_option()
        {
            var orderByNone = OrderByConstants.OrderByNone;
            Assert.That(orderByNone.IsOrderByAsc(), Is.False);
            Assert.That(orderByNone.IsOrderByDesc(), Is.False);
            Assert.That(orderByNone.IsOrderByNone(), Is.True);
            Assert.That(orderByNone.ToString(), Is.EqualTo("no order by"));
        }

        [Test]
        public void It_should_have_asc_option()
        {
            var orderByNone = OrderByConstants.OrderByAsc;
            Assert.That(orderByNone.IsOrderByAsc(), Is.True);
            Assert.That(orderByNone.IsOrderByDesc(), Is.False);
            Assert.That(orderByNone.IsOrderByNone(), Is.False);
            Assert.That(orderByNone.ToString(), Is.EqualTo("order by asc"));
        }

        [Test]
        public void It_should_have_desc_option()
        {
            var orderByNone = OrderByConstants.OrderByDesc;
            Assert.That(orderByNone.IsOrderByAsc(), Is.False);
            Assert.That(orderByNone.IsOrderByDesc(), Is.True);
            Assert.That(orderByNone.IsOrderByNone(), Is.False);
            Assert.That(orderByNone.ToString(), Is.EqualTo("order by desc"));
        }
    }
}