namespace SearchEngineProject
{
    partial class MainWindow
    {
        /// <summary>
        /// Variable nécessaire au concepteur.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Nettoyage des ressources utilisées.
        /// </summary>
        /// <param name="disposing">true si les ressources managées doivent être supprimées ; sinon, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Code généré par le Concepteur Windows Form

        /// <summary>
        /// Méthode requise pour la prise en charge du concepteur - ne modifiez pas
        /// le contenu de cette méthode avec l'éditeur de code.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainWindow));
            this.searchTextBox = new System.Windows.Forms.TextBox();
            this.searchButton = new System.Windows.Forms.Button();
            this.articleTextBox = new System.Windows.Forms.RichTextBox();
            this.numberResultsLabel = new System.Windows.Forms.Label();
            this.menu = new System.Windows.Forms.MenuStrip();
            this.indexToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.indexADirectoryToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.statisticsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.quitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.helpToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.aboutToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.progressBar = new System.Windows.Forms.ProgressBar();
            this.boolCBox = new System.Windows.Forms.CheckBox();
            this.rankCbox = new System.Windows.Forms.CheckBox();
            this.panel1 = new System.Windows.Forms.Panel();
            this.flowLayoutPanel1 = new System.Windows.Forms.FlowLayoutPanel();
            this.correctedWordLabel = new System.Windows.Forms.Label();
            this.indexingLabel = new System.Windows.Forms.Label();
            this.panel2 = new System.Windows.Forms.Panel();
            this.panel3 = new System.Windows.Forms.Panel();
            this.menu.SuspendLayout();
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            this.panel3.SuspendLayout();
            this.SuspendLayout();
            // 
            // searchTextBox
            // 
            this.searchTextBox.BackColor = System.Drawing.Color.White;
            this.searchTextBox.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.searchTextBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.searchTextBox.Enabled = false;
            this.searchTextBox.Font = new System.Drawing.Font("Segoe Print", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.searchTextBox.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.searchTextBox.Location = new System.Drawing.Point(5, 0);
            this.searchTextBox.Name = "searchTextBox";
            this.searchTextBox.Size = new System.Drawing.Size(263, 34);
            this.searchTextBox.TabIndex = 0;
            this.searchTextBox.Click += new System.EventHandler(this.searchTextBox_Click);
            this.searchTextBox.TextChanged += new System.EventHandler(this.textBox1_TextChanged);
            // 
            // searchButton
            // 
            this.searchButton.BackColor = System.Drawing.SystemColors.ButtonHighlight;
            this.searchButton.BackgroundImage = global::SearchEngineProject.Properties.Resources.SearchIcon;
            this.searchButton.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.searchButton.FlatAppearance.BorderColor = System.Drawing.Color.White;
            this.searchButton.FlatAppearance.BorderSize = 0;
            this.searchButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.searchButton.Location = new System.Drawing.Point(276, 34);
            this.searchButton.Name = "searchButton";
            this.searchButton.Size = new System.Drawing.Size(31, 33);
            this.searchButton.TabIndex = 1;
            this.searchButton.Text = "\r\n";
            this.searchButton.UseVisualStyleBackColor = false;
            // 
            // articleTextBox
            // 
            this.articleTextBox.BackColor = System.Drawing.Color.WhiteSmoke;
            this.articleTextBox.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.articleTextBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.articleTextBox.Font = new System.Drawing.Font("Calibri", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.articleTextBox.ForeColor = System.Drawing.Color.DimGray;
            this.articleTextBox.Location = new System.Drawing.Point(13, 5);
            this.articleTextBox.Name = "articleTextBox";
            this.articleTextBox.ReadOnly = true;
            this.articleTextBox.Size = new System.Drawing.Size(456, 339);
            this.articleTextBox.TabIndex = 3;
            this.articleTextBox.Text = "";
            // 
            // numberResultsLabel
            // 
            this.numberResultsLabel.BackColor = System.Drawing.Color.WhiteSmoke;
            this.numberResultsLabel.Font = new System.Drawing.Font("Calibri Light", 14.25F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.numberResultsLabel.ForeColor = System.Drawing.Color.Gray;
            this.numberResultsLabel.Location = new System.Drawing.Point(294, 2);
            this.numberResultsLabel.Name = "numberResultsLabel";
            this.numberResultsLabel.Size = new System.Drawing.Size(127, 24);
            this.numberResultsLabel.TabIndex = 5;
            this.numberResultsLabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // menu
            // 
            this.menu.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.menu.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.menu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.indexToolStripMenuItem,
            this.helpToolStripMenuItem});
            this.menu.Location = new System.Drawing.Point(0, 0);
            this.menu.Name = "menu";
            this.menu.Size = new System.Drawing.Size(916, 25);
            this.menu.TabIndex = 7;
            this.menu.Text = "menuStrip1";
            // 
            // indexToolStripMenuItem
            // 
            this.indexToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.indexADirectoryToolStripMenuItem,
            this.statisticsToolStripMenuItem,
            this.toolStripSeparator1,
            this.quitToolStripMenuItem});
            this.indexToolStripMenuItem.ForeColor = System.Drawing.Color.Gold;
            this.indexToolStripMenuItem.Name = "indexToolStripMenuItem";
            this.indexToolStripMenuItem.Size = new System.Drawing.Size(51, 21);
            this.indexToolStripMenuItem.Text = "Index";
            // 
            // indexADirectoryToolStripMenuItem
            // 
            this.indexADirectoryToolStripMenuItem.Name = "indexADirectoryToolStripMenuItem";
            this.indexADirectoryToolStripMenuItem.Size = new System.Drawing.Size(187, 22);
            this.indexADirectoryToolStripMenuItem.Text = "Choose a directory";
            this.indexADirectoryToolStripMenuItem.Click += new System.EventHandler(this.indexADirectoryToolStripMenuItem_Click);
            // 
            // statisticsToolStripMenuItem
            // 
            this.statisticsToolStripMenuItem.Enabled = false;
            this.statisticsToolStripMenuItem.Name = "statisticsToolStripMenuItem";
            this.statisticsToolStripMenuItem.Size = new System.Drawing.Size(187, 22);
            this.statisticsToolStripMenuItem.Text = "Statistics";
            this.statisticsToolStripMenuItem.Click += new System.EventHandler(this.statisticsToolStripMenuItem_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(184, 6);
            // 
            // quitToolStripMenuItem
            // 
            this.quitToolStripMenuItem.Name = "quitToolStripMenuItem";
            this.quitToolStripMenuItem.Size = new System.Drawing.Size(187, 22);
            this.quitToolStripMenuItem.Text = "Quit";
            this.quitToolStripMenuItem.Click += new System.EventHandler(this.quitToolStripMenuItem_Click);
            // 
            // helpToolStripMenuItem
            // 
            this.helpToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.aboutToolStripMenuItem});
            this.helpToolStripMenuItem.ForeColor = System.Drawing.Color.Gold;
            this.helpToolStripMenuItem.Name = "helpToolStripMenuItem";
            this.helpToolStripMenuItem.Size = new System.Drawing.Size(47, 21);
            this.helpToolStripMenuItem.Text = "Help";
            // 
            // aboutToolStripMenuItem
            // 
            this.aboutToolStripMenuItem.Name = "aboutToolStripMenuItem";
            this.aboutToolStripMenuItem.Size = new System.Drawing.Size(111, 22);
            this.aboutToolStripMenuItem.Text = "About";
            this.aboutToolStripMenuItem.Click += new System.EventHandler(this.aboutToolStripMenuItem_Click);
            // 
            // progressBar
            // 
            this.progressBar.BackColor = System.Drawing.Color.White;
            this.progressBar.ForeColor = System.Drawing.Color.Gold;
            this.progressBar.Location = new System.Drawing.Point(8, 34);
            this.progressBar.Name = "progressBar";
            this.progressBar.Size = new System.Drawing.Size(268, 33);
            this.progressBar.Style = System.Windows.Forms.ProgressBarStyle.Continuous;
            this.progressBar.TabIndex = 10;
            this.progressBar.UseWaitCursor = true;
            this.progressBar.Visible = false;
            // 
            // boolCBox
            // 
            this.boolCBox.Appearance = System.Windows.Forms.Appearance.Button;
            this.boolCBox.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.boolCBox.CheckAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.boolCBox.Checked = true;
            this.boolCBox.CheckState = System.Windows.Forms.CheckState.Checked;
            this.boolCBox.FlatAppearance.BorderColor = System.Drawing.Color.Black;
            this.boolCBox.FlatAppearance.CheckedBackColor = System.Drawing.Color.Gold;
            this.boolCBox.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Gold;
            this.boolCBox.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Gold;
            this.boolCBox.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.boolCBox.Font = new System.Drawing.Font("Segoe Print", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.boolCBox.Location = new System.Drawing.Point(328, 31);
            this.boolCBox.Name = "boolCBox";
            this.boolCBox.Size = new System.Drawing.Size(51, 37);
            this.boolCBox.TabIndex = 12;
            this.boolCBox.Text = "Bool";
            this.boolCBox.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.boolCBox.UseVisualStyleBackColor = false;
            this.boolCBox.Click += new System.EventHandler(this.checkBox1_Click);
            // 
            // rankCbox
            // 
            this.rankCbox.Appearance = System.Windows.Forms.Appearance.Button;
            this.rankCbox.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.rankCbox.CheckAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.rankCbox.FlatAppearance.BorderColor = System.Drawing.Color.Black;
            this.rankCbox.FlatAppearance.CheckedBackColor = System.Drawing.Color.Gold;
            this.rankCbox.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.rankCbox.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.rankCbox.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.rankCbox.Font = new System.Drawing.Font("Segoe Print", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.rankCbox.ForeColor = System.Drawing.Color.Gold;
            this.rankCbox.Location = new System.Drawing.Point(378, 31);
            this.rankCbox.Name = "rankCbox";
            this.rankCbox.Size = new System.Drawing.Size(51, 37);
            this.rankCbox.TabIndex = 13;
            this.rankCbox.Text = "Rank";
            this.rankCbox.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.rankCbox.UseVisualStyleBackColor = false;
            this.rankCbox.Click += new System.EventHandler(this.checkBox2_Click);
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.WhiteSmoke;
            this.panel1.Controls.Add(this.flowLayoutPanel1);
            this.panel1.Controls.Add(this.correctedWordLabel);
            this.panel1.Controls.Add(this.numberResultsLabel);
            this.panel1.Location = new System.Drawing.Point(8, 80);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(421, 229);
            this.panel1.TabIndex = 14;
            // 
            // flowLayoutPanel1
            // 
            this.flowLayoutPanel1.AutoScroll = true;
            this.flowLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.flowLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.flowLayoutPanel1.Name = "flowLayoutPanel1";
            this.flowLayoutPanel1.Size = new System.Drawing.Size(421, 229);
            this.flowLayoutPanel1.TabIndex = 0;
            // 
            // correctedWordLabel
            // 
            this.correctedWordLabel.AutoSize = true;
            this.correctedWordLabel.Cursor = System.Windows.Forms.Cursors.Hand;
            this.correctedWordLabel.Font = new System.Drawing.Font("Calibri", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.correctedWordLabel.ForeColor = System.Drawing.Color.Firebrick;
            this.correctedWordLabel.Location = new System.Drawing.Point(7, 3);
            this.correctedWordLabel.Name = "correctedWordLabel";
            this.correctedWordLabel.Size = new System.Drawing.Size(0, 23);
            this.correctedWordLabel.TabIndex = 0;
            this.correctedWordLabel.MouseClick += new System.Windows.Forms.MouseEventHandler(this.correctedWordLabel_MouseClick);
            this.correctedWordLabel.MouseEnter += new System.EventHandler(this.correctedWordLabel_MouseEnter);
            this.correctedWordLabel.MouseLeave += new System.EventHandler(this.correctedWordLabel_MouseLeave);
            // 
            // indexingLabel
            // 
            this.indexingLabel.BackColor = System.Drawing.Color.Transparent;
            this.indexingLabel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.indexingLabel.Font = new System.Drawing.Font("Segoe Print", 27.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.indexingLabel.ForeColor = System.Drawing.Color.Gold;
            this.indexingLabel.Location = new System.Drawing.Point(0, 0);
            this.indexingLabel.Name = "indexingLabel";
            this.indexingLabel.Size = new System.Drawing.Size(916, 387);
            this.indexingLabel.TabIndex = 11;
            this.indexingLabel.Text = "We are indexing the directory for you :)";
            this.indexingLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // panel2
            // 
            this.panel2.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panel2.BackColor = System.Drawing.Color.WhiteSmoke;
            this.panel2.Controls.Add(this.articleTextBox);
            this.panel2.Location = new System.Drawing.Point(439, 31);
            this.panel2.Name = "panel2";
            this.panel2.Padding = new System.Windows.Forms.Padding(13, 5, 0, 5);
            this.panel2.Size = new System.Drawing.Size(469, 349);
            this.panel2.TabIndex = 15;
            // 
            // panel3
            // 
            this.panel3.BackColor = System.Drawing.Color.White;
            this.panel3.Controls.Add(this.searchTextBox);
            this.panel3.Location = new System.Drawing.Point(8, 34);
            this.panel3.Name = "panel3";
            this.panel3.Padding = new System.Windows.Forms.Padding(5, 0, 0, 0);
            this.panel3.Size = new System.Drawing.Size(268, 33);
            this.panel3.TabIndex = 15;
            // 
            // MainWindow
            // 
            this.AcceptButton = this.searchButton;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Black;
            this.ClientSize = new System.Drawing.Size(916, 387);
            this.Controls.Add(this.progressBar);
            this.Controls.Add(this.rankCbox);
            this.Controls.Add(this.boolCBox);
            this.Controls.Add(this.searchButton);
            this.Controls.Add(this.menu);
            this.Controls.Add(this.panel3);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.indexingLabel);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MainMenuStrip = this.menu;
            this.MinimumSize = new System.Drawing.Size(932, 426);
            this.Name = "MainWindow";
            this.Text = "Cal State LB Search Engine";
            this.TransparencyKey = System.Drawing.Color.Maroon;
            this.menu.ResumeLayout(false);
            this.menu.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.panel2.ResumeLayout(false);
            this.panel3.ResumeLayout(false);
            this.panel3.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox searchTextBox;
        private System.Windows.Forms.Button searchButton;
        private System.Windows.Forms.RichTextBox articleTextBox;
        private System.Windows.Forms.Label numberResultsLabel;
        private System.Windows.Forms.MenuStrip menu;
        private System.Windows.Forms.ToolStripMenuItem indexToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem statisticsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem quitToolStripMenuItem;
        private System.Windows.Forms.ProgressBar progressBar;
        private System.Windows.Forms.CheckBox boolCBox;
        private System.Windows.Forms.CheckBox rankCbox;
        private System.Windows.Forms.ToolStripMenuItem indexADirectoryToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label indexingLabel;
        private System.Windows.Forms.Label correctedWordLabel;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel1;
        private System.Windows.Forms.ToolStripMenuItem helpToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem aboutToolStripMenuItem;
    }
}

