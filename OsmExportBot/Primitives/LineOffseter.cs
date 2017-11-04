using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OsmExportBot.Primitives
{
    enum Offset { Right, Left }

    static class LineOffseter
    {
        public static Line Offset(this Line line, Offset offset)
        {
            return line.Offset(offset, "");
        }

        public static Line Offset(this Line line, Offset offset, string color)
        {
            return new Line {
                Points = line.Points.Offset(offset),
                Color = color
            };
        }

        public static Point[] Offset(this Point[] pts, Offset offset)
        {
            Point[] normals = new Point[pts.Length - 1];
            double d = 0.0001;

            if (offset == Primitives.Offset.Left)
                d = d * -1;


            for (int i = 0; i < pts.Length - 1; i++)
            {
                double dx = pts[i + 1].Lat - pts[i].Lat;
                double dy = pts[i + 1].Lon - pts[i].Lon;
                double len = Math.Sqrt(dx * dx + dy * dy);
                normals[i] = new Point(-dy / len, dx / len);
            }

            Point[] ppts = new Point[pts.Length];

            Point prevA = Add(pts[0], Scale(normals[0], d));
            Point prevB = Add(pts[1], Scale(normals[0], d));
            for (int i = 1; i < pts.Length - 1; i++)
            {
                Point A = Add(pts[i], Scale(normals[i], d));
                Point B = Add(pts[i + 1], Scale(normals[i], d));
                if (IsParallelSegments(A, B, prevA, prevB))
                    ppts[i] = A;
                else
                    ppts[i] = GetLineLineIntersection(A, B, prevA, prevB);
                prevA = A;
                prevB = B;
            }

            ppts[0] = Add(pts[0], Scale(normals[0], d));
            ppts[pts.Length - 1] = Add(pts[pts.Length - 1], Scale(normals[pts.Length - 2], d));

            return ppts;
        }

        private static bool IsParallelSegments(Point p1, Point p2, Point p3, Point p4)
        {
            // Convert line from (point, point) form to ax+by=c
            double a1 = p2.Lon - p1.Lon;
            double b1 = p1.Lat - p2.Lat;

            double a2 = p4.Lon - p3.Lon;
            double b2 = p3.Lat - p4.Lat;

            // Solve the equations
            double det = a1 * b2 - a2 * b1;
            // remove influence of of scaling factor
            det /= Math.Sqrt(a1 * a1 + b1 * b1) * Math.Sqrt(a2 * a2 + b2 * b2);
            return Math.Abs(det) < 1e-3;
        }

        private static Point GetLineLineIntersection(Point p1, Point p2, Point p3, Point p4)
        {
            // Convert line from (point, point) form to ax+by=c
            double a1 = p2.Lon - p1.Lon;
            double b1 = p1.Lat - p2.Lat;

            double a2 = p4.Lon - p3.Lon;
            double b2 = p3.Lat - p4.Lat;

            // Solve the equations
            double det = a1 * b2 - a2 * b1;
            //if (det == 0)
            //return null; // Lines are parallel

            double c2 = (p4.Lat - p1.Lat) * (p3.Lon - p1.Lon) - (p3.Lat - p1.Lat) * (p4.Lon - p1.Lon);

            return new Point(b1 * c2 / det + p1.Lat, -a1 * c2 / det + p1.Lon);
        }

        private static Point Scale(Point v, double s) =>
            new Point(s * v.Lat, s * v.Lon);

        private static Point Add(Point v, Point other)
        {
            return new Point(v.Lat + other.Lat, v.Lon + other.Lon);
        }
    }
}
