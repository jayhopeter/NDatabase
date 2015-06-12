using System;
using Domain;
using NDatabase;

namespace PrepareDb
{
    class Program
    {
        static void Main()
        {
            const string dbName = "employees.ndb";

            Console.WriteLine("Cleaning old database");
            OdbFactory.Delete(dbName);

            Console.WriteLine("Done");
            Console.WriteLine("Preparing new data set");

            var employee1 = new Employee {Name = "Tom", City = "New York City", EmploymentDate = new DateTime(2001, 1, 1), Age = 23};
            var employee2 = new Employee { Name = "John", City = "Cansas", EmploymentDate = new DateTime(2007, 1, 1), Age = 25 };
            var employee3 = new Employee { Name = "Meggy", City = "San Francisco", EmploymentDate = new DateTime(2010, 1, 1), Age = 26 };

            using (var odb = OdbFactory.Open(dbName))
            {
                odb.Store(employee1);
                odb.Store(employee2);
                odb.Store(employee3);
            }

            Console.WriteLine("Data set prepared and stored in: " + dbName);
            Console.WriteLine("Finished");

            Console.WriteLine("Press <enter> to finish the work...");
            Console.ReadLine();
        }
    }
}
