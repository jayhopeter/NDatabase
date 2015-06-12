using System.Collections;
using System.Text;

namespace Test.NDatabase.Odb.Test.Tutorial
{
    public class Team
    {
        private string name;

        private IList players;

        public Team(string name)
        {
            this.name = name;
            players = new ArrayList();
        }

        /// <returns> the name </returns>
        public virtual string GetName()
        {
            return name;
        }

        /// <param name="name"> the name to set </param>
        public virtual void SetName(string name)
        {
            this.name = name;
        }

        /// <returns> the players </returns>
        public virtual IList GetPlayers()
        {
            return players;
        }

        /// <param name="players"> the players to set </param>
        public virtual void SetPlayers(IList players)
        {
            this.players = players;
        }

        public virtual void AddPlayer(Player player)
        {
            players.Add(player);
        }

        public override string ToString()
        {
            var buffer = new StringBuilder();
            buffer.Append("Team ").Append(name).Append(" ").Append(players);
            return buffer.ToString();
        }
    }
}
