using System.Collections.Generic;

namespace NDatabase.Performance.Tester.Domain
{
    public class HolderA
    {
        public HolderA()
        {
            ListOfHolderBItems = new List<HolderB>();
        }

        public string Name { get; set; }
        public long Id { get; set; }

        public IList<HolderB> ListOfHolderBItems { get; set; }
    }

    public class HolderB
    {
        public HolderB()
        {
            ListOfHolderCItems = new List<HolderC>();
        }

        public string Name { get; set; }
        public long Id { get; set; }

        public IList<HolderC> ListOfHolderCItems { get; set; }
    }

    public class HolderC
    {
        public HolderC()
        {
            FinalItems = new List<FinalItem>();
        }

        public string Name { get; set; }
        public long Id { get; set; }

        public IList<FinalItem> FinalItems { get; set; }
    }

    public class FinalItem
    {
        public string Name { get; set; }
        public long Id { get; set; }
    }
}