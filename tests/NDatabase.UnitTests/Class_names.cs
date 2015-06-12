using System;

namespace NDatabase.UnitTests
{
    public class Class_names
    {
        public string Value1 { get; set; }

        public void SomeMethod()
        {
            var val = new {x = "20", y = 30};
            Console.WriteLine(val.x);
            Console.WriteLine(val.y);
        }

        public class OtherClass : Class_names
        {
            public string Value2 { get; set; }

            public void SomeMethod2()
            {
                var val = new { x = "30", y = 40 };
                Console.WriteLine(val.x);
                Console.WriteLine(val.y);
            }

            public class OtherClass2 : Class_names
            {
                public string Value3 { get; set; }

                public new void SomeMethod()
                {
                    var val = new { x = "40", y = 50, z = 60L };
                    Console.WriteLine(val.x);
                    Console.WriteLine(val.y);
                    Console.WriteLine(val.z);
                }
            }
        }
    }
}