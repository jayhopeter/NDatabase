using NDatabase.Tutorial.Shared.Items;

namespace NDatabase.Tutorial.Shared.Heroes
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