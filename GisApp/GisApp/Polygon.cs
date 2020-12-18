using System;
using System.Collections.Generic;
using System.Text;

namespace GisApp
{
    public class Polygon
    {
        public PolygonBoundingBox BoundingBox   {get;set;}
        public List<Point> OuterRingPoints      {get;set;}
        public List<Point> InnerRingPoints { get; set; }

        public Polygon(PolygonBoundingBox boundingBox, List<Point> outerRingPoints, List<Point> innerRingPoints)
        {
            this.BoundingBox = boundingBox;
            this.OuterRingPoints = outerRingPoints;
            this.InnerRingPoints = innerRingPoints;
        }

        
    }
}
