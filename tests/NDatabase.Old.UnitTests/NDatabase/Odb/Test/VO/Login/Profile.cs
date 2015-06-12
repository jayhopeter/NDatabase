using System.Collections.Generic;

namespace Test.NDatabase.Odb.Test.VO.Login
{
    public class Profile
    {
        private IList<Function> functions;
        private string name;

        public Profile()
        {
        }

        public Profile(string name)
        {
            this.name = name;
        }

        public Profile(string name, IList<Function> functions)
        {
            this.functions = functions;
            this.name = name;
        }

        public Profile(string name, Function function)
        {
            functions = new List<Function>();
            functions.Add(function);
            this.name = name;
        }

        public virtual void AddFunction(Function function)
        {
            if (functions == null)
                functions = new List<Function>();
            functions.Add(function);
        }

        public virtual IList<Function> GetFunctions()
        {
            return functions;
        }

        public virtual void SetFunctions(IList<Function> functions)
        {
            this.functions = functions;
        }

        public virtual string GetName()
        {
            return name;
        }

        public virtual void SetName(string name)
        {
            this.name = name;
        }

        public override string ToString()
        {
            return name + " - " + (functions != null
                                       ? functions.ToString()
                                       : "null");
        }

        public virtual bool Equals2(object obj)
        {
            if (obj == null || obj.GetType() != typeof (Profile))
                return false;
            var p = (Profile) obj;
            if (name == null && p.name != null)
                return false;
            return (name == null && p.name == null) || (name.Equals(p.name));
        }
    }
}
