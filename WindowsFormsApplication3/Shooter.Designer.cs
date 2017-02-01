namespace WindowsFormsApplication3
{
    partial class Form1
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.Torpedo = new System.Windows.Forms.PictureBox();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.pictureBox2 = new System.Windows.Forms.PictureBox();
            this.Uboat = new System.Windows.Forms.PictureBox();
            this.CurrentSpeed = new System.Windows.Forms.Label();
            this.IsClicked = new System.Windows.Forms.Label();
            this.TorpXY = new System.Windows.Forms.Label();
            this.UboatXY = new System.Windows.Forms.Label();
            this.ScoreLabel = new System.Windows.Forms.Label();
            this.AmmoLabel = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.Torpedo)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.Uboat)).BeginInit();
            this.SuspendLayout();
            // 
            // Torpedo
            // 
            this.Torpedo.AccessibleDescription = "Torpedo";
            this.Torpedo.AccessibleName = "Torpedo";
            this.Torpedo.BackColor = System.Drawing.Color.Transparent;
            this.Torpedo.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("Torpedo.BackgroundImage")));
            this.Torpedo.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.Torpedo.Location = new System.Drawing.Point(1058, 438);
            this.Torpedo.Name = "Torpedo";
            this.Torpedo.Size = new System.Drawing.Size(19, 95);
            this.Torpedo.TabIndex = 0;
            this.Torpedo.TabStop = false;
            this.Torpedo.Visible = false;
            // 
            // timer1
            // 
            this.timer1.Enabled = true;
            this.timer1.Interval = 16;
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // pictureBox2
            // 
            this.pictureBox2.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("pictureBox2.BackgroundImage")));
            this.pictureBox2.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.pictureBox2.Cursor = System.Windows.Forms.Cursors.Cross;
            this.pictureBox2.Location = new System.Drawing.Point(0, 0);
            this.pictureBox2.Margin = new System.Windows.Forms.Padding(0);
            this.pictureBox2.Name = "pictureBox2";
            this.pictureBox2.Size = new System.Drawing.Size(1257, 533);
            this.pictureBox2.TabIndex = 1;
            this.pictureBox2.TabStop = false;
            this.pictureBox2.Visible = false;
            this.pictureBox2.Click += new System.EventHandler(this.pictureBox2_Click);
            // 
            // Uboat
            // 
            this.Uboat.AccessibleDescription = "Uboat object";
            this.Uboat.AccessibleName = "Uboat";
            this.Uboat.BackColor = System.Drawing.Color.Transparent;
            this.Uboat.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("Uboat.BackgroundImage")));
            this.Uboat.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.Uboat.Location = new System.Drawing.Point(866, 125);
            this.Uboat.Name = "Uboat";
            this.Uboat.Size = new System.Drawing.Size(149, 67);
            this.Uboat.TabIndex = 2;
            this.Uboat.TabStop = false;
            this.Uboat.Visible = false;
            // 
            // CurrentSpeed
            // 
            this.CurrentSpeed.AutoSize = true;
            this.CurrentSpeed.Location = new System.Drawing.Point(3, 520);
            this.CurrentSpeed.Name = "CurrentSpeed";
            this.CurrentSpeed.Size = new System.Drawing.Size(35, 13);
            this.CurrentSpeed.TabIndex = 3;
            this.CurrentSpeed.Text = "label1";
            // 
            // IsClicked
            // 
            this.IsClicked.AutoSize = true;
            this.IsClicked.Location = new System.Drawing.Point(3, 537);
            this.IsClicked.Name = "IsClicked";
            this.IsClicked.Size = new System.Drawing.Size(35, 13);
            this.IsClicked.TabIndex = 4;
            this.IsClicked.Text = "label1";
            // 
            // TorpXY
            // 
            this.TorpXY.AutoSize = true;
            this.TorpXY.Location = new System.Drawing.Point(1212, 536);
            this.TorpXY.Name = "TorpXY";
            this.TorpXY.Size = new System.Drawing.Size(35, 13);
            this.TorpXY.TabIndex = 5;
            this.TorpXY.Text = "label1";
            // 
            // UboatXY
            // 
            this.UboatXY.AutoSize = true;
            this.UboatXY.Location = new System.Drawing.Point(1212, 520);
            this.UboatXY.Name = "UboatXY";
            this.UboatXY.Size = new System.Drawing.Size(35, 13);
            this.UboatXY.TabIndex = 6;
            this.UboatXY.Text = "label1";
            // 
            // ScoreLabel
            // 
            this.ScoreLabel.AutoSize = true;
            this.ScoreLabel.Location = new System.Drawing.Point(0, 17);
            this.ScoreLabel.Name = "ScoreLabel";
            this.ScoreLabel.Size = new System.Drawing.Size(35, 13);
            this.ScoreLabel.TabIndex = 7;
            this.ScoreLabel.Text = "label1";
            // 
            // AmmoLabel
            // 
            this.AmmoLabel.AutoSize = true;
            this.AmmoLabel.Location = new System.Drawing.Point(0, 34);
            this.AmmoLabel.Name = "AmmoLabel";
            this.AmmoLabel.Size = new System.Drawing.Size(35, 13);
            this.AmmoLabel.TabIndex = 8;
            this.AmmoLabel.Text = "label1";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("$this.BackgroundImage")));
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.ClientSize = new System.Drawing.Size(1261, 627);
            this.Controls.Add(this.AmmoLabel);
            this.Controls.Add(this.ScoreLabel);
            this.Controls.Add(this.UboatXY);
            this.Controls.Add(this.TorpXY);
            this.Controls.Add(this.IsClicked);
            this.Controls.Add(this.CurrentSpeed);
            this.Controls.Add(this.Uboat);
            this.Controls.Add(this.Torpedo);
            this.Controls.Add(this.pictureBox2);
            this.Name = "Form1";
            this.Text = "Form1";
            this.Click += new System.EventHandler(this.Form1_Click);
            this.KeyUp += new System.Windows.Forms.KeyEventHandler(this.Form1_KeyUp);
            this.Resize += new System.EventHandler(this.Form1_Resize);
            ((System.ComponentModel.ISupportInitialize)(this.Torpedo)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.Uboat)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.PictureBox Torpedo;
        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.PictureBox pictureBox2;
        private System.Windows.Forms.PictureBox Uboat;
        private System.Windows.Forms.Label CurrentSpeed;
        private System.Windows.Forms.Label IsClicked;
        private System.Windows.Forms.Label TorpXY;
        private System.Windows.Forms.Label UboatXY;
        private System.Windows.Forms.Label ScoreLabel;
        private System.Windows.Forms.Label AmmoLabel;
    }
}

