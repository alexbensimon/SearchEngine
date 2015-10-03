using System.Collections.Generic;
using System.Windows.Forms;

namespace SearchEngineProject
{
    internal class KGramIndex
    {
        private static readonly Dictionary<string, IList<string>>[] _kGramIndex = new Dictionary<string, IList<string>>[3] { new Dictionary<string, IList<string>>(), new Dictionary<string, IList<string>>(), new Dictionary<string, IList<string>>() };

        public static void AddType(string term)
        {
            term = '$' + term + '$';

            for (var counter = 0; counter < 3; counter++)
            {
                var kGramList = new List<string>();
                for (var charIndex = 0; charIndex < term.Length - counter; charIndex++)
                {
                    var kGram = "";
                    for (var charOffset = 0; charOffset <= counter; charOffset++)
                        kGram = kGram + term[charIndex + charOffset];

                    if (!kGramList.Contains(kGram))
                        kGramList.Add(kGram);
                }

                foreach (var kGram in kGramList)
                {
                    if (_kGramIndex[counter].ContainsKey(kGram))
                        _kGramIndex[counter][kGram].Add(term);
                    else
                        _kGramIndex[counter].Add(kGram, new List<string>() {term});
                }
            }
        }

        public static IList<string> GetTypes(string kGram)
        {
            return _kGramIndex[kGram.Length - 1][kGram];
        }
    }
}
