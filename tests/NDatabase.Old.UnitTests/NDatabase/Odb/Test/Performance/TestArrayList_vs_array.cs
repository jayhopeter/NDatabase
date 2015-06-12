using System.Collections.Generic;
using NDatabase.Tool.Wrappers;
using NUnit.Framework;

namespace Test.NDatabase.Odb.Test.Performance
{
    /// <author>olivier</author>
    [TestFixture]
    public class TestArrayList_vs_array : ODBTest
    {
        [Test]
        public virtual void Test1()
        {
            var size = 100000;
            var arrayOfInts = new int[size];
            IList<int> listOfInts = new List<int>(size);
            var startArray = OdbTime.GetCurrentTimeInTicks();
            for (var i = 0; i < size; i++)
                arrayOfInts[i] = i;
            for (var i = 0; i < size; i++)
            {
                var ii = arrayOfInts[i];
            }
            var endArray = OdbTime.GetCurrentTimeInTicks();
            var startList = OdbTime.GetCurrentTimeInTicks();
            for (var i = 0; i < size; i++)
                listOfInts.Add(i);
            for (var i = 0; i < size; i++)
            {
                var ii = listOfInts[i];
            }
            var endList = OdbTime.GetCurrentTimeInTicks();
            Println("Time for array = " + (endArray - startArray));
            Println("Time for list = " + (endList - startList));
        }
    }
}
