using System;
using System.Text;

namespace Test.NDatabase.Odb.Test.VO.Download
{
    public class User
    {
        private string city;
        private string country;
        private string email;

        private DateTime lastDownload;
        private string name;
        private int nbDownloads;
        private string runtimeVersion;

        public virtual string GetCity()
        {
            return city;
        }

        public virtual void SetCity(string city)
        {
            this.city = city;
        }

        public virtual string GetCountry()
        {
            return country;
        }

        public virtual void SetCountry(string country)
        {
            this.country = country;
        }

        public virtual string GetEmail()
        {
            return email;
        }

        public virtual void SetEmail(string email)
        {
            this.email = email;
        }

        public virtual string GetName()
        {
            return name;
        }

        public virtual void SetName(string name)
        {
            this.name = name;
        }

        public virtual string GetRuntimeVersion()
        {
            return runtimeVersion;
        }

        public virtual void SetRuntimeVersion(string runtimeVersion)
        {
            this.runtimeVersion = runtimeVersion;
        }

        public virtual int GetNbDownloads()
        {
            return nbDownloads;
        }

        public virtual void SetNbDownloads(int nbDownloads)
        {
            this.nbDownloads = nbDownloads;
        }

        public virtual DateTime GetLastDownload()
        {
            return lastDownload;
        }

        public virtual void SetLastDownload(DateTime lastDownload)
        {
            this.lastDownload = lastDownload;
        }

        public override string ToString()
        {
            var buffer = new StringBuilder();
            buffer.Append("name=").Append(name).Append(" - email=").Append(email);
            buffer.Append("lastDownload").Append(lastDownload).Append(" - nb downloads=").Append(nbDownloads);
            return buffer.ToString();
        }
    }
}
