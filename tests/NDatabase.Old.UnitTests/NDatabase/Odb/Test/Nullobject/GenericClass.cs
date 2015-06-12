namespace Test.NDatabase.Odb.Test.Nullobject
{
    public class GenericClass
    {
        private object @object;

        private object[] objects;

        public GenericClass(object @object)
        {
            this.@object = @object;
            objects = new object[10];
        }

        public virtual object GetObject()
        {
            return @object;
        }

        public virtual void SetObject(object @object)
        {
            this.@object = @object;
        }

        public virtual object[] GetObjects()
        {
            return objects;
        }

        public virtual void SetObjects(object[] objects)
        {
            this.objects = objects;
        }
    }
}
