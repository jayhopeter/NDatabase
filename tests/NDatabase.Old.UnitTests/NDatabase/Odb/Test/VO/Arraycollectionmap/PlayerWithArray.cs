using System.Text;

namespace Test.NDatabase.Odb.Test.VO.Arraycollectionmap
{
    public class PlayerWithArray
    {
        private readonly string[] games;
        private readonly string name;

        private int numberOfGames;

        public PlayerWithArray()
        {
        }

        public PlayerWithArray(string name)
        {
            this.name = name;
            games = new string[50];
            numberOfGames = 0;
        }

        public virtual void AddGame(string gameName)
        {
            games[numberOfGames] = gameName;
            numberOfGames++;
        }

        public virtual string GetGame(int index)
        {
            return games[index];
        }

        public override string ToString()
        {
            var buffer = new StringBuilder();
            buffer.Append("Name=").Append(name).Append("[");
            for (var i = 0; i < numberOfGames; i++)
                buffer.Append(games[i]).Append(" ");
            buffer.Append("]");
            return buffer.ToString();
        }
    }
}
