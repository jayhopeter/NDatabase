namespace Test.NDatabase.Odb.Test.VO.Interfaces
{
    public class ObjectWithInterfaces
    {
        private object attribute1;

        public ObjectWithInterfaces(object attribute1)
        {
            this.attribute1 = attribute1;
        }

        public virtual object GetAttribute1()
        {
            return attribute1;
        }

        public virtual void SetAttribute1(object attribute1)
        {
            this.attribute1 = attribute1;
        }
    }
}
