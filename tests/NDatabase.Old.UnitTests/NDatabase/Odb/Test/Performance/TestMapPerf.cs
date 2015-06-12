using System.Collections;
using System.Collections.Generic;
using NDatabase;
using NDatabase.Api;
using NDatabase.Oid;
using NUnit.Framework;
using Test.NDatabase.Tool;

namespace Test.NDatabase.Odb.Test.Performance
{
    /// <summary>
    ///   Test map strategy
    ///   We need to cache loaded objects.
    /// </summary>
    /// <remarks>
    ///   Test map strategy
    ///   We need to cache loaded objects. But some of this loaded objects will be
    ///   modified and we need to keep track of the modified object (without
    ///   duplication)
    ///   What is the best strategy?
    ///   1- having two maps, one for loaded objects and one for save objects. Knowing
    ///   that all saved objects are in the loaded objects
    ///   2- having one map, where the value is not the object but an Object Wrapper
    ///   that has a boolean to indicate if it has been update and the object
    ///   ??
    /// </remarks>
    /// <author>osmadja</author>
    [TestFixture]
    public class TestMapPerf : ODBTest
    {
        public static int size = 50000;

        /// <summary>
        ///   Loading x objects, x/2 are modified, using strategy 1
        /// </summary>
        [Test]
        public virtual void Test1()
        {
            var stopWatch = new StopWatch();
            stopWatch.Start();
            IDictionary loadedObjects = new Dictionary<object, object>();
            IDictionary modifiedObjects = new Dictionary<object, object>();
            VO.Login.Function f = null;
            OID oid = null;
            for (var i = 0; i < size; i++)
            {
                f = new VO.Login.Function("function " + i);
                oid = OIDFactory.BuildObjectOID(i);
                loadedObjects.Add(oid, f);
                if (i < size / 2)
                    modifiedObjects.Add(oid, f);
            }
            var j = 0;
            var nbModified = 0;
            // Now get all modified objects
            var iterator = modifiedObjects.Keys.GetEnumerator();
            while (iterator.MoveNext())
            {
                oid = (OID) iterator.Current;
                var o = modifiedObjects[oid];
                j++;
                nbModified++;
            }
            stopWatch.End();
            Println("time for 2 maps =" + stopWatch.GetDurationInMiliseconds());
            AssertEquals(size / 2, nbModified);
        }

        /// <summary>
        ///   Loading x objects, x/2 are modified, using strategy 2
        /// </summary>
        [Test]
        public virtual void Test2()
        {
            var stopWatch = new StopWatch();
            stopWatch.Start();
            IDictionary objects = new Dictionary<object, object>();
            VO.Login.Function f = null;
            OID oid = null;
            ObjectWrapper ow = null;
            var i = 0;
            for (i = 0; i < size; i++)
            {
                f = new VO.Login.Function("function " + i);
                oid = OIDFactory.BuildObjectOID(i);
                objects.Add(oid, new ObjectWrapper(f, false));
                if (i < size / 2)
                {
                    ow = (ObjectWrapper) objects[oid];
                    ow.SetModified(true);
                }
            }
            i = 0;
            var nbModified = 0;
            // Now get all modified objects
            var iterator = objects.Keys.GetEnumerator();
            while (iterator.MoveNext())
            {
                oid = (OID) iterator.Current;
                ow = (ObjectWrapper) objects[oid];
                if (ow.IsModified())
                    nbModified++;
                i++;
            }
            stopWatch.End();
            Println("time for 1 map =" + stopWatch.GetDurationInMiliseconds());
            AssertEquals(size / 2, nbModified);
        }
    }

    internal class ObjectWrapper
    {
        private bool modified;

        private object @object;

        public ObjectWrapper(object @object, bool modified)
        {
            this.@object = @object;
            this.modified = modified;
        }

        public virtual bool IsModified()
        {
            return modified;
        }

        public virtual void SetModified(bool modified)
        {
            this.modified = modified;
        }

        public virtual object GetObject()
        {
            return @object;
        }

        public virtual void SetObject(object @object)
        {
            this.@object = @object;
        }

        public override bool Equals(object obj)
        {
            return @object.Equals(obj);
        }

        public override int GetHashCode()
        {
            return @object.GetHashCode();
        }
    }
}
