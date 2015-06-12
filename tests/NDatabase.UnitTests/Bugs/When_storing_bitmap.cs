using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using NUnit.Framework;

namespace NDatabase.UnitTests.Bugs
{
    public class When_storing_bitmap
    {
        private const string DbName = "bitmap.ndb";

        [SetUp]
        public void SetUp()
        {
            OdbFactory.Delete(DbName);
        }

        [Test]
        [Ignore("Well-known bug - storing bitmap")]
        public static void It_should_be_stored_correctly()
        {
            var bitmap = new Bitmap("product_box.png");

            using (var odb = OdbFactory.Open(DbName))
            {
                odb.Store(bitmap);
            }

            using (var odb = OdbFactory.Open(DbName))
            {
                var storedBitmap = odb.QueryAndExecute<Bitmap>().GetFirst();

                using (Stream writer = new FileStream("product_box_restored.png", FileMode.Create, FileAccess.Write))
                {
                    storedBitmap.Save(writer, ImageFormat.Png);
                }
                
                Assert.That(bitmap.Size, Is.EqualTo(storedBitmap.Size));
            }
        }
    }
}