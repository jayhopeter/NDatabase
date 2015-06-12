namespace NDatabase.Client.UnitTests.Data.Items
{
    public sealed class NoItem : IItem
    {
        private NoItem()
        {   
        }

        public int Attack { get { return 0; } }
        public int Defense { get { return 0; } }

        public static readonly IItem Instance = new NoItem();

        public override string ToString()
        {
            return "NoItem";
        }
    }
}