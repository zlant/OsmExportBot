using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OsmExportBot.Primitives
{
    public class PrimitiveCollections
    {
        public List<Point> Points { get; set; } = new List<Point>();

        public List<Line> Lines { get; set; } = new List<Line>();
    }
}
