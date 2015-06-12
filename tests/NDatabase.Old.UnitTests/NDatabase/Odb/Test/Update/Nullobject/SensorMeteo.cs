using System;

namespace Test.NDatabase.Odb.Test.Update.Nullobject
{
    /// <summary>
    ///   SensorMeteo
    /// </summary>
    public class SensorMeteo
    {
        private DateTime creationDate;
        private bool deleted;
        private float km;

        private Meteo meteo;
        private string name;

        private int state;

        private bool status;

        private DateTime updateDate;

        private User user;
        private int way;

        // S ou N
        // Sim ou Nao
        public override string ToString()
        {
            return "[" + name + "][" + state + "][" + status + "][" + way + "][" + km + "]";
        }

        public virtual float GetKm()
        {
            return km;
        }

        public virtual string GetName()
        {
            return name;
        }

        public virtual int GetState()
        {
            return state;
        }

        public virtual int GetWay()
        {
            return way;
        }

        public virtual DateTime GetCreationDate()
        {
            return creationDate;
        }

        public virtual bool GetDeleted()
        {
            return deleted;
        }

        public virtual Meteo GetMeteo()
        {
            return meteo;
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

        public virtual void SetCreationDate(DateTime creationDate)
        {
            this.creationDate = creationDate;
        }

        public virtual void SetDeleted(bool deleted)
        {
            this.deleted = deleted;
        }

        public virtual void SetMeteo(Meteo meteo)
        {
            this.meteo = meteo;
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

        public virtual void SetWay(int way)
        {
            this.way = way;
        }

        public virtual void SetKm(float km)
        {
            this.km = km;
        }

        public virtual void SetName(string name)
        {
            this.name = name;
        }

        public virtual void SetState(int state)
        {
            this.state = state;
        }
    }
}
