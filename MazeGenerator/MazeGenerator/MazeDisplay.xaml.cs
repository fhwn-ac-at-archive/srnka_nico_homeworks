using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace MazeGenerator
{
    /// <summary>
    /// Interaktionslogik für Maze.xaml
    /// </summary>
    public partial class MazeDisplay : Window
    {
        public MazeDisplay(Configuration con)
        {
            InitializeComponent();
            SetUpCanvas(con.CanvasHeight, con.CanvasWidth);
            DrawMaze(con.H, con.V);
        }


        private void DrawMaze(int h, int v)
        {
            Width = MazeCan.Width + 2 * MazeCan.Margin.Left;
            Height = MazeCan.Height + 2 * MazeCan.Margin.Top;
            Wilson wilson = new Wilson();
            Maze maze = wilson.Create(v, h);
            
            double hCellSize = MazeCan.Width / h;
            double vCellSize = MazeCan.Height / v;
            // start at top left
            for (int row = 0; row < v; row++)
            {
                double vOffset = row * vCellSize;
                for (int col = 0; col < h; col++)
                {
                    double hOffset = col * hCellSize;
                    Cell thisCell = maze._cells[row, col];
                    if (!thisCell.Linked(thisCell.South))
                    {
                        DrawLine(hOffset, vOffset + vCellSize, hOffset + hCellSize, vOffset + vCellSize);
                    }
                    if (!thisCell.Linked(thisCell.East))
                    {
                        DrawLine(hOffset + hCellSize, vOffset, hOffset + hCellSize, vOffset + vCellSize);
                    }
                }
            }
            DrawLine(0, 0, MazeCan.Width, 0);
            DrawLine(0, 0, 0, MazeCan.Height);
        }

        private void DrawLine(double x1, double y1, double x2, double y2)
        {
            Line line = new Line { X1 = x1, Y1 = y1, X2 = x2, Y2 = y2, Stroke = Brushes.Blue, StrokeThickness = .8 };
            MazeCan.Children.Add(line);
        }
        private void SetUpCanvas(int hPixels, int vPixels)
        {
            MazeCan.Width = hPixels;
            MazeCan.Height = vPixels;
            Width = MazeCan.Width + 2 * MazeCan.Margin.Left;
            Height = MazeCan.Height + 2 * MazeCan.Margin.Top;
            MazeCan.Children.Clear();
        }
    }
}
