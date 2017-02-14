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
    /// Disclaimer: I do not feel like this project shows my current potential. Half-way through the project I knew 
    /// I should have started over, but I didn't. Therefore there have been a whole lot of bad solutions and bad
    /// structure. I have, however, learned a lot during this project.
    /// 
    /// Wanted implementation / original idea:
    /// Enemy Class
    ///     Uboat class, which inherits values from Enemy class.
    ///     Boss class, which inherits values from Enemy Class.
    /// Torpedo / Missile / Projectile Class
    /// 
    /// The Form is resizable, and resizes when changes are made. All the elements should be resize-friendly, 
    /// except the Enemy placement on the Y axis. I think I can fix this quite easily with making the uPosY a range between
    /// object.height and ClientRectangle.Height - 25%(?) of ClientRectangle.Height. This means that the top 75% of the screen,
    /// -object height, can spawn enemies. Speed and enemy sizes are not resize-friendly, so the bigger your resolution the easier
    /// the game is, this is counterweighted by the extra travel length for the torp.
    /// 
    /// Also wanted to add a proper function for whether the elements spawn left or right, which has been started on but abandoned
    /// mainly because I have focused on other implementations, bugfixes and other stuff that has annoyed me, and that are vital for passing.
    /// 
    /// The entire project would need a full re-write from the most basic of functions (missile movement, enemy movement) to
    /// the more "advanced" (spawning enemies, ticks etc.) If I were to do it again, I would have done a more OOP approach, pref with 
    /// more self-aware objects, asynchronous functions and other cool stuff I've learned the past months.
    /// 
    /// I also need to optimize the program further, since I see that it takes a bit more RAM than I would expect 
    /// for such a small project.
    /// </summary>

    public partial class Form1 : Form
    {
        public Form1()
        {
            // Let's set the form to be maximized and without borders for a fullscreen app.
            FormBorderStyle = FormBorderStyle.None;  // <- Removes the borders.
            WindowState = FormWindowState.Maximized; // <- This function automatically calls SetGUI, since the size changes. Neat!

            // Initialize components, bring the top menu to the front and start the game instantly (Timer).
            InitializeComponent();                  // Built-in function that initializes the Shooter.cs[Design]-elements we added etc.
            menuStrip1.BringToFront();              // <- Brings the added menu strip to the front.
            timer1.Enabled = true;                  // Starts timer1.

            // Sets base values ready the game.
            SetMinimumAndMaxSpeed(false);           // Sets the speed values needed for launching our enemies.
            SetScoreAndAmmo();                      // Self-explanatory title.
        }

        int UPosX, UPosY, 
            UboatMinSpeed, UboatMaxSpeed, USpeed,
            TorpSpeed, Ammunition, Score, DiffModifier;

        Point ClickedPoint;

        private bool ShotsFired = false;
        private bool UboatIsHit, BigBoatIsHit;
        private bool LaunchBigBoat;
        private int BigSpeed;
        private int BigHP;
        private int TorpedoDamage = 1;
        private int UboatHP;

        private void pictureBox2_Click(object sender, EventArgs e)
        {
            if (ShotsFired == false && Ammunition > 0)
            {
                GetTargetAndFire();
                Ammunition--;
                ShotsFired = true;
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

        private void GetTargetAndFire()
        {
            ClickedPoint = PointToClient(Cursor.Position);
            TorpSpeed = 25;
            ShotsFired = true;
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            EnemyControl();
            MissileControl();
            CollisionControl();
            UpdateLabelValues();
            if (lossCondition())
            {
                timer1.Stop();
                Console.WriteLine("You lost! " + Ammunition + " ammo, " + Score + " score");
            }
        }

        private void newGameToolStripMenuItem_Click(object sender, EventArgs e)
        {
            NewGame();
        }

        private void NewGame()
        {
            timer1.Stop();
                SetScoreAndAmmo();
                SetMinimumAndMaxSpeed(false);
                ResetAllComp();
                UpdateLabelValues();
                DiffModifier = 0;
            timer1.Start();
        }

        private void SetScoreAndAmmo()
        {
            Ammunition = 10;
            Score = 0;
        }

        private bool lossCondition()
        {
            if (Ammunition == 0 && ShotsFired == false)
                return true;
            else
                return false;
        }

        string BossLevelString ()
        {
            if (LaunchBigBoat)
                return "Boss Level";
            else
                return "No Boss";
        }


        private void UpdateLabelValues()
        {
            IsClicked.Text = ShotsFired.ToString();
            UboatXY.Text = Uboat.Location.X + ", " + Uboat.Location.Y;
            ScoreLabel.Text = "Score:" + Score;
            AmmoLabel.Text = "Ammo: " + Ammunition;
            Difficulty.Text = "Level " + DiffModifier / 4;
            BossLevel.Text = BossLevelString();
        }

        private void SetGUI()
        {
            // Set the background picture(s) to have the same bounds, 
            // and for the foremost of the two to be visible.
            if (pictureBox2.Bounds != ClientRectangle)
                pictureBox2.Bounds = ClientRectangle;
            if (pictureBox2.Visible == false)
                pictureBox2.Visible = true;
            // Placing the labels where we want them on the screen.
            PlaceLabels(50); // The parameter is the inset in pixels.
        }

        private void PlaceLabels(int inset)
        {
            // Placing the labels where we want them on the screen.
            UboatXY.Location = new Point(ClientRectangle.Width - inset, ClientRectangle.Height - inset);
            TorpXY.Location = new Point(ClientRectangle.Width - inset, ClientRectangle.Height - inset / 2);
            ScoreLabel.Location = new Point(inset - ScoreLabel.Width, inset / 2);
            AmmoLabel.Location = new Point(inset - AmmoLabel.Width, inset);
            CurrentSpeed.Location = new Point(inset - CurrentSpeed.Width, ClientRectangle.Height - inset);
            IsClicked.Location = new Point(inset - IsClicked.Width, ClientRectangle.Height - inset / 2);
            Difficulty.Location = new Point(ClientRectangle.Width - inset, inset / 2);
            BossLevel.Location = new Point(ClientRectangle.Width - inset, inset);
        }

        private void CollisionControl()
        {
            if (Torpedo.Bounds.IntersectsWith(Uboat.Bounds) && Uboat.Visible == true)
            {
                UboatHP -= TorpedoDamage;
                if (UboatHP >= 0)
                {
                    UboatIsHit = true;
                    DiffModifier++;
                    Score += (USpeed + DiffModifier);
                    Uboat.Visible = false;
                    MissileReset();

                    if (DiffModifier % 4 == 0 && DiffModifier != 0)
                    {
                        Ammunition += 1;
                    }
                    if (DiffModifier % 10 == 0 && DiffModifier != 0)
                    {
                        LaunchBigBoat = true;
                        SetMinimumAndMaxSpeed(true);
                    }
                }
                
            }
            if (Torpedo.Bounds.IntersectsWith(BigBoat.Bounds) && BigBoat.Visible == true)
            {
                BigHP -= TorpedoDamage;
                Ammunition += 3;

                if (BigHP <= 0)
                {
                    LaunchBigBoat = false;
                    BigBoatIsHit = true;
                    BigBoat.Visible = false;
                    MissileReset();
                    Score += ((BigSpeed + DiffModifier) * 5);
                } else
                {
                    MissileReset();
                }
            }
        }

        private void ResetAllComp()
        {
            if (BigBoatIsHit)
                BigBoat.Visible = false;
            if (UboatIsHit)
                Uboat.Visible = false;
            MissileReset();
        }

        private void MissileControl()
        {
            if (ShotsFired == true)
                MoveMissile();
            else
                MissileReset();
        }

        private void EnemyControl()
        {
            if (LaunchBigBoat)
                CreateEnemy("Big");
            if (Uboat.Visible != true)
                CreateEnemy("Small");
            else
                MoveEnemy();
        }

        private void MoveEnemy()
        {
            if (Uboat.Visible == true)
            {
                Uboat.Location = new Point(UPosX += USpeed, UPosY); // Moves our Uboat along the X axis by USpeed. The Y remains constant.
                if (UPosX >= ClientRectangle.Width)     // If the leftmost x coordinate of our Uboat hitbox passes outside the right side of the screen
                { 
                    Uboat.Visible = false;              // Set visibility to false. Redirects to CreateEnemy() in the EnemyControl, which sets this Uboat up with new values.
                    Score -= (USpeed + DiffModifier);   // If the _Uboat reaches the end, you lose it's value in score!
                }
            }
            if (BigBoat.Visible == true)
            {
                BigBoat.Location = new Point(BigBoat.Location.X + BigSpeed, BigBoat.Location.Y);   // Moves our BigBoat along the X axis by USpeed. The Y remains constant.
                if (BigBoat.Location.X >= ClientRectangle.Width)        // If the leftmost x coordinate of our Uboat hitbox passes outside the right side of the screen
                {
                    LaunchBigBoat = false;
                    BigBoat.Visible = false;                            // Set visibility to false. Redirects to CreateEnemy() in the EnemyControl, which sets this Uboat up with new values.
                    DiffModifier++;
                }
            }
            
        }

        private void CreateEnemy(string EnemyType)
        {
            Random r = new Random(); 
            // Only place we use Random in the entire program.

            if (EnemyType == "Small")
            {
                if (Uboat.Visible == false)
                {
                    UboatHP = 1;
                    UPosX = -Uboat.Width;                           // Makes the Uboat picturebox start just outside form1
                    UPosY = 10 + r.Next(10, 150);                   // Must be a rand
                    USpeed = r.Next(UboatMinSpeed, UboatMaxSpeed);  // Must be a rand
                    CurrentSpeed.Text = USpeed.ToString();          // Showing the speed of the Uboat, mainly used for debugging.
                    Uboat.Visible = true;                           // Sets visibility (initiates movement through the EnemyControl function)
                    Uboat.Location = new Point(UPosX, UPosY);       // Sets start location
                }
            }
            if (EnemyType == "Big")
            {
                if (BigBoat.Visible == false)
                {
                    BigHP = 5;
                    BigSpeed = r.Next(UboatMinSpeed, UboatMaxSpeed);
                    BigBoat.Location = new Point(-BigBoat.Width, r.Next(10 + BigBoat.Height, 750));
                    BigBoat.Visible = true;
                }
                
            }
            
        }

        private void SetMinimumAndMaxSpeed(bool Set)
        { 
            if (UboatMinSpeed > 0 && UboatMaxSpeed > 10 && Set)
            {
                UboatMinSpeed++;
                UboatMaxSpeed++;
            }
            else
            {
                UboatMinSpeed = 1;
                UboatMaxSpeed = 11;
            }
        }

        private void MoveMissile()
        {
            if (ShotsFired == true)
            {
                Torpedo.Visible = true;
                if (Torpedo.Location.Y >= 0)
                    Torpedo.Location = new Point(ClickedPoint.X, Torpedo.Location.Y - TorpSpeed); 
                // Originally had this Torpedo.Location.Y var stored in a seperate value, and used -=. It was redundant.
                else
                    MissileReset();
            }
        }

        private void MissileReset()
        {
            Torpedo.Visible = false;
            ShotsFired = false;
            Torpedo.Location = new Point(0, ClientRectangle.Height);
        }
    }
}

/*
 Buglist:
 
    F = Finished
    f = Finished, but not really implemented as was originally intended. In need of touchup or rework
    N = Neglected / Abandoned
    n = currently neglected, might take it up again later.

    N.Make Enemy class
        n.Randomize right/left direction of attack

    Make Boss / Bonus Enemy
        F.Create a new picturebox 
            F.Similar to the Uboat picturebox -- Copy called "BigBoat"

    F.Enemies/Invaders
        F.Need to add invaders
            F.Pictures
            F.Moving enemies
            F.Random speeds
            F.Random heights
            N.Random quantities --  cannot instansiate new Uboat objects, 
                                    would need to rework the entire setup, 
                                    or simply have multiple Uboats with 
                                    spawn timers or the like, but meh.
        F.Boss invader
            F.Implement hp system (?)
    
    Score
        

    Add loss/game over condition
        F.Add condition
        F.Add effect
        Open visual Menu for Highscore save within the Client
        F.Add highscore saving 
        Add new highscores instead of overwriting
        F.Save to .xml -- This is currently done, but in a seperate file so as to not much up this project.
        Show highscores
        F.Make the player lose score when a uboat passes
            
    F.Missiles
        F.Missile no longer work as they should
            
    F.Multiple projectiles:
        F.Can launch several missiles by clicking several times.
        F.The objectives dont all dissappear until the last click goes through.
 */
 