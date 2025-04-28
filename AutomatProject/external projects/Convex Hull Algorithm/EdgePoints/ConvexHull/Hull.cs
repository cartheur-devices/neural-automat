using System;
using System.Drawing;

namespace ConvexHull
{
    public struct Segment
    {
        public PointF p;
        public PointF q;

        public bool contains(SuperPoint point)
        {
            if (p.Equals(point.P) || q.Equals(point.P))
                return true;
            return false;
        }
    }

    public struct SuperPoint
    {
        public PointF P;
        public int ID;

        public SuperPoint(PointF p, int id)
        {
            P = p;
            ID = id;
        }
    }
}
