using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;

namespace SearchEngineProject
{
    static class Program
    {
        /// <summary>
        /// Point d'entrée principal de l'application.
        /// </summary>
        [STAThread]
        public static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Form1());


            // The inverted index
            NaiveInvertedIndex index = new NaiveInvertedIndex();

            // The list of file name strings.
            IList<string> fileNames = new List<string>();

            // The ID of the next document to be added.
            int documentId = 0;

            // Iterate through all .txt files in the current directory.
            foreach (string fileName in Directory.EnumerateFiles(Environment.CurrentDirectory, "*.txt"))
            {
                // for each file, open the file and index it.
                SimpleEngine.IndexFile(fileName, index, documentId);
                documentId++;
                // add the file's name to the list of filenames.
                fileNames.Add(Path.GetFileName(fileName));
            }
            //PrintResults(index, fileNames);

            // Implement the same program as in Homework 1: ask the user for a term,
            // retrieve the postings list for that term, and print the names of the documents
            // which contain the term.

            while (true)
            {
                Console.Write("What word are you looking for? ");
                string word = Console.ReadLine();

                if (word == "quit")
                    break;

                IList<int> postings = index.GetPostings(PorterStemmer.ProcessToken(word));

                if (postings == null)
                    Console.WriteLine("This word does not exist in the documents.\n");
                else
                {
                    Console.Write("The word is contained in:");
                    foreach (int id in postings)
                    {
                        Console.Write(" " + fileNames[id]);
                    }
                    Console.Write("\n\n");
                }
            }
        }
    }
}
