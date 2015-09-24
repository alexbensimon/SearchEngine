using System;
using System.Collections.Generic;
using System.Linq;

namespace SearchEngineProject
{
    public class NaiveInvertedIndex
    {
        private readonly Dictionary<string, IList<int>> _mIndex = new Dictionary<string, IList<int>>();

        /// <summary>
        /// Associates the given term with the given document ID in the index.
        /// </summary>
        public void AddTerm(string term, int documentId)
        {
            // TO-DO: add the term to the index Dictionary. If the index does not have
            // an entry for the term, initialize a new List<int>, add the 
            // docID to the list, and put it into the Dictionary. Otherwise add the docID
            // to the list that already exists in the Dictionary, but ONLY IF the list does
            // not already contain the docID.

            if (_mIndex.ContainsKey(term))
            {
                if (_mIndex[term].Last() < documentId)
                    _mIndex[term].Add(documentId);
            }
            else
            {
                IList<int> liste = new List<int>();
                liste.Add(documentId);
                _mIndex.Add(term, liste);
            }

        }

        /// <summary>
        /// Gets the number of terms in the index dictionary.
        /// </summary>
        public int TermCount
        {
            get
            {
                // TO-DO: return the number of terms in the index.

                return _mIndex.Count;
            }
        }

        /// <summary>
        /// Retrieves the postings list for the given term.
        /// </summary>
        public IList<int> GetPostings(string term)
        {
            // TO-DO: return the postings list for the given term from the index Dictionary.
            try
            {
                return _mIndex[term];
            }
            catch (KeyNotFoundException)
            {
                return null;
            }
        }

        /// <summary>
        /// Retrieves a sorted array of the dictionary terms in the index.
        /// </summary>
        public string[] GetDictionary()
        {
            // TO-DO: fill an array of strings with all the keys from the Dictionary.
            // Sort the array and return it.
            string[] terms = _mIndex.Keys.ToArray();
            Array.Sort(terms);

            return terms;
        }
    }
}
