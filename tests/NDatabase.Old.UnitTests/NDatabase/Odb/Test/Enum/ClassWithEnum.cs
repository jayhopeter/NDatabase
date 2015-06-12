namespace Test.NDatabase.Odb.Test.Enum
{
    public enum ObjectType
    {
        Small,
        Medium,
        Big
    }

    internal class ClassWithEnum
    {
        protected string name;
        private ObjectType type;

        public ClassWithEnum(string name, ObjectType type)
        {
            this.name = name;
            this.type = type;
        }

        public string GetName()
        {
            return name;
        }

        public ObjectType GetObjectType()
        {
            return type;
        }

        internal void SetObjectType(ObjectType objectType)
        {
            type = objectType;
        }
    }
}
