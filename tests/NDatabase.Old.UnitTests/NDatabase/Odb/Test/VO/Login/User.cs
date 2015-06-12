namespace Test.NDatabase.Odb.Test.VO.Login
{
    public class User
    {
        private string name;        //2
        private Profile profile;    //3
        private string email;       //1

        public User()
        {
        }

        public User(string name, string email, Profile profile)
        {
            this.name = name;
            this.email = email;
            this.profile = profile;
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

        public virtual Profile GetProfile()
        {
            return profile;
        }

        public virtual void SetProfile(Profile profile)
        {
            this.profile = profile;
        }

        public override string ToString()
        {
            return name + " - " + email + " - " + profile;
        }
    }
}
