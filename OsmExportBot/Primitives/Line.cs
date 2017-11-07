using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OsmExportBot.Primitives
{
    public struct Line
    {
        public Point[] Points;
        public string Color;

        public Line(IEnumerable<Point> points)
            : this(points, "") { }

        public Line(IEnumerable<Point> points, string color)
        {
            Color = color;
            Points = points.ToArray();
        }
    }
}
