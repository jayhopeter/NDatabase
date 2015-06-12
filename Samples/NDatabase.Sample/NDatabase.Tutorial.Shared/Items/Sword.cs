namespace NDatabase.Tutorial.Shared.Items
{
    public sealed class Sword : IWeapon
    {
        private readonly int _attack;

        public Sword(int value)
        {
            _attack = value;
        }

        #region IWeapon Members

        public int Attack
        {
            get { return _attack; }
        }

        public int Defense
        {
            get { return 0; }
        }

        #endregion

        public override string ToString()
        {
            return string.Format("Sword +{0}A", Attack);
        }
    }
}
