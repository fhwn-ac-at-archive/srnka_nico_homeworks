using System;
using System.Collections.Generic;
using System.Text;

namespace GisApp
{
    public class PolygonBoundingBox
    {
        public double XMin { get; set; }
        public double YMin{get;set;}
        public double XMax{get;set;}
        public double YMax{get;set;}
        public PolygonBoundingBox(double xMin, double yMin, double xMax, double yMax)
        {
            this.XMin = xMin;
            this.XMax = xMax;
            this.YMin = yMin;
            this.YMax = yMax;
        }
    }
}
