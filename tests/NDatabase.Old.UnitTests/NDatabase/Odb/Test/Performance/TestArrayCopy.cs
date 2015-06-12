using System;
using NDatabase.Tool.Wrappers;
using NUnit.Framework;

namespace Test.NDatabase.Odb.Test.Performance
{
    [TestFixture]
    public class TestArrayCopy : ODBTest
    {
        [Test]
        public virtual void Test1()
        {
            var size = 1000;
            var arraySize = 100000;
            var bs1 = new byte[arraySize];
            var bs2 = new byte[arraySize];
            var start = OdbTime.GetCurrentTimeInTicks();
            for (var i = 0; i < size; i++)
                Array.Copy(bs1, 0, bs2, 0, arraySize);
            var step1 = OdbTime.GetCurrentTimeInTicks();
            var time1 = step1 - start;
            for (var i = 0; i < size; i++)
            {
                for (var j = 0; j < arraySize; j++)
                    bs2[j] = bs1[j];
            }
            var step2 = OdbTime.GetCurrentTimeInTicks();
            var time2 = step2 - step1;
            for (var i = 0; i < size; i++)
                bs2 = bs1;
            var step3 = OdbTime.GetCurrentTimeInTicks();
            var time3 = step3 - step2;
            Println("ArraySize=" + arraySize + " : arraycopy=" + time1 + " - loop copy=" + time2 + " - clone=" + time3);
            AssertTrue(time1 <= time2);
        }
    }
}
