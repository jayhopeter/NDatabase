using System;

namespace NDatabase.UnitTests.CodeSnippets.Data
{
    public sealed class Player
    {
        public Player(string name, DateTime birthDate, Sport favoriteSport)
        {
            Name = name;
            BirthDate = birthDate;
            FavoriteSport = favoriteSport;
        }

        public DateTime BirthDate { get; set; }
        public Sport FavoriteSport { get; set; }

        public string Name { get; set; }

        public override string ToString()
        {
            return Name;
        }
    }
}
