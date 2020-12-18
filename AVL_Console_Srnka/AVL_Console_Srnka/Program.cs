using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AVL_Console_Srnka
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Welcome to my AVL-Tree implementation.");
            Help();
            AvlTree tree = new AvlTree();
            string input = "";
            Console.Write("Command: ");
            while((input = Console.ReadLine()) != "quit")
            {
                Console.WriteLine();
                var split = input.Split(' ');
                if (input.Contains("insert"))
                {
                    tree.Insert(Int32.Parse(split[1]));
                    tree.PrintTree(tree.Root);
                }
                else if(input.Contains("remove"))
                {
                    tree.RemoveItem(Int32.Parse(split[1]));
                }
                else if (input.Contains("postorder"))
                {
                    tree.PrintPostorder(tree.Root);
                    Console.WriteLine();
                    var list = tree.PrintPostorder(tree.Root);
                    foreach (var item in list)
                    {
                        Console.Write(item + " ");
                    }
                }
                else if (input.Contains("inorder"))
                {
                   
                    var list = tree.PrintInorder(tree.Root);
                    foreach (var item in list)
                    {
                        Console.Write(item + " ");
                    }
                    Console.WriteLine();
                }
                else if (input.Contains("print-nice"))
                {
                    tree.PrintTree(tree.Root);
                }
                else if (input.Contains("preorder"))
                {
                    var list = tree.PrintPreorder(tree.Root);
                    foreach (var item in list)
                    {
                        Console.Write(item + " ");
                    }
                    Console.WriteLine();
                }
                else if (input.Contains("count"))
                {
                    Console.WriteLine(tree.CountTreeRecursive(tree.Root)); 
                    Console.WriteLine();
                }
                else if (input.Contains("clear"))
                {
                    tree.ClearTree();
                    Console.WriteLine();
                }
                else if (input.Contains("contains"))
                {
                    Console.WriteLine(tree.Contains(Int32.Parse(split[1]), tree.Root));
                    Console.WriteLine();
                }
                else if(input == "help")
                {
                    Help();
                }
                Console.Write("Command: ");
            }
        }

        public static void Help()
        {
            Console.WriteLine("Available commands:");
            Console.WriteLine("[1] insert <number>");
            Console.WriteLine("[2] remove <number>");
            Console.WriteLine("[3] postorder");
            Console.WriteLine("[4] preorder");
            Console.WriteLine("[5] inorder");
            Console.WriteLine("[6] print-nice");
            Console.WriteLine("[7] count");
            Console.WriteLine("[8] clear");
            Console.WriteLine("[9] help");
            Console.WriteLine("[10] quit");
        }

    }
}
