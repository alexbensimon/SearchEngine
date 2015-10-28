using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using SearchEngineProject.Properties;

namespace SearchEngineProject
{
    public partial class MainWindow : Form
    {
        private string _currentWordUnderCursor;
        private DiskPositionalIndex _index;

        public MainWindow()
        {
            InitializeComponent();
            indexingLabel.Hide();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            DisplaySearchResults();
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            DisplaySearchResults();
        }

        private void DisplaySearchResults()
        {
            resultsTextBox.Clear();
            numberResultsLabel.Text = string.Empty;
            numberResultsLabel.ForeColor = SystemColors.HighlightText;
            var query = searchTextBox.Text.ToLower();

            var resultsDocIds = SimpleEngine.ProcessQuery(query, _index);

            if (resultsDocIds == null)
                resultsTextBox.Text = "Wrong syntax";
            else if (resultsDocIds.Count == 0)
                resultsTextBox.Text = "No results";
            else
            {
                // Display the number of returned documents.
                numberResultsLabel.Text = "Results: " + resultsDocIds.Count + " documents";

                // Build the results.
                var finalResults = new StringBuilder();
                foreach (int docId in resultsDocIds)
                {
                    finalResults.Append(_index.FileNames[docId]);
                    finalResults.AppendLine();
                    finalResults.AppendLine();
                }
                resultsTextBox.Text = finalResults.ToString();
            }
        }

        private string prettyBytes(long numberOfBytes)
        {
            var counter = 0;
            var unit = new string[] { "B", "KB", "MB", "GB" };
            while (numberOfBytes > 1024)
            {
                numberOfBytes /= 1024;
                counter++;
            }
            return numberOfBytes + unit[counter];
        }

        private void richTextBox1_MouseClick_1(object sender, MouseEventArgs e)
        {
            var control = sender as RichTextBox;
            // Get the word under the cursor.
            var word = GetWordUnderCursor(control, e);
            if (word != null && word.EndsWith(".txt"))
            {
                resultsTextBox.Select(resultsTextBox.Text.IndexOf(word), word.Length);
                resultsTextBox.SelectionColor = Color.Gold;
                articleTextBox.Text = File.ReadAllText("Corpus/" + word);
            }
        }

        public static string GetWordUnderCursor(RichTextBox control, MouseEventArgs e)
        {
            // Check if there is any text entered.
            if (string.IsNullOrWhiteSpace(control.Text))
                return null;
            // Get index of nearest character.
            var index = control.GetCharIndexFromPosition(e.Location);
            // Check if mouse is above a word (non-whitespace character).
            if (char.IsWhiteSpace(control.Text[index]))
                return null;
            // Find the start index of the word.
            var start = index;
            while (start > 0 && !char.IsWhiteSpace(control.Text[start - 1]))
                start--;
            // Find the end index of the word.
            var end = index;
            while (end < control.Text.Length - 1 && !char.IsWhiteSpace(control.Text[end + 1]))
                end++;
            // Get and return the whole word.
            return control.Text.Substring(start, end - start + 1);
        }

        private void richTextBox1_MouseMove(object sender, MouseEventArgs e)
        {
            var control = sender as RichTextBox;
            // Get the word under the cursor.
            var word = GetWordUnderCursor(control, e);
            if (word != null)
                resultsTextBox.Cursor = Cursors.Hand;
            else
                resultsTextBox.Cursor = Cursors.Default;
            if (word != _currentWordUnderCursor)
            {
                if (_currentWordUnderCursor != null)
                {
                    resultsTextBox.Select(resultsTextBox.Text.IndexOf(_currentWordUnderCursor), _currentWordUnderCursor.Length);
                    resultsTextBox.SelectionColor = Color.Black;
                    resultsTextBox.SelectionFont = new Font(resultsTextBox.SelectionFont, FontStyle.Regular);
                    _currentWordUnderCursor = null;
                }
                if (word != null && word.EndsWith(".txt"))
                {
                    resultsTextBox.Select(resultsTextBox.Text.IndexOf(word), word.Length);
                    resultsTextBox.SelectionColor = Color.Gold;
                    resultsTextBox.SelectionFont = new Font(resultsTextBox.SelectionFont, FontStyle.Underline);
                    _currentWordUnderCursor = word;
                }
            }
        }

        private void statisticsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var statistics = new StringBuilder();
            statistics.Append("Number of terms in the index: ");
            statistics.AppendLine(IndexWriter.IndexSize.ToString() + " terms\n");

            statistics.Append("Average number of documents in the postings list: ");
            statistics.AppendLine(IndexWriter.AvgNumberDocsInPostingsList.ToString() + " documents\n");

            statistics.AppendLine("Proportion of documents that contain each of the 10 most frequent terms:");
            foreach (var pair in IndexWriter.ProportionDocContaining10MostFrequent)
            {
                statistics.Append(pair.Key + ": " + Math.Round(pair.Value, 2) * 100 + "%; ");
            }
            statistics.AppendLine("\n");

            statistics.Append("Approximate memory requirement of the index: ");
            statistics.Append(prettyBytes(IndexWriter.IndexSizeInMemory));

            MessageBox.Show(statistics.ToString(), Resources.StatMessageBoxTitle);
        }

        private void indexADirectoryToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var fbd = new FolderBrowserDialog
            {
                ShowNewFolderButton = false,
                Description = "Choose the directory you want to index"
            };
            fbd.ShowDialog();
            var directoryPath = fbd.SelectedPath;

            if (string.IsNullOrEmpty(directoryPath)) return;

            indexingLabel.Show();
            Update();
            var writer = new IndexWriter(directoryPath);
            writer.BuildIndex(this);

            //Write the KGram Index to the disk
            KGramIndex.ToDisk(directoryPath);
            
            _index = new DiskPositionalIndex(directoryPath);

            //Load the KGram index in memory
            KGramIndex.ToMemory(directoryPath);

            indexingLabel.Hide();
            searchTextBox.Enabled = true;
            searchTextBox.Select();
            searchTextBox.Text = "Search for whatever you want ^^";
            searchTextBox.SelectionStart = 0;
            searchTextBox.SelectionLength = searchTextBox.Text.Length;
        }

        public void InitiateprogressBar(string directory)
        {
            int counter = Directory.EnumerateFiles(directory).Count();
            foreach (var subDirectory in Directory.EnumerateDirectories(directory))
            {
                counter += Directory.EnumerateFiles(subDirectory).Count();
            }

            //Initiate the progress bar
            // Display the ProgressBar control.
            progressBar.Visible = true;
            // Set Minimum to 1 to represent the first file being copied.
            progressBar.Minimum = 1;
            // Set Maximum to the total number of files to copy.
            progressBar.Maximum = counter;
            // Set the initial value of the ProgressBar.
            progressBar.Value = 1;
            // Set the Step property to a value of 1 to represent each file being copied.
            progressBar.Step = 1;
        }

        public void IncrementProgressBar()
        {
            progressBar.PerformStep();
        }

        public void HideProgressBar()
        {
            var t = new System.Windows.Forms.Timer { Interval = 3000 };
            indexingLabel.Show();
            
            t.Tick += (s, e) =>
            {
                indexingLabel.Hide();
                t.Stop();
            };
            t.Start();
            indexingLabel.Hide();
            progressBar.Hide();
        }

        private void checkBox1_Click(object sender, EventArgs e)
        {
            boolCBox.CheckState=CheckState.Checked;
            boolCBox.ForeColor = Color.Black;
            boolCBox.FlatAppearance.MouseOverBackColor = Color.Gold;
            boolCBox.FlatAppearance.MouseDownBackColor = Color.Gold;

            rankCbox.CheckState = CheckState.Unchecked;
            rankCbox.ForeColor = Color.Gold;
            rankCbox.FlatAppearance.MouseOverBackColor = Color.FromArgb(64,64,64);
            rankCbox.FlatAppearance.MouseDownBackColor = Color.FromArgb(64, 64, 64);
        }

        private void checkBox2_Click(object sender, EventArgs e)
        {
            rankCbox.CheckState = CheckState.Checked;
            rankCbox.ForeColor = Color.Black;
            rankCbox.FlatAppearance.MouseOverBackColor = Color.Gold;
            rankCbox.FlatAppearance.MouseDownBackColor = Color.Gold;

            boolCBox.CheckState = CheckState.Unchecked;
            boolCBox.ForeColor = Color.Gold;
            boolCBox.FlatAppearance.MouseOverBackColor = Color.FromArgb(64, 64, 64);
            boolCBox.FlatAppearance.MouseDownBackColor = Color.FromArgb(64, 64, 64);
        }

    }
}