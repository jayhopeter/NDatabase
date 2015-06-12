using NDatabase.Exceptions;
using NDatabase.Tool.Wrappers;
using NUnit.Framework;

namespace NDatabase.UnitTests.Utils
{
    public class When_working_with_string_utils
    {
        [Test]
        public void It_should_replace_in_any_string_any_token_given_number_times()
        {
            const string input = "val%val%";

            var result = ExceptionsHelper.ReplaceToken(input, "%", "(.)*", 1);
            Assert.That(result, Is.EqualTo("val(.)*val%"));
        }

        [Test]
        public void It_should_throw_exception_if_we_are_looking_for_non_existing_token_with_replace_all_setting()
        {
            const string input = "val%val%";
            Assert.That(() => ExceptionsHelper.ReplaceToken(input, "%", "%%", -1), Throws.Exception);
        }

        [Test]
        public void It_should_match_any_string_by_regex()
        {
            const string input = "Julia Spólnik";
            var result = OdbString.Matches("Julia.*", input);

            Assert.That(result, Is.True);
        }
    }
}