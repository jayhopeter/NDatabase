using System;
using System.IO;
using System.Linq;
using NDatabase.Api;
using NDatabase.Tutorial.Shared.Heroes;
using NDatabase.Tutorial.Shared.Items;
using log4net.Config;

namespace NDatabase.Tutorial.Logging.log4net
{
    class Program
    {
        private const int Count = 3;

        static void Main()
        {
            XmlConfigurator.Configure(new FileInfo("log4net.xml"));

            const string dbName = "mydb.ndb";
            OdbFactory.Delete(dbName);

            OdbConfiguration.RegisterLogger(new Log4NetLogger());

            using (var odb = OdbFactory.Open(dbName))
            {
                for (var i = 0; i < Count; i++)
                    odb.Store(new Warrior(string.Format("Warrior {0}", i + 1)));
            }

            using (var odb = OdbFactory.OpenLast())
            {
                var warriors = odb.Query<Warrior>().Execute<Warrior>().ToList();

                for (var i = 0; i < Count; i++)
                {
                    warriors[i].LeftHand = new Field(i + 3);
                    warriors[i].RightHand = new Sword(i + 2);
                    odb.Store(warriors[i]);
                }
            }

            using (var odb = OdbFactory.OpenLast())
            {
                var warriors = odb.Query<Warrior>().Execute<Warrior>().ToList();

                foreach (var warrior in warriors)
                    odb.Delete(warrior);
            }

            using (var odb = OdbFactory.OpenLast())
            {
                var finalCount = odb.Query<Warrior>().Execute<Warrior>().Count;
                Console.WriteLine("Final count: {0}", finalCount);
            }
        }
    }
}
