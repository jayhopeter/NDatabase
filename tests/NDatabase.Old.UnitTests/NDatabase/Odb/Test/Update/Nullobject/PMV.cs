using System;

namespace Test.NDatabase.Odb.Test.Update.Nullobject
{
    /// <summary>
    ///   PMV - Painel
    /// </summary>
    public class PMV : Device
    {
        private Constructor constructor;

        private DateTime creationDate;
        private bool deleted;
        private string ipAddress;
        private float km;

        private string name;
        private string physicalAddress;
        private int port;

        private int state;

        private bool status;

        private DateTime updateDate;

        private User user;
        private int way;

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
            return "[" + ipAddress + "][" + port + "][" + name + "][" + state + "][" + km + "][" + creationDate + "][" +
                   updateDate + "][" + user + "]";
        }

        public virtual float GetKm()
        {
            return km;
        }

        public virtual int GetState()
        {
            return state;
        }

        public virtual bool GetDeleted()
        {
            return deleted;
        }

        public virtual DateTime GetCreationDate()
        {
            return creationDate;
        }

        public virtual User GetUser()
        {
            return user;
        }

        public virtual DateTime GetUpdateDate()
        {
            return updateDate;
        }

        public virtual Constructor GetConstructor()
        {
            return constructor;
        }

        public virtual bool GetStatus()
        {
            return status;
        }

        public virtual void SetConstructor(Constructor constructor)
        {
            this.constructor = constructor;
        }

        public virtual void SetStatus(bool status)
        {
            this.status = status;
        }

        public virtual void SetUpdateDate(DateTime updateDate)
        {
            this.updateDate = updateDate;
        }

        public virtual void SetCreationDate(DateTime creationDate)
        {
            this.creationDate = creationDate;
        }

        public virtual void SetUser(User user)
        {
            this.user = user;
        }

        public virtual void SetDeleted(bool deleted)
        {
            this.deleted = deleted;
        }

        public virtual void SetIpAddress(string ipAddress)
        {
            this.ipAddress = ipAddress;
        }

        public virtual void SetKm(float km)
        {
            this.km = km;
        }

        public virtual void SetName(string name)
        {
            this.name = name;
        }

        public virtual void SetPort(int port)
        {
            this.port = port;
        }

        public virtual void SetState(int state)
        {
            this.state = state;
        }

        public virtual void SetPhysicalAddress(string physicalAddress)
        {
            this.physicalAddress = physicalAddress;
        }

        public virtual int GetWay()
        {
            return way;
        }

        public virtual void SetWay(int way)
        {
            this.way = way;
        }
    }
}
