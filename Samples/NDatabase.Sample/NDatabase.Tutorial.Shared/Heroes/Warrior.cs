using System;
using NDatabase.Tutorial.Shared.Items;

namespace NDatabase.Tutorial.Shared.Heroes
{
    public sealed class Warrior : IHero
    {
        private readonly string _name;
        private int _attack;
        private int _defense;

        public Warrior(string name)
        {
            if (string.IsNullOrEmpty(name))
                throw new ArgumentNullException("name", "Name of warrior cannot be empty.");

            _name = name;
            RightHand = NoItem.Instance;
            LeftHand = NoItem.Instance;

            Attack = 3;
            Defense = 3;
        }

        #region IHero Members

        public IItem RightHand { private get; set; }
        public IItem LeftHand { private get; set; }

        public int Attack
        {
            get { return _attack + RightHand.Attack + LeftHand.Attack; }
            private set { _attack = value; }
        }

        public int Defense
        {
            get { return _defense + RightHand.Defense + LeftHand.Defense; }
            private set { _defense = value; }
        }

        public int Level
        {
            get { return 1; }
        }

        public string Name
        {
            get { return _name; }
        }

        #endregion

        public override string ToString()
        {
            return string.Format("[{0}]: RH: {1}, LH: {2}, Att: {3}, Def: {4}, Lvl: {5}",
                                 Name, RightHand, LeftHand, Attack, Defense, Level);
        }
    }
}
