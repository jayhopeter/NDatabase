using System;
using System.Collections.Generic;
using NDatabase.Api;
using NDatabase.Api.Query;
using NDatabase.Core;
using NDatabase.Meta;
using NDatabase.Tool.Wrappers;

namespace Test.NDatabase.Odb.Test.IO
{
    internal class MockObjectReader : IObjectReader
    {
        #region IObjectReader Members

        public void ReadDatabaseHeader()
        {
            //empty
        }

        public void LoadMetaModel(IMetaModel metaModel, bool full)
        {
            //empty
        }

        public Object BuildOneInstance(NonNativeObjectInfo objectInfo)
        {
            return null;
        }

        public void Close()
        {
        }

        public IList<long> GetAllIds(byte idType)
        {
            return null;
        }

        public IObjectSet<TResult> GetObjectInfos<TResult, TObject>(IQuery query, bool inMemory, int startIndex, int endIndex, bool returnObjects, IMatchingObjectAction queryResultAction) where TObject : class
        {
            throw new NotImplementedException();
        }

        public String GetBaseIdentification()
        {
            return null;
        }

        public OID GetIdOfObjectAt(long position, bool includeDeleted)
        {
            return null;
        }

        public IInstanceBuilder GetInstanceBuilder()
        {
            return null;
        }

        public OID GetNextObjectOID(OID oid)
        {
            return null;
        }

        public Object GetObjectFromOid(OID oid, bool returnInstance, bool useCache)
        {
            return null;
        }

        public IObjectSet<T> GetObjectInfos<T>(IQuery query, bool inMemory, int startIndex, int endIndex,
                                             bool returnObjects, IMatchingObjectAction queryResultAction)
        {
            return null;
        }

        public long GetObjectPositionFromItsOid(OID oid, bool useCache, bool throwException)
        {
            return 0;
        }

        public IInternalObjectSet<T> GetObjects<T>(IQuery query, bool inMemory, int startIndex, int endIndex)
        {
            return null;
        }

        public IValues GetValues<T>(IInternalValuesQuery query, int startIndex, int endIndex) where T : class
        {
            return null;
        }

        public IValues GetValues(IInternalValuesQuery query, int startIndex, int endIndex)
        {
            return null;
        }

        public AtomicNativeObjectInfo ReadAtomicNativeObjectInfo(int odbTypeId)
        {
            return null;
        }

        public AttributeValuesMap ReadObjectInfoValuesFromOID(ClassInfo classInfo, OID oid, bool useCache,
                                                              IOdbList<string> attributeNames,
                                                              IOdbList<string> relationAttributeNames,
                                                              int recursionLevel)
        {
            return null;
        }

        public Object ReadAtomicNativeObjectInfoAsObject(int odbTypeId)
        {
            return null;
        }

        public IOdbList<ClassInfoIndex> ReadClassInfoIndexesAt(long position, ClassInfo classInfo)
        {
            return null;
        }

        public NonNativeObjectInfo ReadNonNativeObjectInfoFromOid(ClassInfo classInfo, OID oid, bool useCache,
                                                                  bool returnObjects)
        {
            return null;
        }

        public NonNativeObjectInfo ReadNonNativeObjectInfoFromPosition(ClassInfo classInfo, OID oid, long position,
                                                                       bool useCache, bool returnInstance)
        {
            return null;
        }

        public ObjectInfoHeader ReadObjectInfoHeaderFromOid(OID oid, bool useCache)
        {
            return null;
        }

        public long ReadOidPosition(OID oid)
        {
            return 0;
        }

        #endregion

        public void ReadDatabaseHeader(String user, String password)
        {
        }

        public AttributeValuesMap ReadObjectInfoValuesFromOID(ClassInfo classInfo, OID oid, bool useCache,
                                                              IOdbList<String> attributeNames,
                                                              IOdbList<String> relationAttributeNames,
                                                              int recursionLevel, String[] orderByFields,
                                                              bool useOidForObject)
        {
            return null;
        }
    }
}
