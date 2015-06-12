using System.Collections;

namespace Test.NDatabase.Odb.Test.VO.School
{
    public class Course
    {
        private IList listOfDiscipline;
        private string name;

        public Course()
        {
        }

        public Course(string name)
        {
            this.name = name;
            listOfDiscipline = new ArrayList();
        }

        public virtual string GetName()
        {
            return name;
        }

        public virtual void SetName(string name)
        {
            this.name = name;
        }

        public virtual IList GetListOfDiscipline()
        {
            return listOfDiscipline;
        }

        public virtual void SetListOfDiscipline(IList listOfDiscipline)
        {
            this.listOfDiscipline = listOfDiscipline;
        }

        public override string ToString()
        {
            return "name=" + name + " | disciplines=" + listOfDiscipline;
        }
    }
}
