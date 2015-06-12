using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using NDatabase.Tutorial.Shared;
using NDatabase.Tutorial.Shared.Heroes;
using NDatabase.Tutorial.Shared.Items;
using NeoDatis.Odb;
using NeoDatis.Odb.Impl.Core.Query.Criteria;

namespace NDatabase.Tutorial.Basic.NeoDatis
{
    public sealed class Warrior : IHero
    {
        private readonly string _name;
        private int _attack;
        private int _defense;

        public Warrior(string name)
        {
            _name = name;
            RightHand = NoItem.Instance;
            LeftHand = NoItem.Instance;

            Attack = 3;
            Defense = 3;
        }

        #region IHero Members

        public IItem RightHand { private get; set; }
        public IItem LeftHand { private get; set; }

        public int Attack
        {
            get { return _attack + RightHand.Attack + LeftHand.Attack; }
            private set { _attack = value; }
        }

        public int Defense
        {
            get { return _defense + RightHand.Defense + LeftHand.Defense; }
            private set { _defense = value; }
        }

        public int Level
        {
            get { return 1; }
        }

        public string Name
        {
            get { return _name; }
        }

        #endregion

        public override string ToString()
        {
            return string.Format("[{0}]: RH: {1}, LH: {2}, Att: {3}, Def: {4}, Lvl: {5}",
                                 Name, RightHand, LeftHand, Attack, Defense, Level);
        }
    }

    class Program
    {
        private const string DBName = "game.odb";
        private const string ProjectName = "NeoDatis";

        static void Main()
        {
            BasicSampleSet.Process(DBName, ProjectName, Step1, Step2, Step3, Step4, Step5, PerformanceStep);
        }

        private static void PerformanceStep()
        {
            const int count = 10000;

            var stopwatch = new Stopwatch();
            stopwatch.Start();

            var odb = ODBFactory.Open(DBName);
            for (var i = 0; i < count; i++)
                odb.Store(new Warrior(string.Format("Warrior {0}", i + 1)));
            odb.Close();

            stopwatch.Stop();
            Display(stopwatch, string.Format("insert of {0} warriors", count));

            stopwatch.Start();

            odb = ODBFactory.Open(DBName);
            var warriors = odb.GetObjects<Warrior>().ToList();

            for (var i = 0; i < count; i++)
            {
                warriors[i].LeftHand = new Field(i + 3);
                warriors[i].RightHand = new Sword(i + 2);
                odb.Store(warriors[i]);
            }
            odb.Close();

            stopwatch.Stop();
            Display(stopwatch, string.Format("query and update of {0} warriors and {1} items", count, count * 2));

            stopwatch.Start();

            odb = ODBFactory.Open(DBName);
            warriors = odb.GetObjects<Warrior>().ToList();

            foreach (var warrior in warriors)
                odb.Delete(warrior);
            odb.Close();

            stopwatch.Stop();
            Display(stopwatch, string.Format("query and remove of {0} warriors", count));

            stopwatch.Start();

            odb = ODBFactory.Open(DBName);
            var finalCount = odb.GetObjects<Warrior>().Count;
            Console.WriteLine("Final count: {0}", finalCount);
            odb.Close();

            stopwatch.Stop();
            Display(stopwatch, string.Format("query empty list"));
        }

        private static void Display(Stopwatch stopwatch, string action)
        {
            Console.WriteLine("Action: {0}, \n\tEllapsed: {1} ms.", action, stopwatch.ElapsedMilliseconds);
            Console.WriteLine("\tFile size: {0} bytes", new FileInfo(DBName).Length);
        }

        private static void Step1()
        {
            var warrior1 = new Warrior("Warrior 1");
            var warrior2 = new Warrior("Warrior 2");

            var odb = ODBFactory.Open(DBName);
            odb.Store(warrior1);
            odb.Store(warrior2);
            odb.Close();
        }

        private static void Step2()
        {
            var sword1 = new Sword(5);
            var sword2 = new Sword(3);

            var field1 = new Field(3);
            var field2 = new Field(5);


            var odb = ODBFactory.Open(DBName);

            IList<Warrior> warriors1 = odb.GetObjects<Warrior>().ToList();

            warriors1[0].RightHand = sword1;
            warriors1[0].LeftHand = field1;

            warriors1[1].RightHand = sword2;
            warriors1[1].LeftHand = field2;

            odb.Store(warriors1[0]);
            odb.Store(warriors1[1]);
            odb.Close();
        }

        private static void Step3()
        {
            var odb1 = ODBFactory.Open(DBName);
            {
                var query = new CriteriaQuery(typeof (IItem));
                query.SetPolymorphic(true);

                IList<IItem> items = odb1.GetObjects<IItem>(query).ToList();

                foreach (var item in items)
                    Console.WriteLine(item);
            }

            odb1.Close();
        }

        private static void Step4()
        {
            var odb1 = ODBFactory.Open(DBName);

            IList<Warrior> warriors = odb1.GetObjects<Warrior>().ToList();

            foreach (var warrior in warriors)
                Console.WriteLine(warrior);

            Console.WriteLine("Remove warriors");

            odb1.Delete(warriors[0]);
            odb1.Delete(warriors[1]);
            odb1.Close();
        }

        private static void Step5()
        {
            var odb = ODBFactory.Open(DBName);
            var count = odb.GetObjects<Warrior>().Count;
            odb.Close();

            Console.WriteLine("Warriors count: {0}", count);
        }
    }
}
