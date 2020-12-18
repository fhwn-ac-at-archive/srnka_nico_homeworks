using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace AVL_Console_Srnka
{
    public class AvlTree
    {
        public Node Root;
        public const int SPACER = 10;
        public AvlTree() { }

        public int CountTreeRecursive(Node current)
        {
            int count = 1;
            if (current == null)
            {
                return 0;
            }
            if (current.Left != null)
            {
                count += CountTreeRecursive(current.Left);
            }
            if (current.Right != null)
            {
                count += CountTreeRecursive(current.Right);
            }
            return count;
        }



        public List<int> PrintPreorder(Node node)
        {
            List<int> preorderedList = new List<int>();
            if (node == null)
            {
                return null;
            }
            Stack<Node> nodes = new Stack<Node>();
            nodes.Push(Root);
            while (nodes.Count > 0)
            {
                Node currNode = nodes.Peek();
                preorderedList.Add(nodes.Pop().Data);
                if (currNode.Right != null)
                {
                    nodes.Push(currNode.Right);
                }
                if (currNode.Left != null)
                {
                    nodes.Push(currNode.Left);
                }
            }
            return preorderedList;
        }

        public List<int> PrintInorder(Node node)
        {
            var inorderList = new List<int>();
            if (node == null)
            {
                return null;
            }
            Stack<Node> nodes = new Stack<Node>();
            Node curr = node;
            while (curr != null || nodes.Count > 0)
            {
                while (curr != null)
                {
                    nodes.Push(curr);
                    curr = curr.Left;
                }
                curr = nodes.Pop();
                inorderList.Add(curr.Data);
                curr = curr.Right;
            }
            return inorderList;
        }

        public List<int> PrintPostorder(Node root)
        {
            if(root == null)
            {
                return null;
            }
            List<int> postorderList = new List<int>();
            Stack<Node> nodes = new Stack<Node>();
            while (true)
            {
                while (root != null)
                {
                    nodes.Push(root);
                    nodes.Push(root);
                    root = root.Left;
                }

                if (nodes.Count == 0)
                {
                    break;
                }
                root = nodes.Pop();

                if (nodes.Count != 0 && nodes.Peek() == root)
                {
                    root = root.Right;
                }
                else
                {
                    Console.WriteLine(root.Data + " ");
                    postorderList.Add(root.Data);
                    root = null;
                }
            }
            return postorderList; 
        }

      

        public void ClearTree()
        {
            Root = null;
        }
        public void Insert(int value)
        {
            Node newItem = new Node(value);
            if (Root == null)
            {
                Root = newItem;
            }
            else
            {
                var tempRoot = RecursiveInsert(Root, newItem);
                if (tempRoot != null)
                {
                    Root = tempRoot;
                }
                else
                {
                    Console.WriteLine("already Inserted");
                }
            }
        }
        private Node RecursiveInsert(Node current, Node n)
        {
            if (current == null)
            {
                current = n;
                return current;
            }
            else if (n.Data < current.Data)
            {
                current.Left = RecursiveInsert(current.Left, n);
                current = BalanceTree(current);
            }
            else if (n.Data > current.Data)
            {
                current.Right = RecursiveInsert(current.Right, n);
                current = BalanceTree(current);
            }
            else if (n.Data == current.Data)
            {
                return null;
            }
            return current;
        }
        private Node BalanceTree(Node current)
        {
            int balanceNumber = GetBlanceFactor(current);
            if (balanceNumber > 1)
            {
                if (GetBlanceFactor(current.Left) > 0)
                {
                    current = RotateLeftLeft(current);
                }
                else
                {
                    current = RotateLeftRight(current);
                }
            }
            else if (balanceNumber < -1)
            {
                if (GetBlanceFactor(current.Right) > 0)
                {
                    current = RotateRightLeft(current);
                }
                else
                {
                    current = RotateRightRight(current);
                }
            }
            return current;
        }

        public void PrettyPrintRecursive(Node root, int space)
        {
            if (root == null)
                return;
            space += SPACER;
            PrettyPrintRecursive(root.Right, space);
            Console.WriteLine();
            for (int i = SPACER; i < space; i++)
            {
                Console.Write(" ");
            }
            Console.WriteLine(root.Data);
            PrettyPrintRecursive(root.Left, space);
        }

        public void PrintTree(Node root)
        {
            PrettyPrintRecursive(root, 0);
        }

        public void RemoveItem(int target)
        {
            Root = Remove(Root, target);
        }
        private Node Remove(Node current, int target)
        {
            Node parent;
            if (current == null)
            {
                return null;
            }
            else
            {
                //left side of tree
                if (target < current.Data)
                {
                    current.Left = Remove(current.Left, target);
                    if (GetBlanceFactor(current) == -2)
                    {
                        if (GetBlanceFactor(current.Right) <= 0)
                        {
                            current = RotateRightRight(current);
                        }
                        else
                        {
                            current = RotateRightLeft(current);
                        }
                    }
                }
                //right side of tree
                else if (target > current.Data)
                {
                    current.Right = Remove(current.Right, target);
                    if (GetBlanceFactor(current) == 2)
                    {
                        if (GetBlanceFactor(current.Left) >= 0)
                        {
                            current = RotateLeftLeft(current);
                        }
                        else
                        {
                            current = RotateLeftRight(current);
                        }
                    }
                }
                //searched value equals to deleted node
                else
                {
                    if (current.Right != null)
                    {
                        parent = current.Right;
                        while (parent.Left != null)
                        {
                            parent = parent.Left;
                        }
                        current.Data = parent.Data;
                        current.Right = Remove(current.Right, parent.Data);
                        if (GetBlanceFactor(current) == 2)
                        {
                            if (GetBlanceFactor(current.Left) >= 0)
                            {
                                current = RotateLeftLeft(current);
                            }
                            else
                            {
                                current = RotateLeftRight(current);
                            }
                        }
                    }
                    else
                    {
                        return current.Left;
                    }
                }
            }
            return current;
        }

        public bool Contains(int target, Node node)
        {
            // Traverse untill root reaches to dead end 
            while (node != null)
            {
                if (target > node.Data)
                {
                    node = node.Right;
                }
                else if (target < node.Data)
                {
                    node = node.Left;
                }
                else
                {
                    return true;
                }
            }
            return false;
        }

        private void InOrderDisplayTree(Node current)
        {
            if (current != null)
            {
                InOrderDisplayTree(current.Left);
                Console.Write("({0}) ", current.Data);
                InOrderDisplayTree(current.Right);
            }
        }
        private int Max(int l, int r)
        {
            return l > r ? l : r;
        }
        private int GetHeight(Node current)
        {
            int height = 0;
            if (current != null)
            {
                int l = GetHeight(current.Left);
                int r = GetHeight(current.Right);
                int m = Max(l, r);
                height = m + 1;
            }
            return height;
        }
        private int GetBlanceFactor(Node current)
        {
            int l = GetHeight(current.Left);
            int r = GetHeight(current.Right);
            int b_factor = l - r;
            return b_factor;
        }
        private Node RotateRightRight(Node parent)
        {
            Node pivot = parent.Right;
            parent.Right = pivot.Left;
            pivot.Left = parent;
            return pivot;
        }
        private Node RotateLeftLeft(Node parent)
        {
            Node pivot = parent.Left;
            parent.Left = pivot.Right;
            pivot.Right = parent;
            return pivot;
        }
        private Node RotateLeftRight(Node parent)
        {
            Node pivot = parent.Left;
            parent.Left = RotateRightRight(pivot);
            return RotateLeftLeft(parent);
        }
        private Node RotateRightLeft(Node parent)
        {
            Node pivot = parent.Right;
            parent.Right = RotateLeftLeft(pivot);
            return RotateRightRight(parent);
        }
    }
}
