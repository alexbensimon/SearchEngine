using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace SearchEngineProject
{
    internal class KGramIndex
    {
        private static readonly Dictionary<string, IList<string>>[] _kGramIndex = new Dictionary<string, IList<string>>[3] { new Dictionary<string, IList<string>>(), new Dictionary<string, IList<string>>(), new Dictionary<string, IList<string>>() };
        private static HashSet<string> _typeList = new HashSet<string>(); 

        public static void AddType(string type)
        {
            if (_typeList.Contains(type))
                return;
            
            _typeList.Add(type);

            type = '$' + type + '$';

            for (var counter = 0; counter < 3; counter++)
            {
                var kGramList = new HashSet<string>();
                for (var charIndex = 0; charIndex < type.Length - counter; charIndex++)
                {
                    var kGram = "";
                    for (var charOffset = 0; charOffset <= counter; charOffset++)
                        kGram = kGram + type[charIndex + charOffset];

                    if (!kGramList.Contains(kGram))
                        kGramList.Add(kGram);
                }

                foreach (var kGram in kGramList)
                {
                    if (_kGramIndex[counter].ContainsKey(kGram))
                        _kGramIndex[counter][kGram].Add(type);
                    else
                        _kGramIndex[counter].Add(kGram, new List<string>() { type });
                }
            }
        }

        private static IList<string> GetTypes(string kGram)
        {
            return _kGramIndex[kGram.Length - 1][kGram];
        }

        private static List<string> GenerateKgrams(string wildcardQuery)
        {
            var usableWildcardQuery = '$' + wildcardQuery.Trim() + '$';
            var finalKgrams = new List<string>();
            var tempKgrams = usableWildcardQuery.Split('*');
            foreach (var kgram in tempKgrams)
            {
                if (kgram.Length <= 3)
                    finalKgrams.Add(kgram);
                else
                {
                    var tmpKgram = kgram;

                    while (tmpKgram.Length > 3)
                    {
                        finalKgrams.Add(tmpKgram.Substring(0, 3));
                        tmpKgram = tmpKgram.Remove(0, 3);
                    }
                    finalKgrams.Add(tmpKgram);
                }
            }

            return finalKgrams;
        }

        private static IList<string> MergePostings(string query)
        {
            IList<string> finalPostingList = null;
            foreach (var kgram in GenerateKgrams(query))
            {
                if (finalPostingList == null)
                    finalPostingList = GetTypes(kgram);
                else
                    finalPostingList = (IList<string>) finalPostingList.Intersect(GetTypes(kgram));
            }
            return FilterPostings(finalPostingList, query);
        }

        //Ici je pars du principe que la query est clean, elle ne contient plus aucun espace

        private static List<string> FilterPostings(IList<string> postingList, string query)
        {
            var filteredList = new List<string>();
            foreach (var candidateWord in postingList)
            {
                if(Regex.IsMatch(candidateWord, "^" + query.Replace("*", "\\w*") + "$"))
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
            return newQuery.Substring(0, newQuery.Length-1);
        }

    }
}
