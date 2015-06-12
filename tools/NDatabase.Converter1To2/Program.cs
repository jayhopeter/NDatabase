using System;
using System.IO;
using System.Linq;
using NDatabase.Odb;

namespace NDatabase.Converter1To2
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args.Length != 2)
            {
                Console.WriteLine("Please specify input and output ndatabase paths");
                return;
            }

            var inputDbFileName = args[0];
            var outputDbFileName = args[1];

            if (inputDbFileName.Equals(outputDbFileName))
            {
                Console.WriteLine("Db file names cannot be equal!");
                return;
            }

            var inputDbFile = new FileInfo(inputDbFileName);

            if (!inputDbFile.Exists)
            {
                Console.WriteLine("Input db file doesn't exist! File name: {0}", inputDbFileName);
                return;
            }

            int count1, count2;

            using (var odb1 = Odb.OdbFactory.Open(inputDbFileName))
            {
                using (var odb2 = OdbFactory.Open(outputDbFileName))
                {
                    var objects = odb1.GetObjects<object>().ToList();
                    count1 = objects.Count;

                    foreach (var plainObject in objects)
                        odb2.Store(plainObject);
                }
            }

            using (var odb2 = OdbFactory.Open(outputDbFileName))
            {
                var query = odb2.Query<object>();
                var objects = query.Execute<object>().ToList<object>();
                count2 = objects.Count;
            }

            if (count1 == count2)
            {
                Console.WriteLine("Successfuly moved {0} objects!", count1);
            }
            else
            {
                Console.WriteLine("Problem with moving all objects, original db has {0} objects, moved to the new one {1} objects.");
            }
        }
    }
}
