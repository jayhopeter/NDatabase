using System;

namespace Test.NDatabase.Odb.Test.Update.Nullobject
{
    /// <summary>
    ///   PMV - Painel
    /// </summary>
    public class Meteo : Device
    {
        private Constructor constructor;

        private DateTime creationDate;
        private bool deleted;
        private string ipAddress;

        private string name;

        private string physicalAddress;
        private int port;

        private bool status;

        private DateTime updateDate;

        private User user;

        // S ou N
        // Sim ou Nao

        #region Device Members

        public virtual string GetIpAddress()
        {
            return ipAddress;
        }

        public virtual string GetName()
        {
            return name;
        }

        public virtual int GetPort()
        {
            return port;
        }

        public virtual string GetPhysicalAddress()
        {
            return physicalAddress;
        }

        #endregion

        public override string ToString()
        {
            return "[" + ipAddress + "][" + port + "][" + name + "]";
        }

        public virtual Constructor GetConstructor()
        {
            return constructor;
        }

        public virtual DateTime GetCreationDate()
        {
            return creationDate;
        }

        public virtual bool GetDeleted()
        {
            return deleted;
        }

        public virtual bool GetStatus()
        {
            return status;
        }

        public virtual DateTime GetUpdateDate()
        {
            return updateDate;
        }

        public virtual User GetUser()
        {
            return user;
        }

        public virtual void SetConstructor(Constructor constructor)
        {
            this.constructor = constructor;
        }

        public virtual void SetCreationDate(DateTime creationDate)
        {
            this.creationDate = creationDate;
        }

        public virtual void SetDeleted(bool deleted)
        {
            this.deleted = deleted;
        }

        public virtual void SetStatus(bool status)
        {
            this.status = status;
        }

        public virtual void SetUpdateDate(DateTime updateDate)
        {
            this.updateDate = updateDate;
        }

        public virtual void SetUser(User user)
        {
            this.user = user;
        }

        public virtual void SetIpAddress(string ipAddress)
        {
            this.ipAddress = ipAddress;
        }

        public virtual void SetName(string name)
        {
            this.name = name;
        }

        public virtual void SetPort(int port)
        {
            this.port = port;
        }

        public virtual void SetPhysicalAddress(string physicalAddress)
        {
            this.physicalAddress = physicalAddress;
        }
    }
}
