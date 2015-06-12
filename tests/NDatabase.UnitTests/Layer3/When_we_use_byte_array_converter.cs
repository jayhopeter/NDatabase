using System;
using NDatabase.Core.Engine;
using NUnit.Framework;

namespace NDatabase.UnitTests.Layer3
{
    public class When_we_use_byte_array_converter
    {
        [Test]
        public void It_should_convert_decimal_to_binary_and_from_binary_to_decimal_with_the_success()
        {
            const decimal value = 1.23456789101112131415161718192021222324252627282930m;

            var byteArray = ByteArrayConverter.DecimalToByteArray(value);
            var convertedValue = ByteArrayConverter.ByteArrayToDecimal(byteArray);

            Assert.That(convertedValue, Is.EqualTo(value));
        }

        [Test]
        public void It_should_convert_double_to_binary_and_from_binary_to_double_with_the_success()
        {
            const double value = 1.2345678910111213141516171d;

            var byteArray = ByteArrayConverter.DoubleToByteArray(value);
            var convertedValue = ByteArrayConverter.ByteArrayToDouble(byteArray);

            Assert.That(convertedValue, Is.EqualTo(value));
        }

        [Test]
        public void It_should_convert_float_to_binary_and_from_binary_to_float_with_the_success()
        {
            const float value = 1.2345678910111213141516171f;

            var byteArray = ByteArrayConverter.FloatToByteArray(value);
            var convertedValue = ByteArrayConverter.ByteArrayToFloat(byteArray);

            Assert.That(convertedValue, Is.EqualTo(value));
        }

        [Test]
        public void It_should_convert_long_to_binary_and_from_binary_to_long_with_the_success()
        {
            const long value = 1234567891011121314L;

            var byteArray = ByteArrayConverter.LongToByteArray(value);
            var convertedValue = ByteArrayConverter.ByteArrayToLong(byteArray);

            Assert.That(convertedValue, Is.EqualTo(value));
        }

        [Test]
        public void It_should_convert_int_to_binary_and_from_binary_to_int_with_the_success()
        {
            const int value = 1234567891;

            var byteArray = ByteArrayConverter.IntToByteArray(value);
            var convertedValue = ByteArrayConverter.ByteArrayToInt(byteArray);

            Assert.That(convertedValue, Is.EqualTo(value));
        }

        [Test]
        public void It_should_convert_short_to_binary_and_from_binary_to_short_with_the_success()
        {
            const short value = 12345;

            var byteArray = ByteArrayConverter.ShortToByteArray(value);
            var convertedValue = ByteArrayConverter.ByteArrayToShort(byteArray);

            Assert.That(convertedValue, Is.EqualTo(value));
        }

        [Test]
        public void It_should_convert_char_to_binary_and_from_binary_to_char_with_the_success()
        {
            const char value = 'c';

            var byteArray = ByteArrayConverter.CharToByteArray(value);
            var convertedValue = ByteArrayConverter.ByteArrayToChar(byteArray);

            Assert.That(convertedValue, Is.EqualTo(value));
        }

        [Test]
        public void It_should_convert_bool_to_binary_and_from_binary_to_bool_with_the_success()
        {
            const bool value = true;

            var byteArray = ByteArrayConverter.BooleanToByteArray(value);
            var convertedValue = ByteArrayConverter.ByteArrayToBoolean(byteArray);

            Assert.That(convertedValue, Is.EqualTo(value));
        }

        [Test]
        public void It_should_convert_string_to_binary_and_from_binary_to_string_with_success()
        {
            const string name = "Magdalena Płatek-Spólnik";

            var byteArray = ByteArrayConverter.StringToByteArray(name, -1);
            var convertedValue = ByteArrayConverter.ByteArrayToString(byteArray);

            Assert.That(convertedValue, Is.EqualTo(name));
        }

        [Test]
        public void It_should_convert_string_to_binary_and_from_binary_to_string_with_success_with_size_bigger_than_string_size()
        {
            const string name = "Magdalena Płatek-Spólnik";

            var byteArray = ByteArrayConverter.StringToByteArray(name, 50);
            var convertedValue = ByteArrayConverter.ByteArrayToString(byteArray);

            Assert.That(convertedValue, Is.EqualTo(name));
        }

        [Test]
        public void It_should_convert_string_to_binary_and_from_binary_to_string_with_success_with_size_smaller_than_string_size()
        {
            const string name = "Magdalena Płatek-Spólnik";

            var byteArray = ByteArrayConverter.StringToByteArray(name, 10);
            var convertedValue = ByteArrayConverter.ByteArrayToString(byteArray);

            Assert.That(convertedValue, Is.EqualTo(name));
        }

        [Test]
        public void It_should_convert_datetime_to_binary_and_from_datetime_to_bool_with_the_success()
        {
            var value = new DateTime(1988, 8, 6);

            var byteArray = ByteArrayConverter.DateToByteArray(value);
            var convertedValue = ByteArrayConverter.ByteArrayToDate(byteArray);

            Assert.That(convertedValue, Is.EqualTo(value));
        }
    }
}
