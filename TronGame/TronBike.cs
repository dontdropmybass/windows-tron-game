using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace TronGame
{
    public class TronBike
    {
        public List<Color> colors = new List<Color> { Color.Red, Color.Orange, Color.Yellow, Color.Green, Color.Blue, Color.Violet };
        public List<Color> darkColors = new List<Color> { Color.DarkRed, Color.DarkOrange, Color.DarkGoldenrod, Color.DarkGreen, Color.DarkBlue, Color.DarkViolet };
        public bool seizureMode { get; set; }
        public List<Position> path { get; }
        public Position position { get; }
        private DIRECTION _direction;
        public DIRECTION direction
        {
            get
            {
                return _direction;
            }
            set
            {
                switch (_direction)
                {
                    case DIRECTION.UP:
                        if (value == DIRECTION.LEFT || value == DIRECTION.RIGHT)
                        {
                            _direction = value;
                        }
                        break;
                    case DIRECTION.DOWN:
                        if (value == DIRECTION.LEFT || value == DIRECTION.RIGHT)
                        {
                            _direction = value;
                        }
                        break;
                    case DIRECTION.LEFT:
                        if (value == DIRECTION.UP || value == DIRECTION.DOWN)
                        {
                            _direction = value;
                        }
                        break;
                    case DIRECTION.RIGHT:
                        if (value == DIRECTION.UP || value == DIRECTION.DOWN)
                        {
                            _direction = value;
                        }
                        break;
                }
            }
        }
        public Color color { get; }
        public Color trailColor { get; }

        public TronBike(int x, int y, DIRECTION direction, Color color, Color trailColor)
        {
            position = new Position(x, y);
            this.direction = direction;
            this.color = color;
            this.trailColor = trailColor;
            path = new List<Position>();
            seizureMode = false;
        }

        public TronBike(int x, int y, DIRECTION direction, Color color, Color trailColor, bool seizureMode)
        {
            position = new Position(x, y);
            this.direction = direction;
            this.color = color;
            this.trailColor = trailColor;
            path = new List<Position>();
            this.seizureMode = seizureMode;
        }

        public void Paint(Graphics graphics)
        {
            if (seizureMode)
            {
                int c = new Random().Next(colors.Count);
                path.Add(new Position(position));
                SolidBrush brush = new SolidBrush(colors[c]);
                switch (direction)
                {
                    case DIRECTION.UP:
                        position.Y--;
                        break;
                    case DIRECTION.DOWN:
                        position.Y++;
                        break;
                    case DIRECTION.LEFT:
                        position.X--;
                        break;
                    case DIRECTION.RIGHT:
                        position.X++;
                        break;
                }
                graphics.FillRectangle(brush, position.X, position.Y, 1, 1);
                brush.Color = darkColors[c];
                foreach (Position pos in path)
                {
                    graphics.FillRectangle(brush, pos.X, pos.Y, 1, 1);
                }
            }
            else
            {
                path.Add(new Position(position));
                SolidBrush brush = new SolidBrush(color);
                switch (direction)
                {
                    case DIRECTION.UP:
                        position.Y--;
                        break;
                    case DIRECTION.DOWN:
                        position.Y++;
                        break;
                    case DIRECTION.LEFT:
                        position.X--;
                        break;
                    case DIRECTION.RIGHT:
                        position.X++;
                        break;
                }
                graphics.FillRectangle(brush, position.X, position.Y, 1, 1);
                brush.Color = trailColor;
                foreach (Position pos in path)
                {
                    graphics.FillRectangle(brush, pos.X, pos.Y, 1, 1);
                }
            }
        }
    }

    public class Position
    {
        public int X { get; set; }
        public int Y { get; set; }

        public Position(int x, int y)
        {
            X = x;
            Y = y;
        }

        public Position(Position position)
        {
            X = position.X;
            Y = position.Y;
        }
    }

    public enum DIRECTION { UP, LEFT, DOWN, RIGHT }
}
