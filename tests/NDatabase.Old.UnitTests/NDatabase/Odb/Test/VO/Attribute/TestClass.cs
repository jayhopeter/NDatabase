using System;

namespace Test.NDatabase.Odb.Test.VO.Attribute
{
    public class TestClass
    {
        private Decimal bigDecimal1;
        private bool boolean1;

        private bool boolean2;

        private char char1;

        private DateTime date1;
        private double double1;
        private int int1;
        private string string1;

        public virtual DateTime GetDate1()
        {
            return date1;
        }

        public virtual void SetDate1(DateTime date1)
        {
            this.date1 = date1;
        }

        public virtual Decimal GetBigDecimal1()
        {
            return bigDecimal1;
        }

        public virtual void SetBigDecimal1(Decimal bigDecimal1)
        {
            this.bigDecimal1 = bigDecimal1;
        }

        public virtual bool IsBoolean1()
        {
            return boolean1;
        }

        public virtual void SetBoolean1(bool boolean1)
        {
            this.boolean1 = boolean1;
        }

        public virtual char GetChar1()
        {
            return char1;
        }

        public virtual void SetChar1(char char1)
        {
            this.char1 = char1;
        }

        public virtual double GetDouble1()
        {
            return double1;
        }

        public virtual void SetDouble1(double double1)
        {
            this.double1 = double1;
        }

        public virtual int GetInt1()
        {
            return int1;
        }

        public virtual void SetInt1(int int1)
        {
            this.int1 = int1;
        }

        public virtual string GetString1()
        {
            return string1;
        }

        public virtual void SetString1(string string1)
        {
            this.string1 = string1;
        }

        public virtual void Change()
        {
            string1 = "ola";
        }

        public override string ToString()
        {
            return double1 + " | " + string1 + " | " + int1 + "\n";
        }

        public virtual bool GetBoolean2()
        {
            return boolean2;
        }

        public virtual void SetBoolean2(bool boolean2)
        {
            this.boolean2 = boolean2;
        }
    }
}
