using System.IO;
using NDatabase.IO;
using NUnit.Framework;

namespace NDatabase.UnitTests.Layer3
{
    internal class When_we_use_file_identification_as_db_identification : InstanceSpecification<IDbIdentification>
    {
        private bool _isNew;
        private FileIdentification _fileIdentificationForNonExistingFile;
        private string _currentDirectory;

        private const string ExistingDbName = "fileidentification.ndb";
        private const string NonExistingDbName = "folder\\nonexisting.ndb";

        protected override void Establish_context()
        {
            File.Create(ExistingDbName).Dispose();

            _currentDirectory = Directory.GetCurrentDirectory();
            _fileIdentificationForNonExistingFile = new FileIdentification(NonExistingDbName);
        }

        protected override IDbIdentification Create_subject_under_test()
        {
            return new FileIdentification(ExistingDbName);
        }

        protected override void Because()
        {
            _isNew = SubjectUnderTest.IsNew();
        }

        [Test]
        public void It_should_treat_existing_db_files_as_non_new_db()
        {
            Assert.That(_isNew, Is.False);
        }

        [Test]
        public void It_should_return_pure_file_name_in_the_case_when_you_dont_specify_directories_in_string_arg_for_ctor()
        {
            Assert.That(SubjectUnderTest.FileName, Is.EqualTo(ExistingDbName));
            Assert.That(SubjectUnderTest.ToString(), Is.EqualTo(ExistingDbName));
        }

        [Test]
        public void It_should_return_directory_equals_to_actual_application_directory()
        {
            Assert.That(SubjectUnderTest.Directory, Is.EqualTo(_currentDirectory));
        }

        [Test]
        public void It_should_treat_non_existing_db_file_as_the_new_db()
        {
            Assert.That(_fileIdentificationForNonExistingFile.IsNew(), Is.True);
        }

        [Test]
        public void It_should_return_id_as_the_pure_file_name()
        {
            Assert.That(_fileIdentificationForNonExistingFile.Id, Is.EqualTo("nonexisting.ndb"));
        }

        [Test]
        public void It_should_return_file_name_as_it_was_given_to_ctor()
        {
            Assert.That(_fileIdentificationForNonExistingFile.FileName, Is.EqualTo(NonExistingDbName));
            Assert.That(_fileIdentificationForNonExistingFile.ToString(), Is.EqualTo(NonExistingDbName));
        }

        [Test]
        public void It_should_return_directory_equals_to_actual_application_directory_extended_with_a_given_folder_in_a_filename()
        {
            Assert.That(_fileIdentificationForNonExistingFile.Directory,
                        Is.EqualTo(Path.Combine(_currentDirectory, "folder")));
        }
    }
}