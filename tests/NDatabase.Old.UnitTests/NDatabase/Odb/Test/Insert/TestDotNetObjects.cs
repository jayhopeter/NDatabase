using System;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using NUnit.Framework;

namespace Test.NDatabase.Odb.Test.Insert
{
    [TestFixture]
    public class TestDotNetObjects : ODBTest
    {
        [Test]
        public virtual void Test22StringBuffer()
        {
            DeleteBase("test.stringbuffer.odb");
            var buffer = new StringBuilder("Ol√° chico");
            var odb = Open("test.stringbuffer.odb");
            odb.Store(buffer);
            odb.Close();
            odb = Open("test.stringbuffer.odb");
            var query = odb.Query<StringBuilder>();
            var l = query.Execute<StringBuilder>();
            odb.Close();
            var b2 = l.GetFirst();
            AssertEquals(buffer.ToString(), b2.ToString());
        }

        [Test]
        public virtual void Test22TextBox()
        {
            DeleteBase("test.textbox.odb");
            var textBox = new TextBox {Text = "Ol\u00E1 chico"};

            var odb = Open("test.textbox.odb");

            odb.Store(textBox);
            odb.Close();
            odb = Open("test.textbox.odb");
            var query = odb.Query<TextBox>();
            var l = query.Execute<TextBox>();
            odb.Close();
            var textBox2 = l.GetFirst();
            AssertEquals(textBox.Text, textBox2.Text);
        }

        /// <summary>
        ///   This nunit does not work because of a problem? in URL: The hashcode of
        ///   the 2 urls url1 & url2 are equal! even if theu point to different domains
        ///   (having the same IP)
        /// </summary>
        /// <exception cref="System.Exception">System.Exception</exception>
        [Test]
        public virtual void Test22URL()
        {
            DeleteBase("test.url.odb");

            var url1 = new Uri("http://google.com");
            var url2 = new Uri("http://nprogramming.wordpress.com");

            var h1 = url1.GetHashCode();
            var h2 = url2.GetHashCode();
            Println(h1 + " - " + h2);
            Println(url1.Host + " - " + url1.Port);
            Println(url2.Host + " - " + url2.Port);

            var odb = Open("test.url.odb");

            odb.Store(url1);
            odb.Store(url2);
            odb.Close();
            odb = Open("test.url.odb");
            var query = odb.Query<Uri>();
            var l = query.Execute<Uri>();

            var first = l.FirstOrDefault(x => x.AbsoluteUri == "http://google.com/");
            Assert.That(first, Is.Not.Null);

            var second = l.FirstOrDefault(x => x.AbsoluteUri == "http://nprogramming.wordpress.com/");
            Assert.That(second, Is.Not.Null);

            odb.Close();

            AssertEquals("Same HashCode Problem", 2, l.Count());
        }
    }
}
