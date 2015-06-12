using System;

namespace Test.NDatabase.Odb.Test.Arraycollectionmap
{
    public class ObjectWithNativeArrayOfBigDecimal
    {
        private string name;

        private Decimal[] numbers;

        public ObjectWithNativeArrayOfBigDecimal(string name, Decimal[] numbers)
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

        public virtual Decimal[] GetNumbers()
        {
            return numbers;
        }

        public virtual Decimal GetNumber(int index)
        {
            return numbers[index];
        }

        public virtual void SetNumbers(Decimal[] numbers)
        {
            this.numbers = numbers;
        }

        public virtual void SetNumber(int index, Decimal bd)
        {
            numbers[index] = bd;
        }
    }
}
