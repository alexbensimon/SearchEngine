using System;
using System.Collections.Generic;
using System.Linq;

namespace SearchEngineProject
{
    public class PositionalInvertedIndex
    {
        private readonly Dictionary<string, Dictionary<int, IList<int>>> _mIndex = new Dictionary<string, Dictionary<int, IList<int>>>();
        public int IndexSize { get; private set; }
        public int AvgNumberDocsInPostingsList { get; private set; }

        public Dictionary<string, double> ProportionDocContaining10MostFrequent { get; } =
            new Dictionary<string, double>();

        public long IndexSizeInMemory { get; private set; }
        private int _corpusSize;

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
                if (_mIndex[term].Keys.Last() >= documentId)
                    _mIndex[term][documentId].Add(position);
                //If not
                else
                    _mIndex[term].Add(documentId, new List<int> { position });

            }
            else
            {
                var list = new List<int> { position };
                var dict = new Dictionary<int, IList<int>> { { documentId, list } };
                _mIndex.Add(term, dict);
            }

            _corpusSize = documentId + 1;
        }

        /// <summary>
        /// Gets the number of terms in the index dictionary.
        /// </summary>
        public int TermCount => _mIndex.Count;

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

        public void ComputeStatistics()
        {
            IndexSize = _mIndex.Count;
            IndexSizeInMemory = 24 + 36 * IndexSize;

            int totalPostings = 0;
            Dictionary<string, int> mostFrequentTermPostingNumber = new Dictionary<string, int>();
            Dictionary<string, int> mostFrequentTermPositionNumber = new Dictionary<string, int>();
            string termSmallestPosition = "";

            foreach (var term in _mIndex.Keys)
            {
                int termPositionsNumber = 0;
                int termPostingsNumber = _mIndex[term].Count;
                totalPostings += termPostingsNumber;

                foreach (var positionList in _mIndex[term].Values)
                {
                    termPositionsNumber += positionList.Count;
                    IndexSizeInMemory += 48 + 4 * positionList.Count;
                }

                if (mostFrequentTermPositionNumber.Count < 10)
                {
                    mostFrequentTermPositionNumber.Add(term, termPositionsNumber);
                    mostFrequentTermPostingNumber.Add(term, termPostingsNumber);
                    var ordered = mostFrequentTermPositionNumber.OrderBy(i => i.Value);
                    termSmallestPosition = ordered.First().Key;
                }
                else if (termPositionsNumber > mostFrequentTermPositionNumber.Values.Min())
                {
                    mostFrequentTermPositionNumber.Remove(termSmallestPosition);
                    mostFrequentTermPostingNumber.Remove(termSmallestPosition);
                    mostFrequentTermPositionNumber.Add(term, termPositionsNumber);
                    mostFrequentTermPostingNumber.Add(term, termPostingsNumber);
                    var ordered = mostFrequentTermPositionNumber.OrderBy(i => i.Value);
                    termSmallestPosition = ordered.First().Key;
                }

                IndexSizeInMemory += 40 + 2 * term.Length;
                IndexSizeInMemory += 24 + 8 * termPostingsNumber;
            }
            AvgNumberDocsInPostingsList = totalPostings / IndexSize;
            foreach (var term in mostFrequentTermPositionNumber.OrderByDescending(i => i.Value))
            {
                ProportionDocContaining10MostFrequent.Add(term.Key, (double)mostFrequentTermPostingNumber[term.Key] / _corpusSize);
            }
        }
    }
}
