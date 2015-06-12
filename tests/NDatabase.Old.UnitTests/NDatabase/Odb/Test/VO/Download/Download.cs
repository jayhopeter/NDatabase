using System;

namespace Test.NDatabase.Odb.Test.VO.Download
{
    public class Download
    {
        private string fileName;
        private string type;

        private User user;
        private DateTime when;

        public virtual string GetFileName()
        {
            return fileName;
        }

        public virtual void SetFileName(string fileName)
        {
            this.fileName = fileName;
        }

        public new virtual string GetType()
        {
            return type;
        }

        public virtual void SetType(string type)
        {
            this.type = type;
        }

        public virtual User GetUser()
        {
            return user;
        }

        public virtual void SetUser(User user)
        {
            this.user = user;
        }

        public virtual DateTime GetWhen()
        {
            return when;
        }

        public virtual void SetWhen(DateTime when)
        {
            this.when = when;
        }
    }
}
