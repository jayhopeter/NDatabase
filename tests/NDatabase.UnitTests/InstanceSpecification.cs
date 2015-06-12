using System.Diagnostics;
using NUnit.Framework;

namespace NDatabase.UnitTests
{
    public abstract class InstanceSpecification<TSubjectUnderTest>
    {
        [SetUp]
        public virtual void BaseSetUp()
        {
            Establish_context();
            Initialize_subject_under_test();
            Because();
        }

        [TearDown]
        public virtual void BaseTearDown()
        {
            Dispose_context();
        }

        [DebuggerStepThrough]
        protected virtual void Establish_context() { }

        [DebuggerStepThrough]
        protected virtual void Because() { }

        [DebuggerStepThrough]
        protected virtual void Dispose_context() { }

        [DebuggerStepThrough]
        protected virtual void Initialize_subject_under_test()
        {
            SubjectUnderTest = Create_subject_under_test();
        }

        protected abstract TSubjectUnderTest Create_subject_under_test();

        protected TSubjectUnderTest SubjectUnderTest { get; private set; }
    }

}
