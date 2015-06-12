namespace NDatabase.Client.UnitTests.Data.Items
{
    public sealed class Field : IArmor
    {
        private readonly int _defense;

        public Field(int value)
        {
            _defense = value;
        }

        #region IArmor Members

        public int Defense
        {
            get { return _defense; }
        }

        public int Attack
        {
            get { return 0; }
        }

        #endregion

        public override string ToString()
        {
            return string.Format("Field +{0}D", Defense);
        }
    }
}
