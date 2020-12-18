using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MazeGenerator
{
    public class Wilson
    {
        private List<Cell> tempList;

        public Maze Create(int rows, int cols)
        {
            //Create a new maze with given size parameters
            Maze maze = new Maze(rows, cols);
            //Init random
            Random r = maze.R;
            List<Cell> walk = new List<Cell>();
            // Pick a random cell to mark as visited. This will be the end point of our first walk
            Cell first = maze.Cells.ToList()[r.Next(0, maze.Cells.Count())];
            // Pick a starting cell for our first walk
            // and set it as visited
            tempList = maze.Cells.Where(c => c.Row != first.Row && c.Col != first.Col).ToList();
            Cell current = tempList[r.Next(0, tempList.Count)];
            walk.Add(maze.Cells.First(c => c.Row == current.Row && c.Col == current.Col));
            while (maze.Cells.Any(c => !c.Links.Any()))
            {
                //set our nex cell to check and walk
                Cell next = walk.Last().Neighbours.ToList()[r.Next(0, walk.Last().Neighbours.Count)];
                //check if our next cell is the first one or linked to any others
                if (next == first || next.Links.Any())
                {
                    walk.Add(next);
                    // get along the current iteration and add a new sequenze to walk
                    walk.Zip(walk.Skip(1), (thisCell, nextCell) => (thisCell, nextCell));
                    walk = LinkCells(walk);
                    // if any cell is not visited yet start a new walk with a random cell
                    if (maze.Cells.Any(c => !c.Links.Any()))
                    {
                        tempList = maze.Cells.Where(c => !c.Links.Any()).ToList();
                        walk = new List<Cell> { tempList[r.Next(0, tempList.Count)] };
                    }
                }
                // if walk already contains the next cell we check
                else if (walk.Contains(next))
                {
                    // here we take all cells after the last time we visited this already containing cell
                    walk = walk.Take(walk.IndexOf(next) + 1).ToList();
                }
                //if no links and next is not the first cell we just add it to our walk
                else
                {
                    walk.Add(next);
                }
            }
            return maze;
        }

        public List<Cell> LinkCells(List<Cell> cells)
        {
            for (int i = 0; i < cells.Count - 1; i++)
            {
                cells[i].Link(cells[i + 1]);
            }
            return cells;
        }
    }
}
