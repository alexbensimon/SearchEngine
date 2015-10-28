using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace SearchEngineProject
{
    internal class KGramIndex
    {
        private static readonly Dictionary<string, List<string>>[] _kGramIndexes = new Dictionary<string, List<string>>[3] { new Dictionary<string, List<string>>(), new Dictionary<string, List<string>>(), new Dictionary<string, List<string>>() };
        private static readonly HashSet<string> _typeList = new HashSet<string>();

        public static void AddType(string type)
        {
            if (_typeList.Contains(type))
                return;

            _typeList.Add(type);

            var modifiedType = '$' + type + '$';

            for (var counter = 0; counter < 3; counter++)
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
                    if (_kGramIndexes[counter].ContainsKey(kGram))
                        _kGramIndexes[counter][kGram].Add(type);
                    else if (kGram != "$")
                        _kGramIndexes[counter].Add(kGram, new List<string>() { type });
                }
            }
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
            if (postings == null || postings.Count==0)
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
                    //Wtite the number of words associated to the kGram
                    byte[] wordNumbBytes = BitConverter.GetBytes(kGramIndex[kGram].Count);
                    if (BitConverter.IsLittleEndian)
                        Array.Reverse(wordNumbBytes);
                    kGramIndexFile.Write(wordNumbBytes, 0, wordNumbBytes.Length);

                    //Write the words to the file
                    foreach (var word in kGramIndex[kGram])
                    {
                        byte[] wordBytes = BitConverter.GetBytes(word.Length);
                        if (BitConverter.IsLittleEndian)
                            Array.Reverse(wordBytes);
                        kGramIndexFile.Write(wordBytes, 0, wordBytes.Length);
                    }
                }
            }

            kGramIndexFile.Close();
        }
    }
}
