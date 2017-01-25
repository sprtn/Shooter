using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApplication3
{


    public partial class Form1 : Form
    {


        public Form1()
        {
            InitializeComponent();

            //FormBorderStyle = FormBorderStyle.None;             // Revomes frames
            //this.TopMost = true;                                // Game is highest on the Z axis
            //this.Bounds = Screen.PrimaryScreen.Bounds;          // FullScreen Application
            //pictureBox2.Bounds = Screen.PrimaryScreen.Bounds;   // Front layer also full-screen
            timer1.Enabled = false;                             // Sets timer to false.
            //Refresh();                                          // Re-draws Form1 with all it's components.
        }

        int startPosX, startPosY;
        float curPosX, curPosY, torpSpeed;
        Point startPos, relativePoint;

        private void pictureBox2_Click(object sender, EventArgs e)
        {
            if (timer1.Enabled == false)                        // Seems like the click doesn't register until the previous click is done (i.e. it's queuing up torpedos)
            {
                setTorpedoVariables();

                timer1.Start();

                Console.WriteLine("Launching missile against " + relativePoint);
                MoveMissile();

                timer1.Stop();
            }
            else
            {
                Console.WriteLine("A torp is already flying."); // This never executes - "if" does not work as intended.
            }

            
        }


        private void Form1_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape) { this.Close(); }     // Use Esc key to close app
        }

        private void Form1_Click(object sender, EventArgs e)
        {
            // Console.WriteLine("Wrong click recorded");
        }

        private void setTorpedoVariables()
        {
            relativePoint = PointToClient(Cursor.Position);
            torpSpeed = 10;
            startPosX = ClientRectangle.Width / 2;
            startPosY = ClientRectangle.Height;
            curPosX = startPosX;
            curPosY = startPosY;
            startPos = new Point(startPosX, startPosY);

            pictureBox1.Location = startPos;
            Console.WriteLine(startPos);
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            
        }

        private void MoveMissile()
        {
            pictureBox1.Visible = true;
            

            Console.WriteLine("to " + relativePoint.Y + " from" + pictureBox1.Location.Y);

            

            //pictureBox1 <- Here I want to change the angle of the picturebox

            while (pictureBox1.Location.Y > relativePoint.Y)
            {
                float xDiff = relativePoint.X - startPos.X;
                float yDiff = relativePoint.Y - startPos.Y;

                double angle = Math.Atan2(yDiff, xDiff) * 180.0 / Math.PI;
                
                float newPosX = pictureBox1.Location.X;
                float newPosY = pictureBox1.Location.Y;

                newPosX += (float) Math.Cos(angle) * torpSpeed;
                newPosY += (float) Math.Sin(angle) * torpSpeed;

                curPosY -= 10;

                Console.WriteLine("X coords, cur and new:");
                Console.WriteLine((int)curPosX);
                Console.WriteLine((int)newPosX);

                Console.WriteLine("Y coords, cur and new:");
                Console.WriteLine((int)curPosY);
                Console.WriteLine((int)newPosY);
                
                pictureBox1.Location = new Point((int)curPosX, (int)curPosY);
                // pictureBox1.

                Refresh();

                Console.WriteLine("");
            }
            pictureBox1.Visible = false;
        }
    }
}
