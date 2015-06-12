using System;
using System.Collections.Generic;
using NDatabase.Northwind.Domain;
using NDatabase.TypeResolution;
using NUnit.Framework;

namespace NDatabase.UnitTests.TypeResolution
{
    public class TypeResolution_TestCase
    {
        [Test]
        public void Test_simple_case_with_northwind_library()
        {
            var resolvedType = TypeResolutionUtils.ResolveType("NDatabase.Northwind.Domain.Category");

            Assert.That(resolvedType, Is.Not.Null);
            Assert.That(resolvedType, Is.EqualTo(typeof (Category)));
        }

        [Test]
        public void Test_case_with_assembly_qualified_name_from_type()
        {
            var resolvedType = TypeResolutionUtils.ResolveType(typeof (Category).AssemblyQualifiedName);

            Assert.That(resolvedType, Is.Not.Null);
            Assert.That(resolvedType, Is.EqualTo(typeof (Category)));
        }

        [Test]
        public void Test_simple_case_with_northwind_library_for_assembly_qualified_name()
        {
            var resolvedType =
                TypeResolutionUtils.ResolveType("NDatabase.Northwind.Domain.Category, NDatabase.Northwind.Domain");

            Assert.That(resolvedType, Is.Not.Null);
            Assert.That(resolvedType, Is.EqualTo(typeof (Category)));
        }

        [Test]
        public void Test_resolution_of_non_existing_type()
        {
            var resolvedType = TypeResolutionUtils.ResolveType("NonExisting.Class");
            Assert.That(resolvedType, Is.Null);
        }

        [Test]
        [ExpectedException(typeof (ArgumentNullException))]
        public void Test_resolution_of_null()
        {
            TypeResolutionUtils.ResolveType(null);
        }

        [Test]
        public void LoadTypeFromSystemAssemblySpecifyingOnlyTheAssemblyDisplayName()
        {
            var stringType = typeof (string).FullName + ", System";
            var t = TypeResolutionUtils.ResolveType(stringType);
            Assert.AreEqual(typeof (string), t);
        }

        [Test]
        public void LoadTypeFromSystemAssemblySpecifyingTheFullAssemblyName()
        {
            var stringType = typeof (string).AssemblyQualifiedName;
            var t = TypeResolutionUtils.ResolveType(stringType);
            Assert.AreEqual(typeof (string), t);
        }

        [Test]
        public void CanTakeQualifiedType()
        {
            var testType = typeof (Category);
            var typeAssemblyHolder = new TypeAssemblyHolder(testType.AssemblyQualifiedName);
            Assert.IsTrue(typeAssemblyHolder.IsAssemblyQualified);
            Assert.AreEqual(testType.FullName, typeAssemblyHolder.GetTypeName());
            Assert.AreEqual(testType.Assembly.FullName, typeAssemblyHolder.GetAssemblyName());
        }

        [Test]
        public void CanTakeUnqualifiedType()
        {
            var testType = typeof (Category);
            var typeAssemblyHolder = new TypeAssemblyHolder(testType.FullName);
            Assert.IsFalse(typeAssemblyHolder.IsAssemblyQualified);
            Assert.AreEqual(testType.FullName, typeAssemblyHolder.GetTypeName());
            Assert.AreEqual(null, typeAssemblyHolder.GetAssemblyName());
        }

        [Test]
        public void CanTakeUnqualifiedGenericType()
        {
            var testType = typeof (TestGenericObject<int, string>);
            var tah = new TypeAssemblyHolder(testType.FullName);
            Assert.IsFalse(tah.IsAssemblyQualified);
            Assert.AreEqual(testType.FullName, tah.GetTypeName());
            Assert.AreEqual(null, tah.GetAssemblyName());
        }

        [Test]
        public void CanTakeQualifiedGenericType()
        {
            var testType = typeof (TestGenericObject<int, string>);
            var tah = new TypeAssemblyHolder(testType.AssemblyQualifiedName);
            Assert.IsTrue(tah.IsAssemblyQualified);
            Assert.AreEqual(testType.FullName, tah.GetTypeName());
            Assert.AreEqual(testType.Assembly.FullName, tah.GetAssemblyName());
        }

        [Test]
        public void ResolveLocalAssemblyGenericType()
        {
            var t = TypeResolutionUtils.ResolveType("NDatabase.UnitTests.TypeResolution.TestGenericObject< int, string>");
            Assert.AreEqual(typeof (TestGenericObject<int, string>), t);
        }

        [Test]
        public void ResolveLocalAssemblyGenericTypeDefinition()
        {
            // CLOVER:ON
            var t = TypeResolutionUtils.ResolveType("NDatabase.UnitTests.TypeResolution.TestGenericObject< ,>");
            // CLOVER:OFF
            Assert.AreEqual(typeof (TestGenericObject<,>), t);
        }

        [Test]
        [ExpectedException(typeof (TypeLoadException))]
        public void ResolveLocalAssemblyGenericTypeOpen()
        {
            TypeResolutionUtils.ResolveType("NDatabase.UnitTests.TypeResolution.TestGenericObject<int >");
        }

        [Test]
        public void ResolveGenericTypeWithAssemblyName()
        {
            var t = TypeResolutionUtils.ResolveType("System.Collections.Generic.Stack<string>, System");
            Assert.AreEqual(typeof (Stack<string>), t);
        }

        [Test]
        public void ResolveGenericArrayType()
        {
            var t = TypeResolutionUtils.ResolveType("System.Nullable<[System.Int32, mscorlib]>[,]");
            Assert.AreEqual(typeof (int?[,]), t);
            t = TypeResolutionUtils.ResolveType("System.Nullable`1[int][,]");
            Assert.AreEqual(typeof (int?[,]), t);
        }

        [Test]
        public void ResolveGenericArrayTypeWithAssemblyName()
        {
            var t = TypeResolutionUtils.ResolveType("System.Nullable<[System.Int32, mscorlib]>[,], mscorlib");
            Assert.AreEqual(typeof (int?[,]), t);
            t = TypeResolutionUtils.ResolveType("System.Nullable<[System.Int32, mscorlib]>[,], mscorlib");
            Assert.AreEqual(typeof (int?[,]), t);
            t = TypeResolutionUtils.ResolveType("System.Nullable`1[[System.Int32, mscorlib]][,], mscorlib");
            Assert.AreEqual(typeof (int?[,]), t);
        }

        [Test]
        [ExpectedException(typeof (TypeLoadException))]
        public void ResolveAmbiguousGenericTypeWithAssemblyName()
        {
            var t =
                TypeResolutionUtils.ResolveType(
                    "NDatabase.UnitTests.TypeResolution.TestGenericObject<System.Collections.Generic.Stack<int>, System, string>");
        }

        [Test]
        [ExpectedException(typeof (TypeLoadException))]
        public void ResolveMalformedGenericType()
        {
            var t =
                TypeResolutionUtils.ResolveType("NDatabase.UnitTests.TypeResolution.TestGenericObject<int, <string>>");
        }

        [Test]
        public void ResolveNestedGenericTypeWithAssemblyName()
        {
            var t =
                TypeResolutionUtils.ResolveType(
                    "System.Collections.Generic.Stack< NDatabase.UnitTests.TypeResolution.TestGenericObject<int, string> >, System");
            Assert.AreEqual(typeof (Stack<TestGenericObject<int, string>>), t);
        }

        [Test]
        public void ResolveClrNotationStyleGenericTypeWithAssemblyName()
        {
            var t =
                TypeResolutionUtils.ResolveType(
                    "System.Collections.Generic.Stack`1[ [NDatabase.UnitTests.TypeResolution.TestGenericObject`2[int, string], NDatabase.UnitTests] ], System");
            Assert.AreEqual(typeof (Stack<TestGenericObject<int, string>>), t);
        }

        [Test]
        public void ResolveNestedQuotedGenericTypeWithAssemblyName()
        {
            var t =
                TypeResolutionUtils.ResolveType(
                    "System.Collections.Generic.Stack< [NDatabase.UnitTests.TypeResolution.TestGenericObject<int, string>, NDatabase.UnitTests] >, System");
            Assert.AreEqual(typeof (Stack<TestGenericObject<int, string>>), t);
        }
    }
}