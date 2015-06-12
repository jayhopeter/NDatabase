using NDatabase.Api;
using NDatabase.Api.Triggers;

namespace Test.NDatabase.Odb.Test.Trigger
{
    public class LocalAutoIncrementTrigger : InsertTrigger
    {
        private readonly static object SyncObject = new object();

        public override void AfterInsert(object @object, OID oid)
        {
        }

        public override bool BeforeInsert(object @object)
        {
            if (@object.GetType() != typeof (ObjectWithAutoIncrementId))
                return false;
            var o = (ObjectWithAutoIncrementId) @object;
            
            lock (SyncObject)
            {
                var id = GetNextId("test");
                o.SetId(id);
                
                return true;
            }
        }

        /// <summary>
        ///   Actually gets the next id Gets the object of type ID from the database
        ///   with the specific name.
        /// </summary>
        /// <remarks>
        ///   Actually gets the next id Gets the object of type ID from the database
        ///   with the specific name. Then increment the id value and returns. If
        ///   object does not exist, creates t.
        /// </remarks>
        /// <param name="idName"> </param>
        /// <returns> </returns>
        private long GetNextId(string idName)
        {
            var odb = Odb;
            var query = odb.Query<ID>();
            query.Descend("idName").Constrain((object) idName).Equal();
            var objects = query.Execute<ID>();
            if (objects.Count == 0)
            {
                var id1 = new ID(idName, 1);
                odb.Store(id1);
                return 1;
            }
            var id = objects.GetFirst();
            var lid = id.GetNext();
            odb.Store(id);
            return lid;
        }
    }
}
