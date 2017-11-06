using OsmExportBot.Primitives;
using SharpKml.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace OsmExportBot.DataSource
{
    class ParkingLanes
    {
        public static bool IsParkingLane(osmWay way)
        {
            return way.tag.Any(x => x.k.StartsWith("parking:lane:") || x.k.StartsWith("parking:condition:"));
        }

        public Line Right { get; set; }
        public Line Left { get; set; }

        public ParkingLanes(osmWay way, osmNode[] nodes)
        {
            var line = OverpassModelsConverter.ConvertOsmWayToPrimitiveLine(way, nodes);

            string color;
            if (GetLaneColor(way, "both", out color))
            {
                Right = line.Offset(Offset.Right, color);
                Left = line.Offset(Offset.Left, color);
            }
            if (GetLaneColor(way, "right", out color))
            {
                Right = line.Offset(Offset.Right, color);
            }
            if (GetLaneColor(way, "left", out color))
            {
                Left = line.Offset(Offset.Left, color);
            }
        }

        bool GetLaneColor(osmWay way, string side, out string color)
        {
            var lane = way.tag.FirstOrDefault(x => x.k == $"parking:lane:{side}")?.v ?? "";
            var condition = way.tag.FirstOrDefault(x => x.k == $"parking:condition:{side}")?.v ?? "";

            switch (lane)
            {
                case "no_parking": color = "orange"; return true;
                case "no_stopping": color = "red"; return true;
            }
            switch (condition)
            {
                case "free": color = "green"; return true;
                case "ticket": color = "blue"; return true;
                case "customers":
                case "residents": color = "purpure"; return true;
            }

            color = "gray";
            return false;
        }

        public List<Line> GetLines()
        {
            List<Line> lines = new List<Line>();
            if (Right.Points != null) lines.Add(Right);
            if (Left.Points != null) lines.Add(Left);
            return lines;
        }
    }
}
