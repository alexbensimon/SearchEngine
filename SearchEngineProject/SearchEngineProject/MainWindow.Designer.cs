using System.Windows.Forms;

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
            this.textBoxSearch = new System.Windows.Forms.TextBox();
            this.buttonSearch = new System.Windows.Forms.Button();
            this.textBoxArticle = new System.Windows.Forms.RichTextBox();
            this.labelNumberResults = new System.Windows.Forms.Label();
            this.menu = new System.Windows.Forms.MenuStrip();
            this.toolStripMenuItemIndex = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItemindexADirectory = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItemStatistics = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparatorIndex = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripMenuItemQuit = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItemHelp = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItemAbout = new System.Windows.Forms.ToolStripMenuItem();
            this.progressBar = new System.Windows.Forms.ProgressBar();
            this.checkBoxBool = new System.Windows.Forms.CheckBox();
            this.checkBoxRank = new System.Windows.Forms.CheckBox();
            this.panelResults = new System.Windows.Forms.Panel();
            this.panelQueryPropositions = new System.Windows.Forms.FlowLayoutPanel();
            this.buttonPrevious = new System.Windows.Forms.Button();
            this.labelPage = new System.Windows.Forms.Label();
            this.buttonNext = new System.Windows.Forms.Button();
            this.tableLayoutPanelResults = new System.Windows.Forms.TableLayoutPanel();
            this.labelCorrectedWord = new System.Windows.Forms.Label();
            this.labelIndexing = new System.Windows.Forms.Label();
            this.panelArticle = new System.Windows.Forms.Panel();
            this.panelSearch = new System.Windows.Forms.Panel();
            this.menu.SuspendLayout();
            this.panelResults.SuspendLayout();
            this.panelArticle.SuspendLayout();
            this.panelSearch.SuspendLayout();
            this.SuspendLayout();
            // 
            // textBoxSearch
            // 
            this.textBoxSearch.BackColor = System.Drawing.Color.White;
            this.textBoxSearch.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.textBoxSearch.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textBoxSearch.Enabled = false;
            this.textBoxSearch.Font = new System.Drawing.Font("Segoe Print", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBoxSearch.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.textBoxSearch.Location = new System.Drawing.Point(5, 0);
            this.textBoxSearch.Name = "textBoxSearch";
            this.textBoxSearch.Size = new System.Drawing.Size(263, 34);
            this.textBoxSearch.TabIndex = 0;
            this.textBoxSearch.Click += new System.EventHandler(this.searchTextBox_Click);
            this.textBoxSearch.TextChanged += new System.EventHandler(this.textBox1_TextChanged);
            // 
            // buttonSearch
            // 
            this.buttonSearch.BackColor = System.Drawing.SystemColors.ButtonHighlight;
            this.buttonSearch.BackgroundImage = global::SearchEngineProject.Properties.Resources.SearchIcon;
            this.buttonSearch.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.buttonSearch.FlatAppearance.BorderColor = System.Drawing.Color.White;
            this.buttonSearch.FlatAppearance.BorderSize = 0;
            this.buttonSearch.FlatAppearance.MouseDownBackColor = System.Drawing.Color.White;
            this.buttonSearch.FlatAppearance.MouseOverBackColor = System.Drawing.Color.White;
            this.buttonSearch.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonSearch.Location = new System.Drawing.Point(276, 34);
            this.buttonSearch.Name = "buttonSearch";
            this.buttonSearch.Size = new System.Drawing.Size(33, 33);
            this.buttonSearch.TabIndex = 1;
            this.buttonSearch.Text = "\r\n";
            this.buttonSearch.UseVisualStyleBackColor = false;
            // 
            // textBoxArticle
            // 
            this.textBoxArticle.BackColor = System.Drawing.Color.WhiteSmoke;
            this.textBoxArticle.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.textBoxArticle.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textBoxArticle.Font = new System.Drawing.Font("Calibri", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBoxArticle.ForeColor = System.Drawing.Color.DimGray;
            this.textBoxArticle.Location = new System.Drawing.Point(13, 5);
            this.textBoxArticle.Name = "textBoxArticle";
            this.textBoxArticle.ReadOnly = true;
            this.textBoxArticle.Size = new System.Drawing.Size(456, 338);
            this.textBoxArticle.TabIndex = 3;
            this.textBoxArticle.Text = "";
            // 
            // labelNumberResults
            // 
            this.labelNumberResults.BackColor = System.Drawing.Color.WhiteSmoke;
            this.labelNumberResults.Font = new System.Drawing.Font("Calibri Light", 14.25F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelNumberResults.ForeColor = System.Drawing.Color.Gray;
            this.labelNumberResults.Location = new System.Drawing.Point(307, 2);
            this.labelNumberResults.Name = "labelNumberResults";
            this.labelNumberResults.Size = new System.Drawing.Size(114, 24);
            this.labelNumberResults.TabIndex = 5;
            this.labelNumberResults.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // menu
            // 
            this.menu.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.menu.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.menu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripMenuItemIndex,
            this.toolStripMenuItemHelp});
            this.menu.Location = new System.Drawing.Point(0, 0);
            this.menu.Name = "menu";
            this.menu.Size = new System.Drawing.Size(916, 25);
            this.menu.TabIndex = 7;
            this.menu.Text = "menuStrip1";
            // 
            // toolStripMenuItemIndex
            // 
            this.toolStripMenuItemIndex.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripMenuItemindexADirectory,
            this.toolStripMenuItemStatistics,
            this.toolStripSeparatorIndex,
            this.toolStripMenuItemQuit});
            this.toolStripMenuItemIndex.ForeColor = System.Drawing.Color.Gold;
            this.toolStripMenuItemIndex.Name = "toolStripMenuItemIndex";
            this.toolStripMenuItemIndex.Size = new System.Drawing.Size(51, 21);
            this.toolStripMenuItemIndex.Text = "Index";
            // 
            // toolStripMenuItemindexADirectory
            // 
            this.toolStripMenuItemindexADirectory.Name = "toolStripMenuItemindexADirectory";
            this.toolStripMenuItemindexADirectory.Size = new System.Drawing.Size(187, 22);
            this.toolStripMenuItemindexADirectory.Text = "Choose a directory";
            this.toolStripMenuItemindexADirectory.Click += new System.EventHandler(this.indexADirectoryToolStripMenuItem_Click);
            // 
            // toolStripMenuItemStatistics
            // 
            this.toolStripMenuItemStatistics.Enabled = false;
            this.toolStripMenuItemStatistics.Name = "toolStripMenuItemStatistics";
            this.toolStripMenuItemStatistics.Size = new System.Drawing.Size(187, 22);
            this.toolStripMenuItemStatistics.Text = "Statistics";
            this.toolStripMenuItemStatistics.Click += new System.EventHandler(this.statisticsToolStripMenuItem_Click);
            // 
            // toolStripSeparatorIndex
            // 
            this.toolStripSeparatorIndex.Name = "toolStripSeparatorIndex";
            this.toolStripSeparatorIndex.Size = new System.Drawing.Size(184, 6);
            // 
            // toolStripMenuItemQuit
            // 
            this.toolStripMenuItemQuit.Name = "toolStripMenuItemQuit";
            this.toolStripMenuItemQuit.Size = new System.Drawing.Size(187, 22);
            this.toolStripMenuItemQuit.Text = "Quit";
            this.toolStripMenuItemQuit.Click += new System.EventHandler(this.quitToolStripMenuItem_Click);
            // 
            // toolStripMenuItemHelp
            // 
            this.toolStripMenuItemHelp.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripMenuItemAbout});
            this.toolStripMenuItemHelp.ForeColor = System.Drawing.Color.Gold;
            this.toolStripMenuItemHelp.Name = "toolStripMenuItemHelp";
            this.toolStripMenuItemHelp.Size = new System.Drawing.Size(47, 21);
            this.toolStripMenuItemHelp.Text = "Help";
            // 
            // toolStripMenuItemAbout
            // 
            this.toolStripMenuItemAbout.Name = "toolStripMenuItemAbout";
            this.toolStripMenuItemAbout.Size = new System.Drawing.Size(111, 22);
            this.toolStripMenuItemAbout.Text = "About";
            this.toolStripMenuItemAbout.Click += new System.EventHandler(this.aboutToolStripMenuItem_Click);
            // 
            // progressBar
            // 
            this.progressBar.BackColor = System.Drawing.Color.Black;
            this.progressBar.Cursor = System.Windows.Forms.Cursors.WaitCursor;
            this.progressBar.ForeColor = System.Drawing.Color.Gold;
            this.progressBar.Location = new System.Drawing.Point(79, 206);
            this.progressBar.Name = "progressBar";
            this.progressBar.Size = new System.Drawing.Size(753, 10);
            this.progressBar.Style = System.Windows.Forms.ProgressBarStyle.Continuous;
            this.progressBar.TabIndex = 10;
            this.progressBar.UseWaitCursor = true;
            this.progressBar.Visible = false;
            // 
            // checkBoxBool
            // 
            this.checkBoxBool.Appearance = System.Windows.Forms.Appearance.Button;
            this.checkBoxBool.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.checkBoxBool.CheckAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.checkBoxBool.Checked = true;
            this.checkBoxBool.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBoxBool.Enabled = false;
            this.checkBoxBool.FlatAppearance.BorderColor = System.Drawing.Color.Black;
            this.checkBoxBool.FlatAppearance.CheckedBackColor = System.Drawing.Color.Gold;
            this.checkBoxBool.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Gold;
            this.checkBoxBool.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Gold;
            this.checkBoxBool.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.checkBoxBool.Font = new System.Drawing.Font("Segoe Print", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.checkBoxBool.Location = new System.Drawing.Point(328, 31);
            this.checkBoxBool.Name = "checkBoxBool";
            this.checkBoxBool.Size = new System.Drawing.Size(51, 37);
            this.checkBoxBool.TabIndex = 12;
            this.checkBoxBool.Text = "Bool";
            this.checkBoxBool.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.checkBoxBool.UseVisualStyleBackColor = false;
            this.checkBoxBool.Click += new System.EventHandler(this.checkBox1_Click);
            // 
            // checkBoxRank
            // 
            this.checkBoxRank.Appearance = System.Windows.Forms.Appearance.Button;
            this.checkBoxRank.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.checkBoxRank.CheckAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.checkBoxRank.Enabled = false;
            this.checkBoxRank.FlatAppearance.BorderColor = System.Drawing.Color.Black;
            this.checkBoxRank.FlatAppearance.CheckedBackColor = System.Drawing.Color.Gold;
            this.checkBoxRank.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.checkBoxRank.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.checkBoxRank.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.checkBoxRank.Font = new System.Drawing.Font("Segoe Print", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.checkBoxRank.ForeColor = System.Drawing.Color.Gold;
            this.checkBoxRank.Location = new System.Drawing.Point(378, 31);
            this.checkBoxRank.Name = "checkBoxRank";
            this.checkBoxRank.Size = new System.Drawing.Size(51, 37);
            this.checkBoxRank.TabIndex = 13;
            this.checkBoxRank.Text = "Rank";
            this.checkBoxRank.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.checkBoxRank.UseVisualStyleBackColor = false;
            this.checkBoxRank.Click += new System.EventHandler(this.checkBox2_Click);
            // 
            // panelResults
            // 
            this.panelResults.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.panelResults.BackColor = System.Drawing.Color.WhiteSmoke;
            this.panelResults.Controls.Add(this.panelQueryPropositions);
            this.panelResults.Controls.Add(this.buttonPrevious);
            this.panelResults.Controls.Add(this.labelPage);
            this.panelResults.Controls.Add(this.buttonNext);
            this.panelResults.Controls.Add(this.tableLayoutPanelResults);
            this.panelResults.Controls.Add(this.labelCorrectedWord);
            this.panelResults.Controls.Add(this.labelNumberResults);
            this.panelResults.Location = new System.Drawing.Point(8, 80);
            this.panelResults.Name = "panelResults";
            this.panelResults.Padding = new System.Windows.Forms.Padding(10, 25, 10, 40);
            this.panelResults.Size = new System.Drawing.Size(421, 299);
            this.panelResults.TabIndex = 14;
            // 
            // panelQueryPropositions
            // 
            this.panelQueryPropositions.Location = new System.Drawing.Point(0, 2);
            this.panelQueryPropositions.Name = "panelQueryPropositions";
            this.panelQueryPropositions.Size = new System.Drawing.Size(301, 53);
            this.panelQueryPropositions.TabIndex = 17;
            // 
            // buttonPrevious
            // 
            this.buttonPrevious.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.buttonPrevious.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.buttonPrevious.Enabled = false;
            this.buttonPrevious.FlatAppearance.BorderSize = 0;
            this.buttonPrevious.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonPrevious.Font = new System.Drawing.Font("Segoe Print", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.buttonPrevious.ForeColor = System.Drawing.Color.Gold;
            this.buttonPrevious.Location = new System.Drawing.Point(3, 263);
            this.buttonPrevious.Name = "buttonPrevious";
            this.buttonPrevious.Size = new System.Drawing.Size(75, 31);
            this.buttonPrevious.TabIndex = 7;
            this.buttonPrevious.Text = "Previous";
            this.buttonPrevious.UseVisualStyleBackColor = false;
            this.buttonPrevious.Visible = false;
            this.buttonPrevious.EnabledChanged += new System.EventHandler(this.previousButton_EnabledChanged);
            this.buttonPrevious.Click += new System.EventHandler(this.previousButton_Click);
            // 
            // labelPage
            // 
            this.labelPage.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.labelPage.AutoSize = true;
            this.labelPage.Font = new System.Drawing.Font("Segoe Print", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelPage.ForeColor = System.Drawing.Color.Black;
            this.labelPage.Location = new System.Drawing.Point(188, 268);
            this.labelPage.Name = "labelPage";
            this.labelPage.Size = new System.Drawing.Size(0, 23);
            this.labelPage.TabIndex = 8;
            // 
            // buttonNext
            // 
            this.buttonNext.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.buttonNext.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.buttonNext.Enabled = false;
            this.buttonNext.FlatAppearance.BorderSize = 0;
            this.buttonNext.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonNext.Font = new System.Drawing.Font("Segoe Print", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.buttonNext.ForeColor = System.Drawing.Color.Gold;
            this.buttonNext.Location = new System.Drawing.Point(344, 263);
            this.buttonNext.Name = "buttonNext";
            this.buttonNext.Size = new System.Drawing.Size(74, 31);
            this.buttonNext.TabIndex = 6;
            this.buttonNext.Text = "Next";
            this.buttonNext.UseVisualStyleBackColor = false;
            this.buttonNext.Visible = false;
            this.buttonNext.EnabledChanged += new System.EventHandler(this.nextButton_EnabledChanged);
            this.buttonNext.Click += new System.EventHandler(this.nextButton_Click);
            // 
            // tableLayoutPanelResults
            // 
            this.tableLayoutPanelResults.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.tableLayoutPanelResults.AutoScroll = true;
            this.tableLayoutPanelResults.ColumnCount = 2;
            this.tableLayoutPanelResults.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 49.87531F));
            this.tableLayoutPanelResults.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50.12469F));
            this.tableLayoutPanelResults.Location = new System.Drawing.Point(4, 61);
            this.tableLayoutPanelResults.Name = "tableLayoutPanelResults";
            this.tableLayoutPanelResults.RowCount = 1;
            this.tableLayoutPanelResults.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanelResults.Size = new System.Drawing.Size(414, 198);
            this.tableLayoutPanelResults.TabIndex = 4;
            // 
            // labelCorrectedWord
            // 
            this.labelCorrectedWord.AutoSize = true;
            this.labelCorrectedWord.Cursor = System.Windows.Forms.Cursors.Hand;
            this.labelCorrectedWord.Font = new System.Drawing.Font("Calibri", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelCorrectedWord.ForeColor = System.Drawing.Color.Firebrick;
            this.labelCorrectedWord.Location = new System.Drawing.Point(4, 4);
            this.labelCorrectedWord.Name = "labelCorrectedWord";
            this.labelCorrectedWord.Size = new System.Drawing.Size(0, 23);
            this.labelCorrectedWord.TabIndex = 0;
            this.labelCorrectedWord.MouseClick += new System.Windows.Forms.MouseEventHandler(this.correctedWordLabel_MouseClick);
            this.labelCorrectedWord.MouseEnter += new System.EventHandler(this.correctedWordLabel_MouseEnter);
            this.labelCorrectedWord.MouseLeave += new System.EventHandler(this.correctedWordLabel_MouseLeave);
            // 
            // labelIndexing
            // 
            this.labelIndexing.BackColor = System.Drawing.Color.Transparent;
            this.labelIndexing.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelIndexing.Font = new System.Drawing.Font("Segoe Print", 27.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelIndexing.ForeColor = System.Drawing.Color.Gold;
            this.labelIndexing.Location = new System.Drawing.Point(0, 0);
            this.labelIndexing.Name = "labelIndexing";
            this.labelIndexing.Size = new System.Drawing.Size(916, 386);
            this.labelIndexing.TabIndex = 11;
            this.labelIndexing.Text = "We are indexing the directory for you :)\r\n20%";
            this.labelIndexing.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // panelArticle
            // 
            this.panelArticle.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panelArticle.BackColor = System.Drawing.Color.WhiteSmoke;
            this.panelArticle.Controls.Add(this.textBoxArticle);
            this.panelArticle.Location = new System.Drawing.Point(439, 31);
            this.panelArticle.Name = "panelArticle";
            this.panelArticle.Padding = new System.Windows.Forms.Padding(13, 5, 0, 5);
            this.panelArticle.Size = new System.Drawing.Size(469, 348);
            this.panelArticle.TabIndex = 15;
            // 
            // panelSearch
            // 
            this.panelSearch.BackColor = System.Drawing.Color.White;
            this.panelSearch.Controls.Add(this.textBoxSearch);
            this.panelSearch.Location = new System.Drawing.Point(8, 34);
            this.panelSearch.Name = "panelSearch";
            this.panelSearch.Padding = new System.Windows.Forms.Padding(5, 0, 0, 0);
            this.panelSearch.Size = new System.Drawing.Size(268, 33);
            this.panelSearch.TabIndex = 15;
            // 
            // MainWindow
            // 
            this.AcceptButton = this.buttonSearch;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Black;
            this.ClientSize = new System.Drawing.Size(916, 386);
            this.Controls.Add(this.panelResults);
            this.Controls.Add(this.progressBar);
            this.Controls.Add(this.checkBoxRank);
            this.Controls.Add(this.checkBoxBool);
            this.Controls.Add(this.buttonSearch);
            this.Controls.Add(this.menu);
            this.Controls.Add(this.panelSearch);
            this.Controls.Add(this.panelArticle);
            this.Controls.Add(this.labelIndexing);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MainMenuStrip = this.menu;
            this.MinimumSize = new System.Drawing.Size(932, 425);
            this.Name = "MainWindow";
            this.Text = "Cal State LB Search Engine";
            this.TransparencyKey = System.Drawing.Color.Maroon;
            this.ResizeEnd += new System.EventHandler(this.MainWindow_ResizeEnd);
            this.SizeChanged += new System.EventHandler(this.MainWindow_SizeChanged);
            this.menu.ResumeLayout(false);
            this.menu.PerformLayout();
            this.panelResults.ResumeLayout(false);
            this.panelResults.PerformLayout();
            this.panelArticle.ResumeLayout(false);
            this.panelSearch.ResumeLayout(false);
            this.panelSearch.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox textBoxSearch;
        private System.Windows.Forms.Button buttonSearch;
        private System.Windows.Forms.RichTextBox textBoxArticle;
        private System.Windows.Forms.Label labelNumberResults;
        private System.Windows.Forms.MenuStrip menu;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItemIndex;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItemStatistics;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItemQuit;
        private System.Windows.Forms.ProgressBar progressBar;
        private System.Windows.Forms.CheckBox checkBoxBool;
        private System.Windows.Forms.CheckBox checkBoxRank;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItemindexADirectory;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparatorIndex;
        private System.Windows.Forms.Panel panelResults;
        private System.Windows.Forms.Label labelCorrectedWord;
        private System.Windows.Forms.Panel panelArticle;
        private System.Windows.Forms.Panel panelSearch;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItemHelp;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItemAbout;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanelResults;
        private System.Windows.Forms.Button buttonPrevious;
        private System.Windows.Forms.Button buttonNext;
        private System.Windows.Forms.Label labelPage;
        private Label labelIndexing;
        private FlowLayoutPanel panelQueryPropositions;
    }
}

