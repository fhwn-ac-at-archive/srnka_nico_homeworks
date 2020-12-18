using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AVL_Console_Srnka
{
    public class Node
    {
        public int Data;
        public Node Left;
        public Node Right;
        public Node(int data)
        {
            this.Data = data;
        }
    }
}
