using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace AVL_Srnka
{
    /// <summary>
    /// Interaktionslogik für MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        public MainWindow()
        {
            InitializeComponent();
            AvlTree tree = new AvlTree();
            tree.Insert(5);
            tree.Insert(3);
            tree.Insert(7);
            tree.Insert(2);
            tree.Insert(9);
            tree.Insert(10);
            tree.Insert(12);
            tree.Insert(11);
            tree.Inorder(tree.root);
            Console.WriteLine();
            tree.Postorder(tree.root);
            Console.WriteLine();
            tree.Preorder(tree.root);
            Console.WriteLine(tree.Count());
            tree.print2D(tree.root);
        }
    }
}
