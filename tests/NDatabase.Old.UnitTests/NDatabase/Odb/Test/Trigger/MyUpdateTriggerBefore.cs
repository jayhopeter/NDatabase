using NDatabase.Api;
using NDatabase.Api.Triggers;

namespace Test.NDatabase.Odb.Test.Trigger
{
    public class MyUpdateTriggerBefore : UpdateTrigger
    {
        public virtual void AfterInsert(object @object, OID oid)
        {
        }

        public virtual bool BeforeInsert(object @object)
        {
            var so = (SimpleObject) @object;
            // just add 1
            so.SetId(so.GetId() + 1);
            return true;
        }

        public override void AfterUpdate(IObjectRepresentation oldObjectRepresentation, object newObject, OID oid)
        {
        }

        public override bool BeforeUpdate(IObjectRepresentation oldObjectRepresentation, object newObject, OID oid)
        {
            var so = (SimpleObject) newObject;
            // just add 1
            so.SetId(so.GetId() + 1);
            return true;
        }
    }
}
