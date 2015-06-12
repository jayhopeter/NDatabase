using System.Collections.Generic;
using System.Text;

namespace NDatabase.UnitTests.CodeSnippets.Data
{
    public sealed class Team
    {
        public Team(string name)
        {
            Name = name;
            Players = new List<Player>();
        }

        public string Name { get; set; }
        public IList<Player> Players { get; set; }

        public void AddPlayer(Player player)
        {
            Players.Add(player);
        }

        public override string ToString()
        {
            var buffer = new StringBuilder();
            buffer.Append("Team ").Append(Name).AppendLine();

            foreach (var player in Players)
                buffer.AppendLine(string.Format("\t{0}", player));

            return buffer.ToString();
        }
    }
}