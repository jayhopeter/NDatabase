using System;
using NDatabase;
using NDatabase.Btree;
using NDatabase.Core.BTree;
using NDatabase.Tool.Wrappers;
using NUnit.Framework;
using Test.NDatabase.Odb.Test;
using Test.NDatabase.Tool;

namespace Test.NDatabase.Test.Btree.Impl.Singlevalue
{
    [TestFixture]
    internal class TestBTreeSingleValue : ODBTest
    {
        private IBTreeSingleValuePerKey GetBTree(int degree)
        {
            var odb = OdbFactory.Open(GetBaseName());

            var storageEngine = ((global::NDatabase.Odb)odb).GetStorageEngine();
            return new OdbBtreeSingle(degree, new LazyOdbBtreePersister(storageEngine));
        }

        private IBTreeNodeOneValuePerKey GetBTreeNode(IBTree tree, string name)
        {
            return new OdbBtreeNodeSingle(tree);
        }

        /// <exception cref="System.Exception"></exception>
        [Test]
        public virtual void TestDelete()
        {
            var btree = GetBTree(3);
            btree.Insert(1, "key 1");
            AssertEquals(1, btree.GetSize());
            AssertEquals("key 1", btree.Search(1));
            var o = btree.Delete(1, "key 1");
            AssertEquals("key 1", o);
            AssertEquals(0, btree.GetSize());
            AssertEquals(1, btree.GetHeight());
            AssertEquals(0, btree.GetRoot().GetNbKeys());
        }

        /// <exception cref="System.Exception"></exception>
        [Test]
        public virtual void TestDelete1()
        {
            IBTree btree = GetBTree(10);
            var size = 5000;
            var t0 = OdbTime.GetCurrentTimeInMs();
            for (var i = 0; i < size; i++)
                btree.Insert(i, "key " + i);
            var t1 = OdbTime.GetCurrentTimeInMs();
            Console.WriteLine("insert time=" + (t1 - t0));
            AssertEquals(size, btree.GetSize());
            AssertEquals("key 4999", btree.Delete(4999, "key 4999"));
        }

        /// <exception cref="System.Exception"></exception>
        [Test]
        public virtual void TestDelete1000()
        {
            IBTree btree = GetBTree(20);
            var size = 1000;
            for (var i = 0; i < size; i++)
                btree.Insert(i, "key " + i);
            AssertEquals(size, btree.GetSize());
            for (var i = 0; i < size; i++)
                AssertEquals("key " + i, btree.Delete(i, "key " + i));
            AssertEquals(0, btree.GetSize());
            AssertEquals(1, btree.GetHeight());
            AssertEquals(0, btree.GetRoot().GetNbKeys());
            AssertEquals(0, btree.GetRoot().GetNbChildren());
        }

        /// <exception cref="System.Exception"></exception>
        [Test]
        public virtual void TestDelete10000()
        {
            IBTree btree = GetBTree(3);
            var size = 10000;
            for (var i = 0; i < size; i++)
                btree.Insert(i, "key " + i);
            AssertEquals(size, btree.GetSize());
            for (var i = 0; i < size; i++)
                AssertEquals("key " + i, btree.Delete(i, "key " + i));
            AssertEquals(0, btree.GetSize());
            AssertEquals(1, btree.GetHeight());
            AssertEquals(0, btree.GetRoot().GetNbKeys());
            AssertEquals(0, btree.GetRoot().GetNbChildren());
        }

        [Test]
        public virtual void TestDelete1000Alpha_2()
        {
            var btree = GetBTree(2);
            var size = 1000;
            for (var i = 0; i < size; i++)
                btree.Insert("key" + i, "value " + i);
            var o = btree.Search("key71");
            AssertEquals(size, btree.GetSize());
            for (var i = size - 1; i >= 0; i--)
            {
                // println(new BTreeDisplay().build(btree));
                AssertEquals("value " + i, btree.Delete("key" + i, "value " + i));
            }
            AssertEquals(0, btree.GetSize());
            AssertEquals(1, btree.GetHeight());
            AssertEquals(0, btree.GetRoot().GetNbKeys());
            AssertEquals(0, btree.GetRoot().GetNbChildren());
        }

        /// <exception cref="System.Exception"></exception>
        [Test]
        public virtual void TestDelete1000Alpha_3()
        {
            var btree = GetBTree(3);
            var size = 1000;
            for (var i = 0; i < size; i++)
                btree.Insert("key" + i, "value " + i);
            var o = btree.Search("key71");
            AssertEquals(size, btree.GetSize());
            for (var i = size - 1; i >= 0; i--)
            {
                // println(new BTreeDisplay().build(btree));
                AssertEquals("value " + i, btree.Delete("key" + i, "value " + i));
            }
            AssertEquals(0, btree.GetSize());
            AssertEquals(1, btree.GetHeight());
            AssertEquals(0, btree.GetRoot().GetNbKeys());
            AssertEquals(0, btree.GetRoot().GetNbChildren());
        }

        /// <exception cref="System.Exception"></exception>
        [Test]
        public virtual void TestDelete100Alpha_2()
        {
            var btree = GetBTree(2);
            var size = 100;
            for (var i = 0; i < size; i++)
                btree.Insert("key" + i, "value " + i);
            var o = btree.Search("key71");
            AssertEquals(size, btree.GetSize());
            for (var i = size - 1; i >= 0; i--)
            {
                // println(new BTreeDisplay().build(btree));
                AssertEquals("value " + i, btree.Delete("key" + i, "value " + i));
            }
            AssertEquals(0, btree.GetSize());
            AssertEquals(1, btree.GetHeight());
            AssertEquals(0, btree.GetRoot().GetNbKeys());
            AssertEquals(0, btree.GetRoot().GetNbChildren());
        }

        /// <exception cref="System.Exception"></exception>
        [Test]
        public virtual void TestDelete100_3()
        {
            IBTree btree = GetBTree(3);
            var size = 100;
            for (var i = 0; i < size; i++)
                btree.Insert(i, "value " + i);
            AssertEquals(size, btree.GetSize());
            for (var i = size - 1; i >= 0; i--)
                AssertEquals("value " + i, btree.Delete(i, "value " + i));
            AssertEquals(0, btree.GetSize());
            AssertEquals(1, btree.GetHeight());
            AssertEquals(0, btree.GetRoot().GetNbKeys());
            AssertEquals(0, btree.GetRoot().GetNbChildren());
        }

        /// <exception cref="System.Exception"></exception>
        [Test]
        public virtual void TestDelete10_3()
        {
            IBTree btree = GetBTree(3);
            var size = 10;
            for (var i = 0; i < size; i++)
                btree.Insert(i, "value " + i);
            AssertEquals(size, btree.GetSize());
            for (var i = size - 1; i >= 0; i--)
                AssertEquals("value " + i, btree.Delete(i, "value " + i));
            AssertEquals(0, btree.GetSize());
            AssertEquals(1, btree.GetHeight());
            AssertEquals(0, btree.GetRoot().GetNbKeys());
            AssertEquals(0, btree.GetRoot().GetNbChildren());
        }

        /// <exception cref="System.Exception"></exception>
        [Test]
        public virtual void TestDelete2()
        {
            var btree = GetBTree(3);
            btree.Insert(1, "key 1");
            btree.Insert(2, "key 2");
            btree.Insert(3, "key 3");
            btree.Insert(4, "key 4");
            btree.Insert(5, "key 5");
            AssertEquals(5, btree.GetSize());
            AssertEquals("key 1", btree.Search(1));
            AssertEquals("key 2", btree.Search(2));
            AssertEquals("key 3", btree.Search(3));
            AssertEquals("key 4", btree.Search(4));
            AssertEquals("key 5", btree.Search(5));
            var o = btree.Delete(1, "key 1");
            AssertEquals("key 1", o);
            o = btree.Delete(2, "key 2");
            AssertEquals("key 2", o);
            o = btree.Delete(3, "key 3");
            AssertEquals("key 3", o);
            o = btree.Delete(4, "key 4");
            AssertEquals("key 4", o);
            o = btree.Delete(5, "key 5");
            AssertEquals("key 5", o);
            AssertEquals(0, btree.GetSize());
            AssertEquals(0, btree.GetRoot().GetNbKeys());
        }

        [Test]
        public virtual void TestDelete25()
        {
            IBTree tree = GetBTree(3);
            tree.Insert("A", "A");
            tree.Insert("B", "B");
            tree.Insert("C", "C");
            tree.Insert("D", "D");
            tree.Insert("E", "E");
            tree.Insert("F", "F");
            tree.Insert("G", "G");
            tree.Insert("J", "J");
            tree.Insert("K", "K");
            tree.Insert("L", "L");
            tree.Insert("M", "M");
            tree.Insert("N", "N");
            tree.Insert("O", "O");
            tree.Insert("P", "P");
            tree.Insert("Q", "Q");
            tree.Insert("R", "R");
            tree.Insert("S", "S");
            tree.Insert("T", "T");
            tree.Insert("U", "U");
            tree.Insert("V", "V");
            tree.Insert("X", "X");
            tree.Insert("Y", "Y");
            tree.Insert("Z", "Z");
        }

        [Test]
        public virtual void TestDelete26()
        {
            IBTree tree = GetBTree(3);
            tree.Insert("Z", "Z");
            tree.Insert("Y", "Y");
            tree.Insert("X", "X");
            tree.Insert("V", "V");
            tree.Insert("U", "U");
            tree.Insert("T", "T");
            tree.Insert("S", "S");
            tree.Insert("R", "R");
            tree.Insert("Q", "Q");
            tree.Insert("P", "P");
            tree.Insert("O", "O");
            tree.Insert("N", "N");
            tree.Insert("M", "M");
            tree.Insert("L", "L");
            tree.Insert("K", "K");
            tree.Insert("J", "J");
            tree.Insert("G", "G");
            tree.Insert("F", "F");
            tree.Insert("E", "E");
            tree.Insert("D", "D");
            tree.Insert("C", "C");
            tree.Insert("B", "B");
            tree.Insert("A", "A");
            AssertEquals(23, tree.GetSize());
            var child1 = (IBTreeNodeOneValuePerKey) tree.GetRoot().GetChildAt(0, false);
            var child14 = (IBTreeNodeOneValuePerKey) child1.GetChildAt(3, false);
            AssertNotNull(child14);
        }

        /// <summary>
        ///   Cromen example, second edition, page 450
        /// </summary>
        /// <exception cref="System.Exception">System.Exception</exception>
        [Test]
        public virtual void TestDelete27()
        {
            IBTree tree = GetBTree(3);
            tree.Insert("P", "P");
            var PNode = (IBTreeNodeOneValuePerKey) tree.GetRoot();
            var CGMNode = GetBTreeNode(tree, "cgm");
            CGMNode.InsertKeyAndValue("C", "C");
            CGMNode.InsertKeyAndValue("G", "G");
            CGMNode.InsertKeyAndValue("M", "M");
            var TXNode = GetBTreeNode(tree, "tx");
            TXNode.InsertKeyAndValue("T", "T");
            TXNode.InsertKeyAndValue("X", "X");
            PNode.SetChildAt(CGMNode, 0);
            PNode.SetChildAt(TXNode, 1);
            PNode.SetNbChildren(2);
            var ABNode = GetBTreeNode(tree, "ab");
            ABNode.InsertKeyAndValue("A", "A");
            ABNode.InsertKeyAndValue("B", "B");
            var DEFode = GetBTreeNode(tree, "def");
            DEFode.InsertKeyAndValue("D", "D");
            DEFode.InsertKeyAndValue("E", "E");
            DEFode.InsertKeyAndValue("F", "F");
            var JKLNode = GetBTreeNode(tree, "jkl");
            JKLNode.InsertKeyAndValue("J", "J");
            JKLNode.InsertKeyAndValue("K", "K");
            JKLNode.InsertKeyAndValue("L", "L");
            var NONode = GetBTreeNode(tree, "no");
            NONode.InsertKeyAndValue("N", "N");
            NONode.InsertKeyAndValue("O", "O");
            CGMNode.SetChildAt(ABNode, 0);
            CGMNode.SetChildAt(DEFode, 1);
            CGMNode.SetChildAt(JKLNode, 2);
            CGMNode.SetChildAt(NONode, 3);
            CGMNode.SetNbChildren(4);
            var QRSNode = GetBTreeNode(tree, "qrs");
            QRSNode.InsertKeyAndValue("Q", "Q");
            QRSNode.InsertKeyAndValue("R", "R");
            QRSNode.InsertKeyAndValue("S", "S");
            var UVNode = GetBTreeNode(tree, "uv");
            UVNode.InsertKeyAndValue("U", "U");
            UVNode.InsertKeyAndValue("V", "V");
            var YZNode = GetBTreeNode(tree, "yz");
            YZNode.InsertKeyAndValue("Y", "Y");
            YZNode.InsertKeyAndValue("Z", "Z");
            TXNode.SetChildAt(QRSNode, 0);
            TXNode.SetChildAt(UVNode, 1);
            TXNode.SetChildAt(YZNode, 2);
            TXNode.SetNbChildren(3);
            // case 1
            var s2AfterDeleteingF = "h=1:[P]" + "h=2:[C,G,M][T,X]" + "h=3:[A,B][D,E][J,K,L][N,O][Q,R,S][U,V][Y,Z]";
            var F = tree.Delete("F", "F");
            AssertEquals("F", F);
            var s = new BTreeDisplay().Build(tree.GetRoot(), 3, false).ToString();
            s = s.Replace(" ", string.Empty);
            s = s.Replace("\n", string.Empty);
            AssertEquals(s2AfterDeleteingF, s);
            // case 2a
            var s2AfterDeleteingM = "h=1:[P]" + "h=2:[C,G,L][T,X]" + "h=3:[A,B][D,E][J,K][N,O][Q,R,S][U,V][Y,Z]";
            var M = tree.Delete("M", "M");
            AssertEquals("M", M);
            s = new BTreeDisplay().Build(tree.GetRoot(), 3, false).ToString();
            s = s.Replace(" ", string.Empty);
            s = s.Replace("\n", string.Empty);
            AssertEquals(s2AfterDeleteingM, s);
            // case 2c
            var s2AfterDeleteingG = "h=1:[P]" + "h=2:[C,L][T,X]" + "h=3:[A,B][D,E,J,K][N,O][Q,R,S][U,V][Y,Z]";
            var G = tree.Delete("G", "G");
            AssertEquals("G", G);
            s = new BTreeDisplay().Build(tree.GetRoot(), 3, false).ToString();
            s = s.Replace(" ", string.Empty);
            s = s.Replace("\n", string.Empty);
            AssertEquals(s2AfterDeleteingG, s);
            // case 3b
            var s2AfterDeleteingD = "h=1:[C,L,P,T,X]" + "h=2:[A,B][E,J,K][N,O][Q,R,S][U,V][Y,Z]";
            var D = tree.Delete("D", "D");
            // assertEquals(2, tree.getHeight());
            AssertEquals("D", D);
            s = new BTreeDisplay().Build(tree.GetRoot(), 3, false).ToString();
            s = s.Replace(" ", string.Empty);
            s = s.Replace("\n", string.Empty);
            s = s.Replace("h=3:", string.Empty);
            AssertEquals(s2AfterDeleteingD, s);
            // case 3a
            var s2AfterDeleteingB = "h=1:[E,L,P,T,X]" + "h=2:[A,C][J,K][N,O][Q,R,S][U,V][Y,Z]";
            var B = tree.Delete("B", "B");
            AssertEquals("B", B);
            s = new BTreeDisplay().Build(tree.GetRoot(), 3, false).ToString();
            s = s.Replace(" ", string.Empty);
            s = s.Replace("\n", string.Empty);
            s = s.Replace("h=3:", string.Empty);
            AssertEquals(s2AfterDeleteingB, s);
        }

        /// <exception cref="System.Exception"></exception>
        [Test]
        public virtual void TestDelete3()
        {
            IBTree btree = GetBTree(3);
            var size = 10;
            for (var i = 0; i < size; i++)
                btree.Insert(i, "key " + i);
            AssertEquals(size, btree.GetSize());
            for (var i = 0; i < size; i++)
                AssertEquals("key " + i, btree.Delete(i, "key " + i));
            AssertEquals(0, btree.GetSize());
        }

        /// <exception cref="System.Exception"></exception>
        [Test]
        public virtual void TestDeleteInsert100000()
        {
            IBTree btree = GetBTree(3);
            var size = 2000;
            for (var i = 0; i < size; i++)
                btree.Insert(i, "key " + i);
            AssertEquals(size, btree.GetSize());
            for (var i = 0; i < size; i++)
                AssertEquals("key " + i, btree.Delete(i, "key " + i));
            AssertEquals(0, btree.GetSize());
            AssertEquals(1, btree.GetHeight());
            AssertEquals(0, btree.GetRoot().GetNbKeys());
            AssertEquals(0, btree.GetRoot().GetNbChildren());
            for (var i = 0; i < size; i++)
                btree.Insert(size + i, "key " + (i + size));
            for (var i = 0; i < size; i++)
                AssertEquals("key " + (i + size), btree.Delete(i + size, "key " + (i + size)));
            AssertEquals(0, btree.GetSize());
            AssertEquals(1, btree.GetHeight());
            AssertEquals(0, btree.GetRoot().GetNbKeys());
            AssertEquals(0, btree.GetRoot().GetNbChildren());
        }

        /// <exception cref="System.Exception"></exception>
        [Test]
        public virtual void TestDeleteStringKey()
        {
            IBTree btree = GetBTree(3);
            btree.Insert("key70", "70");
            btree.Insert("key71", "71");
            // println(new BTreeDisplay().build(btree));
            AssertEquals("70", btree.GetRoot().GetKeyAndValueAt(0).GetValue());
            AssertEquals("71", btree.GetRoot().GetKeyAndValueAt(1).GetValue());
        }

        /// <exception cref="System.Exception"></exception>
        [Test]
        public virtual void TestDeleteStringKey2()
        {
            IBTree btree = GetBTree(3);
            btree.Insert("key700", "700");
            btree.Insert("key710", "710");
            btree.Insert("key720", "720");
            btree.Insert("key730", "730");
            btree.Insert("key740", "740");
            btree.Insert("key715", "715");
        }

        // println(new BTreeDisplay().build(btree));
        // assertEquals("70", btree.getRoot().getKeyAndValueAt(0).getValue());
        // assertEquals("71", btree.getRoot().getKeyAndValueAt(1).getValue());

        [Test]
        public virtual void TestInsert()
        {
            var btree = GetBTree(3);
            btree.Insert(1, "key 1");
            AssertEquals(1, btree.GetSize());
            AssertEquals("key 1", btree.Search(1));
        }

        [Test]
        public virtual void TestInsert2()
        {
            var btree = GetBTree(3);
            btree.Insert(1, "key 1");
            btree.Insert(2, "key 2");
            btree.Insert(3, "key 3");
            btree.Insert(4, "key 4");
            btree.Insert(5, "key 5");
            AssertEquals(5, btree.GetSize());
            AssertEquals("key 1", btree.Search(1));
            AssertEquals("key 2", btree.Search(2));
            AssertEquals("key 3", btree.Search(3));
            AssertEquals("key 4", btree.Search(4));
            AssertEquals("key 5", btree.Search(5));
        }

        [Test]
        public virtual void TestInsert3()
        {
            var btree = GetBTree(3);
            btree.Insert(1, "key 1");
            btree.Insert(2, "key 2");
            btree.Insert(3, "key 3");
            btree.Insert(4, "key 4");
            btree.Insert(5, "key 5");
            btree.Insert(6, "key 6");
            AssertEquals(6, btree.GetSize());
            AssertEquals("key 1", btree.Search(1));
            AssertEquals("key 2", btree.Search(2));
            AssertEquals("key 3", btree.Search(3));
            AssertEquals("key 4", btree.Search(4));
            AssertEquals("key 5", btree.Search(5));
            AssertEquals("key 6", btree.Search(6));
            AssertEquals(2, btree.GetRoot().GetNbChildren());
            // child 1 should be [1,2]
            var child1 = (IBTreeNodeOneValuePerKey) btree.GetRoot().GetChildAt(0, false);
            AssertEquals(2, child1.GetNbKeys());
            AssertEquals(0, child1.GetNbChildren());
            AssertEquals("key 1", child1.GetKeyAndValueAt(0).GetValue());
            AssertEquals(1, child1.GetKeyAndValueAt(0).GetKey());
            // child 2 should be [4,5,6]
            var child2 = (IBTreeNodeOneValuePerKey) btree.GetRoot().GetChildAt(1, false);
            AssertEquals(3, child2.GetNbKeys());
            AssertEquals(0, child2.GetNbChildren());
            AssertEquals("key 4", child2.GetKeyAndValueAt(0).GetValue());
            AssertEquals("key 5", child2.GetKeyAndValueAt(1).GetValue());
            AssertEquals("key 6", child2.GetKeyAndValueAt(2).GetValue());
            // child 2 should be null
            var child3 = (IBTreeNodeOneValuePerKey) btree.GetRoot().GetChildAt(2, false);
            AssertEquals(null, child3);
        }

        /// <summary>
        ///   <pre>node1 = [    10,     100]
        ///     |        |
        ///     |        |
        ///     c1       c2
        ///     c1 = [1,2,3]
        ///     result of split should be
        ///     node1 = [    2  ,    10,     100]
        ///     |        |      |
        ///     |        |      |
        ///     c1       c1'    c2
        ///     where c1 = [1]
        ///     and c1'=[3]</pre>
        /// </summary>
        [Test]
        public virtual void TestSplit()
        {
            IBTree tree = GetBTree(2);
            tree.Insert(10, "Key 10");
            tree.Insert(100, "Key 100");
            var c1 = GetBTreeNode(tree, "child 1");
            var c2 = GetBTreeNode(tree, "child 2");
            var node1 = (IBTreeNodeOneValuePerKey) tree.GetRoot();
            node1.SetChildAt(c1, 0);
            node1.SetChildAt(c2, 1);
            node1.SetNbKeys(2);
            node1.SetNbChildren(2);
            c1.SetKeyAndValueAt(1, "Key 1", 0);
            c1.SetKeyAndValueAt(2, "Key 2", 1);
            c1.SetKeyAndValueAt(3, "Key 3", 2);
            c1.SetNbKeys(3);
            AssertEquals(0, c1.GetNbChildren());
            tree.Split(node1, c1, 0);
            AssertEquals(3, node1.GetNbKeys());
            AssertEquals(3, node1.GetNbChildren());
            AssertEquals(2, node1.GetKeyAndValueAt(0).GetKey());
            AssertEquals(10, node1.GetKeyAndValueAt(1).GetKey());
            AssertEquals(100, node1.GetKeyAndValueAt(2).GetKey());
            var c1New = (IBTreeNodeOneValuePerKey) node1.GetChildAt(0, false);
            AssertEquals(1, c1New.GetNbKeys());
            AssertEquals(0, c1New.GetNbChildren());
            AssertEquals(1, c1New.GetKeyAt(0));
            AssertEquals(null, c1New.GetKeyAt(1));
            AssertEquals(null, c1New.GetKeyAt(2));
            AssertEquals(node1, c1New.GetParent());
            var c1bis = (IBTreeNodeOneValuePerKey) node1.GetChildAt(1, false);
            AssertEquals(1, c1bis.GetNbKeys());
            AssertEquals(0, c1bis.GetNbChildren());
            AssertEquals(3, c1bis.GetKeyAt(0));
            AssertEquals(node1, c1bis.GetParent());
            AssertEquals(null, c1bis.GetKeyAt(1));
            AssertEquals(null, c1bis.GetKeyAt(2));
            var c2New = (IBTreeNodeOneValuePerKey) node1.GetChildAt(2, false);
            AssertEquals(c2, c2New);
        }

        /// <exception cref="System.Exception"></exception>
        [Test]
        public virtual void TestgetBiggestSmallest1()
        {
            IBTree btree = GetBTree(3);
            btree.Insert(1, "key 1");
            btree.Insert(2, "key 2");
            btree.Insert(3, "key 3");
            btree.Insert(4, "key 4");
            btree.Insert(5, "key 5");
            AssertEquals(5, btree.GetSize());
            AssertEquals("key 5", btree.GetBiggest(btree.GetRoot(), false).GetValue());
            AssertEquals("key 1", btree.GetSmallest(btree.GetRoot(), false).GetValue());
        }

        /// <exception cref="System.Exception"></exception>
        [Test]
        public virtual void TestgetBiggestSmallest1WithDelete()
        {
            var btree = GetBTree(3);
            btree.Insert(1, "key 1");
            btree.Insert(2, "key 2");
            btree.Insert(3, "key 3");
            btree.Insert(4, "key 4");
            btree.Insert(5, "key 5");
            AssertEquals(5, btree.GetSize());
            AssertEquals("key 5", btree.GetBiggest(btree.GetRoot(), true).GetValue());
            AssertEquals("key 1", btree.GetSmallest(btree.GetRoot(), true).GetValue());
            AssertEquals(null, btree.Search(1));
            AssertEquals(null, btree.Search(5));
        }

        /// <exception cref="System.Exception"></exception>
        [Test]
        public virtual void TestgetBiggestSmallest1WithDelete2()
        {
            var btree = GetBTree(10);
            var size = 5000;
            for (var i = 0; i < size; i++)
                btree.Insert(i, "key " + i);
            AssertEquals(size, btree.GetSize());
            AssertEquals("key 4999", btree.GetBiggest(btree.GetRoot(), true).GetValue());
            AssertEquals("key 0", btree.GetSmallest(btree.GetRoot(), true).GetValue());
            AssertEquals(null, btree.Search(0));
            AssertEquals(null, btree.Search(4999));
        }

        [Test]
        public virtual void Testsearch10()
        {
            var btree = GetBTree(3);
            for (var i = 0; i < 10; i++)
                btree.Insert(i, "key " + i);
            AssertEquals(10, btree.GetSize());
            AssertEquals("key 1", btree.Search(1));
            AssertEquals("key 9", btree.Search(9));
            var child3 = (IBTreeNodeOneValuePerKey) btree.GetRoot().GetChildAt(2, false);
            AssertEquals(4, child3.GetNbKeys());
            AssertEquals(6, child3.GetKeyAt(0));
            AssertEquals(7, child3.GetKeyAt(1));
            AssertEquals(8, child3.GetKeyAt(2));
            AssertEquals(9, child3.GetKeyAt(3));
            AssertEquals(null, child3.GetKeyAt(4));
        }

        [Test]
        public virtual void Testsearch10000()
        {
            var btree = GetBTree(10);
            var size = 10000;
            for (var i = 0; i < size; i++)
                btree.Insert(i, "key " + i);
            AssertEquals(size, btree.GetSize());
            for (var i = 0; i < size; i++)
                AssertEquals("key " + i, btree.Search(i));
        }

        [Test]
        public virtual void Testsearch500()
        {
            var btree = GetBTree(3);
            for (var i = 0; i < 500; i++)
                btree.Insert(i, "key " + i);
            AssertEquals(500, btree.GetSize());
            AssertEquals("key 1", btree.Search(1));
            AssertEquals("key 499", btree.Search(499));
        }

        [Test]
        public virtual void Testsearch5000()
        {
            var btree = GetBTree(10);
            var size = 5000;
            for (var i = 0; i < size; i++)
                btree.Insert(i, "key " + i);
            AssertEquals(size, btree.GetSize());
            for (var i = 0; i < size; i++)
                AssertEquals("key " + i, btree.Search(i));
        }
    }
}
