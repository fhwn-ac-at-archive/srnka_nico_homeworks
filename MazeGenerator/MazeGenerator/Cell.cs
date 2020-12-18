using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MazeGenerator
{
    public class Cell
    {
        public int Row { get; }
        public int Col { get; }
        public List<Cell> Links { get; }
        public Cell North { get; set; }

        public Cell South { get; set; }

        public Cell East { get; set; }

        public Cell West { get; set; }

        public Cell(int row, int col)
        {
            Row = row;
            Col = col;
            Links = new List<Cell>();
        }

        public Cell Link(Cell cell, bool bidirectional = true)
        {
            Links.Add(cell);
            if (bidirectional)
            {
                cell.Link(this, !bidirectional);
            }
            return this;
        }

        public Cell Unlink(Cell cell, bool bidirectional = true)
        {
            Links.Remove(cell);
            if (bidirectional)
            {
                cell.Unlink(this, !bidirectional);
            }
            return this;
        }

        public bool Linked(Cell cell) =>
          Links.Contains(cell);

        public List<Cell> Neighbours =>
          new List<Cell> { North, South, East, West }.Where(c => c != null).ToList();



    }
}
