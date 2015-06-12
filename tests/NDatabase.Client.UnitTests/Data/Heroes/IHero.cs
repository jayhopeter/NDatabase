using NDatabase.Client.UnitTests.Data.Items;

namespace NDatabase.Client.UnitTests.Data.Heroes
{
    public interface IHero
    {
        IItem RightHand { set; }
        IItem LeftHand { set; }

        int Attack { get; }
        int Defense { get; }

        int Level { get; }
        string Name { get; }
    }
}