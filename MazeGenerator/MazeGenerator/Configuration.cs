using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MazeGenerator
{
    public class Configuration
    {
        public Configuration(int canvasWidth, int canvasHeight, int v, int h)
        {
            CanvasWidth = canvasWidth;
            CanvasHeight = canvasHeight;
            V = v;
            H = h;
        }

        public int CanvasWidth { get; set; }
        public int CanvasHeight { get; set; }
        public int V { get; set; }
        public int H { get; set; }
    }
}
