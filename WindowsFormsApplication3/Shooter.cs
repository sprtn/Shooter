using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;

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
        delegate void Anon();
        delegate void Bnon(int arg1);

        static int Ammunition, Score, DiffModifier;

        int UPosX, UPosY,
            UboatMinSpeed,
            UboatMaxSpeed,
            USpeed,
            TorpSpeed;

        Point ClickedPoint;

        private bool
            ShotsFired = false,
            UboatIsHit,
            LaunchBigBoat, BigBoatIsHit;

        private int TorpedoDamage = 1,
            UboatHP, BigHP, BigSpeed;

        protected override CreateParams CreateParams
        {
            get
            {
                CreateParams cp = base.CreateParams;
                cp.ExStyle |= 0x02000000;  // Turn on WS_EX_COMPOSITED
                return cp;
            }
        }
        
        Anon StartVariables = () => { Ammunition = 15; Score = 0; DiffModifier = 0; };

        public Form1()
        {
            Anon Exit = () => { this.Close(); };
            // Let's set the form to be maximized and without borders for a fullscreen app.
            FormBorderStyle = FormBorderStyle.None;  // <- Removes the borders.
            WindowState = FormWindowState.Maximized; // <- This function automatically calls SetGUI, since the size changes. Neat!

            // Initialize components, bring the top menu to the front and start the game instantly (Timer).
            InitializeComponent();                  // Built-in function that initializes the Shooter.cs[Design]-elements we added etc.
            menuStrip1.BringToFront();              // <- Brings the added menu strip to the front.
            timer1.Enabled = true;                  // Starts timer1.

            // Sets base values ready the game.
            SetMinimumAndMaxSpeed();                // Sets the speed values needed for launching our enemies.
            StartVariables();                       // Self-explanatory title.
        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {
            if (ShotsFired == false && Ammunition > 0)
            {
                ShotsFired = true;
                Ammunition--;
                ClickedPoint = PointToClient(Cursor.Position);
                TorpSpeed = 25;
            }
        }


        private void Form1_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                if (NameBox.Visible == true)
                {
                    NameBox.Visible = false;
                    // Turn off pause and everything.
                }
                else
                    Close();
            }
        }

        private void Form1_Resize(object sender, EventArgs e)
        {
            SetGUI();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            EnemyControl();
            MissileControl();
            CollisionControl();
            UpdateLabelValues();
            if (Ammunition == 0 && ShotsFired == false)
                timer1.Stop();
        }

        private void newGameToolStripMenuItem_Click(object sender, EventArgs e)
        {
            NewGame();
        }

        // Game management, loss condition, start values

        private void NewGame()
        {
            timer1.Stop();
                StartVariables();
                SetMinimumAndMaxSpeed();
                ResetAllComp();
                UpdateLabelValues();
            timer1.Start();
        }

        // Labels and GUI

        private void UpdateLabelValues()
        {
            IsClicked.Text = ShotsFired.ToString();
            UboatXY.Text = Uboat.Location.X + ", " + Uboat.Location.Y;
            ScoreLabel.Text = "Score:" + Score;
            AmmoLabel.Text = "Ammo: " + Ammunition;
            Difficulty.Text = "Level " + DiffModifier / 4;
            Anon BossCheck = () => { if (LaunchBigBoat) BossLevel.Text = "Boss Level"; else BossLevel.Text = "No Boss"; }; 
            // Unnecessary use of anonymous function, but it looks better.
            BossCheck();
        }

        private void SetGUI()
        {
            // Place the labels where we want them on the screen.
            Bnon UX = (arg1) =>
            {
                // Placing the labels where we want them on the screen. arg1 == inset.
                UboatXY.Location = new Point(ClientRectangle.Width - arg1, ClientRectangle.Height - arg1);
                TorpXY.Location = new Point(ClientRectangle.Width - arg1, ClientRectangle.Height - arg1 / 2);
                ScoreLabel.Location = new Point(arg1 - ScoreLabel.Width, arg1 / 2);
                AmmoLabel.Location = new Point(arg1 - AmmoLabel.Width, arg1);
                CurrentSpeed.Location = new Point(arg1 - CurrentSpeed.Width, ClientRectangle.Height - arg1);
                IsClicked.Location = new Point(arg1 - IsClicked.Width, ClientRectangle.Height - arg1 / 2);
                Difficulty.Location = new Point(ClientRectangle.Width - arg1, arg1 / 2);
                BossLevel.Location = new Point(ClientRectangle.Width - arg1, arg1);
                NameBox.Location = new Point((ClientRectangle.Width / 2) - (int)((double) HighscoreHeader.Width * 3.2), ClientRectangle.Height / 2 + NameBox.Height);
                HighscoreHeader.Location = new Point((ClientRectangle.Width / 2) - (HighscoreHeader.Width * 2), ClientRectangle.Height / 2 );
            };
            Bnon SetElements = (arg1) => {
                if (pictureBox2.Bounds != ClientRectangle)
                    pictureBox2.Bounds = ClientRectangle;
                if (pictureBox2.Visible == false)
                    pictureBox2.Visible = true;
                UX(arg1);
            };
            SetElements(50);
        }

        // Collision control, save, difficulty progression etc, BigBoat launching etc.

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
                        SetMinimumAndMaxSpeed();
                    }
                }
                
            } else if (Torpedo.Bounds.IntersectsWith(BigBoat.Bounds) && BigBoat.Visible == true) {
                BigHP -= TorpedoDamage;
                Ammunition += 3;

                if (BigHP <= 0)
                {
                    LaunchBigBoat = false;
                    BigBoatIsHit = true;
                    BigBoat.Visible = false;
                    Score += ((BigSpeed + DiffModifier) * 5);
                }
                MissileReset();
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

        private void pauseResumeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            PauseGame();
        }

        private void PauseGame(bool PauseGame)
        {
            if (PauseGame == true)
            {
                timer1.Stop();
            }
            else
            {
                DontDisplayScoreInput();
                timer1.Start();
            }
        }

        private void DontDisplayScoreInput()
        {
            if (HighscoreHeader.Visible == true)
                HighscoreHeader.Visible = false;
            if (NameBox.Visible == true)
                NameBox.Visible = false;
            NameBox.Text = null;
        }

        private void NameBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter)
            {
                string playerName = NameBox.Text ?? "Anon";
                if (playerName.Length == 0 || playerName == null)
                    playerName = "Anon"; // The text-box starts off as "". An empty string, but not null.

                XMLManager.XMLSubmit(playerName, Score);
                PauseGame(false);
            }
            else if (e.KeyChar == (char)Keys.Escape)
            {
                if (NameBox.Visible == true)
                {
                    DontDisplayScoreInput();
                    timer1.Start();
                }
                else
                {
                    Close();
                }
                
            }
        }

        private void PauseGame()
        {
            if (timer1.Enabled)
                timer1.Stop();
            else
            {
                DontDisplayScoreInput();
                timer1.Start();
            }
        }

        private void Form1_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == ' ' || e.KeyChar == 'p')
                PauseGame();
            return;
        }

        private void Form1_Click(object sender, EventArgs e)
        {
            if (ShotsFired == false && Ammunition > 0)
            {
                ShotsFired = true;
                Ammunition--;
                ClickedPoint = PointToClient(Cursor.Position);
                TorpSpeed = 25;
            }
        }

        // Enemy movement, creation and management.

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

        // Speed modification

        private void SetMinimumAndMaxSpeed()
        { 
            if (UboatMinSpeed > 0 && UboatMaxSpeed > 10)
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

        // Missile management

        private void MissileControl()
        {
            if (ShotsFired == true)
                MoveMissile();
            else
                MissileReset();
        }

        private void MoveMissile()
        {
            if (ShotsFired == true)
            {
                Torpedo.Visible = true;
                if (Torpedo.Location.Y >= 0)
                    Torpedo.Location = new Point(ClickedPoint.X, Torpedo.Location.Y - TorpSpeed); 
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

        // Highscore stuffs:

        private void showHighscoresToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            // For showing highscores.
        }

        private void submitHighscoreToolStripMenuItem_Click(object sender, EventArgs e)
        {
            PauseGame(true);
            HighscoreHeader.Text = "Input your name below!";
            HighscoreHeader.Visible = true;
            NameBox.Visible = true;
        }

        class XMLManager
        {

            public static void XMLSubmit(string _player, int _score)
            {
                string m_filepath = "Highscores.xml";
                try
                {
                    SaveToFile(_player, _score, m_filepath);
                }
                catch (Exception e)
                {
                    Console.WriteLine("{0} tells us we are unable to locate {1}, so we created it.", e, m_filepath);
                    CreateNewXML(_player, _score, m_filepath);
                }
                finally
                {
                    if (File.Exists(m_filepath))
                    {
                        SortXMLByScore(m_filepath);
                    }
                    else
                    {
                        Console.WriteLine("XML error: {0} cannot be updated with \nPlayer: {1}, Score: {2}", m_filepath, _player, _score);
                    }
                }
            }


            /// <summary>
            /// Sorts the Highscores in ascending order.
            /// </summary>
            /// <param name="filepath"> Filepath to save the highscores to </param>
            private static void SortXMLByScore(string filepath)
            {
                string temp_file = filepath.Remove(filepath.Length - 4);    // Removes the last 4 characters of the string. We get a temporary file that is not .xml so we can alter it and overwrite the .xml later.
                XDocument doc = XDocument.Load(filepath);                   //Loading the file from the folder.
                var SortScores = doc.Element(temp_file).Elements("Score").OrderByDescending(Score => (int)Score.Attribute("Points")); // Sorting the file based on their Points
                doc = new XDocument(new XElement(temp_file, SortScores));   // Preparing the file for saving
                doc.Save(filepath);                                         // Overwriting original file
            }

            /// <summary>
            /// Creates a new XML document in our wanted format
            /// </summary>
            /// <param name="player"> The player name input </param>
            /// <param name="score"> Score to be registered on player name </param>
            /// <param name="file"> Filepath to save the highscores to </param>
            private static void CreateNewXML(string player, int score, string file)
            {
                try
                {
                    XDocument doc = new XDocument(
                                        new XElement("Highscores"));        // Creating an XML with the "Highscores" element.
                    doc.Save(file);                                         // Saving a fresh XML-file
                    SaveToFile(player, score, file);                        // Calling SaveToFile-function
                }
                catch (Exception e)                                         // Catches if there's an error writing to file.
                {
                    Console.WriteLine("CreateNewXML.Feilmelding: {0}", e);  // Not necessary, but good practice.
                }
            }

            /// <summary>
            /// Saves current score and the name input into the XML document.
            /// 
            /// Adding the attrubute "Player" and "Score" under a Score-element.
            /// This is then added under the "Highscores" element we created
            /// in the CreateNewXML-method.
            /// 
            /// </summary>
            /// <param name="m_player"> The player name input </param>
            /// <param name="m_score"> Score to be registered on player name </param>
            /// <param name="m_filepath"> Filepath to save the highscores to </param>
            private static void SaveToFile(string m_player, int m_score, string m_filepath)
            {
                XDocument doc = XDocument.Load(m_filepath);     // Loading the file from the filepath and storing it in a temporary variable
                var newScore = new XElement("Score",            // Adding a new element, Score.
                    new XAttribute("Player", m_player),         // Adding "Player" attribute to the Score element
                    new XAttribute("Points", m_score)           // Adding "Points" attribute to the Score element.
                );
                doc.Element("Highscores").Add(newScore);        // Adding the newly created "Score" Element under the "Highscore" element in the temporary XML-file.
                doc.Save(m_filepath);                           // Updating the xml-doc at the filepath
                SortXMLByScore(m_filepath);                     // Calling the sort function
            }
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
            New Game doesn't reset the DiffModifier!
    
    F.Score
        F.Make the player lose score when a uboat passes
        F.Add highscore saving 
        F.Add new highscores instead of overwriting
        F.Save to .xml -- This is currently done, but in a seperate file so as to not much up this project.
        Show highscores
        Open visual Menu for Highscore save within the Client

    Pause
        When submitting score, you automatically start the game again.
        F.Make overloading pause function
            F.One with true/false
            F.One that sets it to the opposite value, no arg.
            On timer1.Start(), check that the score input boxes aren't showing using the designated method. Also on new game.

    Add loss/game over condition
        F.Add condition
        F.Add effect

    Add Pause function
        Pause button in menu field, also accessible by pressing "p" or middle mouse button.
            
    F.Missiles
        F.Missile no longer work as they should
            
    F.Multiple projectiles:
        F.Can launch several missiles by clicking several times.
        F.The objectives dont all dissappear until the last click goes through.
 */
