using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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
            labelIndexing.Hide();
        }

        #region Menu

        #region Index menu item

        private void quitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Dispose();
            Close();
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
                labelNumberResults.Hide();
                labelIndexing.Show();
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

            toolStripMenuItemStatistics.Enabled = true;
            labelIndexing.Hide();
            textBoxSearch.Enabled = true;
            textBoxSearch.Select();
            textBoxSearch.Text = "Indexing done ^^";
            textBoxSearch.SelectionStart = 0;
            textBoxSearch.SelectionLength = textBoxSearch.Text.Length;

            checkBoxBool.Enabled = true;
            checkBoxRank.Enabled = true;
        }

        #endregion

        #region Index menu help

        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Class: CECS 529\n" +
                "Project: Milestone 2\n" +
                "Authors: Alexandre Bensimon and Vincent Gagneux\n\n" +
                "Category A options: Wildcard queries and spelling correction\n" +
                "Category B options: Syntax checking, GUI, Statistics, K-gram index on disk\n" +
                "Category C options: NOT queries", "About");
        }

        #endregion

        #endregion

        #region Search textBox

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            if (checkBoxBool.Checked)
                DisplayBooleanSearchResults();
            else if (checkBoxRank.Checked)
                DisplayRankSearchResults();
        }

        private void searchTextBox_Click(object sender, EventArgs e)
        {
            if (textBoxSearch.Text == "Indexing done ^^")
                textBoxSearch.Clear();
        }

        #endregion

        #region Retrieval modes

        private void DisplayRankSearchResults()
        {
            tableLayoutPanelResults.Controls.Clear();
            labelNumberResults.Text = string.Empty;
            var query = textBoxSearch.Text.ToLower();
            var results = SimpleEngine.ProcessRankQuery(query, _index, _directoryPath);
            if (!results.Any() || results != null)
                tableLayoutPanelResults.Controls.Add(new Label
                {
                    Text = "No results",
                    AutoSize = true,
                    Font = new Font("Segoe Print", (float)14.25)
                });
            else
            {
                int numberOfResults;
                if (results.Count() < 10) numberOfResults = results.Count() * 2;
                else numberOfResults = 20;

                labelNumberResults.Text = numberOfResults + " results";

                _finalResults = new List<string>();
                for (int i = 0; i < numberOfResults / 2; i++)
                {
                    _finalResults.Add(_index.FileNames[results.ElementAt(i).Value]);
                    _finalResults.Add(results.ElementAt(i).Key.ToString());
                }

                UpdateDisplayResults(_currentPage);
                buttonPrevious.Visible = true;
                buttonNext.Visible = true;
            }
        }

        private void DisplayBooleanSearchResults()
        {
            tableLayoutPanelResults.Controls.Clear();
            SimpleEngine.FoundTerms.Clear();
            labelNumberResults.Text = string.Empty;
            var query = textBoxSearch.Text.ToLower();

            var resultsDocIds = SimpleEngine.ProcessQuery(query, _index);

            if (resultsDocIds == null)
                tableLayoutPanelResults.Controls.Add(new Label
                {
                    Text = "Wrong syntax",
                    AutoSize = true,
                    Font = new Font("Segoe Print", (float)14.25)
                });
            else if (resultsDocIds.Count == 0)
                tableLayoutPanelResults.Controls.Add(new Label
                {
                    Text = "No results",
                    AutoSize = true,
                    Font = new Font("Segoe Print", (float)14.25)
                });
            else
            {
                // Display the number of returned documents.
                labelNumberResults.Text = resultsDocIds.Count + " results";

                // Build the results.
                _finalResults = new List<string>();
                foreach (int docId in resultsDocIds)
                {
                    _finalResults.Add(_index.FileNames[docId]);
                }

                UpdateDisplayResults(_currentPage);
                buttonPrevious.Visible = true;
                buttonNext.Visible = true;
            }

            //Display potential correction of search terms if needed
            if (SimpleEngine.PotentialMisspelledWords.Any())
            {
                string correctedQuery = textBoxSearch.Text;
                bool correctionFound = false;

                foreach (var potentialMisspelledWord in SimpleEngine.PotentialMisspelledWords)
                {
                    var correctedWords = KGramIndex.GetCorrectedWord(potentialMisspelledWord);

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

                labelCorrectedWord.Show();
                labelCorrectedWord.Text = "Did you mean: " + correctedQuery + "?";
            }
            else
            {
                labelCorrectedWord.Hide();
            }
        }

        #endregion

        #region RetrievalMods Checkboxes

        private void checkBox1_Click(object sender, EventArgs e)
        {
            checkBoxBool.CheckState = CheckState.Checked;
            checkBoxBool.ForeColor = Color.Black;
            checkBoxBool.FlatAppearance.MouseOverBackColor = Color.Gold;
            checkBoxBool.FlatAppearance.MouseDownBackColor = Color.Gold;

            checkBoxRank.CheckState = CheckState.Unchecked;
            checkBoxRank.ForeColor = Color.Gold;
            checkBoxRank.FlatAppearance.MouseOverBackColor = Color.FromArgb(64, 64, 64);
            checkBoxRank.FlatAppearance.MouseDownBackColor = Color.FromArgb(64, 64, 64);

            DisplayBooleanSearchResults();
        }

        private void checkBox2_Click(object sender, EventArgs e)
        {
            checkBoxRank.CheckState = CheckState.Checked;
            checkBoxRank.ForeColor = Color.Black;
            checkBoxRank.FlatAppearance.MouseOverBackColor = Color.Gold;
            checkBoxRank.FlatAppearance.MouseDownBackColor = Color.Gold;

            checkBoxBool.CheckState = CheckState.Unchecked;
            checkBoxBool.ForeColor = Color.Gold;
            checkBoxBool.FlatAppearance.MouseOverBackColor = Color.FromArgb(64, 64, 64);
            checkBoxBool.FlatAppearance.MouseDownBackColor = Color.FromArgb(64, 64, 64);

            DisplayRankSearchResults();
        }

        #endregion

        #region Results display

        #region Filename labels

        private void UpdateDisplayResults(int pageToDisplay)
        {
            tableLayoutPanelResults.Controls.Clear();

            for (int i = (pageToDisplay * _numberOfResultsByPage) - _numberOfResultsByPage; i < pageToDisplay * _numberOfResultsByPage; i++)
            {
                if (_finalResults.Count <= i) break;
                AddNewLabel(_finalResults.ElementAt(i));
            }

            UpdatePageNumber();

            buttonPrevious.Enabled = _currentPage != 1;

            buttonNext.Enabled = _currentPage != _numberOfPages;
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
            tableLayoutPanelResults.Controls.Add(fileNameLabel);
        }

        private void FileNameLabel_Click(object sender, EventArgs e)
        {
            var label = sender as Label;

            if (label != null)
            {
                foreach (Label tempLabel in tableLayoutPanelResults.Controls)
                {
                    tempLabel.ForeColor = Color.Black;
                }
                label.ForeColor = Color.Gold;

                textBoxArticle.Text = File.ReadAllText(_directoryPath + "/" + label.Text);

                // Highlight the search terms.
                HighlightText();
            }

        }

        #endregion

        #region Error correction

        private void correctedWordLabel_MouseEnter(object sender, EventArgs e)
        {
            labelCorrectedWord.Font = new Font(labelCorrectedWord.Font, FontStyle.Underline);
        }

        private void correctedWordLabel_MouseLeave(object sender, EventArgs e)
        {
            labelCorrectedWord.Font = new Font(labelCorrectedWord.Font, FontStyle.Regular);
        }

        private void correctedWordLabel_MouseClick(object sender, MouseEventArgs e)
        {
            textBoxSearch.Text = labelCorrectedWord.Text.Replace("Did you mean: ", "").Replace("?", "");
        }

        #endregion

        #region Page management

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

        private void UpdateNumberOfResultsDisplayed()
        {
            int tmp = Size.Height;
            _numberOfResultsByPage = 14;
            tableLayoutPanelResults.RowCount = 0;

            while (tmp > MinimumSize.Height + 32)
            {
                _numberOfResultsByPage += 2;
                tmp -= 32;
            }

            UpdatePageNumber();
            UpdateDisplayResults(_currentPage);
        }

        private void nextButton_EnabledChanged(object sender, EventArgs e)
        {
            if (buttonNext.Enabled)
            {
                buttonNext.BackColor = Color.Gold;
                buttonNext.ForeColor = Color.Black;
            }
            else
            {
                buttonNext.BackColor = Color.FromArgb(64, 64, 64);
                buttonNext.ForeColor = Color.Gold;
            }
        }

        private void previousButton_EnabledChanged(object sender, EventArgs e)
        {
            if (buttonPrevious.Enabled)
            {
                buttonPrevious.BackColor = Color.Gold;
                buttonPrevious.ForeColor = Color.Black;
            }
            else
            {
                buttonPrevious.BackColor = Color.FromArgb(64, 64, 64);
                buttonPrevious.ForeColor = Color.Gold;
            }
        }

        private void UpdatePageNumber()
        {
            int numberOfResults = int.Parse(labelNumberResults.Text.Remove(labelNumberResults.Text.Length - 8));
            _numberOfPages = (int)Math.Ceiling((double)numberOfResults / _numberOfResultsByPage);

            while (_currentPage > _numberOfPages) _currentPage--;

            labelPage.Text = _currentPage + "/" + _numberOfPages;
        }

        #endregion

        #endregion

        #region Article textBox

        public void HighlightText()
        {

            int sStart = textBoxArticle.SelectionStart, startIndex = 0;

            foreach (var articleWord in textBoxArticle.Text.Split(new char[] { ' ', '-' }))
            {
                var cleanWord = Regex.Replace(articleWord, @"[^-\w\s]*", "").ToLower();
                if (SimpleEngine.FoundTerms.Contains(PorterStemmer.ProcessToken(cleanWord)))
                {
                    var index = textBoxArticle.Text.IndexOf(articleWord, startIndex, StringComparison.Ordinal);
                    textBoxArticle.Select(index, articleWord.Length);
                    textBoxArticle.SelectionColor = Color.Gold;
                    textBoxArticle.SelectionBackColor = Color.Black;
                }
                startIndex += articleWord.Length + 1;
            }

            textBoxArticle.SelectionStart = sStart;
            textBoxArticle.SelectionLength = 0;
        }

        #endregion

        #region Form

        private void MainWindow_ResizeEnd(object sender, EventArgs e)
        {
            if (buttonPrevious.Visible)
                UpdateNumberOfResultsDisplayed();
        }

        private void MainWindow_SizeChanged(object sender, EventArgs e)
        {
            if (WindowState != _formerWindowsState && buttonPrevious.Visible)
            {
                _formerWindowsState = WindowState;
                UpdateNumberOfResultsDisplayed();
            }
        }

        #endregion

        #region ProgressBar

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
            var t = new Timer { Interval = 3000 };
            labelIndexing.Show();

            t.Tick += (s, e) =>
            {
                labelIndexing.Hide();
                t.Stop();
            };
            t.Start();
            labelIndexing.Hide();
            progressBar.Hide();
        }

        #endregion

        #region Others

        private string prettyBytes(long numberOfBytes)
        {
            var counter = 0;
            var unit = new[] { "B", "KB", "MB", "GB" };
            while (numberOfBytes > 1024)
            {
                numberOfBytes /= 1024;
                counter++;
            }
            return numberOfBytes + unit[counter];
        }

        #endregion
    }
}
