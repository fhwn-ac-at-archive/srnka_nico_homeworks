using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace Befunge93_Srnka
{
    public class Befunge
    {
        char[,] BefungeCode;
        public Stack<int> BefungeStack;
        bool StringMode = false;
        int InstructionX = 0;
        int InstructionY = 0;
        Direction pointerDirection = Direction.right;
        public List<string> Output;

        public Befunge(string path)
        {
            Output = new List<string>();
            BefungeCode = new char[80, 25]; // befunge restricts each valid program to a grid of 80 instructions horizontally by 25 instructions vertically.
            for (int x = 0; x < 80; x++)
            {
                for (int y = 0; y < 25; y++)
                {
                    BefungeCode[x, y] = ' ';
                }

            }
            this.BefungeStack = new Stack<int>();

            //load the befunge code
            using (StreamReader sr = new StreamReader(path))
            {
                string line = "";
                int j = 0;
                while (line != null)
                {
                    if (j >= 25)
                    {
                        break; //break if vertical count is larger than vertival limit
                    }
                    line = sr.ReadLine();
                    if (line != null) // iterate new line
                    {
                        for (int i = 0; i < line.Length; i++)
                        {
                            if (i >= 80)
                            {
                                break; //break if  line is bigger than horizontal limit
                            }
                            BefungeCode[i, j] = line.ToCharArray()[i]; //insert loaded code to exact location
                        }
                    }
                    j++; //increase on each new line
                }
            }
        }



        private void GetNextInstruction()
        {
            switch (pointerDirection)
            {
                case Direction.up:
                    InstructionY = (InstructionY <= 0) ? 25 : InstructionY - 1;
                    break;
                case Direction.down:
                    InstructionY = (InstructionY >= 50) ? 0 : InstructionY + 1;
                    break;
                case Direction.right:
                    InstructionX = (InstructionX >= 80) ? 0 : InstructionX + 1;
                    break;
                case Direction.left:
                    InstructionX = (InstructionX <= 0) ? 80 : InstructionX - 1;
                    break;
            }
        }

        public void CalcBefunge()
        {
            while (BefungeCode[InstructionX, InstructionY] != '@') //@ means end of program
            {
                Execute(BefungeCode[InstructionX, InstructionY]);
                GetNextInstruction();
            }
        }

        public void Execute(char c)
        {
            if (c == ' ')
            {
                if (StringMode)
                {
                    BefungeStack.Push(Convert.ToInt32(c));
                }
                return;
            }
            if (StringMode && c != '"')
            {
                BefungeStack.Push(Convert.ToInt32(c));
                return;

            }
            if (Char.IsNumber(c)) // Push this number on the stack 0-9
            {
                BefungeStack.Push(int.Parse(c.ToString()));
                return;
            }

            int a, b, x, y, v;

            switch (c)
            {
                case '+': // Addition: Pop a and b, then push a+b
                    a = BefungeStack.Pop();
                    b = BefungeStack.Pop();
                    BefungeStack.Push(a + b);
                    break;
                case '-': // Subtraction: Pop a and b, then push b-a
                    a = BefungeStack.Pop();
                    b = BefungeStack.Pop();
                    BefungeStack.Push(b-a);
                    break;
                case '*': // Multiplication: Pop a and b, then push a*b
                    a = BefungeStack.Pop();
                    b = BefungeStack.Pop();
                    BefungeStack.Push(a * b);
                    break;
                case '/': // Integer division: Pop a and b, then push b/a, rounded towards 0.
                    a = BefungeStack.Pop();
                    b = BefungeStack.Pop();
                    BefungeStack.Push(b / a);
                    break;
                case '%': // Modulo: Pop a and b, then push the remainder of the integer division of b/a.
                    a = BefungeStack.Pop();
                    b = BefungeStack.Pop();
                    BefungeStack.Push(b % a);
                    break;
                case '!': // Logical NOT: Pop a value. If the value is zero, push 1; otherwise, push zero.
                    a = BefungeStack.Pop();
                    BefungeStack.Push((a == 0) ? 1 : 0);
                    break;
                case '`': //Greater than: Pop a and b, then push 1 if b>a, otherwise zero.
                    a = BefungeStack.Pop();
                    b = BefungeStack.Pop();
                    BefungeStack.Push((b > a) ? 1 : 0);
                    break;
                case '>': //Start moving right
                    pointerDirection = Direction.right;
                    break;
                case '<': //Start moving left
                    pointerDirection = Direction.left;
                    break;
                case '^': //Start moving up
                    pointerDirection = Direction.up;
                    break;
                case 'v': //Start moving down
                    pointerDirection = Direction.down;
                    break;
                case '?': //Start moving in a random cardinal direction
                    Random r = new Random();
                    pointerDirection = (Direction)r.Next(4);
                    break;
                case '_': // Pop a value; move right if value=0, left otherwise
                    a = BefungeStack.Pop();
                    pointerDirection = (a == 0) ? Direction.right : Direction.left;
                    break;
                case '|': //Pop a value; move down if value=0, up otherwise
                    a = BefungeStack.Pop();
                    pointerDirection = (a == 0) ? Direction.down : Direction.up; ;
                    break;
                case '"':
                    StringMode = !StringMode; // Start string mode: push each character's ASCII value all the way up to the next "
                    break;
                case ':': //Duplicate value on top of the stack
                    BefungeStack.Push((BefungeStack.Count > 0) ? BefungeStack.Peek() : 0);
                    break;
                case '\\': //Swap two values on top of the stack
                    a = BefungeStack.Pop();
                    b = BefungeStack.Pop();
                    BefungeStack.Push(a);
                    BefungeStack.Push(b);
                    break;
                case '$': //Pop value from the stack and discard it
                    if (BefungeStack.Count > 0)
                    {
                        BefungeStack.Pop();
                    }
                    break;
                case '.': //Pop value and output as an integer followed by a space
                    var popedVal = BefungeStack.Pop();
                    Console.Write(popedVal + " ");
                    Output.Add(popedVal.ToString());
                    break;
                case ',': //Pop value and output as ASCII character
                    var popedVal2 = BefungeStack.Pop();
                    Console.Write(Convert.ToChar(popedVal2));
                    Output.Add(Convert.ToChar(popedVal2).ToString());
                    break;
                case '#': //Bridge: Skip next cell
                    GetNextInstruction();
                    break;
                case 'p': //A "put" call (a way to store a value for later use). Pop y, x, and v, then change the character at (x,y) in the program to the character with ASCII value v
                    y = BefungeStack.Pop();
                    x = BefungeStack.Pop();
                    v = BefungeStack.Pop();
                    BefungeCode[x, y] = Convert.ToChar(v);
                    break;
                case 'g': //A "get" call (a way to retrieve data in storage). Pop y and x, then push ASCII value of the character at that position in the program
                    a = BefungeStack.Pop();
                    b = BefungeStack.Pop();
                    BefungeStack.Push(BefungeCode[b, a]);
                    break;
                case '&': //Ask user for a number and push it
                    Console.Write("Enter number:");
                    BefungeStack.Push(Console.Read());
                    break;
                case '~': //Ask user for a character and push its ASCII value
                    Console.Write("Enter character:");
                    var input = Console.ReadKey();
                    BefungeStack.Push(input.KeyChar);
                    break;
            }
        }
    }
}
