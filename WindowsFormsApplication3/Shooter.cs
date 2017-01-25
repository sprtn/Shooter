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
            /*
            FormBorderStyle = FormBorderStyle.None;             // Revomes frames
            this.TopMost = true;                                // Game is highest on the Z axis
            this.Bounds = Screen.PrimaryScreen.Bounds;          // FullScreen Application
            pictureBox2.Bounds = Screen.PrimaryScreen.Bounds; */  // Front layer also full-screen

            pictureBox2.Bounds = ClientRectangle;
            timer1.Enabled = true;                             // Sets timer to false.
            //Refresh();                                        // Re-draws Form1 with all it's components.
        }

        int startPosX, startPosY, curPosX, curPosY, torpSpeed;
        Point startPos, relativePoint;
        private int timerCounter;

        private void pictureBox2_Click(object sender, EventArgs e)
        {
            if (pictureBox1.Location.Y <= ClientRectangle.Height-1)
            {
                setTorpedoVariables();
                Console.WriteLine("Launching missile against " + relativePoint);
                MoveMissile();
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
            torpSpeed = 1;
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
            timerCounter++;
        }

        private Boolean EverySecondTick()
        {
            return timerCounter % 200 == 0;
        }

        private void MoveMissile()
        {
            pictureBox1.Visible = true;

            while (pictureBox1.Location.Y > relativePoint.Y)
            {                 
                pictureBox1.Location = new Point(relativePoint.X, curPosY -= torpSpeed);
                pictureBox1.Refresh();
            }

            pictureBox1.Visible = false;
        }
    }
}
/*
 Buglist:

    Multiple projectiles:
        Can launch several missiles by clicking several times.
        The objectives dont all dissappear until the last click goes through.

    Enemies/Invaders
        Need to add invaders

 */