using System;
using NDatabase.Api;
using NDatabase.Api.Triggers;

namespace Test.NDatabase.Odb.Test.Trigger
{
    public class MyTrigger : InsertTrigger
    {
        public int nbInsertsAfter;
        public int nbInsertsBefore;

        public override void AfterInsert(object @object, OID oid)
        {
            Console.WriteLine("after insert object with id " + oid + "(" + @object.GetType().Name + ")");
            nbInsertsAfter++;
        }

        public override bool BeforeInsert(object @object)
        {
            Console.WriteLine("trigger before inserting " + @object);
            nbInsertsBefore++;
            return true;
        }
    }
}
