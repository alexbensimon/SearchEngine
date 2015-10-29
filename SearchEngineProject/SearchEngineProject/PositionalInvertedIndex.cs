using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace SearchEngineProject
{
    public class PositionalInvertedIndex
    {
        private readonly Dictionary<string, Dictionary<int, List<int>>> _mIndex = new Dictionary<string, Dictionary<int, List<int>>>();
        private int _indexSize;
        private int _avgNumberDocsInPostingsList;
        private Dictionary<string, double> _proportionDocContaining10MostFrequent =
            new Dictionary<string, double>();
        private long _indexSizeInMemory;
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

            // If the index contains the term.
            if (_mIndex.ContainsKey(term))
            {
                // If the index contains the docID.
                if (_mIndex[term].Keys.Last() >= documentId)
                    _mIndex[term][documentId].Add(position);
                // If not.
                else
                    _mIndex[term].Add(documentId, new List<int> { position });

            }
            else
            {
                var list = new List<int> { position };
                var dict = new Dictionary<int, List<int>> { { documentId, list } };
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
        public Dictionary<int, List<int>> GetPostings(string term)
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
            _indexSize = _mIndex.Count;
            _indexSizeInMemory = 24 + 36 * _indexSize;

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
                    _indexSizeInMemory += 48 + 4 * positionList.Count;
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

                _indexSizeInMemory += 40 + 2 * term.Length;
                _indexSizeInMemory += 24 + 8 * termPostingsNumber;
            }
            _avgNumberDocsInPostingsList = totalPostings / _indexSize;
            foreach (var term in mostFrequentTermPositionNumber.OrderByDescending(i => i.Value))
            {
                _proportionDocContaining10MostFrequent.Add(term.Key, (double)mostFrequentTermPostingNumber[term.Key] / _corpusSize);
            }
        }

        public void statToDisk(string folder)
        {
            //Create the stats file
            FileStream statsFile = new FileStream(Path.Combine(folder, "statistics.bin"), FileMode.Create);
            //Create the top 10 most frequent word file
            StreamWriter mostFreqWordFile = new StreamWriter(Path.Combine(folder, "mostFreqWord.bin"), false,
                Encoding.ASCII);

            //Write the index size
            byte[] indexSizeBytes = BitConverter.GetBytes(_indexSize);
            if (BitConverter.IsLittleEndian)
                Array.Reverse(indexSizeBytes);
            statsFile.Write(indexSizeBytes, 0, indexSizeBytes.Length);

            //Write the average number of docs in postings list
            byte[] avgNumberBytes = BitConverter.GetBytes(_avgNumberDocsInPostingsList);
            if (BitConverter.IsLittleEndian)
                Array.Reverse(avgNumberBytes);
            statsFile.Write(avgNumberBytes, 0, avgNumberBytes.Length);

            foreach (var word in _proportionDocContaining10MostFrequent.Keys)
            {
                //Write the length of the word to the binary file
                byte[] lengthBytes = BitConverter.GetBytes(word.Length);
                if (BitConverter.IsLittleEndian)
                    Array.Reverse(lengthBytes);
                statsFile.Write(lengthBytes, 0, lengthBytes.Length);

                //Write the word to the other file
                mostFreqWordFile.Write(word);

                //Write the percentage of the 10 most frequent words
                byte[] percentageBytes = BitConverter.GetBytes(_proportionDocContaining10MostFrequent[word]);
                if (BitConverter.IsLittleEndian)
                    Array.Reverse(percentageBytes);
                statsFile.Write(percentageBytes, 0, percentageBytes.Length);
            }

            //Write the percentage of the 10 most frequent words
            byte[] indexSizeInMemoryBytes = BitConverter.GetBytes(_indexSizeInMemory);
            if (BitConverter.IsLittleEndian)
                Array.Reverse(indexSizeInMemoryBytes);
            statsFile.Write(indexSizeInMemoryBytes, 0, indexSizeInMemoryBytes.Length);

            statsFile.Close();
            mostFreqWordFile.Close();
        }
    }
}
