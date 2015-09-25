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
            //PrintResults(index, fileNames);

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
    }
}