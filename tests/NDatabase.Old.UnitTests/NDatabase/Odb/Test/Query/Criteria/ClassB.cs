using System.Collections.Generic;
using Test.NDatabase.Odb.Test.VO.Login;

namespace Test.NDatabase.Odb.Test.Query.Criteria
{
    /// <author>olivier</author>
    public class ClassB
    {
        private string name;

        private IList<Profile> profiles;

        public ClassB(string name, IList<Profile> profiles)
        {
            this.name = name;
            this.profiles = profiles;
        }

        public virtual string GetName()
        {
            return name;
        }

        public virtual void SetName(string name)
        {
            this.name = name;
        }

        public virtual IList<Profile> GetProfiles()
        {
            return profiles;
        }

        public virtual void SetProfiles(IList<Profile> profiles)
        {
            this.profiles = profiles;
        }
    }
}
