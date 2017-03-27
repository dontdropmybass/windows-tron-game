using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TronGame
{
    public partial class Form1 : Form
    {
        bool playing = false;
        
        TronBike redBike;
        TronBike blueBike;

        string winner = "";

        //text font
        Font font;
        SolidBrush textBrush;

        public Form1()
        {
            InitializeComponent();
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
                    redBike = new TronBike(Width / 10, Height / 2, DIRECTION.RIGHT, Color.Red, Color.OrangeRed);
                    blueBike = new TronBike((Width / 10) * 9, Height / 2, DIRECTION.LEFT, Color.Blue, Color.LightBlue);
                    playing = true;
                    timer1.Start();
                }
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            WindowState = FormWindowState.Maximized;

            font = new Font("Comic Sans MS", 18);
            textBrush = new SolidBrush(Color.White);
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            Invalidate();
        }

        private Collision CheckCollision()
        {
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
                redBike.Paint(e.Graphics);
                blueBike.Paint(e.Graphics);
                Collision collision = CheckCollision();
                switch (collision)
                {
                    case Collision.Red:
                        winner = "Red Bike";
                        playing = false;
                        break;
                    case Collision.Blue:
                        winner = "Blue Bike";
                        playing = false;
                        break;
                    default:
                        break;
                }
            }
            else
            {
                e.Graphics.DrawString
                (
                    "Red bike uses WASD, blue bike uses arrow keys. Press enter to start.",
                    font,
                    textBrush,
                    new Point(0, 0)
                );
                timer1.Stop();
            }
        }
    }

    enum Collision { None, Red, Blue }
}
