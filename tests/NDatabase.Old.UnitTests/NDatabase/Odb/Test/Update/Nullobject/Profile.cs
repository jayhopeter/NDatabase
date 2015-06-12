namespace Test.NDatabase.Odb.Test.Update.Nullobject
{
    /// <summary>
    ///   Profile
    /// </summary>
    public class Profile
    {
        private readonly string _name;

        public Profile(string name)
        {
            _name = name;
        }

        protected bool Equals(Profile other)
        {
            return string.Equals(_name, other._name);
        }

        public override int GetHashCode()
        {
            return (_name != null
                        ? _name.GetHashCode()
                        : 0);
        }

        public override string ToString()
        {
            return "[" + _name + "]";
        }

        /// <returns> Returns the name. </returns>
        public virtual string GetName()
        {
            return _name;
        }

        /// <summary>
        ///   return boolean
        /// </summary>
        public override bool Equals(object obj)
        {
            if (obj == null)
                return false;
            else
            {
                if (!(obj is Profile))
                    return false;
                else
                    return _name.Equals(((Profile) obj).GetName());
            }
        }
    }
}
