using System;
using System.Collections.Generic;
using NDatabase.Btree;
using NDatabase.Tool.Wrappers;
using NUnit.Framework;

namespace Test.NDatabase.Odb.Test.Btree.Odb
{
    [TestFixture]
    public class TestBTreeInsertAndSearch : ODBTest
    {
        [Test]
        public virtual void Test1()
        {
            var degree = 3;
            IBTreeMultipleValuesPerKey tree = new InMemoryBTreeMultipleValuesPerKey(degree);
            tree.Insert(1, "Value 1");
            tree.Insert(20, "Value 20");
            tree.Insert(25, "Value 25");
            tree.Insert(29, "Value 29");
            tree.Insert(21, "Value 21");
            AssertEquals(5, tree.GetRoot().GetNbKeys());
            AssertEquals(0, tree.GetRoot().GetNbChildren());
            AssertEquals(21, tree.GetRoot().GetMedian().GetKey());
            var values = tree.GetRoot().GetMedian().GetValue() as List<object>;
            Assert.That(values, Is.Not.Null);
            AssertEquals("Value 21", values[0].ToString());
            AssertEquals(0, tree.GetRoot().GetNbChildren());
            // println(tree.getRoot());
            tree.Insert(45, "Value 45");
            AssertEquals(2, tree.GetRoot().GetNbChildren());
            AssertEquals(1, tree.GetRoot().GetNbKeys());
            AssertEquals(21, tree.GetRoot().GetKeyAt(0));
            var valuesAsObjectAt = tree.GetRoot().GetValueAsObjectAt(0) as List<object>;
            AssertEquals("Value 21", valuesAsObjectAt[0].ToString());
            // println(tree.getRoot());
            var o = tree.Search(20);
            AssertEquals("Value 20", o[0]);
            o = tree.Search(29);
            AssertEquals("Value 29", o[0]);
            o = tree.Search(45);
            AssertEquals("Value 45", o[0]);
        }

        [Test]
        public virtual void Test2()
        {
            var degree = 10;
            IBTreeMultipleValuesPerKey tree = new InMemoryBTreeMultipleValuesPerKey(degree);
            for (var i = 0; i < 50000; i++)
                tree.Insert(i, "Value " + i);
            AssertEquals("Value 0", tree.Search(0)[0]);
            AssertEquals("Value 1000", tree.Search(1000)[0]);
            AssertEquals("Value 2000", tree.Search(2000)[0]);
            AssertEquals("Value 3000", tree.Search(3000)[0]);
            // tree.resetNbRead();
            AssertEquals("Value 4999", tree.Search(4999)[0]);
        }

        // println("Nb reads = " + tree.getNbRead());
        // println("root = " + tree.getRoot().keysToString(false));
        // println("root[0] = " +
        // tree.getRoot().getChild(0).keysToString(false));
        // println("root[1] = " +
        // tree.getRoot().getChild(1).keysToString(false));
        // println("root[5] = " +
        // tree.getRoot().getChild(3).keysToString(false));
        [Test]
        public virtual void Test3()
        {
            var degree = 3;
            IBTree tree = new InMemoryBTreeMultipleValuesPerKey(degree);
            tree.Insert(1, "A");
            // tree.insert(new Integer(2),"B");
            tree.Insert(3, "C");
            tree.Insert(4, "D");
            tree.Insert(5, "E");
            // tree.insert(new Integer(6),"F");
            tree.Insert(7, "G");
            // tree.insert(new Integer(8),"H");
            // tree.insert(new Integer(9),"I");
            tree.Insert(10, "J");
            tree.Insert(11, "K");
            // tree.insert(new Integer(12),"L");
            tree.Insert(13, "M");
            tree.Insert(14, "N");
            tree.Insert(15, "O");
            tree.Insert(16, "P");
            // tree.insert(new Integer(17),"Q");
            tree.Insert(18, "R");
            tree.Insert(19, "S");
            tree.Insert(20, "T");
            tree.Insert(21, "U");
            tree.Insert(22, "V");
            // tree.insert(new Integer(23),"W");
            tree.Insert(24, "X");
            tree.Insert(25, "Y");
            tree.Insert(26, "Z");
        }

        [Test]
        public virtual void Test4()
        {
            var degree = 3;
            IBTreeMultipleValuesPerKey tree1 = new InMemoryBTreeMultipleValuesPerKey(degree);
            tree1.Insert(1, "A");
            // tree.insert(new Integer(2),"B");
            tree1.Insert(3, "C");
            tree1.Insert(4, "D");
            tree1.Insert(5, "E");
            IBTreeMultipleValuesPerKey tree2 = new InMemoryBTreeMultipleValuesPerKey(degree);
            tree2.Insert(10, "J");
            tree2.Insert(11, "K");
            IBTreeMultipleValuesPerKey tree3 = new InMemoryBTreeMultipleValuesPerKey(degree);
            tree3.Insert(14, "N");
            tree3.Insert(15, "O");
            IBTreeMultipleValuesPerKey tree4 = new InMemoryBTreeMultipleValuesPerKey(degree);
            tree4.Insert(18, "R");
            tree4.Insert(19, "S");
            tree4.Insert(20, "T");
            tree4.Insert(21, "U");
            tree4.Insert(22, "V");
            IBTreeMultipleValuesPerKey tree5 = new InMemoryBTreeMultipleValuesPerKey(degree);
            tree5.Insert(25, "Y");
            tree5.Insert(26, "Z");
            IBTreeMultipleValuesPerKey tree6 = new InMemoryBTreeMultipleValuesPerKey(degree);
            tree6.Insert(7, "G");
            tree6.Insert(13, "M");
            tree6.Insert(16, "P");
            tree6.Insert(24, "X");
            tree6.GetRoot().SetChildAt(tree1.GetRoot(), 0);
            tree6.GetRoot().SetChildAt(tree2.GetRoot(), 1);
            tree6.GetRoot().SetChildAt(tree3.GetRoot(), 2);
            tree6.GetRoot().SetChildAt(tree4.GetRoot(), 3);
            tree6.GetRoot().SetChildAt(tree5.GetRoot(), 4);
            tree6.GetRoot().SetNbChildren(5);
            // println("Test 4");
            tree6.Insert(2, "B");

            var valuesAsObjectAt1 = tree6.GetRoot().GetChildAt(0, true).GetValueAsObjectAt(1) as List<object>;
            AssertEquals("B", valuesAsObjectAt1[0].ToString());
            tree6.Insert(17, "Q");

            AssertEquals(5, tree6.GetRoot().GetNbKeys());

            var valuesAsObjectAt2 = tree6.GetRoot().GetChildAt(3, true).GetValueAsObjectAt(0) as List<object>;
            AssertEquals("Q", valuesAsObjectAt2[0].ToString());
            var valuesAsObjectAt3 = tree6.GetRoot().GetChildAt(3, true).GetValueAsObjectAt(1) as List<object>;
            AssertEquals("R", valuesAsObjectAt3[0].ToString());
            var valuesAsObjectAt4 = tree6.GetRoot().GetChildAt(3, true).GetValueAsObjectAt(2) as List<object>;
            AssertEquals("S", valuesAsObjectAt4[0].ToString());

            var valuesAsObjectAt5 = tree6.GetRoot().GetChildAt(4, true).GetValueAsObjectAt(0) as List<object>;
            AssertEquals("U", valuesAsObjectAt5[0].ToString());
            var valuesAsObjectAt6 = tree6.GetRoot().GetChildAt(4, true).GetValueAsObjectAt(1) as List<object>;
            AssertEquals("V", valuesAsObjectAt6[0].ToString());
            tree6.Insert(12, "L");

            AssertEquals(1, tree6.GetRoot().GetNbKeys());
            AssertEquals(2, tree6.GetRoot().GetChildAt(0, true).GetNbKeys());

            var valuesAsObjectAt7 = tree6.GetRoot().GetChildAt(0, true).GetValueAsObjectAt(0) as List<object>;
            AssertEquals("G", valuesAsObjectAt7[0].ToString());
            var valuesAsObjectAt8 = tree6.GetRoot().GetChildAt(0, true).GetValueAsObjectAt(1) as List<object>;
            AssertEquals("M", valuesAsObjectAt8[0].ToString());

            var valuesAsObjectAt9 =
                tree6.GetRoot().GetChildAt(0, true).GetChildAt(1, true).GetValueAsObjectAt(0) as List<object>;
            AssertEquals("J", valuesAsObjectAt9[0].ToString());
            var valuesAsObjectAt10 =
                tree6.GetRoot().GetChildAt(0, true).GetChildAt(1, true).GetValueAsObjectAt(1) as List<object>;
            AssertEquals("K", valuesAsObjectAt10[0].ToString());
            var valuesAsObjectAt11 =
                tree6.GetRoot().GetChildAt(0, true).GetChildAt(1, true).GetValueAsObjectAt(2) as List<object>;
            AssertEquals("L", valuesAsObjectAt11[0].ToString());
            tree6.Insert(6, "F");

            AssertEquals(1, tree6.GetRoot().GetNbKeys());
            AssertEquals(3, tree6.GetRoot().GetChildAt(0, true).GetNbKeys());
            AssertEquals(2, tree6.GetRoot().GetChildAt(0, true).GetChildAt(0, true).GetNbKeys());

            var valuesAsObjectAt12 =
                tree6.GetRoot().GetChildAt(0, true).GetChildAt(0, true).GetValueAsObjectAt(0) as List<object>;
            AssertEquals("A", valuesAsObjectAt12[0].ToString());
            var valuesAsObjectAt13 =
                tree6.GetRoot().GetChildAt(0, true).GetChildAt(0, true).GetValueAsObjectAt(1) as List<object>;
            AssertEquals("B", valuesAsObjectAt13[0].ToString());

            var valuesAsObjectAt14 =
                tree6.GetRoot().GetChildAt(1, true).GetChildAt(2, true).GetValueAsObjectAt(1) as List<object>;
            AssertEquals("Z", valuesAsObjectAt14[0].ToString());
        }

        [Test]
        public virtual void Test5()
        {
            var degree = 40;
            IBTreeMultipleValuesPerKey tree = new InMemoryBTreeMultipleValuesPerKey(degree);
            var a0 = OdbTime.GetCurrentTimeInMs();
            for (var i = 0; i < 500000; i++)
                tree.Insert(i, "Value " + i);
            var a1 = OdbTime.GetCurrentTimeInMs();
            Console.WriteLine("insert time = {0}", (a1 - a0));
            AssertEquals("Value 0", tree.Search(0)[0].ToString());
            AssertEquals("Value 1000", tree.Search(1000)[0].ToString());
            AssertEquals("Value 2000", tree.Search(2000)[0].ToString());
            AssertEquals("Value 48000", tree.Search(48000)[0].ToString());
            // tree.resetNbRead();
            var t0 = OdbTime.GetCurrentTimeInMs();
            for (var i = 0; i < 100000; i++)
                AssertEquals("Value 490000", tree.Search(490000)[0].ToString());
            var t1 = OdbTime.GetCurrentTimeInMs();
            // tree.resetNbRead();
            AssertEquals("Value 490000", tree.Search(490000)[0].ToString());

            Console.WriteLine("Search in {0} ms", t1 - t0);
        }

        [Test]
        public virtual void TestInsertUsingInt1()
        {
            IBTreeMultipleValuesPerKey tree = new InMemoryBTreeMultipleValuesPerKey(5);
            tree.Insert(50, "50");
            tree.Insert(40, "40");
            tree.Insert(30, "30");
            tree.Insert(20, "20");
            tree.Insert(10, "10");
            tree.Insert(15, "15");
            tree.Insert(25, "25");
            tree.Insert(35, "35");
            tree.Insert(21, "21");
            tree.Insert(22, "22");
            tree.Insert(23, "23");
            var l = tree.Search(22);
            AssertEquals("22", l[0]);
        }

        [Test]
        public virtual void TestInsertUsingInt2()
        {
            var size = 8000;
            IBTreeMultipleValuesPerKey tree = new InMemoryBTreeMultipleValuesPerKey(5);
            for (var i = 1; i < size; i++)
                tree.Insert(i, i.ToString());
            var l = tree.Search(1);
            AssertEquals("1", l[0].ToString());
            l = tree.Search(1000);
            AssertEquals("1000", l[0]);
            l = tree.Search(2000);
            AssertEquals("2000", l[0]);
            l = tree.Search(9800);
            AssertNull(l);
            l = tree.Search(99999);
            AssertEquals(null, l);
        }

        // println("Test5 compl- Nb reads = " + tree.getNbRead()+ " -
        // nb comp="+IntKeyBTree.getNbComparison()+ " - t="+(t1-t0));
        [Test]
        public virtual void TestNonUniqueKey()
        {
            var degree = 3;
            IBTreeMultipleValuesPerKey tree1 = new InMemoryBTreeMultipleValuesPerKey(degree);
            tree1.Insert(1, "A");
            tree1.Insert(1, "AA");
            tree1.Insert(1, "AAA");
            AssertEquals(3, tree1.Search(1).Count);
            AssertEquals("A", tree1.Search(1)[0].ToString());
            AssertEquals("AA", tree1.Search(1)[1].ToString());
            AssertEquals("AAA", tree1.Search(1)[2].ToString());
            AssertEquals(3, tree1.GetSize());
        }

        [Test]
        public virtual void TestNonUniqueKey2()
        {
            var degree = 3;
            IBTreeMultipleValuesPerKey tree1 = new InMemoryBTreeMultipleValuesPerKey(degree);
            tree1.Insert(1, "A");
            tree1.Insert(1, "AA");
            tree1.Insert(1, "AAA");
            tree1.Insert(1, "BBB");
            var c = tree1.Search(1);
            AssertEquals(4, c.Count);
            var iterator = c.GetEnumerator();
            iterator.MoveNext();
            AssertEquals("A", iterator.Current);
            iterator.MoveNext();
            AssertEquals("AA", iterator.Current);
            AssertEquals(4, tree1.GetSize());
            AssertEquals("A", c[0].ToString());
            AssertEquals("AA", c[1].ToString());
            AssertEquals("AAA", c[2].ToString());
            AssertEquals("BBB", c[3].ToString());
        }

        [Test]
        public virtual void TestString1()
        {
            IBTreeMultipleValuesPerKey tree = new InMemoryBTreeMultipleValuesPerKey(5);
            tree.Insert("50", "50");
            tree.Insert("40", "40");
            tree.Insert("30", "30");
            tree.Insert("20", "20");
            tree.Insert("10", "10");
            tree.Insert("15", "15");
            tree.Insert("25", "25");
            tree.Insert("35", "35");
            tree.Insert("21", "21");
            tree.Insert("22", "22");
            tree.Insert("23", "23");
            var p = tree.Search("22");
            AssertEquals("22", p[0]);
        }

        [Test]
        public virtual void TestString2()
        {
            var size = 300;
            var max = 0;
            IBTreeMultipleValuesPerKey tree = new InMemoryBTreeMultipleValuesPerKey(5);
            for (var i = 1; i < size; i++)
            {
                for (var j = 1; j < size; j++)
                {
                    var key = ((i + 1) * size + j).ToString();
                    var value = (i * j).ToString();
                    tree.Insert(key, value);
                    if (i * j > max)
                        max = i * j;
                }
            }
            // println("max = " + max);
            for (var i = 1; i < size; i++)
            {
                for (var j = 1; j < size; j++)
                {
                    var key = ((i + 1) * size + j).ToString();
                    var value = (i * j).ToString();
                    var p = tree.Search(key);
                    AssertEquals(value, p[0]);
                }
            }
        }
    }
}
