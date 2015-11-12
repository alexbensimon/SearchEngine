using System;
using System.Collections.Generic;
using System.Diagnostics.Eventing.Reader;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace SearchEngineProject
{
    internal class KGramIndex
    {
        private static readonly Dictionary<string, List<string>>[] _kGramIndexes = new Dictionary<string, List<string>>[3] { new Dictionary<string, List<string>>(), new Dictionary<string, List<string>>(), new Dictionary<string, List<string>>() };
        private static readonly HashSet<string> _typeList = new HashSet<string>();

        public static List<string> GenerateKgrams(string type, bool add, int kMin)
        {
            if (add)
            {
                if (_typeList.Contains(type))
                    return null;

                _typeList.Add(type);
            }

            var modifiedType = '$' + type + '$';
            List<string> kgrams = new List<string>();

            for (var counter = kMin; counter < 3; counter++)
            {
                var kGramList = new HashSet<string>();
                for (var charIndex = 0; charIndex < modifiedType.Length - counter; charIndex++)
                {
                    var kGram = "";
                    for (var charOffset = 0; charOffset <= counter; charOffset++)
                        kGram = kGram + modifiedType[charIndex + charOffset];

                    if (!kGramList.Contains(kGram))
                        kGramList.Add(kGram);
                }


                foreach (var kGram in kGramList)
                {
                    if (kGram == "$") continue;

                    if (add)
                    {
                        if (_kGramIndexes[counter].ContainsKey(kGram))
                            _kGramIndexes[counter][kGram].Add(type);
                        else if (kGram != "$")
                            _kGramIndexes[counter].Add(kGram, new List<string>() { type });
                    }
                    else
                    {
                        kgrams.Add(kGram);
                    }
                }
            }

            return kgrams;
        }

        private static List<string> GetTypes(string kGram)
        {
            if (_kGramIndexes[kGram.Length - 1].ContainsKey(kGram))
                return _kGramIndexes[kGram.Length - 1][kGram];
            return null;
        }

        private static List<string> GenerateKgrams(string wildcardQuery)
        {
            var usableWildcardQuery = '$' + wildcardQuery.Trim() + '$';
            var finalKgrams = new List<string>();
            var tempKgrams = usableWildcardQuery.Split('*');

            foreach (var kgram in tempKgrams)
            {
                if (kgram == "") continue;
                if (kgram.Length <= 3 && kgram != "$")
                    finalKgrams.Add(kgram);
                else
                {
                    var tmpKgram = kgram;

                    while (tmpKgram.Length > 3)
                    {
                        finalKgrams.Add(tmpKgram.Substring(0, 3));
                        tmpKgram = tmpKgram.Remove(0, 3);
                    }
                    if (tmpKgram != "$")
                        finalKgrams.Add(tmpKgram);
                }
            }

            return finalKgrams;
        }

        private static List<string> MergePostings(string query)
        {
            List<string> finalPostingList = null;
            foreach (var kgram in GenerateKgrams(query))
            {
                if (finalPostingList == null)
                    finalPostingList = GetTypes(kgram);
                else
                    finalPostingList = finalPostingList.Intersect(GetTypes(kgram)).ToList();
            }

            if (finalPostingList == null)
                return null;
            return FilterPostings(finalPostingList, query);
        }

        private static List<string> FilterPostings(List<string> postingList, string query)
        {
            var filteredList = new List<string>();
            foreach (var candidateWord in postingList)
            {
                if (Regex.IsMatch(candidateWord, "^" + query.Replace("*", "\\w*") + "$"))
                    filteredList.Add(candidateWord);
            }
            return filteredList;
        }

        public static string GenerateNormalQuery(string initialQuery)
        {
            var newQuery = "";
            List<string> postings = MergePostings(initialQuery);
            if (postings == null || postings.Count == 0)
                return "";

            foreach (var posting in postings)
            {
                newQuery += posting + "+";
            }

            // Remove the last + and return the query.
            return newQuery.Substring(0, newQuery.Length - 1);
        }

        public static void ToDisk(string folder)
        {
            //Create the kgramIndex file
            FileStream kGramIndexFile = new FileStream(Path.Combine(folder, "kGramIndex.bin"), FileMode.Create);
            //Create the kGramVocabList
            StreamWriter kGramVocabList = new StreamWriter(Path.Combine(folder, "kGramVocab.bin"), false, Encoding.ASCII);
            //Create the kGramList
            StreamWriter kGramList = new StreamWriter(Path.Combine(folder, "kGram.bin"), false, Encoding.ASCII);

            //Write the array to the file
            foreach (var kGramIndex in _kGramIndexes)
            {
                //Write number of kgrams
                byte[] kGramNumbBytes = BitConverter.GetBytes(kGramIndex.Count);
                if (BitConverter.IsLittleEndian)
                    Array.Reverse(kGramNumbBytes);
                kGramIndexFile.Write(kGramNumbBytes, 0, kGramNumbBytes.Length);

                //Write the dictionnary to the file
                foreach (var kGram in kGramIndex.Keys)
                {
                    //Write the kgram
                    kGramList.Write(kGram);

                    //Wtite the number of words associated to the kGram
                    byte[] wordNumbBytes = BitConverter.GetBytes(kGramIndex[kGram].Count);
                    if (BitConverter.IsLittleEndian)
                        Array.Reverse(wordNumbBytes);
                    kGramIndexFile.Write(wordNumbBytes, 0, wordNumbBytes.Length);

                    //Write the words to the file
                    foreach (var word in kGramIndex[kGram])
                    {
                        //Write the length of the word
                        byte[] wordPosBytes = BitConverter.GetBytes(word.Length);
                        if (BitConverter.IsLittleEndian)
                            Array.Reverse(wordPosBytes);
                        kGramIndexFile.Write(wordPosBytes, 0, wordPosBytes.Length);

                        //Write the word
                        kGramVocabList.Write(word);
                    }
                }
            }

            kGramIndexFile.Close();
            kGramVocabList.Close();
            kGramList.Close();
        }

        public static void ToMemory(string path)
        {
            var kGramIndexFile = new FileStream(Path.Combine(path, "kGramIndex.bin"), FileMode.Open, FileAccess.Read);
            var kGramVocabList = new FileStream(Path.Combine(path, "kGramVocab.bin"), FileMode.Open, FileAccess.Read);
            var kGramList = new FileStream(Path.Combine(path, "kGram.bin"), FileMode.Open, FileAccess.Read);

            //Rank of the GramIndex
            int k = 1;

            foreach (var kGramIndex in _kGramIndexes)
            {
                kGramIndex.Clear();

                //Read size of the the kGramIndex
                var buffer = new byte[4];
                kGramIndexFile.Read(buffer, 0, buffer.Length);
                if (BitConverter.IsLittleEndian)
                    Array.Reverse(buffer);
                int kGramIndexSize = BitConverter.ToInt32(buffer, 0);

                //Read the kgram
                for (int i = 0; i < kGramIndexSize; i++)
                {
                    buffer = new byte[k];
                    kGramList.Read(buffer, 0, k);
                    string kGram = Encoding.ASCII.GetString(buffer);
                    _kGramIndexes[k - 1].Add(kGram, new List<string>());

                    //Read the number of words associated to the kgram
                    buffer = new byte[4];
                    kGramIndexFile.Read(buffer, 0, buffer.Length);
                    if (BitConverter.IsLittleEndian)
                        Array.Reverse(buffer);
                    int wordsNumber = BitConverter.ToInt32(buffer, 0);

                    for (int j = 0; j < wordsNumber; j++)
                    {
                        //Read the length of the word
                        buffer = new byte[4];
                        kGramIndexFile.Read(buffer, 0, buffer.Length);
                        if (BitConverter.IsLittleEndian)
                            Array.Reverse(buffer);
                        int wordLength = BitConverter.ToInt32(buffer, 0);

                        //Read the word
                        buffer = new byte[wordLength];
                        kGramVocabList.Read(buffer, 0, wordLength);
                        string word = Encoding.ASCII.GetString(buffer);

                        //Add the word to index list
                        _kGramIndexes[k - 1][kGram].Add(word);
                    }
                }

                k++;
            }

            kGramIndexFile.Close();
            kGramVocabList.Close();
            kGramList.Close();
        }

        public static HashSet<string> getCorrectedWord(string misspelledTerm)
        {
            var kgrams = GenerateKgrams(misspelledTerm, false, 1);
            HashSet<string> potentialCorrectedWords = new HashSet<string>();
            int smallestEditDistance = Int32.MaxValue;

            foreach (var kgram in kgrams)
            {
                var typesList = GetTypes(kgram);

                if (typesList == null) continue;

                foreach (var type in typesList)
                {
                    if (potentialCorrectedWords.Contains(type) ||
                        PorterStemmer.ProcessToken(type) == PorterStemmer.ProcessToken(misspelledTerm)) continue;
                    float jaccardCoefficient = getJaccardCoefficient(kgrams, GenerateKgrams(type, false, 1));
                    if (jaccardCoefficient >= 0.1)
                    {
                        int editDistance = getEditDistance(misspelledTerm, type);
                        if (editDistance < smallestEditDistance)
                        {
                            smallestEditDistance = editDistance;
                            potentialCorrectedWords = new HashSet<string> {type};
                        }
                        else if (editDistance == smallestEditDistance)
                            potentialCorrectedWords.Add(type);
                    }
                }
            }

            return potentialCorrectedWords;
        }

        private static float getJaccardCoefficient(List<string> kgramsMisspelledTerm, List<string> kgramsVocabularyType)
        {
            float intersection = kgramsMisspelledTerm.Intersect(kgramsVocabularyType).Count();
            float union = kgramsMisspelledTerm.Count() + kgramsVocabularyType.Count() - intersection;

            return intersection / union;
        }

        private static int getEditDistance(string misspelledTerm, string type)
        {
            int[,] m = new int[misspelledTerm.Length + 1, type.Length + 1];

            for (int i = 1; i <= misspelledTerm.Length; i++)
            {
                m[i, 0] = i;
            }

            for (int j = 1; j <= type.Length; j++)
            {
                m[0, j] = j;
            }

            for (int i = 1; i <= misspelledTerm.Length; i++)
            {
                for (int j = 1; j <= type.Length; j++)
                {
                    int offset;
                    if (misspelledTerm[i - 1] == type[j - 1])
                        offset = 0;
                    else
                    {
                        offset = 1;
                    }

                    m[i, j] = Math.Min(m[i - 1, j - 1] + offset, Math.Min(m[i - 1, j] + 1, m[i, j - 1] + 1));
                }
            }

            return m[misspelledTerm.Length, type.Length];
        }
    }
}
