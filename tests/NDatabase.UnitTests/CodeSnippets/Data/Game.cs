using System;
using System.Text;

namespace NDatabase.UnitTests.CodeSnippets.Data
{
    public sealed class Game
    {
        private readonly DateTime _when;

        public Game(DateTime when, Sport sport, Team team1, Team team2)
        {
            _when = when;
            Sport = sport;
            Team1 = team1;
            Team2 = team2;
        }

        public string Result { get; set; }
        public Sport Sport { get; set; }
        public Team Team1 { get; set; }
        public Team Team2 { get; set; }

        public override string ToString()
        {
            var buffer = new StringBuilder();
            buffer.Append(_when).Append(" : Game of ").Append(Sport).Append(" between ").Append
                (Team1.Name).Append(" and ").Append(Team2.Name);

            return buffer.ToString();
        }
    }
}
