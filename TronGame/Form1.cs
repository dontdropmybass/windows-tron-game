using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Media;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TronGame
{
    public partial class Form1 : Form
    {
        bool playing = false;

        bool dank = false;
        
        TronBike redBike;
        TronBike blueBike;

        string winner = "";

        //border stuff
        List<Pen> pens = new List<Pen>
        {
            new Pen(Color.Cyan, 10),
            new Pen(Color.Magenta, 10),
            new Pen(Color.Yellow, 10)
        };
        Pen borderPen = new Pen(Color.Green, 10);
        int penIndex = 0;
        Rectangle r;

        //text font
        Font font;
        SolidBrush textBrush;

        //music player
        private SoundPlayer Player = new SoundPlayer();

        public Form1()
        {
            InitializeComponent();
            DoubleBuffered = true;
        }

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            if (playing)
            {
                switch (e.KeyCode)
                {
                    // Red Bike Controls
                    case Keys.W:
                        redBike.direction = DIRECTION.UP;
                        break;
                    case Keys.A:
                        redBike.direction = DIRECTION.LEFT;
                        break;
                    case Keys.S:
                        redBike.direction = DIRECTION.DOWN;
                        break;
                    case Keys.D:
                        redBike.direction = DIRECTION.RIGHT;
                        break;

                    // Blue Bike Controls
                    case Keys.Up:
                        blueBike.direction = DIRECTION.UP;
                        break;
                    case Keys.Left:
                        blueBike.direction = DIRECTION.LEFT;
                        break;
                    case Keys.Down:
                        blueBike.direction = DIRECTION.DOWN;
                        break;
                    case Keys.Right:
                        blueBike.direction = DIRECTION.RIGHT;
                        break;
                }
            }
            else
            {
                if (e.KeyCode == Keys.Return)
                {
                    redBike = new TronBike(Width / 10, Height / 2, DIRECTION.RIGHT, Color.Red, Color.DarkRed);
                    blueBike = new TronBike((Width / 10) * 9, Height / 2, DIRECTION.LEFT, Color.Blue, Color.DarkBlue);
                    playing = true;
                    dank = false;
                    timer1.Start();
                    timer1.Interval = 1; // as fast as it can possibly go
                }
                else if (e.KeyCode == Keys.Space)
                {
                    redBike = new TronBike(Width / 10, Height / 2, DIRECTION.RIGHT, Color.Red, Color.DarkRed, true);
                    blueBike = new TronBike((Width / 10) * 9, Height / 2, DIRECTION.LEFT, Color.Blue, Color.DarkBlue, true);
                    dank = true;
                    playing = true;
                    Player.SoundLocation = @"all-star.wav";
                    Player.PlayLooping();
                    timer1.Start();
                    timer1.Interval = 1; // as fast as it can possibly go
                }
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            WindowState = FormWindowState.Maximized;
            r = new Rectangle(0, 0, ActiveForm.Width-20, ActiveForm.Height-80);
            font = new Font("Comic Sans MS", 18);
            textBrush = new SolidBrush(Color.Magenta);
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            Invalidate();
        }

        private Collision CheckCollision()
        {
            if (blueBike.position.X < 10 || blueBike.position.X >= Width-30 || blueBike.position.Y < 10 || blueBike.position.Y >= Height-90)
            {
                return Collision.Blue;
            }
            if (redBike.position.X < 10 || redBike.position.X >= Width-30 || redBike.position.Y < 10 || redBike.position.Y >= Height-90)
            {
                return Collision.Red;
            }
            foreach (Position position in redBike.path)
            {
                if (position.X == blueBike.position.X && position.Y == blueBike.position.Y)
                {
                    return Collision.Blue;
                }
                else if (position.X == redBike.position.X && position.Y == redBike.position.Y)
                {
                    return Collision.Red;
                }
            }
            foreach (Position position in blueBike.path)
            {
                if (position.X == blueBike.position.X && position.Y == blueBike.position.Y)
                {
                    return Collision.Blue;
                }
                else if (position.X == redBike.position.X && position.Y == redBike.position.Y)
                {
                    return Collision.Red;
                }
            }
            return Collision.None;
        }

        private void Form1_Paint(object sender, PaintEventArgs e)
        {
            if (playing)
            {
                if (dank)
                {
                    // draw the border then change the colour
                    e.Graphics.DrawRectangle(pens[penIndex], r);
                    penIndex++;
                    if (penIndex >= pens.Count) penIndex = 0;
                }
                else
                {
                    e.Graphics.DrawRectangle(borderPen, r);
                }

                redBike.Paint(e.Graphics);
                blueBike.Paint(e.Graphics);
                Collision collision = CheckCollision();
                switch (collision)
                {
                    case Collision.Red:
                        winner = "Blue Bike";
                        playing = false;
                        break;
                    case Collision.Blue:
                        winner = "Red Bike";
                        playing = false;
                        break;
                    default:
                        break;
                }
            }
            else
            {
                Player.Stop();
                if (winner=="")
                {
                    e.Graphics.DrawString
                    (
                        "Red bike uses WASD, blue bike uses arrow keys. Press enter to start.",
                        font,
                        textBrush,
                        new Point(0, 0)
                    );
                }
                else
                {
                    e.Graphics.DrawString
                    (
                        winner + " wins! Press enter to play again...",
                        font,
                        textBrush,
                        new Point(0, 0)
                    );
                }
                timer1.Stop();
            }
        }
    }

    enum Collision { None, Red, Blue }
}
