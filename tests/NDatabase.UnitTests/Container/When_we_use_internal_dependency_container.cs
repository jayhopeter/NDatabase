using Moq;
using NDatabase.Api;
using NDatabase.Container;
using NDatabase.Core;
using NDatabase.Meta;
using NDatabase.Services;
using NUnit.Framework;

namespace NDatabase.UnitTests.Container
{
    public class When_we_use_internal_dependency_container
    {
        [Test]
        public void It_should_return_easily_registered_implementation_by_interface()
        {
            DependencyContainer.Register<IMetaModelCompabilityChecker>(() => new MetaModelCompabilityChecker());

            var checker = DependencyContainer.Resolve<IMetaModelCompabilityChecker>();
            Assert.That(checker, Is.Not.Null);
        }

        [Test]
        public void It_should_return_registered_implementation_which_requires_argument()
        {
            DependencyContainer.Register<IOdbForTrigger>((storageEngine) => new Odb((IStorageEngine) storageEngine));

            var mock = new Mock<IStorageEngine>();

            var odbForTrigger = DependencyContainer.Resolve<IOdbForTrigger>(mock.Object);

            Assert.That(odbForTrigger, Is.Not.Null);
        }
    }
}