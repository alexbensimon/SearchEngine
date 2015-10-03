using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Windows.Forms;
using SearchEngineProject.Properties;

namespace SearchEngineProject
{
    public partial class Form1 : Form
    {
        // The list of file name strings.
        private readonly IList<string> _fileNames = new List<string>();
        //The inverted index
        private readonly NaiveInvertedIndex _index = new NaiveInvertedIndex();

        public Form1()
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

            foreach (var term in _index.GetDictionary())
                KGramIndex.AddType(term);

            

            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
        }

        private void button1_Click(object sender, EventArgs e)
        {
            richTextBox1.Clear();

            var word = textBox1.Text;

            var postings = _index.GetPostings(PorterStemmer.ProcessToken(word));

            if (postings == null)
                MessageBox.Show(Resources.inexistantWordMessage);
            else
            {
                var results = new StringBuilder();
                foreach (var id in postings.Keys)
                {
                    results.Append(_fileNames[id]);
                    results.AppendLine();
                    foreach (var position in postings[id])
                    {
                        results.Append(position + " ");
                    }
                    results.AppendLine();
                    results.AppendLine();
                }
                richTextBox1.Text = results.ToString();
            }
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
                statistics.Append(pair.Key + ": " + Math.Round(pair.Value,2)*100 + "%; ");
            }
            statistics.AppendLine("\n");

            statistics.Append("Approximate memory requirement of the index: ");
            statistics.Append(prettyBytes(_index.IndexSizeInMemory));

            MessageBox.Show(statistics.ToString(), "Index statistics");
        }

        private string prettyBytes(long numberOfBytes)
        {
            int counter = 0;
            string[] unit = new string[] {"B", "KB", "MB", "GB"};
            while (numberOfBytes > 1024)
            {
                numberOfBytes /= 1024;
                counter++;
            }
            return numberOfBytes + unit[counter];
        }
    }
}