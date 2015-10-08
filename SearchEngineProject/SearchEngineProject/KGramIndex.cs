using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace SearchEngineProject
{
    internal class KGramIndex
    {
        private static readonly Dictionary<string, List<string>>[] _kGramIndex = new Dictionary<string, List<string>>[3] { new Dictionary<string, List<string>>(), new Dictionary<string, List<string>>(), new Dictionary<string, List<string>>() };
        private static HashSet<string> _typeList = new HashSet<string>();

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
                    if (_kGramIndex[counter].ContainsKey(kGram))
                        _kGramIndex[counter][kGram].Add(type);
                    else if (kGram != "$")
                        _kGramIndex[counter].Add(kGram, new List<string>() { type });
                }
            }
        }

        private static List<string> GetTypes(string kGram)
        {
            if (_kGramIndex[kGram.Length - 1].ContainsKey(kGram))
                return _kGramIndex[kGram.Length - 1][kGram];
            else
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
            return FilterPostings(finalPostingList, query);
        }

        //Ici je pars du principe que la query est clean, elle ne contient plus aucun espace

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

            foreach (var posting in MergePostings(initialQuery))
            {
                newQuery += posting + "+";
            }

            //Remove the last + and return the query
            return newQuery.Substring(0, newQuery.Length - 1);
        }

    }
}
