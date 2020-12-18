using Microsoft.VisualStudio.TestPlatform.TestHost;
using NUnit.Framework;
using SudokuSolver_Srnka;
using System.Diagnostics;

namespace UnitTesting
{
    public class Tests
    {

        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void Test1()
        {
            Assert.Pass();
        }

        [Test]
        public void Test_File_List()
        {
            var test = new Solver();
            string[] list = { "aufgabe1.txt", "aufgabe2.txt", "aufgabe3.txt" };
            Assert.That(test.GetFiles(@"E:\FH\semester-3\Algodat Übung\SudokuSolver_Srnka\SudokuSolver_Srnka").Length == 6);
        }

        [Test]
        public void Test_Create_Board_From_File()
        {
            var test = new Solver();
            Assert.That(test.CreateBoard("aufgabe1.txt", @"E:\FH\semester-3\Algodat Übung\SudokuSolver_Srnka\SudokuSolver_Srnka"));
        }
        [Test]
        public void Test_Create_Board_From_File_Parse_Error()
        {
            var test = new Solver();
            Assert.That(!test.CreateBoard("aufgabe5_invalid_input.txt", @"E:\FH\semester-3\Algodat Übung\SudokuSolver_Srnka\SudokuSolver_Srnka"));
        }

        [Test]
        public void Test_Create_Board_From_File_Invalid_Size()
        {
            var test = new Solver();
            Assert.That(!test.CreateBoard("aufgabe4_invalid_size.txt", @"E:\FH\semester-3\Algodat Übung\SudokuSolver_Srnka\SudokuSolver_Srnka"));
        }
        [Test]
        public void Test_Solveable()
        {
            var test = new Solver();
            test.CreateBoard("aufgabe1.txt", @"E:\FH\semester-3\Algodat Übung\SudokuSolver_Srnka\SudokuSolver_Srnka");
            Assert.That(test.ValidateBoard(test.board));
        }
        [Test]
        public void Test_Not_Solveable()
        {
            var test = new Solver();
            test.CreateBoard("aufgabe6_not_solveable.txt", @"E:\FH\semester-3\Algodat Übung\SudokuSolver_Srnka\SudokuSolver_Srnka");
            Assert.That(!test.CreateBoard("aufgabe4_invalid_size.txt", @"E:\FH\semester-3\Algodat Übung\SudokuSolver_Srnka\SudokuSolver_Srnka"));
        }
        [Test]
        public void Test_Solve_Sudoku()
        {
            var test = new Solver();
            test.CreateBoard("aufgabe1.txt", @"E:\FH\semester-3\Algodat Übung\SudokuSolver_Srnka\SudokuSolver_Srnka");
            Assert.That(test.SolveSudoku(test.board, test.board.GetLength(0)));
        }
    }
}