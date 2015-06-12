namespace Test.NDatabase.Odb.Test.VO.Human
{
    public class Human : Animal
    {
        public Human(string sex, string name) : base("human", sex, name)
        {
        }

        public Human(string specie, string sex, string name) : base(specie, sex, name)
        {
        }
    }
}
