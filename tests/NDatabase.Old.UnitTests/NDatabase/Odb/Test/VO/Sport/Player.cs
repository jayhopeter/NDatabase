using System;

namespace Test.NDatabase.Odb.Test.VO.Sport
{
    public class Player
    {
        private DateTime birthDate;

        private Sport favoriteSport;
        private string name;

        public Player(string name, DateTime birthDate, Sport favoriteSport)
        {
            this.name = name;
            this.birthDate = birthDate;
            this.favoriteSport = favoriteSport;
        }

        public virtual DateTime GetBirthDate()
        {
            return birthDate;
        }

        public virtual void SetBirthDate(DateTime birthDate)
        {
            this.birthDate = birthDate;
        }

        public virtual Sport GetFavoriteSport()
        {
            return favoriteSport;
        }

        public virtual void SetFavoriteSport(Sport favoriteSport)
        {
            this.favoriteSport = favoriteSport;
        }

        public virtual string GetName()
        {
            return name;
        }

        public virtual void SetName(string name)
        {
            this.name = name;
        }

        public override string ToString()
        {
            return name;
        }
    }
}
