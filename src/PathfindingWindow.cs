using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace PathfindingExample
{
    public class PathfindingWindow : Form
    {
        //===================================================================== CONSTANTS
        private const int GRID_SIZE = 40;

        //===================================================================== VARIABLES
        private Map _map = new Map();
        private Point _start = new Point(2, 9);
        private Point _end = new Point(17, 10);

        private bool _currentDraw;

        //===================================================================== INTIALIZE
        public PathfindingWindow()
        {
            this.ClientSize = new Size(Map.GRID_COUNT * GRID_SIZE, Map.GRID_COUNT * GRID_SIZE);
            this.DoubleBuffered = true;
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.StartPosition = FormStartPosition.CenterScreen;
            this.Text = "Pathfinding Example";
        }

        //===================================================================== TERMINATE
        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
        }

        //===================================================================== FUNCTIONS
        private void SetPassable(Point coord, bool isPassable)
        {
            if (coord.X >= 0 && coord.X < Map.GRID_COUNT && coord.Y >= 0 && coord.Y < Map.GRID_COUNT && coord != _start && coord != _end)
            {
                _map.SetPassable(coord.X, coord.Y, isPassable);
                this.Invalidate();
            }
        }
        private Point GetCoord(int mouseX, int mouseY)
        {
            return new Point(mouseX / GRID_SIZE, mouseY / GRID_SIZE);
        }

        //===================================================================== EVENTS
        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            DrawMap(e.Graphics);
            DrawPath(e.Graphics);
            DrawEndPoints(e.Graphics);
        }
        private void DrawMap(Graphics g)
        {
            for (int y = 0; y < Map.GRID_COUNT; y++)
            {
                for (int x = 0; x < Map.GRID_COUNT; x++)
                {
                    Brush brush = (_map.IsPassable(x, y) ? Brushes.White : Brushes.DarkGray);
                    g.FillRectangle(brush, x * GRID_SIZE, y * GRID_SIZE, GRID_SIZE, GRID_SIZE);
                }
            }
        }
        private void DrawPath(Graphics g)
        {
            PointData currentPt = new Pathfinder(_map, _start, _end).FindPath();
            if (currentPt == null) return;
            using (GraphicsPath path = new GraphicsPath())
            using (Pen pen = new Pen(Color.SteelBlue, (10 * GRID_SIZE) / 40))
            {
                do
                {
                    path.AddLine(currentPt.X * GRID_SIZE + GRID_SIZE / 2, currentPt.Y * GRID_SIZE + GRID_SIZE / 2, currentPt.Parent.X * GRID_SIZE + GRID_SIZE / 2, currentPt.Parent.Y * GRID_SIZE + GRID_SIZE / 2);
                    currentPt = currentPt.Parent;
                    g.DrawString(currentPt.MovementCost.ToString(), this.Font, Brushes.Black, currentPt.X * GRID_SIZE + GRID_SIZE / 2, currentPt.Y * GRID_SIZE - GRID_SIZE / 2);
                } while (currentPt.Parent != null);
                g.DrawPath(pen, path);
            }
        }
        private void DrawEndPoints(Graphics g)
        {
            g.FillRectangle(Brushes.IndianRed, _start.X * GRID_SIZE, _start.Y * GRID_SIZE, GRID_SIZE, GRID_SIZE);
            g.FillRectangle(Brushes.YellowGreen, _end.X * GRID_SIZE, _end.Y * GRID_SIZE, GRID_SIZE, GRID_SIZE);
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            base.OnMouseClick(e);
            if (e.Button == MouseButtons.Left) SetPassable(GetCoord(e.X, e.Y), _currentDraw);
        }
        protected override void OnMouseDown(MouseEventArgs e)
        {
            base.OnMouseClick(e);
            if (e.Button == MouseButtons.Left)
            {
                Point coord = GetCoord(e.X, e.Y);
                _currentDraw = !_map.IsPassable(coord);
                SetPassable(coord, _currentDraw);
            }
        }
    }
}
