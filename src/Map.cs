using System;
using System.Drawing;

namespace PathfindingExample
{
    public class Map
    {
        //===================================================================== CONSTANTS
        public const int GRID_COUNT = 20;

        //===================================================================== VARIABLES
        private bool[,] _map = new bool[GRID_COUNT, GRID_COUNT];

        //===================================================================== INITIALIZE
        public Map()
        {
            for (int y = 0; y < GRID_COUNT; y++)
            {
                for (int x = 0; x < GRID_COUNT; x++) SetPassable(x, y, true);
            }
            for (int y = 8; y <= 11; y++)
            {
                for (int x = 9; x <= 10; x++) SetPassable(x, y, false);
            }
        }

        //===================================================================== FUNCTIONS
        public void TogglePassable(int x, int y)
        {
            _map[x, y] = !_map[x, y];
        }
        public void SetPassable(int x, int y, bool isPassable)
        {
            _map[x, y] = isPassable;
        }
        public bool IsPassable(Point pt)
        {
            return IsPassable(pt.X, pt.Y);
        }
        public bool IsPassable(int x, int y)
        {
            if (x < 0 || x >= GRID_COUNT || y < 0 || y >= GRID_COUNT) return false;
            else return _map[x, y];
        }
    }
}
