using System;
using System.Drawing;

namespace PathfindingExample
{
    public class PointData
    {
        //===================================================================== CONSTANTS
        private const int BASE_MOVEMENT_COST = 10;

        //===================================================================== VARIABLES
        public PointData Parent { get; set; }
        public readonly Point Point;
        public readonly int EstimatedTotalCost;
        public bool IsClosed { get; set; }

        //===================================================================== INITIALIZE
        public PointData(PointData parent, Point point, Point end)
        {
            Parent = parent;
            Point = point;
            EstimatedTotalCost = Math.Abs(end.X - point.X) + Math.Abs(end.Y - point.Y) * BASE_MOVEMENT_COST;
        }

        //===================================================================== PROPERTIES
        public int X
        {
            get { return Point.X; }
        }
        public int Y
        {
            get { return Point.Y; }
        }
        public int F
        {
            get { return MovementCost + EstimatedTotalCost; }
        }
        public int MovementCost
        {
            get
            {
                if (Parent == null)
                    return 0;

                return Parent.MovementCost + BASE_MOVEMENT_COST + (IsDiagonal ? 4 : 0);
            }
        }
        private bool IsDiagonal
        {
            get
            {
                if (Parent == null)
                    return false;

                return ((int)(Math.Abs(Point.X - Parent.X) + Math.Abs(Point.Y - Parent.Y)) == 2);
            }
        }
    }
}
