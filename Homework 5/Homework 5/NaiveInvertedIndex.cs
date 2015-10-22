using System;
using System.Collections.Generic;
using System.Linq;

namespace Cecs429
{
    public class NaiveInvertedIndex
    {
        private Dictionary<string, IList<int>> mIndex = new Dictionary<string, IList<int>>();

        /// <summary>
        /// Associates the given term with the given document ID in the index.
        /// </summary>
        public void AddTerm(string term, int documentID)
        {
            if (mIndex.ContainsKey(term))
            {
                if (mIndex[term].Last() < documentID)
                    mIndex[term].Add(documentID);
            }
            else
            {
                IList<int> liste = new List<int>();
                liste.Add(documentID);
                mIndex.Add(term, liste);
            }

        }

        /// <summary>
        /// Gets the number of terms in the index dictionary.
        /// </summary>
        public int TermCount
        {
            get
            {
                return mIndex.Count;
            }
        }

        /// <summary>
        /// Retrieves the postings list for the given term.
        /// </summary>
        public IList<int> GetPostings(string term)
        {
            try
            {
                return mIndex[term];
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
            string[] terms = mIndex.Keys.ToArray();
            Array.Sort(terms);

            return terms;
        }
    }
}
