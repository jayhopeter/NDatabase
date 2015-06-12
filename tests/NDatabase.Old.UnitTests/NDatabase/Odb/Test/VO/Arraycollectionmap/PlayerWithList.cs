using System.Collections;
using System.Text;

namespace Test.NDatabase.Odb.Test.VO.Arraycollectionmap
{
    public class PlayerWithList
    {
        private readonly string name;

        private IList games;

        private int numberOfGames;

        public PlayerWithList()
        {
        }

        public PlayerWithList(string name)
        {
            this.name = name;
            games = new ArrayList();
            numberOfGames = 0;
        }

        public virtual void AddGame(string gameName)
        {
            games.Add(gameName);
            numberOfGames++;
        }

        public virtual string GetGame(int index)
        {
            return (string) games[index];
        }

        public virtual void SetGames(IList games)
        {
            this.games = games;
        }

        public override string ToString()
        {
            var buffer = new StringBuilder();
            buffer.Append("Name=").Append(name).Append("[");
            for (var i = 0; i < numberOfGames; i++)
                buffer.Append(GetGame(i)).Append(" ");
            buffer.Append("]");
            return buffer.ToString();
        }
    }
}
