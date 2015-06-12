using System;

namespace Test.NDatabase.Odb.Test.Update.Nullobject
{
    /// <summary>
    ///   AT
    /// </summary>
    public class AT : Device
    {
        private Constructor constructor;

        private DateTime creationDate;
        private bool deleted;
        private string ipAddress;

        private string name;
        private string physicalAddress;
        private int port;

        private bool status;
        private string type;

        private DateTime updateDate;

        private User user;

        #region Device Members

        public string GetIpAddress()
        {
            return ipAddress;
        }

        public string GetName()
        {
            return name;
        }

        public int GetPort()
        {
            return port;
        }

        public string GetPhysicalAddress()
        {
            return physicalAddress;
        }

        #endregion

        public override string ToString()
        {
            return "[" + ipAddress + "][" + port + "][" + name + "][" + type + "]";
        }

        public new string GetType()
        {
            return type;
        }

        public Constructor GetConstructor()
        {
            return constructor;
        }

        public DateTime GetCreationDate()
        {
            return creationDate;
        }

        public bool GetDeleted()
        {
            return deleted;
        }

        public bool GetStatus()
        {
            return status;
        }

        public DateTime GetUpdateDate()
        {
            return updateDate;
        }

        public User GetUser()
        {
            return user;
        }

        public void SetConstructor(Constructor constructor)
        {
            this.constructor = constructor;
        }

        public void SetCreationDate(DateTime creationDate)
        {
            this.creationDate = creationDate;
        }

        public void SetDeleted(bool deleted)
        {
            this.deleted = deleted;
        }

        public void SetStatus(bool status)
        {
            this.status = status;
        }

        public void SetUpdateDate(DateTime updateDate)
        {
            this.updateDate = updateDate;
        }

        public void SetUser(User user)
        {
            this.user = user;
        }

        public void SetIpAddress(string ipAddress)
        {
            this.ipAddress = ipAddress;
        }

        public void SetType(string type)
        {
            this.type = type;
        }

        public void SetName(string name)
        {
            this.name = name;
        }

        public void SetPort(int port)
        {
            this.port = port;
        }

        public void SetPhysicalAddress(string physicalAddress)
        {
            this.physicalAddress = physicalAddress;
        }
    }
}
