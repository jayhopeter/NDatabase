using System;

namespace Test.NDatabase.Odb.Test.Instantiationhelper
{
    /// <summary>
    ///   It is just a simple test to help the newbies
    /// </summary>
    /// <author>mayworm at
    ///   <xmpp://mayworm@gmail.com>
    /// </author>
    public class Car
    {
        private string model;

        private int year;

        /// <summary>
        ///   The arguments are mandatory
        /// </summary>
        /// <param name="model"> </param>
        /// <param name="year"> </param>
        public Car(string model, int year)
        {
            if (model == null)
                throw new ArgumentException("The argument cannot be null");
            if (year == 0)
                throw new ArgumentException("The argument cannot be null");
            this.model = model;
            this.year = year;
        }

        public virtual int GetYear()
        {
            return year;
        }

        public virtual void SetYear(int year)
        {
            this.year = year;
        }

        public virtual string GetModel()
        {
            return model;
        }

        public virtual void SetModel(string model)
        {
            this.model = model;
        }
    }
}
