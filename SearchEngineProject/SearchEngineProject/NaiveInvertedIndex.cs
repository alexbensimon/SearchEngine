using System;
using System.Collections.Generic;
using System.Linq;

namespace SearchEngineProject
{
    public class NaiveInvertedIndex
    {
        private readonly Dictionary<string, Dictionary<int, IList<int>>> _mIndex = new Dictionary<string, Dictionary<int, IList<int>>>();

        /// <summary>
        /// Associates the given term with the given document ID in the index.
        /// </summary>
        public void AddTerm(string term, int documentId, int position)
        {
            // TO-DO: add the term to the index Dictionary. If the index does not have
            // an entry for the term, initialize a new List<int>, add the 
            // docID to the list, and put it into the Dictionary. Otherwise add the docID
            // to the list that already exists in the Dictionary, but ONLY IF the list does
            // not already contain the docID.

            //If the index contains the term
            if (_mIndex.ContainsKey(term))
            {
                //If the index contains the docID
                if (_mIndex[term].Keys.Last() > documentId)
                    _mIndex[term][documentId].Add(position);
                //If not
                else
                    _mIndex[term].Add(documentId, new List<int>(position));

            }
            else
            {
                Dictionary < int, IList < int >> dict = new Dictionary<int, IList<int>> ();
                dict.Add(documentId, new List<int>(position));
                _mIndex.Add(term, dict);
            }

        }

        /// <summary>
        /// Gets the number of terms in the index dictionary.
        /// </summary>
        public int TermCount
        {
            get
            {
                return _mIndex.Count;
            }
        }

        /// <summary>
        /// Retrieves the postings list for the given term.
        /// </summary>
        public Dictionary<int, IList<int>> GetPostings(string term)
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
