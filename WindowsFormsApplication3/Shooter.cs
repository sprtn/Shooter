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
            //Enemy enemyClass = new Enemy();
            pictureBox2.Bounds = ClientRectangle;
            timer1.Enabled = true;
        }

        int startPosX, startPosY, curPosX, curPosY, torpSpeed, uPosX, uPosY, uSpeed;
        Point startPos, relativePoint;
        private int timerCounter;

        public bool clicked = false;
        private bool impact;

        private void pictureBox2_Click(object sender, EventArgs e)
        {
            if (pictureBox1.Location.Y <= ClientRectangle.Height - 1 && clicked == false)
            {
                setTorpedoVariables();
                clicked = true;
            }
            Console.WriteLine("Not a valid click");
        }


        private void Form1_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape) { this.Close(); }     // Use Esc key to close app
        }

        private void Form1_Click(object sender, EventArgs e)
        {
            Console.WriteLine("Wrong click recorded");
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
            clicked = true;

            pictureBox1.Location = startPos;
            Console.WriteLine(startPos);
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            EnemyControl();
            MissileControl();
            CollisionControl();
            timerCounter++;
        }

        private void CollisionControl()
        {
            impact = pictureBox1.Bounds.IntersectsWith(pictureBox3.Bounds);
            if (impact)
            {
                ResetAllComp();
            }
        }

        private void ResetAllComp()
        {
            Console.WriteLine("Explosion.");
            MissileReset();
            clicked = false;
        }

        private void MissileControl()
        {
            if (clicked == true)
            {
                Console.WriteLine("Clicked.");
                MoveMissile();
            }
        }

        private void EnemyControl()
        {
            if (pictureBox3.Visible == false)
            {
                CreateEnemy();
            }
            if (pictureBox3.Visible == true)
            {
                MoveEnemy();
            }
        }

        private void MoveEnemy()
        {
            uPosX += uSpeed;
            pictureBox3.Location = new Point(uPosX, uPosY);
            if (uPosX >= ClientRectangle.Width)
            {
                pictureBox3.Visible = false;
            }
        }

        private void CreateEnemy()
        {
            if (pictureBox3.Visible == false)
            {
                Random r = new Random();
                uPosX = -pictureBox3.Width;
                uSpeed = 1 + r.Next(10); // Must be a rand
                uPosY = 10 + r.Next(70); // Must be a rand
                pictureBox3.Visible = true;
                pictureBox3.Location = new Point(uPosX, uPosY);
            }
            
        }

        private void MoveMissile()
        {
            if (clicked == true)
            {
                pictureBox1.Visible = true;
                if (pictureBox1.Location.Y >= 10)
                {
                    pictureBox1.Location = new Point(relativePoint.X, curPosY -= torpSpeed);
                }
                else
                {
                    MissileReset();
                }
            }
        }

        private void MissileReset()
        {
            pictureBox1.Visible = false;
            pictureBox1.Location = new Point(startPosX, startPosY);
        }
    }
}
/*
 Buglist:
 
    Enemies/Invaders
        Need to add invaders
            F.Pictures
            F.Moving enemies
            F.Random speeds
            F.Random heights
            Random quantities
            
    Missiles
        Missiles no longer work as they should
            
    F.Multiple projectiles:
        F.Can launch several missiles by clicking several times.
        F.The objectives dont all dissappear until the last click goes through.

    Might find some useful info here:
    http://www.c-sharpcorner.com/article/space-invaders-for-C-Sharp-and-net/
 */
