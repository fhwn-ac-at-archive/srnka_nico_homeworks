using System;
using System.Collections.Generic;
using System.Text;

namespace GisApp
{
    public class PolygonShape
    {
        public string Name { get; set; }
        public Polygon Polygon { get; set; }

        public PolygonShape(Polygon polygon, string name)
        {
            this.Name = name;
            this.Polygon = polygon;
        }

        
    }
}
