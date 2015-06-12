using System;

namespace Test.NDatabase.Odb.Test.Arraycollectionmap
{
    public class ObjectWithNativeArrayOfDate
    {
        private string name;

        private DateTime[] numbers;

        public ObjectWithNativeArrayOfDate(string name, DateTime[] numbers)
        {
            this.name = name;
            this.numbers = numbers;
        }

        public virtual string GetName()
        {
            return name;
        }

        public virtual void SetName(string name)
        {
            this.name = name;
        }

        public virtual DateTime[] GetNumbers()
        {
            return numbers;
        }

        public virtual DateTime GetNumber(int index)
        {
            return numbers[index];
        }

        public virtual void SetNumbers(DateTime[] numbers)
        {
            this.numbers = numbers;
        }

        public virtual void SetNumber(int index, DateTime bd)
        {
            numbers[index] = bd;
        }
    }
}
