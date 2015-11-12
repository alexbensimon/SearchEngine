using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.Remoting.Channels;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Windows.Forms;
using SearchEngineProject.Properties;

namespace SearchEngineProject
{
    public partial class MainWindow : Form
    {
        private DiskPositionalIndex _index;
        private List<string> _finalResults;
        private int _numberOfResultsByPage = 14;
        private int _currentPage = 1;
        private int _numberOfPages;
        private FormWindowState _formerWindowsState = FormWindowState.Normal;
        private string _directoryPath;

        public MainWindow()
        {
            InitializeComponent();
            indexingLabel.Hide();
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            if (boolCBox.Checked)
                DisplayBooleanSearchResults();
            else if (rankCbox.Checked)
                DisplayRankSearchResults();
        }

        private void DisplayRankSearchResults()
        {

        }

        private void DisplayBooleanSearchResults()
        {
            tableLayoutPanel1.Controls.Clear();
            SimpleEngine.FoundTerms.Clear();
            numberResultsLabel.Text = string.Empty;
            var query = searchTextBox.Text.ToLower();

            var resultsDocIds = SimpleEngine.ProcessQuery(query, _index);

            if (resultsDocIds == null)
                tableLayoutPanel1.Controls.Add(new Label
                {
                    Text = "Wrong syntax",
                    AutoSize = true,
                    Font = new Font("Segoe Print", (float)14.25)
                });
            else if (resultsDocIds.Count == 0)
                tableLayoutPanel1.Controls.Add(new Label
                {
                    Text = "No results",
                    AutoSize = true,
                    Font = new Font("Segoe Print", (float)14.25)
                });
            else
            {
                // Display the number of returned documents.
                numberResultsLabel.Text = resultsDocIds.Count + " results";

                // Build the results.
                _finalResults = new List<string>();
                foreach (int docId in resultsDocIds)
                {
                    _finalResults.Add(_index.FileNames[docId]);
                }

                UpdateDisplayResults(_currentPage);
                previousButton.Visible = true;
                nextButton.Visible = true;
            }

            //Display potential correction of search terms if needed
            if (SimpleEngine.PotentialMisspelledWords.Any())
            {
                string correctedQuery = searchTextBox.Text;
                bool correctionFound = false;

                foreach (var potentialMisspelledWord in SimpleEngine.PotentialMisspelledWords)
                {
                    var correctedWords = KGramIndex.getCorrectedWord(potentialMisspelledWord);

                    if (correctedWords.Count > 1)
                    {
                        int maxDocumentFreq = 0;
                        string correctedWord = null;
                        foreach (var word in correctedWords)
                        {
                            var termDocFreq = _index.GetPostings(PorterStemmer.ProcessToken(word), false).Count();
                            if (termDocFreq > maxDocumentFreq)
                            {
                                maxDocumentFreq = termDocFreq;
                                correctedWord = word;
                            }
                        }
                        correctedQuery = correctedQuery.Replace(potentialMisspelledWord, correctedWord);
                        correctionFound = true;
                    }
                    else if (correctedWords.Count == 1)
                    {
                        correctedQuery = correctedQuery.Replace(potentialMisspelledWord, correctedWords.First());
                        correctionFound = true;
                    }
                }

                if (!correctionFound) return;

                correctedWordLabel.Show();
                correctedWordLabel.Text = "Did you mean: " + correctedQuery + "?";
            }
            else
            {
                correctedWordLabel.Hide();
            }
        }

        private void UpdatePageNumber()
        {
            int numberOfResults = int.Parse(numberResultsLabel.Text.Remove(numberResultsLabel.Text.Length - 8));
            _numberOfPages = (int)Math.Ceiling((double)numberOfResults / _numberOfResultsByPage);

            while (_currentPage > _numberOfPages) _currentPage--;

            pageLabel.Text = _currentPage + "/" + _numberOfPages;
        }

        private void AddNewLabel(string text)
        {
            var fileNameLabel = new Label
            {
                Text = text,
                AutoSize = true,
                Font = new Font("Segoe Print", (float)14.25)
            };
            fileNameLabel.Click += FileNameLabel_Click;
            fileNameLabel.MouseEnter += FileNameLabel_MouseEnter;
            fileNameLabel.MouseLeave += FileNameLabel_MouseLeave;
            tableLayoutPanel1.Controls.Add(fileNameLabel);
        }

        private void UpdateDisplayResults(int pageToDisplay)
        {
            tableLayoutPanel1.Controls.Clear();

            for (int i = (pageToDisplay * _numberOfResultsByPage) - _numberOfResultsByPage; i < pageToDisplay * _numberOfResultsByPage; i++)
            {
                if (_finalResults.Count <= i) break;
                AddNewLabel(_finalResults.ElementAt(i));
            }

            UpdatePageNumber();

            previousButton.Enabled = _currentPage != 1;

            nextButton.Enabled = _currentPage != _numberOfPages;
        }

        private void FileNameLabel_Click(object sender, EventArgs e)
        {
            var label = sender as Label;

            if (label != null)
            {
                foreach (Label tempLabel in tableLayoutPanel1.Controls)
                {
                    tempLabel.ForeColor = Color.Black;
                }
                label.ForeColor = Color.Gold;

                articleTextBox.Text = File.ReadAllText(_directoryPath + "/" + label.Text);

                //Highlight the search terms
                HighlightText();
            }

        }

        public void HighlightText()
        {

            int sStart = articleTextBox.SelectionStart, startIndex = 0;

            foreach (var articleWord in articleTextBox.Text.Split(new char[]{' ', '-'}))
            {
                var cleanWord = Regex.Replace(articleWord, @"[^-\w\s]*", "").ToLower();
                if (SimpleEngine.FoundTerms.Contains(PorterStemmer.ProcessToken(cleanWord)))
                {
                    var index = articleTextBox.Text.IndexOf(articleWord, startIndex, StringComparison.Ordinal);
                    articleTextBox.Select(index, articleWord.Length);
                    articleTextBox.SelectionColor=Color.Gold;
                    articleTextBox.SelectionBackColor=Color.Black;
                }
                startIndex += articleWord.Length+1;
            }

            articleTextBox.SelectionStart = sStart;
            articleTextBox.SelectionLength = 0;
        }

        private void FileNameLabel_MouseEnter(object sender, EventArgs e)
        {
            var label = sender as Label;

            if (label != null)
            {
                label.Cursor = Cursors.Hand;
                label.Font = new Font(label.Font.Name, label.Font.SizeInPoints, FontStyle.Underline);
            }
        }

        private void FileNameLabel_MouseLeave(object sender, EventArgs e)
        {
            var label = sender as Label;

            if (label != null)
            {
                label.Cursor = Cursors.Default;
                label.Font = new Font(label.Font.Name, label.Font.SizeInPoints, FontStyle.Regular);
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

        private void statisticsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var statistics = new StringBuilder();
            statistics.Append("Number of terms in the index: ");
            statistics.AppendLine(_index.IndexSize.ToString() + " terms\n");

            statistics.Append("Average number of documents in the postings list: ");
            statistics.AppendLine(_index.AvgNumberDocsInPostingsList.ToString() + " documents\n");

            statistics.AppendLine("Proportion of documents that contain each of the 10 most frequent terms:");
            foreach (var pair in _index.ProportionDocContaining10MostFrequent)
            {
                statistics.Append(pair.Key + ": " + Math.Round(pair.Value, 2) * 100 + "%; ");
            }
            statistics.AppendLine("\n");

            statistics.Append("Approximate memory requirement of the index: ");
            statistics.Append(prettyBytes(_index.IndexSizeInMemory));

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
            _directoryPath = fbd.SelectedPath;

            if (string.IsNullOrEmpty(_directoryPath)) return;

            var filenames = Directory.GetFiles(_directoryPath, "*.bin")
                                     .Select(Path.GetFileNameWithoutExtension)
                                     .ToArray();

            DialogResult result = DialogResult.No;
            if (filenames.Contains("kGramIndex") && filenames.Contains("kGramVocab") && filenames.Contains("kGram") && filenames.Contains("vocab") && filenames.Contains("vocabTable") && filenames.Contains("postings") && filenames.Contains("statistics") && filenames.Contains("mostFreqWord"))
                result = MessageBox.Show("This directory is already indexed, let's skip the long indexation! :)", "Directory already indexed", MessageBoxButtons.YesNo);

            if (result == DialogResult.No)
            {
                if (_index != null)
                    _index.Dispose();
                numberResultsLabel.Hide();
                indexingLabel.Show();
                Update();
                var writer = new IndexWriter(_directoryPath);
                writer.BuildIndex(this);

                //Write the KGram Index to the disk
                KGramIndex.ToDisk(_directoryPath);
            }

            //Load the Disk positional index into memory
            _index = new DiskPositionalIndex(_directoryPath);

            //Load the KGram index in memory
            KGramIndex.ToMemory(_directoryPath);

            statisticsToolStripMenuItem.Enabled = true;
            indexingLabel.Hide();
            searchTextBox.Enabled = true;
            searchTextBox.Select();
            searchTextBox.Text = "Indexing done ^^";
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
            boolCBox.CheckState = CheckState.Checked;
            boolCBox.ForeColor = Color.Black;
            boolCBox.FlatAppearance.MouseOverBackColor = Color.Gold;
            boolCBox.FlatAppearance.MouseDownBackColor = Color.Gold;

            rankCbox.CheckState = CheckState.Unchecked;
            rankCbox.ForeColor = Color.Gold;
            rankCbox.FlatAppearance.MouseOverBackColor = Color.FromArgb(64, 64, 64);
            rankCbox.FlatAppearance.MouseDownBackColor = Color.FromArgb(64, 64, 64);

            DisplayBooleanSearchResults();
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

            DisplayRankSearchResults();
        }

        private void quitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Dispose();
            Close();
        }

        private void correctedWordLabel_MouseClick(object sender, MouseEventArgs e)
        {
            searchTextBox.Text = correctedWordLabel.Text.Replace("Did you mean: ", "").Replace("?", "");
        }

        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Class: CECS 529\n" +
                "Project: Milestone 2\n" +
                "Authors: Alexandre Bensimon and Vincent Gagneux\n\n" +
                "Category A options: Wildcard queries and spelling correction\n" +
                "Category B options: Syntax checking, GUI, Statistics, K-gram index on disk\n" +
                "Category C options: NOT queries", "About");
        }

        private void correctedWordLabel_MouseEnter(object sender, EventArgs e)
        {
            correctedWordLabel.Font = new Font(correctedWordLabel.Font, FontStyle.Underline);
        }

        private void correctedWordLabel_MouseLeave(object sender, EventArgs e)
        {
            correctedWordLabel.Font = new Font(correctedWordLabel.Font, FontStyle.Regular);
        }

        private void searchTextBox_Click(object sender, EventArgs e)
        {
            if (searchTextBox.Text == "Indexing done ^^")
                searchTextBox.Clear();
        }

        private void nextButton_Click(object sender, EventArgs e)
        {
            _currentPage++;
            UpdateDisplayResults(_currentPage);
        }

        private void previousButton_Click(object sender, EventArgs e)
        {
            _currentPage--;
            UpdateDisplayResults(_currentPage);
        }

        private void MainWindow_ResizeEnd(object sender, EventArgs e)
        {
            if (previousButton.Visible)
                UpdateNumberOfResultsDisplayed();
        }

        private void UpdateNumberOfResultsDisplayed()
        {
            int tmp = Size.Height;
            _numberOfResultsByPage = 14;
            tableLayoutPanel1.RowCount = 0;

            while (tmp > MinimumSize.Height + 32)
            {
                _numberOfResultsByPage += 2;
                tmp -= 32;
            }

            UpdatePageNumber();
            UpdateDisplayResults(_currentPage);
        }

        private void MainWindow_SizeChanged(object sender, EventArgs e)
        {
            if (WindowState != _formerWindowsState && previousButton.Visible)
            {
                _formerWindowsState = WindowState;
                UpdateNumberOfResultsDisplayed();
            }
        }

        private void nextButton_EnabledChanged(object sender, EventArgs e)
        {
            if (nextButton.Enabled)
            {
                nextButton.BackColor = Color.Gold;
                nextButton.ForeColor = Color.Black;
            }
            else
            {
                nextButton.BackColor = Color.FromArgb(64, 64, 64);
                nextButton.ForeColor = Color.Gold;
            }
        }

        private void previousButton_EnabledChanged(object sender, EventArgs e)
        {
            if (previousButton.Enabled)
            {
                previousButton.BackColor = Color.Gold;
                previousButton.ForeColor = Color.Black;
            }
            else
            {
                previousButton.BackColor = Color.FromArgb(64, 64, 64);
                previousButton.ForeColor = Color.Gold;
            }
        }
    }
}
