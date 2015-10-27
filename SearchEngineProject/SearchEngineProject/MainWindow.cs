﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
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
            richTextBox1.Clear();
            label1.Text = string.Empty;
            label1.ForeColor = SystemColors.HighlightText;
            var query = textBox1.Text;

            var resultsDocIds = SimpleEngine.ProcessQuery(query, _index);

            if (resultsDocIds == null)
                richTextBox1.Text = "Wrong syntax";
            else if (resultsDocIds.Count == 0)
                richTextBox1.Text = "No results";
            else
            {
                // Display the number of returned documents.
                label1.Text = "Results: " + resultsDocIds.Count + " documents";

                // Build the results.
                var finalResults = new StringBuilder();
                foreach (int docId in resultsDocIds)
                {
                    finalResults.Append(_index.FileNames[docId]);
                    finalResults.AppendLine();
                    finalResults.AppendLine();
                }
                richTextBox1.Text = finalResults.ToString();
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
                richTextBox1.Select(richTextBox1.Text.IndexOf(word), word.Length);
                richTextBox1.SelectionColor = Color.Gold;
                richTextBox2.Text = File.ReadAllText("Corpus/" + word);
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
                richTextBox1.Cursor = Cursors.Hand;
            else
                richTextBox1.Cursor = Cursors.Default;
            if (word != _currentWordUnderCursor)
            {
                if (_currentWordUnderCursor != null)
                {
                    richTextBox1.Select(richTextBox1.Text.IndexOf(_currentWordUnderCursor), _currentWordUnderCursor.Length);
                    richTextBox1.SelectionColor = Color.Black;
                    richTextBox1.SelectionFont = new Font(richTextBox1.SelectionFont, FontStyle.Regular);
                    _currentWordUnderCursor = null;
                }
                if (word != null && word.EndsWith(".txt"))
                {
                    richTextBox1.Select(richTextBox1.Text.IndexOf(word), word.Length);
                    richTextBox1.SelectionColor = Color.Gold;
                    richTextBox1.SelectionFont = new Font(richTextBox1.SelectionFont, FontStyle.Underline);
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
            var fbd = new FolderBrowserDialog();
            fbd.ShowDialog();
            var directoryPath = fbd.SelectedPath;

            if (directoryPath == null) return;

            var writer = new IndexWriter(directoryPath);
            writer.BuildIndex();

            _index = new DiskPositionalIndex(directoryPath);
        }
    }
}