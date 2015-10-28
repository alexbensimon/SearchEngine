﻿namespace SearchEngineProject
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
            this.resultsTextBox = new System.Windows.Forms.RichTextBox();
            this.articleTextBox = new System.Windows.Forms.RichTextBox();
            this.numberResultsLabel = new System.Windows.Forms.Label();
            this.menu = new System.Windows.Forms.MenuStrip();
            this.indexToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.indexADirectoryToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.statisticsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.quitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.progressBar = new System.Windows.Forms.ProgressBar();
            this.indexingLabel = new System.Windows.Forms.Label();
            this.boolCBox = new System.Windows.Forms.CheckBox();
            this.rankCbox = new System.Windows.Forms.CheckBox();
            this.menu.SuspendLayout();
            this.SuspendLayout();
            // 
            // searchTextBox
            // 
            this.searchTextBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.searchTextBox.Enabled = false;
            this.searchTextBox.Font = new System.Drawing.Font("Segoe Print", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.searchTextBox.Location = new System.Drawing.Point(7, 31);
            this.searchTextBox.Name = "searchTextBox";
            this.searchTextBox.Size = new System.Drawing.Size(282, 34);
            this.searchTextBox.TabIndex = 0;
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
            this.searchButton.Location = new System.Drawing.Point(288, 32);
            this.searchButton.Name = "searchButton";
            this.searchButton.Size = new System.Drawing.Size(30, 32);
            this.searchButton.TabIndex = 1;
            this.searchButton.Text = "\r\n";
            this.searchButton.UseVisualStyleBackColor = false;
            this.searchButton.Click += new System.EventHandler(this.button1_Click);
            // 
            // resultsTextBox
            // 
            this.resultsTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.resultsTextBox.BackColor = System.Drawing.Color.WhiteSmoke;
            this.resultsTextBox.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.resultsTextBox.Cursor = System.Windows.Forms.Cursors.Default;
            this.resultsTextBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.resultsTextBox.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.resultsTextBox.Location = new System.Drawing.Point(7, 84);
            this.resultsTextBox.Margin = new System.Windows.Forms.Padding(7);
            this.resultsTextBox.Name = "resultsTextBox";
            this.resultsTextBox.ReadOnly = true;
            this.resultsTextBox.Size = new System.Drawing.Size(422, 296);
            this.resultsTextBox.TabIndex = 2;
            this.resultsTextBox.Text = "";
            this.resultsTextBox.MouseClick += new System.Windows.Forms.MouseEventHandler(this.richTextBox1_MouseClick_1);
            this.resultsTextBox.MouseMove += new System.Windows.Forms.MouseEventHandler(this.richTextBox1_MouseMove);
            // 
            // articleTextBox
            // 
            this.articleTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.articleTextBox.BackColor = System.Drawing.Color.WhiteSmoke;
            this.articleTextBox.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.articleTextBox.Font = new System.Drawing.Font("Calibri", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.articleTextBox.ForeColor = System.Drawing.Color.DimGray;
            this.articleTextBox.Location = new System.Drawing.Point(444, 31);
            this.articleTextBox.Name = "articleTextBox";
            this.articleTextBox.ReadOnly = true;
            this.articleTextBox.Size = new System.Drawing.Size(464, 349);
            this.articleTextBox.TabIndex = 3;
            this.articleTextBox.Text = "";
            // 
            // numberResultsLabel
            // 
            this.numberResultsLabel.AutoSize = true;
            this.numberResultsLabel.Location = new System.Drawing.Point(8, 70);
            this.numberResultsLabel.Name = "numberResultsLabel";
            this.numberResultsLabel.Size = new System.Drawing.Size(0, 13);
            this.numberResultsLabel.TabIndex = 5;
            // 
            // menu
            // 
            this.menu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.indexToolStripMenuItem});
            this.menu.Location = new System.Drawing.Point(0, 0);
            this.menu.Name = "menu";
            this.menu.Size = new System.Drawing.Size(916, 24);
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
            this.indexToolStripMenuItem.Name = "indexToolStripMenuItem";
            this.indexToolStripMenuItem.Size = new System.Drawing.Size(47, 20);
            this.indexToolStripMenuItem.Text = "Index";
            // 
            // indexADirectoryToolStripMenuItem
            // 
            this.indexADirectoryToolStripMenuItem.Name = "indexADirectoryToolStripMenuItem";
            this.indexADirectoryToolStripMenuItem.Size = new System.Drawing.Size(173, 22);
            this.indexADirectoryToolStripMenuItem.Text = "Choose a directory";
            this.indexADirectoryToolStripMenuItem.Click += new System.EventHandler(this.indexADirectoryToolStripMenuItem_Click);
            // 
            // statisticsToolStripMenuItem
            // 
            this.statisticsToolStripMenuItem.Name = "statisticsToolStripMenuItem";
            this.statisticsToolStripMenuItem.Size = new System.Drawing.Size(173, 22);
            this.statisticsToolStripMenuItem.Text = "Statistics";
            this.statisticsToolStripMenuItem.Click += new System.EventHandler(this.statisticsToolStripMenuItem_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(170, 6);
            // 
            // quitToolStripMenuItem
            // 
            this.quitToolStripMenuItem.Name = "quitToolStripMenuItem";
            this.quitToolStripMenuItem.Size = new System.Drawing.Size(173, 22);
            this.quitToolStripMenuItem.Text = "Quit";
            // 
            // progressBar
            // 
            this.progressBar.BackColor = System.Drawing.Color.White;
            this.progressBar.ForeColor = System.Drawing.Color.Gold;
            this.progressBar.Location = new System.Drawing.Point(7, 31);
            this.progressBar.Name = "progressBar";
            this.progressBar.Size = new System.Drawing.Size(282, 33);
            this.progressBar.Style = System.Windows.Forms.ProgressBarStyle.Continuous;
            this.progressBar.TabIndex = 10;
            this.progressBar.UseWaitCursor = true;
            this.progressBar.Visible = false;
            // 
            // indexingLabel
            // 
            this.indexingLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.indexingLabel.BackColor = System.Drawing.Color.Transparent;
            this.indexingLabel.Font = new System.Drawing.Font("Segoe Print", 27.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.indexingLabel.ForeColor = System.Drawing.Color.Gold;
            this.indexingLabel.Location = new System.Drawing.Point(7, 31);
            this.indexingLabel.Name = "indexingLabel";
            this.indexingLabel.Size = new System.Drawing.Size(901, 349);
            this.indexingLabel.TabIndex = 11;
            this.indexingLabel.Text = "We are indexing the directory for you :)";
            this.indexingLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
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
            this.boolCBox.Location = new System.Drawing.Point(330, 31);
            this.boolCBox.Name = "boolCBox";
            this.boolCBox.Size = new System.Drawing.Size(51, 34);
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
            this.rankCbox.Size = new System.Drawing.Size(51, 34);
            this.rankCbox.TabIndex = 13;
            this.rankCbox.Text = "Rank";
            this.rankCbox.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.rankCbox.UseVisualStyleBackColor = false;
            this.rankCbox.Click += new System.EventHandler(this.checkBox2_Click);
            // 
            // MainWindow
            // 
            this.AcceptButton = this.searchButton;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.DarkSlateGray;
            this.ClientSize = new System.Drawing.Size(916, 387);
            this.Controls.Add(this.rankCbox);
            this.Controls.Add(this.boolCBox);
            this.Controls.Add(this.progressBar);
            this.Controls.Add(this.numberResultsLabel);
            this.Controls.Add(this.searchButton);
            this.Controls.Add(this.searchTextBox);
            this.Controls.Add(this.menu);
            this.Controls.Add(this.indexingLabel);
            this.Controls.Add(this.resultsTextBox);
            this.Controls.Add(this.articleTextBox);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MainMenuStrip = this.menu;
            this.MinimumSize = new System.Drawing.Size(932, 426);
            this.Name = "MainWindow";
            this.Text = "Cal State LB Search Engine";
            this.TransparencyKey = System.Drawing.Color.Maroon;
            this.menu.ResumeLayout(false);
            this.menu.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox searchTextBox;
        private System.Windows.Forms.Button searchButton;
        private System.Windows.Forms.RichTextBox resultsTextBox;
        private System.Windows.Forms.RichTextBox articleTextBox;
        private System.Windows.Forms.Label numberResultsLabel;
        private System.Windows.Forms.MenuStrip menu;
        private System.Windows.Forms.ToolStripMenuItem indexToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem statisticsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem quitToolStripMenuItem;
        private System.Windows.Forms.Label indexingLabel;
        private System.Windows.Forms.ProgressBar progressBar;
        private System.Windows.Forms.CheckBox boolCBox;
        private System.Windows.Forms.CheckBox rankCbox;
        private System.Windows.Forms.ToolStripMenuItem indexADirectoryToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
    }
}

