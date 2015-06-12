using System.Collections;

namespace Test.NDatabase.Odb.Test.Update.Nullobject
{
    /// <summary>
    ///   Functions
    /// </summary>
    public class Functions
    {
        private string description;

        private IList listProfile;
        private string name;

        private string nameUrl;

        public override string ToString()
        {
            return "[" + name + "][" + nameUrl + "][" + description + "][" + listProfile + "]";
        }

        /// <returns> Returns the name. </returns>
        public virtual string GetName()
        {
            return name;
        }

        /// <returns> Returns the name_url. </returns>
        public virtual string GetNameUrl()
        {
            return nameUrl;
        }

        /// <returns> Returns the list of profile </returns>
        public virtual IList GetListProfile()
        {
            return listProfile;
        }

        public virtual void AddProfile(Profile profile)
        {
            if (listProfile == null)
                listProfile = new ArrayList();
            listProfile.Add(profile);
        }

        public virtual void SetListProfile(IList listProfile)
        {
            this.listProfile = listProfile;
        }

        /// <param name="name_url"> The name_url to set. </param>
        public virtual void SetNameUrl(string name_url)
        {
            nameUrl = name_url;
        }

        /// <param name="name"> The name to set. </param>
        public virtual void SetName(string name)
        {
            this.name = name;
        }

        /// <returns> Returns the description. </returns>
        public virtual string GetDescription()
        {
            return description;
        }

        /// <param name="description"> The description to set. </param>
        public virtual void SetDescription(string description)
        {
            this.description = description;
        }
    }
}
