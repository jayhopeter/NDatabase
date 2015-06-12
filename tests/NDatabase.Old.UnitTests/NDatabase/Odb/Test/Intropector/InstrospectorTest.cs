using NDatabase;
using NDatabase.Api;
using NDatabase.Meta;
using NDatabase.Meta.Compare;
using NDatabase.Meta.Introspector;
using NDatabase.Oid;
using NDatabase.Tool.Wrappers;
using NDatabase.Triggers;
using NUnit.Framework;
using Test.NDatabase.Odb.Test.VO.Inheritance;
using Test.NDatabase.Odb.Test.VO.Login;

namespace Test.NDatabase.Odb.Test.Intropector
{
    public class InstrospectorTest : ODBTest
    {
        [Test]
        public void TestClassInfo()
        {
            var user = new User("olivier smadja", "user@ndatabase.net",
                                new Profile("operator", new VO.Login.Function("login")));
            var classInfoList = ClassIntrospector.Introspect(user.GetType(), true);
            AssertEquals(OdbClassNameResolver.GetFullName(user.GetType()), classInfoList.GetMainClassInfo().FullClassName);
            AssertEquals(3, classInfoList.GetMainClassInfo().Attributes.Count);
            AssertEquals(3, classInfoList.GetClassInfos().Count);
        }

        [Test]
        public void TestInstanceInfo()
        {
            const string dbName = "TestInstanceInfo.odb";
            DeleteBase(dbName);
            var odb = OdbFactory.Open(dbName);

            var user = new User("olivier smadja", "user@ndatabase.net",
                                new Profile("operator", new VO.Login.Function("login")));
            var ci = ClassIntrospector.Introspect(user.GetType(), true).GetMainClassInfo();

            var storageEngine = ((global::NDatabase.Odb)odb).GetStorageEngine();

            var instanceInfo =
                (NonNativeObjectInfo)
                new ObjectIntrospector(storageEngine.GetClassInfoProvider()).GetMetaRepresentation(user, true, null,
                                                                            new InstrumentationCallbackForStore(null,
                                                                                                                false));
            AssertEquals(OdbClassNameResolver.GetFullName(user.GetType()), instanceInfo.GetClassInfo().FullClassName);
            AssertEquals("olivier smadja", instanceInfo.GetAttributeValueFromId(ci.GetAttributeId("name")).ToString());
            AssertEquals(typeof (AtomicNativeObjectInfo),
                         instanceInfo.GetAttributeValueFromId(ci.GetAttributeId("name")).GetType());

            odb.Close();
        }

        [Test]
        public void TestInstanceInfo2()
        {
            const string dbName = "TestInstanceInfo2.odb";
            DeleteBase(dbName);
            var odb = OdbFactory.Open(dbName);

            var user = new User("olivier smadja", "user@ndatabase.net",
                                new Profile("operator", new VO.Login.Function("login")));
            var ci = ClassIntrospector.Introspect(user.GetType(), true).GetMainClassInfo();

            var storageEngine = ((global::NDatabase.Odb)odb).GetStorageEngine();

            var instanceInfo =
                (NonNativeObjectInfo)
                new ObjectIntrospector(storageEngine.GetClassInfoProvider()).GetMetaRepresentation(user, true, null,
                                                                            new InstrumentationCallbackForStore(null,
                                                                                                                false));
            AssertEquals(instanceInfo.GetClassInfo().FullClassName, OdbClassNameResolver.GetFullName(user.GetType()));
            AssertEquals(instanceInfo.GetAttributeValueFromId(ci.GetAttributeId("name")).ToString(), "olivier smadja");

            odb.Close();
        }

        [Test]
        public void TestCompareCollection1()
        {
            const string dbName = "introspectortest1.odb";
            DeleteBase(dbName);
            var odb = OdbFactory.Open(dbName);

            var user = new User("olivier smadja", "user@ndatabase.net",
                                new Profile("operator", new VO.Login.Function("login")));
            IObjectInfoComparator comparator = new ObjectInfoComparator();
            
            var storageEngine = ((global::NDatabase.Odb)odb).GetStorageEngine();

            var instanceInfo =
                (NonNativeObjectInfo)
                new ObjectIntrospector(storageEngine.GetClassInfoProvider()).GetMetaRepresentation(user, true, null,
                                                                            new InstrumentationCallbackForStore(null,
                                                                                                                false));
            // Sets attributes offsets - this is normally done by reading then from
            // disk, but in this junit,
            // we must set them manually
            var offsets = new[] {1L, 2L, 3L};
            var ids = new[] {1, 2, 3};
            instanceInfo.GetHeader().SetAttributesIdentification(offsets);
            instanceInfo.GetHeader().SetAttributesIds(ids);
            instanceInfo.GetHeader().SetOid(OIDFactory.BuildObjectOID(1));
            var nnoiProfile = (NonNativeObjectInfo) instanceInfo.GetAttributeValueFromId(3);
            nnoiProfile.SetOid(OIDFactory.BuildObjectOID(2));

            var nnoiFunctions = (NonNativeObjectInfo) nnoiProfile.GetAttributeValueFromId(1);
            nnoiFunctions.SetOid(OIDFactory.BuildObjectOID(3));

            user.SetName("Olivier Smadja");
            var instanceInfo3 =
                (NonNativeObjectInfo)
                new ObjectIntrospector(storageEngine.GetClassInfoProvider()).GetMetaRepresentation(user, true, null,
                                                                            new InstrumentationCallbackForStore(null,
                                                                                                                false));
            instanceInfo3.GetHeader().SetOid(OIDFactory.BuildObjectOID(1));
            nnoiProfile = (NonNativeObjectInfo) instanceInfo3.GetAttributeValueFromId(3);
            nnoiProfile.SetOid(OIDFactory.BuildObjectOID(2));

            nnoiFunctions = (NonNativeObjectInfo)nnoiProfile.GetAttributeValueFromId(1);
            nnoiFunctions.SetOid(OIDFactory.BuildObjectOID(3));

            AssertTrue(comparator.HasChanged(instanceInfo, instanceInfo3));
            AssertEquals(1, comparator.GetNbChanges());
            
            odb.Close();
        }

        [Test]
        public void TestCompareCollection11()
        {
            const string dbName = "introspectortest2.odb";
            DeleteBase(dbName);
            var odb = OdbFactory.Open(dbName);

            var user = new User("olivier smadja", "user@ndatabase.net",
                                new Profile("operator", new VO.Login.Function("login")));
            IObjectInfoComparator comparator = new ObjectInfoComparator();
            
            var storageEngine = ((global::NDatabase.Odb)odb).GetStorageEngine();

            var instanceInfo =
                (NonNativeObjectInfo)
                new ObjectIntrospector(storageEngine.GetClassInfoProvider()).GetMetaRepresentation(user, true, null,
                                                                            new InstrumentationCallbackForStore(null,
                                                                                                                false));
            // Sets attributes offsets - this is normally done by reading then from
            // disk, but in this junit,
            // we must set them manually
            var offsets = new[] {1L, 2L, 3L};
            var ids = new[] {1, 2, 3};
            instanceInfo.GetHeader().SetAttributesIdentification(offsets);
            instanceInfo.GetHeader().SetAttributesIds(ids);
            instanceInfo.GetHeader().SetOid(OIDFactory.BuildObjectOID(1));
            object o = instanceInfo.GetAttributeValueFromId(3);
            var nnoiProfile = (NonNativeObjectInfo) o;
            nnoiProfile.SetOid(OIDFactory.BuildObjectOID(2));

            var nnoiFunctions = (NonNativeObjectInfo)nnoiProfile.GetAttributeValueFromId(1);
            nnoiFunctions.SetOid(OIDFactory.BuildObjectOID(3));

            user.SetName("Olivier Smadja");
            user.SetEmail("olivier@ndatabase.net");
            var instanceInfo3 =
                (NonNativeObjectInfo)
                new ObjectIntrospector(storageEngine.GetClassInfoProvider()).GetMetaRepresentation(user, true, null,
                                                                            new InstrumentationCallbackForStore(null,
                                                                                                                false));
            instanceInfo3.GetHeader().SetOid(OIDFactory.BuildObjectOID(1));
            nnoiProfile = (NonNativeObjectInfo) instanceInfo3.GetAttributeValueFromId(3);
            nnoiProfile.SetOid(OIDFactory.BuildObjectOID(2));

            nnoiFunctions = (NonNativeObjectInfo)nnoiProfile.GetAttributeValueFromId(1);
            nnoiFunctions.SetOid(OIDFactory.BuildObjectOID(3));

            AssertTrue(comparator.HasChanged(instanceInfo, instanceInfo3));
            AssertEquals(2, comparator.GetNbChanges());
            
            odb.Close();
        }

        [Test]
        public void TestCompareCollection2()
        {
            const string dbName = "introspectortest3.odb";
            DeleteBase(dbName);
            var odb = OdbFactory.Open(dbName);

            var user = new User("olivier smadja", "user@ndatabase.net",
                                new Profile("operator", new VO.Login.Function("login")));

            IObjectInfoComparator comparator = new ObjectInfoComparator();
            
            var storageEngine = ((global::NDatabase.Odb)odb).GetStorageEngine();

            var instanceInfo =
                (NonNativeObjectInfo)
                new ObjectIntrospector(storageEngine.GetClassInfoProvider()).GetMetaRepresentation(user, true, null,
                                                                            new InstrumentationCallbackForStore(null,
                                                                                                                false));
            // Sets attributes offsets - this is normally done by reading them from
            // disk, but in this junit,
            // we must set them manually
            var offsets = new[] {1L, 2L, 3L};
            var ids = new[] {1, 2, 3};
            instanceInfo.GetHeader().SetAttributesIdentification(offsets);
            instanceInfo.GetHeader().SetAttributesIds(ids);
            instanceInfo.GetHeader().SetOid(OIDFactory.BuildObjectOID(1));

            var nnoiProfile = (NonNativeObjectInfo) instanceInfo.GetAttributeValueFromId(3);
            nnoiProfile.SetOid(OIDFactory.BuildObjectOID(2));

            var nnoiFunctions = (NonNativeObjectInfo)nnoiProfile.GetAttributeValueFromId(1);
            nnoiFunctions.SetOid(OIDFactory.BuildObjectOID(3));

            user.SetName(null);

            var instanceInfo3 =
                (NonNativeObjectInfo)
                new ObjectIntrospector(storageEngine.GetClassInfoProvider()).GetMetaRepresentation(user, true, null,
                                                                            new InstrumentationCallbackForStore(null,
                                                                                                                false));
            instanceInfo3.GetHeader().SetAttributesIdentification(offsets);
            instanceInfo3.GetHeader().SetAttributesIds(ids);
            instanceInfo3.GetHeader().SetOid(OIDFactory.BuildObjectOID(1));

            nnoiProfile = (NonNativeObjectInfo) instanceInfo3.GetAttributeValueFromId(3);
            nnoiProfile.SetOid(OIDFactory.BuildObjectOID(2));

            nnoiFunctions = (NonNativeObjectInfo)nnoiProfile.GetAttributeValueFromId(1);
            nnoiFunctions.SetOid(OIDFactory.BuildObjectOID(3));

            AssertTrue(comparator.HasChanged(instanceInfo, instanceInfo3));
            AssertEquals(1, comparator.GetNbChanges());
            
            odb.Close();
        }

        [Test]
        public void TestCompareCollection4CollectionContentChange()
        {
            const string dbName = "introspectortest22.odb";
            DeleteBase(dbName);
            var odb = OdbFactory.Open(dbName);

            var function = new VO.Login.Function("login");
            var user = new User("olivier smadja", "user@ndatabase.net", new Profile("operator", function));

            var storageEngine = ((global::NDatabase.Odb)odb).GetStorageEngine();

            var instanceInfo =
                (NonNativeObjectInfo)
                new ObjectIntrospector(storageEngine.GetClassInfoProvider()).GetMetaRepresentation(user, true, null,
                                                                            new InstrumentationCallbackForStore(null,
                                                                                                                false));
            // Sets attributes offsets - this is normally done by reading then from
            // disk, but in this junit,
            // we must set them manually
            var offsets = new[] {1L, 2L, 3L};
            var ids = new[] {1, 2, 3};
            instanceInfo.GetHeader().SetAttributesIdentification(offsets);
            instanceInfo.GetHeader().SetAttributesIds(ids);
            instanceInfo.GetHeader().SetOid(OIDFactory.BuildObjectOID(1));
            var nnoiProfile = (NonNativeObjectInfo) instanceInfo.GetAttributeValueFromId(3);
            nnoiProfile.SetOid(OIDFactory.BuildObjectOID(2));

            var nnoiFunctions = (NonNativeObjectInfo)nnoiProfile.GetAttributeValueFromId(1);
            nnoiFunctions.SetOid(OIDFactory.BuildObjectOID(3));

            function.SetName(null);

            var instanceInfo3 =
                (NonNativeObjectInfo)
                new ObjectIntrospector(storageEngine.GetClassInfoProvider()).GetMetaRepresentation(user, true, null,
                                                                            new InstrumentationCallbackForStore(null,
                                                                                                                false));
            instanceInfo3.GetHeader().SetAttributesIdentification(offsets);
            instanceInfo3.GetHeader().SetAttributesIds(ids);
            instanceInfo3.GetHeader().SetOid(OIDFactory.BuildObjectOID(1));

            nnoiProfile = (NonNativeObjectInfo) instanceInfo3.GetAttributeValueFromId(3);
            nnoiProfile.SetOid(OIDFactory.BuildObjectOID(2));

            nnoiFunctions = (NonNativeObjectInfo)nnoiProfile.GetAttributeValueFromId(1);
            nnoiFunctions.SetOid(OIDFactory.BuildObjectOID(3));

            IObjectInfoComparator comparator = new ObjectInfoComparator();
            AssertTrue(comparator.HasChanged(instanceInfo, instanceInfo3));
            AssertEquals(2, comparator.GetNbChanges());
            
            odb.Close();
        }

        [Test]
        public void TestCompareCollection5()
        {
            const string dbName = "introspectortest5.odb";
            DeleteBase(dbName);
            var odb = OdbFactory.Open(dbName);

            var function = new VO.Login.Function("login");
            var profile = new Profile("operator", function);
            var user = new User("olivier smadja", "user@ndatabase.net", profile);
            IObjectInfoComparator comparator = new ObjectInfoComparator();
            
            var storageEngine = ((global::NDatabase.Odb)odb).GetStorageEngine();

            var instanceInfo =
                (NonNativeObjectInfo)
                new ObjectIntrospector(storageEngine.GetClassInfoProvider()).GetMetaRepresentation(user, true, null,
                                                                            new InstrumentationCallbackForStore(null,
                                                                                                                false));
            // Sets attributes offsets - this is normally done by reading then from
            // disk, but in this junit,
            // we must set them manually
            var offsets = new[] {1L, 2L, 3L};
            var ids = new[] {1, 2, 3};
            instanceInfo.GetHeader().SetAttributesIdentification(offsets);
            instanceInfo.GetHeader().SetAttributesIds(ids);
            instanceInfo.GetHeader().SetOid(OIDFactory.BuildObjectOID(1));
            var nnoiProfile = (NonNativeObjectInfo) instanceInfo.GetAttributeValueFromId(3);
            nnoiProfile.SetOid(OIDFactory.BuildObjectOID(2));

            var nnoiFunctions = (NonNativeObjectInfo)nnoiProfile.GetAttributeValueFromId(1);
            nnoiFunctions.SetOid(OIDFactory.BuildObjectOID(3));

            profile.GetFunctions().Add(new VO.Login.Function("logout"));
            var instanceInfo3 =
                (NonNativeObjectInfo)
                new ObjectIntrospector(storageEngine.GetClassInfoProvider()).GetMetaRepresentation(user, true, null,
                                                                            new InstrumentationCallbackForStore(null,
                                                                                                                false));
            instanceInfo3.GetHeader().SetOid(OIDFactory.BuildObjectOID(1));
            nnoiProfile = (NonNativeObjectInfo) instanceInfo3.GetAttributeValueFromId(3);
            nnoiProfile.SetOid(OIDFactory.BuildObjectOID(2));

            nnoiFunctions = (NonNativeObjectInfo)nnoiProfile.GetAttributeValueFromId(1);
            nnoiFunctions.SetOid(OIDFactory.BuildObjectOID(3));

            AssertTrue(comparator.HasChanged(instanceInfo, instanceInfo3));
            AssertEquals(3, comparator.GetNbChanges());
            
            odb.Close();
        }

        [Test]
        public void TestCompareCollection6()
        {
            const string dbName = "introspectortest6.odb";
            DeleteBase(dbName);
            var odb = OdbFactory.Open(dbName);

            var function = new VO.Login.Function("login");
            var profile = new Profile("operator", function);
            var user = new User("olivier smadja", "user@ndatabase.net", profile);
            IObjectInfoComparator comparator = new ObjectInfoComparator();
            var ci = ClassIntrospector.Introspect(user.GetType(), true).GetMainClassInfo();

            var storageEngine = ((global::NDatabase.Odb)odb).GetStorageEngine();

            var instanceInfo =
                (NonNativeObjectInfo)
                new ObjectIntrospector(storageEngine.GetClassInfoProvider()).GetMetaRepresentation(user, true, null,
                                                                            new InstrumentationCallbackForStore(null,
                                                                                                                false));
            // Sets attributes offsets - this is normally done by reading then from
            // disk, but in this junit,
            // we must set them manually
            var offsets = new[] {1L, 2L, 3L};
            var ids = new[] {1, 2, 3};
            instanceInfo.GetHeader().SetAttributesIdentification(offsets);
            instanceInfo.GetHeader().SetAttributesIds(ids);
            instanceInfo.GetHeader().SetOid(OIDFactory.BuildObjectOID(1));
            var nnoiProfile = (NonNativeObjectInfo) instanceInfo.GetAttributeValueFromId(3);
            nnoiProfile.SetOid(OIDFactory.BuildObjectOID(2));

            var nnoiFunctions = (NonNativeObjectInfo)nnoiProfile.GetAttributeValueFromId(1);
            nnoiFunctions.SetOid(OIDFactory.BuildObjectOID(3));

            var nnoi = (NonNativeObjectInfo) instanceInfo.GetAttributeValueFromId(ci.GetAttributeId("profile"));
            nnoi.GetHeader().SetAttributesIdentification(offsets);
            nnoi.GetHeader().SetAttributesIds(ids);
            profile.SetName("ope");
            var instanceInfo3 =
                (NonNativeObjectInfo)
                new ObjectIntrospector(storageEngine.GetClassInfoProvider()).GetMetaRepresentation(user, true, null,
                                                                            new InstrumentationCallbackForStore(null,
                                                                                                                false));
            instanceInfo3.GetHeader().SetOid(OIDFactory.BuildObjectOID(1));
            nnoiProfile = (NonNativeObjectInfo) instanceInfo3.GetAttributeValueFromId(3);
            nnoiProfile.SetOid(OIDFactory.BuildObjectOID(2));

            nnoiFunctions = (NonNativeObjectInfo)nnoiProfile.GetAttributeValueFromId(1);
            nnoiFunctions.SetOid(OIDFactory.BuildObjectOID(3));

            AssertTrue(comparator.HasChanged(instanceInfo, instanceInfo3));
            AssertEquals(1, comparator.GetNbChanges());
            
            odb.Close();
        }

        [Test]
        public void TestCompareCollection7()
        {
            const string dbName = "introspectortest7.odb";
            DeleteBase(dbName);
            var odb = OdbFactory.Open(dbName);

            var function = new VO.Login.Function("login");
            var profile = new Profile("operator", function);
            var user = new User("olivier smadja", "user@ndatabase.net", profile);
            IObjectInfoComparator comparator = new ObjectInfoComparator();
            
            var storageEngine = ((global::NDatabase.Odb)odb).GetStorageEngine();

            var instanceInfo =
                (NonNativeObjectInfo)
                new ObjectIntrospector(storageEngine.GetClassInfoProvider()).GetMetaRepresentation(user, true, null,
                                                                            new InstrumentationCallbackForStore(null,
                                                                                                                false));
            // Sets attributes offsets - this is normally done by reading then from
            // disk, but in this junit,
            // we must set them manually
            var offsets = new[] {1L, 2L, 3L};
            var ids = new[] {1, 2, 3};
            instanceInfo.GetHeader().SetAttributesIdentification(offsets);
            instanceInfo.GetHeader().SetAttributesIds(ids);
            instanceInfo.GetHeader().SetOid(OIDFactory.BuildObjectOID(1));
            var nnoiProfile = (NonNativeObjectInfo) instanceInfo.GetAttributeValueFromId(3);
            nnoiProfile.SetOid(OIDFactory.BuildObjectOID(2));

            var nnoiFunctions = (NonNativeObjectInfo)nnoiProfile.GetAttributeValueFromId(1);
            nnoiFunctions.SetOid(OIDFactory.BuildObjectOID(3));

            // / Set the same name
            profile.SetName("operator");
            var instanceInfo3 =
                (NonNativeObjectInfo)
                new ObjectIntrospector(storageEngine.GetClassInfoProvider()).GetMetaRepresentation(user, true, null,
                                                                            new InstrumentationCallbackForStore(null,
                                                                                                                false));
            instanceInfo3.GetHeader().SetOid(OIDFactory.BuildObjectOID(1));
            nnoiProfile = (NonNativeObjectInfo) instanceInfo3.GetAttributeValueFromId(3);
            nnoiProfile.SetOid(OIDFactory.BuildObjectOID(2));

            nnoiFunctions = (NonNativeObjectInfo)nnoiProfile.GetAttributeValueFromId(1);
            nnoiFunctions.SetOid(OIDFactory.BuildObjectOID(3));

            AssertFalse(comparator.HasChanged(instanceInfo, instanceInfo3));
            AssertEquals(0, comparator.GetNbChanges());

            odb.Close();
        }

        [Test]
        public void TestCompareCollection8()
        {
            const string dbName = "introspectortest8.odb";
            DeleteBase(dbName);
            var odb = OdbFactory.Open(dbName);

            var function = new VO.Login.Function("login");
            var profile = new Profile("operator", function);
            var user = new User("olivier smadja", "user@ndatabase.net", profile);
            IObjectInfoComparator comparator = new ObjectInfoComparator();
            
            var storageEngine = ((global::NDatabase.Odb)odb).GetStorageEngine();

            var instanceInfo =
                (NonNativeObjectInfo)
                new ObjectIntrospector(storageEngine.GetClassInfoProvider()).GetMetaRepresentation(user, true, null,
                                                                            new InstrumentationCallbackForStore(null,
                                                                                                                false));
            // Sets attributes offsets - this is normally done by reading then from
            // disk, but in this junit,
            // we must set them manually
            var offsets = new[] {1L, 2L, 3L};
            var ids = new[] {1, 2, 3};
            instanceInfo.GetHeader().SetAttributesIdentification(offsets);
            instanceInfo.GetHeader().SetAttributesIds(ids);
            user.SetProfile(null);
            var instanceInfo3 =
                (NonNativeObjectInfo)
                new ObjectIntrospector(storageEngine.GetClassInfoProvider()).GetMetaRepresentation(user, true, null,
                                                                            new InstrumentationCallbackForStore(null,
                                                                                                                false));
            AssertTrue(comparator.HasChanged(instanceInfo, instanceInfo3));
            AssertEquals(1, comparator.GetNbChanges());

            odb.Close();
        }

        [Test]
        public void TestCompareCollection9()
        {
            const string dbName = "introspectortest9.odb";
            DeleteBase(dbName);
            var odb = OdbFactory.Open(dbName);

            var function = new VO.Login.Function("login");
            var profile = new Profile("operator", function);
            var user = new User("olivier smadja", "user@ndatabase.net", profile);
            IObjectInfoComparator comparator = new ObjectInfoComparator();
            
            var storageEngine = ((global::NDatabase.Odb)odb).GetStorageEngine();

            var instanceInfo =
                (NonNativeObjectInfo)
                new ObjectIntrospector(storageEngine.GetClassInfoProvider()).GetMetaRepresentation(user, true, null,
                                                                            new InstrumentationCallbackForStore(null,
                                                                                                                false));
            // Sets attributes offsets - this is normally done by reading then from
            // disk, but in this junit,
            // we must set them manually
            var offsets = new[] {1L, 2L, 3L};
            var ids = new[] {1, 2, 3};
            instanceInfo.GetHeader().SetAttributesIdentification(offsets);
            instanceInfo.GetHeader().SetAttributesIds(ids);
            instanceInfo.GetHeader().SetOid(OIDFactory.BuildObjectOID(1));
            var nnoiProfile = (NonNativeObjectInfo) instanceInfo.GetAttributeValueFromId(3);
            nnoiProfile.SetOid(OIDFactory.BuildObjectOID(2));

            var nnoiFunctions = (NonNativeObjectInfo)nnoiProfile.GetAttributeValueFromId(1);
            nnoiFunctions.SetOid(OIDFactory.BuildObjectOID(3));

            user.SetName("Kiko");
            var instanceInfo3 =
                (NonNativeObjectInfo)
                new ObjectIntrospector(storageEngine.GetClassInfoProvider()).GetMetaRepresentation(user, true, null,
                                                                            new InstrumentationCallbackForStore(null,
                                                                                                                false));
            instanceInfo3.GetHeader().SetOid(OIDFactory.BuildObjectOID(1));
            nnoiProfile = (NonNativeObjectInfo) instanceInfo3.GetAttributeValueFromId(3);
            nnoiProfile.SetOid(OIDFactory.BuildObjectOID(2));

            nnoiFunctions = (NonNativeObjectInfo)nnoiProfile.GetAttributeValueFromId(1);
            nnoiFunctions.SetOid(OIDFactory.BuildObjectOID(3));

            AssertTrue(comparator.HasChanged(instanceInfo, instanceInfo3));
            AssertEquals(1, comparator.GetNbChanges());
            
            odb.Close();
        }

        [Test]
        public void TestGetAllFields()
        {
            var allFields = ClassIntrospector.GetAllFieldsFrom(typeof (FootballPlayer));
            AssertEquals(3, allFields.Count);
            AssertEquals("groundName", (allFields[0]).Name);
            AssertEquals("name", (allFields[1]).Name);
            AssertEquals("role", (allFields[2]).Name);
        }

        [Test]
        public void TestIntrospectWithNull()
        {
            const string dbName = "TestIntrospectWithNull.odb";
            DeleteBase(dbName);
            var odb = OdbFactory.Open(dbName);

            var user = new User("olivier smadja", "user@ndatabase.net", null);
            IObjectInfoComparator comparator = new ObjectInfoComparator();
            
            var storageEngine = ((global::NDatabase.Odb)odb).GetStorageEngine();

            var instanceInfo =
                (NonNativeObjectInfo)
                new ObjectIntrospector(storageEngine.GetClassInfoProvider()).GetMetaRepresentation(user, true, null,
                                                                            new InstrumentationCallbackForStore(null,
                                                                                                                false));
            // Sets attributes offsets - this is normally done by reading then from
            // disk, but in this junit,
            // we must set them manually
            var offsets = new[] {1L, 2L, 3L};
            var ids = new[] {1, 2, 3};
            instanceInfo.GetHeader().SetAttributesIdentification(offsets);
            instanceInfo.GetHeader().SetAttributesIds(ids);
            instanceInfo.GetHeader().SetOid(OIDFactory.BuildObjectOID(1));
            object o = instanceInfo.GetAttributeValueFromId(3);
            var nnoiProfile = (NonNativeObjectInfo) o;
            nnoiProfile.SetOid(OIDFactory.BuildObjectOID(2));
            user.SetName("Olivier Smadja");
            user.SetEmail("olivier@ndatabase.net");
            user.SetProfile(new Profile("pname"));
            var instanceInfo3 =
                (NonNativeObjectInfo)
                new ObjectIntrospector(storageEngine.GetClassInfoProvider()).GetMetaRepresentation(user, true, null,
                                                                            new InstrumentationCallbackForStore(null,
                                                                                                                false));
            instanceInfo3.GetHeader().SetOid(OIDFactory.BuildObjectOID(1));
            nnoiProfile = (NonNativeObjectInfo) instanceInfo3.GetAttributeValueFromId(3);
            nnoiProfile.SetOid(OIDFactory.BuildObjectOID(2));
            AssertTrue(comparator.HasChanged(instanceInfo, instanceInfo3));
            AssertEquals(3, comparator.GetNbChanges());
            
            odb.Close();
        }

        [Test]
        public void TestIntrospectWithNull2()
        {
            const string dbName = "TestIntrospectWithNull2.odb";
            DeleteBase(dbName);
            var odb = OdbFactory.Open(dbName);

            var user = new User("olivier smadja", "user@ndatabase.net", null);
            IObjectInfoComparator comparator = new ObjectInfoComparator();
            
            var storageEngine = ((global::NDatabase.Odb)odb).GetStorageEngine();

            var instanceInfo =
                (NonNativeObjectInfo)
                new ObjectIntrospector(storageEngine.GetClassInfoProvider()).GetMetaRepresentation(user, true, null,
                                                                            new InstrumentationCallbackForStore(null,
                                                                                                                false));
            // Sets attributes offsets - this is normally done by reading then from
            // disk, but in this junit,
            // we must set them manually
            var offsets = new[] {1L, 2L, 3L};
            var ids = new[] {1, 2, 3};
            instanceInfo.GetHeader().SetAttributesIdentification(offsets);
            instanceInfo.GetHeader().SetAttributesIds(ids);
            instanceInfo.GetHeader().SetOid(OIDFactory.BuildObjectOID(1));
            var nnoiProfile = (NonNativeObjectInfo) instanceInfo.GetAttributeValueFromId(3);
            nnoiProfile.SetOid(OIDFactory.BuildObjectOID(2));

            user.SetProfile(new Profile("pname"));

            var instanceInfo3 =
                (NonNativeObjectInfo)
                new ObjectIntrospector(storageEngine.GetClassInfoProvider()).GetMetaRepresentation(user, true, null,
                                                                            new InstrumentationCallbackForStore(null,
                                                                                                                false));
            instanceInfo3.GetHeader().SetAttributesIdentification(offsets);
            instanceInfo3.GetHeader().SetAttributesIds(ids);
            instanceInfo3.GetHeader().SetOid(OIDFactory.BuildObjectOID(1));
            nnoiProfile = (NonNativeObjectInfo) instanceInfo3.GetAttributeValueFromId(3);
            nnoiProfile.SetOid(OIDFactory.BuildObjectOID(2));

            AssertTrue(comparator.HasChanged(instanceInfo, instanceInfo3));
            AssertEquals(1, comparator.GetNbChanges());
            
            odb.Close();
        }

        [Test]
        public void TestCopy()
        {
            const string dbName = "introspectortest2.odb";
            DeleteBase(dbName);
            var odb = OdbFactory.Open(dbName);

            var function = new VO.Login.Function("login");
            var profile = new Profile("operator", function);
            var user = new User("olivier smadja", "olivier@ndatabase.net", profile);
            
            var storageEngine = ((global::NDatabase.Odb)odb).GetStorageEngine();

            var instanceInfo =
                (NonNativeObjectInfo)
                new ObjectIntrospector(storageEngine.GetClassInfoProvider()).GetMetaRepresentation(user, true, null,
                                                                            new InstrumentationCallbackForStore(null,
                                                                                                                false));
            var copy = (NonNativeObjectInfo) instanceInfo.CreateCopy(new OdbHashMap<OID, AbstractObjectInfo>(), true);
            AssertEquals(3, copy.GetAttributeValues().Length);
            var aois = copy.GetAttributeValues();
            for (var i = 0; i < aois.Length; i++)
            {
                var aoi = aois[i];
                AssertEquals(instanceInfo.GetAttributeValues()[i].GetOdbTypeId(), aoi.GetOdbTypeId());
            }

            odb.Close();
        }
    }
}
