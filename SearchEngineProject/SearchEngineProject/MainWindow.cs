﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Text;
using System.Windows.Forms;
using SearchEngineProject.Properties;

namespace SearchEngineProject
{
    public partial class MainWindow : Form
    {
        // The list of file name strings.
        private readonly List<string> _fileNames = new List<string>();
        //The inverted index
        private readonly PositionalInvertedIndex _index = new PositionalInvertedIndex();

        public MainWindow()
        {
            // The ID of the next document to be added.
            var documentId = 0;

            // Iterate through all .txt files in the current directory.
            foreach (var fileName in Directory.EnumerateFiles(Environment.CurrentDirectory + @"\Corpus", "*.txt"))
            {
                // for each file, open the file and index it.
                SimpleEngine.IndexFile(fileName, _index, documentId);
                documentId++;
                // add the file's name to the list of filenames.
                _fileNames.Add(Path.GetFileName(fileName));
            }
            _index.ComputeStatistics();
            //PrintResults(index, fileNames);

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
                    finalResults.Append(_fileNames[docId]);
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
            //get the word under the cursor
            var word = GetWordUnderCursor(control, e);
            if (word != null &&  word.EndsWith(".txt"))
            {
                richTextBox2.Text = File.ReadAllText("Corpus/" + word);
            }
        }

        public static string GetWordUnderCursor(RichTextBox control, MouseEventArgs e)
        {
            //check if there's any text entered
            if (string.IsNullOrWhiteSpace(control.Text))
                return null;
            //get index of nearest character
            var index = control.GetCharIndexFromPosition(e.Location);
            //check if mouse is above a word (non-whitespace character)
            if (char.IsWhiteSpace(control.Text[index]))
                return null;
            //find the start index of the word
            var start = index;
            while (start > 0 && !char.IsWhiteSpace(control.Text[start - 1]))
                start--;
            //find the end index of the word
            var end = index;
            while (end < control.Text.Length - 1 && !char.IsWhiteSpace(control.Text[end + 1]))
                end++;
            //get and return the whole word
            return control.Text.Substring(start, end - start + 1);
        }

        private void richTextBox1_MouseMove(object sender, MouseEventArgs e)
        {
            var control = sender as RichTextBox;
            //get the word under the cursor
            var word = GetWordUnderCursor(control, e);
            if (word != null)
            {
                this.Cursor = Cursors.Hand;
                richTextBox1.Select(richTextBox1.Text.IndexOf(word), word.Length);
                richTextBox1.SelectionColor = Color.CornflowerBlue;
                richTextBox1.SelectionFont = new Font(richTextBox1.SelectionFont, FontStyle.Underline);
            }
            else
                this.Cursor = Cursors.Arrow;
            }

        private void button2_Click(object sender, EventArgs e)
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
    }
}