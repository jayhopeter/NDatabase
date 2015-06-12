namespace NDatabase.UnitTests.CodeSnippets.Data
{
    public sealed class Mage : IHero
    {
        public Mage(string name, double attack, double defense)
        {
            Name = name;
            Attack = attack;
            Defense = defense;
        }

        #region Implementation of IHero

        public string Name { get; private set; }
        public double Attack { get; set; }
        public double Defense { get; set; }

        #endregion

        public override string ToString()
        {
            return string.Format("[Mage] Name: {0}, Attack: {1}, Defense: {2}", Name, Attack, Defense);
        }
    }
}