using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MazeGenerator
{
    public class Maze
    {
        public int Rows { get; }
        public int Cols { get; }
        public Cell[,] _cells;
        public readonly Random R = new Random();

        public Maze(int rows, int cols)
        {
            Rows = rows;
            Cols = cols;
            _cells = BuildCells();
        }

        public IEnumerable<Cell> Cells
        {
            get
            {
                for (int row = 0; row < Rows; row++)
                {
                    for (int col = 0; col < Cols; col++)
                    {
                        yield return _cells[row, col];
                    }
                }
            }
        }

        private void ConfigureCells()
        {
            for (int col = 0; col < Cols; col++)
            {
                for (int row = 0; row < Rows; row++)
                {
                    if (row > 0)
                    {
                        _cells[row, col].North = _cells[row - 1, col];
                    }
                    if (row < Rows - 1)
                    {
                        _cells[row, col].South = _cells[row + 1, col];
                    }
                    if (col < Cols - 1)
                    {
                        _cells[row, col].East = _cells[row, col + 1];
                    }
                    if (col > 0)
                    {
                        _cells[row, col].West = _cells[row, col - 1];
                    }
                }
            }
        }
        private Cell[,] BuildCells()
        {
            _cells = new Cell[Rows, Cols];
            for (int col = 0; col < Cols; col++)
            {
                for (int row = 0; row < Rows; row++)
                {
                    _cells[row, col] = new Cell(row, col);
                }
            }
            ConfigureCells();
            return _cells;
        }

       
    }
}
