using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
namespace SudokuSolver_Srnka
{
    class Program
    {
        public static void Main(String[] args)
        {
            var sudokuSolver = new Solver();
            var selectedFile = "";
            Console.WriteLine("Hello!");
            Console.WriteLine("Insert valid Sudoku from the file list. Just type in the number of the file. File list is using Project directory.");
            var fileList = sudokuSolver.GetFiles(@"../../");
            Console.WriteLine(fileList.Length);
            for (int i = 0; i < fileList.Length; i++)
            {
                Console.WriteLine($"[{i + 1}] {fileList[i]}");
            }
            Console.Write("Select File: ");
            selectedFile = fileList[Int32.Parse(Console.ReadLine()) - 1];
            Console.WriteLine($"Selected Suko is from File {selectedFile}");
            Console.WriteLine("----------------------------------------------");
            if (sudokuSolver.CreateBoard(selectedFile, @"../../"))
            {
                if (sudokuSolver.ValidateBoard(sudokuSolver.board))
                {
                    int N = sudokuSolver.board.GetLength(0);
                    Console.WriteLine(N);
                    Console.WriteLine(sudokuSolver.board);
                    if (sudokuSolver.SolveSudoku(sudokuSolver.board, N))
                    {
                        sudokuSolver.PrintBoard(sudokuSolver.board, N);
                    }
                    else
                    {
                        Console.Write("No solution");
                    }
                }
            }
            Console.ReadLine();
        }
    }
}
