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

            FormBorderStyle = FormBorderStyle.None;             // Fjerner rammer.
            this.TopMost = true;                                // Legger spillet øverst på Z-aksen.
            this.Bounds = Screen.PrimaryScreen.Bounds;          // Fullskjerm-applikasjon.
            pictureBox2.Bounds = Screen.PrimaryScreen.Bounds;
            timer1.Enabled = false;
            Refresh();
        }

        int startPosX, startPosY, curPosX, curPosY, torpSpeed;
        Point startPos, relativePoint;

        private void pictureBox2_Click(object sender, EventArgs e)
        {
            if (timer1.Enabled == false)
            {
                setTorpedoVariables();

                timer1.Start();

                MoveMissile();
                Console.WriteLine("Launching missile against " + relativePoint);

                timer1.Stop();
            }
            else
            {
                Console.WriteLine("A torp is already flying.");
            }

            
        }
                

        private void Form1_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape) { this.Close(); }     // Trykk Escape for å avslutte.
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

            float xDiff = relativePoint.X - startPos.X;
            float yDiff = relativePoint.Y - startPos.Y;
            double angle = Math.Atan2(yDiff, xDiff) * 180.0 / Math.PI;

            //pictureBox1 <- Here I want to change the angle of the picturebox

            while (pictureBox1.Location.Y > relativePoint.Y)
            {
                curPosY -= torpSpeed;
                pictureBox1.Location = new Point(curPosX, curPosY);
                //pictureBox1.

                Refresh();

                Console.WriteLine(angle);
                Console.WriteLine("Missile's Y coord is: " + curPosY);
            }
            pictureBox1.Visible = false;
        }
    }
}
