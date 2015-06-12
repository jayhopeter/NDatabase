using System.IO;
using NDatabase.IO;
using NUnit.Framework;

namespace NDatabase.UnitTests.Layer3
{
    [Category("IO")]
    public class When_we_use_odb_directory
    {
        private const string ExistingDbName = "dummy.ndb";
        private const string NonExistingDbName = "folder\\subfolder\\nonexisting.ndb";

        [Test]
        public void It_should_do_nothing_when_want_to_mkdirs_for_existing_path()
        {
            OdbDirectory.Mkdirs(ExistingDbName);
            Assert.Pass();
        }

        [Test]
        public void It_should_create_all_non_existing_dirs_after_mkdir_invocation()
        {
            OdbDirectory.Mkdirs(NonExistingDbName);
            Assert.That(Directory.Exists("folder"), Is.True);
            Assert.That(Directory.Exists("folder\\subfolder"), Is.True);
        }
    }
}