using System;

namespace Test.NDatabase.Odb.Test.List
{
    public class User
    {
        private readonly bool ok;
        private string address;

        private DateTime date;
        private string email;
        private long id;
        private string name;

        public User()
        {
        }

        public User(string name, string email, string address, long id, DateTime date, bool ok)
        {
            // TODO Auto-generated constructor stub
            // TODO Auto-generated constructor stub
            this.name = name;
            this.email = email;
            this.address = address;
            this.id = id;
            this.date = date;
            this.ok = ok;
        }

        /// <returns> Returns the address. </returns>
        public virtual string GetAddress()
        {
            return address;
        }

        /// <param name="address"> The address to set. </param>
        public virtual void SetAddress(string address)
        {
            this.address = address;
        }

        /// <returns> Returns the email. </returns>
        public virtual string GetEmail()
        {
            return email;
        }

        /// <param name="email"> The email to set. </param>
        public virtual void SetEmail(string email)
        {
            this.email = email;
        }

        /// <returns> Returns the name. </returns>
        public virtual string GetName()
        {
            return name;
        }

        /// <param name="name"> The name to set. </param>
        public virtual void SetName(string name)
        {
            this.name = name;
        }

        public override string ToString()
        {
            return name + " - " + email + " - " + address + " - id=" + id + " - " + date + " - ok=" + ok;
        }

        /// <returns> Returns the id. </returns>
        public virtual long GetId()
        {
            return id;
        }

        /// <param name="id"> The id to set. </param>
        public virtual void SetId(long id)
        {
            this.id = id;
        }

        /// <returns> Returns the date. </returns>
        public virtual DateTime GetDate()
        {
            return date;
        }

        /// <param name="date"> The date to set. </param>
        public virtual void SetDate(DateTime date)
        {
            this.date = date;
        }
    }
}
