using System;
using NDatabase;
using NDatabase.Api;
using NUnit.Framework;

namespace Test
{
    public class Program
    {
        [Test]
        public void test1()
        {
            try
            {
                string file = "Test.NDatabase";
                OdbFactory.Delete(file);
                IOdb odb = OdbFactory.Open(file);
                OID oid = odb.Store(new Function("f1"));
                odb.Close();
                Console.WriteLine("Write Done!");

                odb = OdbFactory.Open(file);
                var query = odb.Query<Function>();
                IObjectSet<Function> functions = query.Execute<Function>();
                Console.WriteLine(" Number of functions = " + functions.Count);
                Function f = (Function) odb.GetObjectFromId(oid);
                Console.WriteLine(f.ToString());
                odb.Close();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
            Console.ReadLine();
        }

        [Test]
        public void test2()
        {
            try
            {
                int size = 1000;
                string file = "Test.NDatabase";
                OdbFactory.Delete(file);
                IOdb odb = OdbFactory.Open(file);
                for (int i = 0; i < size; i++)
                {
                    OID oid = odb.Store(new Function("function " + i));
                }
                odb.Close();

                odb = OdbFactory.Open(file);
                var query = odb.Query<Function>();
                var functions = query.Execute<Function>();
                Console.WriteLine(" Number of functions = " + functions.Count);
                
                odb.Close();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
            Console.ReadLine();
        }

        [Test]
        public void test4()
        {
            try
            {
                int size = 1000;
                string file = "Test.NDatabase";
                Console.WriteLine("Oi");
                OdbFactory.Delete(file);
                IOdb odb = OdbFactory.Open(file);
                for (int i = 0; i < size; i++)
                {
                    OID oid = odb.Store(new Function("function " + i));
                }
                odb.Close();

                odb = OdbFactory.Open(file);
                var query = odb.Query<Function>();
                
                query.Descend("name").Constrain((object) "function 199").Equal();
                
                var functions = query.Execute<Function>();
                Console.WriteLine(" Number of functions = " + functions.Count);

                odb.Close();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
            Console.ReadLine();
        }
    }
}
