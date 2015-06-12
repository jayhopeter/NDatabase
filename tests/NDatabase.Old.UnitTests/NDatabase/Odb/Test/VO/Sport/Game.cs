using System;
using System.Text;

namespace Test.NDatabase.Odb.Test.VO.Sport
{
    public class Game
    {
        private readonly DateTime when;
        private string result;

        private Sport sport;

        private Team team1;

        private Team team2;

        public Game(DateTime when, Sport sport, Team team1, Team team2)
        {
            this.when = when;
            this.sport = sport;
            this.team1 = team1;
            this.team2 = team2;
        }

        public virtual string GetResult()
        {
            return result;
        }

        public virtual void SetResult(string result)
        {
            this.result = result;
        }

        public virtual Sport GetSport()
        {
            return sport;
        }

        public virtual void SetSport(Sport sport)
        {
            this.sport = sport;
        }

        public virtual Team GetTeam1()
        {
            return team1;
        }

        public virtual void SetTeam1(Team team1)
        {
            this.team1 = team1;
        }

        public virtual Team GetTeam2()
        {
            return team2;
        }

        public virtual void SetTeam2(Team team2)
        {
            this.team2 = team2;
        }

        public override string ToString()
        {
            var buffer = new StringBuilder();
            buffer.Append(when).Append(" : Game of ").Append(sport).Append(" between ").Append(team1.GetName()).Append(
                " and ").Append(team2.GetName());
            return buffer.ToString();
        }
    }
}
