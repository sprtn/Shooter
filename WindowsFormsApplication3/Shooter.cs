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
            FormBorderStyle = FormBorderStyle.None;
            WindowState = FormWindowState.Maximized;
            InitializeComponent();
            pictureBox2.Bounds = ClientRectangle;
            timer1.Enabled = true;
        }

        int startPosX, startPosY, curPosX, curPosY, torpSpeed, uPosX, uPosY, uSpeed, ammunition = 2, score, UMinSpeed = 1, UMaxSpeed = 11, diffLevel, lossCount;
        Point startPos, relativePoint;
        private int timerCounter;

        private bool clicked = false;
        private bool impact;

        private void pictureBox2_Click(object sender, EventArgs e)
        {
            if ((impact == true || clicked == false) && ammunition > 0)
            {
                setTorpedoVariables();
                ammunition--;
                clicked = true;
                diffLevel++;
                if (diffLevel % 4 == 0)
                {
                    UMinSpeed++;
                    UMaxSpeed++;
                }
            }
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
            torpSpeed = 30;

            startPosX = ClientRectangle.Width / 2;
            startPosY = ClientRectangle.Height;

            curPosX = startPosX;
            curPosY = startPosY;
            startPos = new Point(startPosX, startPosY);
            clicked = true;

            Torpedo.Location = startPos;
            Console.WriteLine(startPos);
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            EnemyControl();
            MissileControl();
            CollisionControl();
            LabelUpdates();
            if (lossCondition())
            {
                // Add loss event
                Console.WriteLine("You lost! " + ammunition + " ammo, " + score + " score");
            }
        }

        private bool lossCondition()
        {
            while (ammunition == 0)
            {
                lossCount++;
                if (lossCount == 50)
                    return true;
            }
            if (ammunition > 0)
            {
                lossCount = 0;
                return false;
            }
            else
                return false;
        }

        private void LabelUpdates()
        {
            if (pictureBox2.Bounds != ClientRectangle)
                pictureBox2.Bounds = ClientRectangle;
            IsClicked.Text = clicked.ToString();
            TorpXY.Text = curPosX + ", " + curPosY;
            UboatXY.Text = Uboat.Location.X + ", " + Uboat.Location.Y;
            ScoreLabel.Text = "Score:" + score;
            AmmoLabel.Text = "Ammo: " + ammunition;
            timerCounter++;
        }

        private void CollisionControl()
        {
            if (Torpedo.Bounds.IntersectsWith(Uboat.Bounds))
            {
                score += uSpeed;
                ammunition += 1;
                Uboat.Visible = false;
                MissileReset();
            }
        }

        private void ResetAllComp()
        {
            if (impact)
                Uboat.Visible = false;
            MissileReset();
        }

        private void MissileControl()
        {
            if (clicked == true)
                MoveMissile();
            else
                MissileReset();
        }

        private void EnemyControl()
        {
            if (Uboat.Visible == false)
                CreateEnemy();
            if (Uboat.Visible == true)
                MoveEnemy();
        }

        private void MoveEnemy()
        {
            uPosX += uSpeed;
            Uboat.Location = new Point(uPosX, uPosY);
            if (uPosX >= ClientRectangle.Width)
                Uboat.Visible = false;
        }

        private void CreateEnemy()
        {
            if (Uboat.Visible == false)
            {
                Random r = new Random();

                uPosX = -Uboat.Width;
                uSpeed = r.Next(UMinSpeed, UMaxSpeed); // Must be a rand
                uPosY = 10 + r.Next(10, 150); // Must be a rand

                CurrentSpeed.Text = uSpeed.ToString();
                
                Uboat.Visible = true;
                Uboat.Location = new Point(uPosX, uPosY);
            }
        }


        private void MoveMissile()
        {
            if (clicked == true)
            {
                Torpedo.Visible = true;
                if (Torpedo.Location.Y >= 10)
                    Torpedo.Location = new Point(relativePoint.X, curPosY -= torpSpeed);
                else
                    MissileReset();
            }
        }

        private void MissileReset()
        {
            Torpedo.Visible = false;
            clicked = false;
            Torpedo.Location = startPos;
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
            Change the speed increase

            F.Random heights
            Random quantities
        Boss invader
            Invader HP + regular's hp
    
    Highscores
        Save to .txt
        Show highscores

    Add loss/game over condition
        F.Add condition
        Add effect
        Add highscore saving 
            
    Missiles
        F.Missileno longer work as they should
            
    F.Multiple projectiles:
        F.Can launch several missiles by clicking several times.
        F.The objectives dont all dissappear until the last click goes through.

    Might find some useful info here:
    http://www.c-sharpcorner.com/article/space-invaders-for-C-Sharp-and-net/
 */
