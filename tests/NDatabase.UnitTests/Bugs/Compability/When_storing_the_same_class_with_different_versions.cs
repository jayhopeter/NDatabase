using System;
using System.Collections.Generic;
using Moq;
using NDatabase.Meta;
using NDatabase.Tool.Wrappers;
using NDatabase.UnitTests.TestData;
using NUnit.Framework;

namespace NDatabase.UnitTests.Bugs.Compability
{
    public class When_storing_the_same_class_with_different_versions
    {
        [Test]
        public void It_should_recognize_properly_the_type()
        {
            IList<ClassInfo> allClasses = new List<ClassInfo>();
            var classInfo = new ClassInfo("NDatabase.UnitTests.TestData.Person, NDatabase.UnitTests, Version=1.3.4816.22343, Culture=neutral, PublicKeyToken=null")
                                { Attributes = new OdbList<ClassAttributeInfo>()};
            allClasses.Add(classInfo);

            var metaModelMock = new Mock<IMetaModel>();
            metaModelMock.Setup(x => x.GetAllClasses()).Returns(() => allClasses);

            var currentCIs = new Dictionary<Type, ClassInfo>();
            var key = typeof(Person);
            var classInfo2 = new ClassInfo(key) {Attributes = new OdbList<ClassAttributeInfo>()};
            currentCIs.Add(key, classInfo2);

            var shouldUpdate = new MetaModelCompabilityChecker().Check(currentCIs, metaModelMock.Object);

            if (shouldUpdate)
                Assert.Fail();
            else
                Assert.Pass();
        }
    }
}