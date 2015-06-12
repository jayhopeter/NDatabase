namespace NDatabase.UnitTests.CodeSnippets.Data
{
    public sealed class Warrior : IHero
    {
        public Warrior(string name, double attack, double defense)
        {
            Name = name;
            Attack = attack;
            Defense = defense;
        }

        #region Implementation of IHero

        public string Name { get; private set; }
        public double Attack { get; private set; }
        public double Defense { get; private set; }

        #endregion

        public override string ToString()
        {
            return string.Format("[Warrior] Name: {0}, Attack: {1}, Defense: {2}", Name, Attack, Defense);
        }
    }
}