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
                    if (surroundingPt.Point == _end)
                        return surroundingPt;

                    PointData overlapPt = GetPointDataAt(surroundingPt.Point);

                    if (overlapPt == null)
                        _points.Add(surroundingPt);
                    else if (surroundingPt.MovementCost < overlapPt.MovementCost)
                        overlapPt.Parent = lowestFOpenPt;
                }

                lowestFOpenPt.IsClosed = true;
            }

            return null;
        }

        private PointData GetLowestFOpenPoint()
        {
            PointData currentLowest = null;

            for (int i = 0; i < _points.Count; i++)
            {
                PointData pt = _points[i];

                if (pt.IsClosed)
                    continue;

                if (currentLowest == null || pt.F < currentLowest.F)
                    currentLowest = pt;
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

                if (_map.IsPassable(surroundingPt) && _map.IsPassable(dirX) && _map.IsPassable(dirY))
                    points.Add(new PointData(currentPt, surroundingPt, _end));
            }

            return points;
        }

        private PointData GetPointDataAt(Point pt)
        {
            foreach (PointData ptData in _points)
                if (pt == ptData.Point)
                    return ptData;

            return null;
        }
    }
}
