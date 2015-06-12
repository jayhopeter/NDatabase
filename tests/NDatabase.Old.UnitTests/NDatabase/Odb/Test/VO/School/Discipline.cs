namespace Test.NDatabase.Odb.Test.VO.School
{
    public class Discipline
    {
        private string name;

        private int numberOfHour;

        public Discipline()
        {
        }

        public Discipline(string name, int hour)
        {
            this.name = name;
            numberOfHour = hour;
        }

        public virtual string GetName()
        {
            return name;
        }

        public virtual int GetNumberOfHour()
        {
            return numberOfHour;
        }

        public virtual void SetName(string name)
        {
            this.name = name;
        }

        public virtual void SetNumberOfHour(int numberOfHour)
        {
            this.numberOfHour = numberOfHour;
        }

        public override string ToString()
        {
            return "name=" + name + " | nbhours=" + numberOfHour;
        }
    }
}
