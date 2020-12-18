using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SudokuSolver_Srnka
{
    public class Solver
    {
        //game board multi-dimensional array
        public int[,] board;
        public static bool CheckNumber(int[,] board, int row, int col, int checkNumber)
        {

            for (int element = 0; element < board.GetLength(0); element++)
            {
                // check if number is already in that row 
                if (board[row, element] == checkNumber)
                {
                    return false;
                }
            }


            for (int rowIdx = 0; rowIdx < board.GetLength(0); rowIdx++)
            {
                // check if number is already in columns
                if (board[rowIdx, col] == checkNumber)
                {
                    return false;
                }
            }

            int sqrt = (int)Math.Sqrt(board.GetLength(0));
            int boxRowStart = row - row % sqrt;
            int boxColStart = col - col % sqrt;

            for (int rowIdx = boxRowStart; rowIdx < boxRowStart + sqrt; rowIdx++)
            {
                for (int numberIdx = boxColStart; numberIdx < boxColStart + sqrt; numberIdx++)
                {
                    if (board[rowIdx, numberIdx] == checkNumber)
                    {
                        return false;
                    }
                }
            }

            // if everything passes return true
            return true;
        }

        public bool SolveSudoku(int[,] board, int boardSize)
        {
            int row = -1;
            int col = -1;
            bool isEmpty = true;
            for (int i = 0; i < boardSize; i++)
            {
                for (int j = 0; j < boardSize; j++)
                {
                    if (board[i, j] == 0)
                    {
                        row = i;
                        col = j;
                        isEmpty = false;
                        break;
                    }
                }
                if (!isEmpty)
                {
                    break;
                }
            }

            // board full 
            if (isEmpty)
            {
                return true;
            }

            // backtracking
            for (int num = 1; num <= boardSize; num++)
            {
                if (CheckNumber(board, row, col, num))
                {
                    board[row, col] = num;
                    if (SolveSudoku(board, boardSize))
                    {
                        return true;
                    }
                    else
                    {
                        board[row, col] = 0;
                    }
                }
            }
            return false;
        }

        public void PrintBoard(int[,] board, int N)
        {

            for (int i = 1; i < N + 1; ++i)
            {
                for (int j = 1; j < N + 1; ++j)
                    Console.Write("|{0}", board[i - 1, j - 1]);

                Console.WriteLine("|");
                if (i % 3 == 0) Console.WriteLine("+-----+-----+-----+");
            }
        }

        public bool ValidateBoard(int[,] grid)
        {
            for (int i = 0; i < 9; i++)
            {
                bool[] row = new bool[10];
                bool[] col = new bool[10];

                for (int j = 0; j < 9; j++)
                {
                    if (row[grid[i, j]] & grid[i, j] > 0)
                    {
                        return false;
                    }
                    row[grid[i, j]] = true;

                    if (col[grid[j, i]] & grid[j, i] > 0)
                    {
                        return false;
                    }
                    col[grid[j, i]] = true;

                    if ((i + 3) % 3 == 0 && (j + 3) % 3 == 0)
                    {
                        bool[] sqr = new bool[10];
                        for (int m = i; m < i + 3; m++)
                        {
                            for (int n = j; n < j + 3; n++)
                            {
                                if (sqr[grid[m, n]] & grid[m, n] > 0)
                                {
                                    return false;
                                }
                                sqr[grid[m, n]] = true;
                            }
                        }
                    }

                }
            }
            return true;
        }



        public bool CreateBoard(string name, string path)
        {
            int width = 0;
            int height = 0;
            int counterX = 0;
            try
            {
                string[] lines = File.ReadAllLines(path + "/" + name);
                height = lines.Length;
                board = new int[height, height];
                foreach (string line in lines)
                {
                    var parsedLine = line.Replace(" ", "");
                    var splitLine = parsedLine.Split(',');
                    width = splitLine.Length;
                    for (int i = 0; i < splitLine.Length; i++)
                    {
                        board[counterX, i] = Int32.Parse(splitLine[i]);
                    }
                    if (width != height)
                    {
                        return false;
                    }
                    counterX++;
                }
            }
            catch (FormatException e)
            {
                Console.WriteLine("File contains strings. Only number allowed.");
                return false;
            }
            catch (Exception e)
            {
                Console.WriteLine("File not readable or not found. File location must be in Project root folder.");
                return false;
            }
            return true;
        }

        public string[] GetFiles(string path)
        {
            var fileList = Directory.GetFiles(path, "*.txt");
            for (int i = 0; i < fileList.Length; i++)
            {
                fileList[i] = Path.GetFileName(fileList[i]);
            }
            return fileList;
        }
    }
}
