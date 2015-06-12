using System;
using NUnit.Framework;
using Test.NDatabase.Odb.Test.VO.Arraycollectionmap.Catalog;
using Test.NDatabase.Odb.Test.VO.Login;

namespace Test.NDatabase.Odb.Test.Delete
{
    [TestFixture]
    public class TestDelete2 : ODBTest
    {
        [Test]
        public virtual void TestDeleteListElements()
        {
            var baseName = GetBaseName();
            DeleteBase(baseName);

            var odb = Open(baseName);
            var p = new Profile("name");
            p.AddFunction(new VO.Login.Function("f1"));
            p.AddFunction(new VO.Login.Function("f2"));
            p.AddFunction(new VO.Login.Function("3"));
            odb.Store(p);
            odb.Close();
            odb = Open(baseName);
            var query = odb.Query<Profile>();
            var objects = query.Execute<Profile>();
            while (objects.HasNext())
            {
                var profile = objects.Next();
                var functions = profile.GetFunctions();
                for (var j = 0; j < functions.Count; j++)
                    odb.Delete(functions[j]);
                odb.Delete(profile);
            }
            odb.Close();
        }

        [Test]
        public virtual void TestDeleteListElements2()
        {
            var baseName = GetBaseName();
            DeleteBase(baseName);

            var odb = Open(baseName);
            var catalog = new Catalog("Fnac");
            var books = new ProductCategory("Books");
            books.GetProducts().Add(new Product("Book1", new Decimal(10.1)));
            books.GetProducts().Add(new Product("Book2", new Decimal(10.2)));
            books.GetProducts().Add(new Product("Book3", new Decimal(10.3)));
            var computers = new ProductCategory("Computers");
            computers.GetProducts().Add(new Product("MacBook", new Decimal(1300.1)));
            computers.GetProducts().Add(new Product("BookBookPro", new Decimal(2000.2)));
            computers.GetProducts().Add(new Product("MacMini", new Decimal(1000.3)));
            catalog.GetCategories().Add(books);
            catalog.GetCategories().Add(computers);
            odb.Store(catalog);
            odb.Close();
            odb = Open(baseName);
            var query = odb.Query<Catalog>();
            var objects = query.Execute<Catalog>();
            Println(objects.Count + " catalog(s)");
            while (objects.HasNext())
            {
                var c = objects.Next();
                var pCategories = c.GetCategories();
                Println(c.GetCategories().Count + " product categories");
                for (var j = 0; j < pCategories.Count; j++)
                {
                    var pc = pCategories[j];
                    Println("\tProduct Category : " + pc.GetName() + " : " + pc.GetProducts().Count + " products");
                    for (var k = 0; k < pc.GetProducts().Count; k++)
                    {
                        var p = pc.GetProducts()[k];
                        Println("\t\tProduct " + p.GetName());
                        odb.Delete(p);
                    }
                    odb.Delete(pc);
                }
                odb.Delete(c);
            }
            odb.Close();
            odb = Open(baseName);
            var query1 = odb.Query<Catalog>();
            var catalogs = query1.Execute<Catalog>();
            var query2 = odb.Query<ProductCategory>();
            var productCategories = query2.Execute<ProductCategory>();
            var query3 = odb.Query<Product>();
            var products = query3.Execute<Product>();
            AssertTrue(catalogs.Count == 0);
            AssertTrue(productCategories.Count == 0);
            AssertTrue(products.Count == 0);

            odb.Close();
            DeleteBase(baseName);
        }
    }
}
