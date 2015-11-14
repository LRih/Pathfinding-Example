using System;
using System.Collections.Generic;
using System.Drawing;

namespace PathfindingExample
{
    public class Pathfinder
    {
        //===================================================================== VARIABLES
        private Map _map;
        private Point _start;
        private Point _end;
        private List<PointData> _points;

        //===================================================================== INITIALIZE
        public Pathfinder(Map map, Point start, Point end)
        {
            _map = map;
            _start = start;
            _end = end;
        }

        //===================================================================== FUNCTIONS
        public PointData FindPath()
        {
            _points = new List<PointData>();
            _points.Add(new PointData(null, _start, _end));
            for (PointData lowestFOpenPt = GetLowestFOpenPoint(); lowestFOpenPt != null; lowestFOpenPt = GetLowestFOpenPoint())
            {
                List<PointData> surroundingPassablePts = GetSurroundingPassablePoints(lowestFOpenPt);
                // check any already added point and replace parent if better path
                foreach (PointData surroundingPt in surroundingPassablePts)
                {
                    if (surroundingPt.Point == _end) return surroundingPt;
                    PointData overlapPt = GetPointDataAt(surroundingPt.Point);
                    if (overlapPt == null) _points.Add(surroundingPt);
                    else if (surroundingPt.MovementCost < overlapPt.MovementCost) overlapPt.Parent = lowestFOpenPt;
                }
                lowestFOpenPt.SetClosed();
            }
            return null;
        }
        private PointData GetLowestFOpenPoint()
        {
            PointData currentLowest = null;
            for (int i = 0; i < _points.Count; i++)
            {
                PointData pt = _points[i];
                if (pt.IsClosed) continue;
                if (currentLowest == null || pt.F < currentLowest.F) currentLowest = pt;
            }
            return currentLowest;
        }
        private List<PointData> GetSurroundingPassablePoints(PointData currentPt)
        {
            List<PointData> points = new List<PointData>();
            foreach (Point direction in new Point[] { new Point(-1, -1), new Point(0, -1), new Point(1, -1), new Point(-1, 0), new Point(1, 0), new Point(-1, 1), new Point(0, 1), new Point(1, 1) })
            {
                Point surroundingPt = new Point(currentPt.X + direction.X, currentPt.Y + direction.Y);
                Point dirX = new Point(currentPt.X + direction.X, currentPt.Y);
                Point dirY = new Point(currentPt.X, currentPt.Y + direction.Y);
                if (_map.IsPassable(surroundingPt) && _map.IsPassable(dirX) && _map.IsPassable(dirY)) points.Add(new PointData(currentPt, surroundingPt, _end));
            }
            return points;
        }
        private PointData GetPointDataAt(Point pt)
        {
            foreach (PointData ptData in _points) if (pt == ptData.Point) return ptData;
            return null;
        }
    }


    public class PointData
    {
        //===================================================================== CONSTANTS
        private const int BASE_MOVEMENT_COST = 10;

        //===================================================================== VARIABLES
        private PointData _parent;
        public readonly Point Point;
        public readonly int EstimatedTotalCost;
        private bool _isClosed;

        //===================================================================== INITIALIZE
        public PointData(PointData parent, Point point, Point end)
        {
            _parent = parent;
            Point = point;
            EstimatedTotalCost = (int)(Math.Abs(end.X - point.X) + Math.Abs(end.Y - point.Y)) * BASE_MOVEMENT_COST;
        }

        //===================================================================== FUNCTIONS
        public void SetClosed()
        {
            _isClosed = true;
        }

        //===================================================================== PROPERTIES
        public PointData Parent
        {
            get { return _parent; }
            set { _parent = value; }
        }
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
                if (Parent == null) return 0;
                else return Parent.MovementCost + BASE_MOVEMENT_COST + (IsDiagonal ? 4 : 0);
            }
        }
        private bool IsDiagonal
        {
            get
            {
                if (_parent == null) return false;
                else return ((int)(Math.Abs(Point.X - _parent.X) + Math.Abs(Point.Y - _parent.Y)) == 2);
            }
        }
        public bool IsClosed
        {
            get { return _isClosed; }
        }
    }
}
