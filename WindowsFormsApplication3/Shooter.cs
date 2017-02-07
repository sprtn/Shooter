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

    /// <summary>
    /// One of the major things in this project is, for me, to show my grasp of all the concepts required to make this game.
    /// As per now (01.02.17), I feel like this is not shown, as I do not implement different classes for the enemies, torpedo's
    /// and such.
    /// 
    /// Future implementation need to implement, before delivery:
    /// Enemy Class
    ///     Uboat class, which inherits values from Enemy class.
    ///     Boss class, which inherits values from Enemy Class.
    /// Torpedo / Missile / Projectile Class
    /// 
    /// The Form is resizable, and resizes when changes are made. All the elements should be resize-friendly, 
    /// except the Enemy placement on the Y axis. I think I can fix this quite easily with making the uPosY a range between
    /// object.height and ClientRectangle.Height - 25%(?) of ClientRectangle.Height. This means that the top 75% of the screen,
    /// -object height, can spawn enemies.
    /// 
    /// Also need a proper function for whether the elements spawn left or right, which has been started on this session,
    /// but not implemented properly, mainly because I have focused on other implementations, bugfixes and other stuff that
    /// has annoyed me for quite some time.
    /// 
    /// The entire project needs a full re-write from the most basic of functions (missile movement, enemy movement) to
    /// the more "advanced" (spawning enemies, ticks etc.)
    /// 
    /// I also need to optimize the program further, since I see that it takes a bit more RAM than I would expect 
    /// for such a small project.
    /// </summary>

    public partial class Form1 : Form
    {
        public Form1()
        {
            FormBorderStyle = FormBorderStyle.None;
            WindowState = FormWindowState.Maximized;
            InitializeComponent();
            timer1.Enabled = true;
            setMinMax();
        }

        int startPosX, startPosY, 
            curPosX, curPosY, 
            uPosX, uPosY, 
            uSpeed, UMinSpeed, UMaxSpeed,
            torpSpeed, ammunition = 10, 
            score, diffLevel;

        Point startPos, relativePoint;

        private bool clicked = false;
        private bool impact;

        private void pictureBox2_Click(object sender, EventArgs e)
        {
            if (clicked == false && ammunition > 0)
            {
                setTorpedoVariables();
                ammunition--;
                clicked = true;
            }
        }


        private void Form1_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape) { this.Close(); }     // Use Esc key to close app
        }

        private void Form1_Resize(object sender, EventArgs e)
        {
            SetGUI();
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
                timer1.Stop();
                Console.WriteLine("You lost! " + ammunition + " ammo, " + score + " score");
            }
        }

        private bool lossCondition()
        {
            if (ammunition == 0 && clicked == false)
                return true;
            else
                return false;
        }

        private void LabelUpdates()
        {
            IsClicked.Text = clicked.ToString();
            TorpXY.Text = curPosX + ", " + curPosY;
            UboatXY.Text = Uboat.Location.X + ", " + Uboat.Location.Y;
            ScoreLabel.Text = "Score:" + score;
            AmmoLabel.Text = "Ammo: " + ammunition;
            Difficulty.Text = "Level " + diffLevel / 4;
        }

        private void SetGUI()
        {
            int inset = 50;
            if (pictureBox2.Bounds != ClientRectangle)
                pictureBox2.Bounds = ClientRectangle;
            if (pictureBox2.Visible == false)
                pictureBox2.Visible = true;

            UboatXY.Location = new Point(ClientRectangle.Width - inset, ClientRectangle.Height - inset);
            TorpXY.Location = new Point(ClientRectangle.Width - inset, ClientRectangle.Height - inset / 2);
            ScoreLabel.Location = new Point(inset - ScoreLabel.Width, inset / 2);
            AmmoLabel.Location = new Point(inset - AmmoLabel.Width, inset);
            CurrentSpeed.Location = new Point(inset - CurrentSpeed.Width, ClientRectangle.Height - inset);
            IsClicked.Location = new Point(inset - IsClicked.Width, ClientRectangle.Height - inset / 2);
            Difficulty.Location = new Point(ClientRectangle.Width - inset, inset / 2);
        }

        private void CollisionControl()
        {
            if (Torpedo.Bounds.IntersectsWith(Uboat.Bounds))
            {
                impact = true;
                diffLevel++;
                score += (10 + uSpeed);
                ammunition += 1;
                Uboat.Visible = false;
                MissileReset();

                if (diffLevel % 4 == 0 && diffLevel != 0)
                {
                    setMinMax(UMinSpeed, UMaxSpeed);
                }
                else if (UMinSpeed == 0 || UMaxSpeed == 0)
                {
                    setMinMax();
                }
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
                int LeftRight = r.Next(100);

                uPosY = 10 + r.Next(10, 150); // Must be a rand
                uSpeed = r.Next(UMinSpeed, UMaxSpeed); // Must be a rand
                Console.WriteLine(LeftRight);

                if (LeftRight >= 50)
                {
                    Console.WriteLine("Left Spawn");
                    uPosX = -Uboat.Width;
                    uSpeed = r.Next(UMinSpeed, UMaxSpeed); // Must be a rand
                }
                else if (LeftRight < 50)
                {
                    Console.WriteLine("Right Spawn");
                    uPosX = pictureBox2.Width + Uboat.Width;
                    uSpeed = -r.Next(UMinSpeed, UMaxSpeed); // Must be a rand
                }

                CurrentSpeed.Text = uSpeed.ToString();
                
                Uboat.Visible = true;
                Uboat.Location = new Point(uPosX, uPosY);
            }
        }

        private void setMinMax(int min, int max)
        { 
            if (max > 10)
                UMaxSpeed++;
            if (min > 0)

                UMinSpeed++;
            // eller 'UMinSpeed += 1;' eller 'UMinSpeed = UMinSpeed + 1;'
        }

        private void setMinMax()
        {
            UMinSpeed = 1;
            UMaxSpeed = 11;
        }

        class Enemy
        {
            Enemy(int _MinSpeed, int _MaxSpeed, String _Type)
            {
                Random r = new Random();
                int _RightLeft = r.Next(0,2);

                if (_RightLeft == 1)
                {
                    Console.WriteLine("Left side spawn.");
                    PictureBox enemyPicture = new PictureBox();
                    
                }
                else
                {
                    Console.WriteLine("Right side spawn.");
                    PictureBox enemyPicture = new PictureBox();
                }
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
            F.Change the speed increase

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
