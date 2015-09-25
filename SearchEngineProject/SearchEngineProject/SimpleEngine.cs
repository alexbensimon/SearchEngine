﻿using System;
using System.Collections.Generic;

namespace SearchEngineProject
{
    public class SimpleEngine
    {
        /// <summary>
        /// Indexes a file by reading a series of tokens from the file, treating each
        /// token read as a term, and then adding the given document's ID to the inverted
        /// index for the term.
        /// </summary>
        /// <param name="fileName">the name of the file to open, which can be used as a parameter
        /// to the SimpleTokenStream constructor.</param>
        /// <param name="index">the current state of the index for the files that have
        /// already been processed.</param>
        /// <param name="documentId">the integer ID of the current document, needed when
        /// indexing each term from the document.</param>
        public static void IndexFile(string fileName, NaiveInvertedIndex index, int documentId)
        {
            // TO-DO: finish this method for indexing a particular file.
            // Construct a SimpleTokenStream for the given File.
            // Read each token from the stream and add it to the index.
            SimpleTokenStream simpleTokenStream = new SimpleTokenStream(fileName);
            int position = 0;

            while (simpleTokenStream.HasNextToken)
            {
                string token = simpleTokenStream.NextToken();
                index.AddTerm(PorterStemmer.ProcessToken(token), documentId, position);
                position++;
            }
            simpleTokenStream.Close();
        }

        /// <summary>
        /// Prints the inverted index.
        /// </summary>
        public static void PrintResults(NaiveInvertedIndex index, IList<string> fileNames)
        {
            // TO-DO: print the inverted index.
            // Retrieve the dictionary of terms from the index. (It will already be sorted.)
            // For each term in the dictionary, retrieve the postings list for the
            // term. Use the postings list to print the list of document names that
            // contain the term. (The document ID in a postings list corresponds to 
            // an index in the fileNames list.)

            // Print the postings list so they are all left-aligned starting at the
            // same column, one space after the longest of the term lengths. Example:
            // 
            // as:      document0 document3 document4 document5
            // engines: document1
            // search:  document2 document4 
            string[] terms = index.GetDictionary();
            int maxlength = 0;
            foreach (string term in terms)
            {
                if (term.Length > maxlength)
                    maxlength = term.Length;
            }

            foreach (string term in terms)
            {
                Console.Write("{0,-" + maxlength + "} ", term + ":");
                foreach (int id in index.GetPostings(term).Keys)
                {
                    Console.Write(fileNames[id] + " ");
                }
                Console.Write("\n");
            }
        }
    }
}
