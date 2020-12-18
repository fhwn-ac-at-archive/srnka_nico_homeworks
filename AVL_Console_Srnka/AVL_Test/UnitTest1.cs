using AVL_Console_Srnka;
using NUnit.Framework;
using System.Collections.Generic;

namespace AVL_Test
{
    public class Tests
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void TestContainsTrue()
        {
            AvlTree tree = new AvlTree();

            tree.Insert(20);
            tree.Insert(50);
            tree.Insert(10);
            tree.Insert(30);

            Assert.True(tree.Contains(20, tree.Root));
        }
        [Test]
        public void TestContainsFalse()
        {
            AvlTree tree = new AvlTree();

            tree.Insert(20);
            tree.Insert(50);
            tree.Insert(10);
            tree.Insert(30);

            Assert.False(tree.Contains(100, tree.Root));
        }
        [Test]
        public void TestCountTest()
        {
            AvlTree tree = new AvlTree();
            tree.Insert(20);
            tree.Insert(50);
            tree.Insert(10);
            tree.Insert(30);
            Assert.That(tree.CountTreeRecursive(tree.Root) == 4);
        }
        [Test]
        public void TestInsert()
        {
            AvlTree tree = new AvlTree();
            tree.Insert(20);
            Assert.That(tree.Root.Data == 20);
        }
        [Test]
        public void TestInsertAlreadyIn()
        {
            AvlTree tree = new AvlTree();
            tree.Insert(20);
            tree.Insert(20);
            Assert.That(tree.Root.Data == 20 && tree.Root.Left == null && tree.Root.Right == null);
        }
        [Test]
        public void TestRemove()
        {
            AvlTree tree = new AvlTree();
            tree.Insert(10);
            tree.RemoveItem(10);
            Assert.That(tree.Root == null);
        }

        [Test]
        public void TestCount()
        {
            AvlTree tree = new AvlTree();
            tree.Insert(10);
            tree.Insert(20);
            tree.Insert(30);
            Assert.That(tree.CountTreeRecursive(tree.Root) == 3);
        }
        [Test]
        public void TestClear()
        {
            AvlTree tree = new AvlTree();
            tree.Insert(10);
            tree.Insert(20);
            tree.Insert(30);
            var before = tree.CountTreeRecursive(tree.Root);
            tree.ClearTree();
            var after = tree.CountTreeRecursive(tree.Root);
            Assert.That(before == 3 && after == 0);
        }

        [Test]
        public void TestInorder()
        {
            AvlTree tree = new AvlTree();
            tree.Insert(10);
            tree.Insert(20);
            tree.Insert(50);
            tree.Insert(100);
            tree.Insert(30);
            tree.PrintInorder(tree.Root);
        }
        [Test]
        public void TestPreorderReturnsList()
        {
            AvlTree tree = new AvlTree();
            tree.Insert(10);
            tree.Insert(20);
            tree.Insert(50);
            tree.Insert(100);
            tree.Insert(30);
            var list = tree.PrintPreorder(tree.Root);
            var checkList = new List<int> { 20, 10, 50, 30, 100 };
            var checkIfEqual = true;
            for (int i = 0; i < list.Count; i++)
            {
                if (list[i] != checkList[i])
                {
                    checkIfEqual = false;
                }
            }
            Assert.True(checkIfEqual);
        }
        [Test]
        public void TestPreorderReturnsNull()
        {
            AvlTree tree = new AvlTree();

            var list = tree.PrintPreorder(tree.Root);
            Assert.True(list == null);
        }
        [Test]
        public void TestInorderReturnsList()
        {
            AvlTree tree = new AvlTree();
            tree.Insert(10);
            tree.Insert(20);
            tree.Insert(50);
            tree.Insert(100);
            tree.Insert(30);
            var list = tree.PrintInorder(tree.Root);
            var checkList = new List<int> { 10, 20, 30, 50, 100 };
            var checkIfEqual = true;
            for (int i = 0; i < list.Count; i++)
            {
                if (list[i] != checkList[i])
                {
                    checkIfEqual = false;
                }
            }
            Assert.True(checkIfEqual);
        }
        [Test]
        public void TestInorderReturnsNull()
        {
            AvlTree tree = new AvlTree();

            var list = tree.PrintInorder(tree.Root);
            Assert.True(list == null);
        }
        [Test]
        public void TestPostorderReturnsList()
        {
            AvlTree tree = new AvlTree();
            tree.Insert(10);
            tree.Insert(20);
            tree.Insert(50);
            tree.Insert(100);
            tree.Insert(30);
            var list = tree.PrintPostorder(tree.Root);
            var checkList = new List<int> { 10, 30, 100, 50, 20 };
            var checkIfEqual = true;
            for (int i = 0; i < list.Count; i++)
            {
                if (list[i] != checkList[i])
                {
                    checkIfEqual = false;
                }
            }
            Assert.True(checkIfEqual);
        }
        [Test]
        public void TestPostorderReturnsNull()
        {
            AvlTree tree = new AvlTree();

            var list = tree.PrintPostorder(tree.Root);
            Assert.True(list == null);
        }
    }
}