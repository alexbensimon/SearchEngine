using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace SearchEngineProject
{
    public class SimpleEngine
    {
        public static HashSet<string> PotentialMisspelledWords = new HashSet<string>();
        public static HashSet<string> FoundTerms = new HashSet<string>();

        public static bool IsQuerySyntaxCorrect(string query)
        {
            // Testing patterns.
            var aParenthese = new Regex(@"\(|\)");
            var aPlusSign = new Regex(@"\+");
            var aQuoteSign = new Regex(@"""");
            var aStar = new Regex(@"\*");
            var aMinusSign = new Regex(@"-");

            // Correct patterns.
            var matchingParentheses = new Regex(@"^([^()]*\([^()]*\)[^()]*)+$");
            var matchingQuoteSigns = new Regex(@"^([^""]*""[^""]*""[^""]*)+$");
            var noAloneStarLeft = new Regex(@"^([^*]*\S\*[^*]*)+$");
            var noAloneStarRight = new Regex(@"^([^*]*\*\S[^*]*)+$");
            var somethingAfterMinusSign = new Regex(@"^([^-]*-\S[^-]*)+$");

            // Wrong patterns
            var twoStars = new Regex(@"\*\*");

            // If the query follows a certain pattern, verify the partern is correct.
            if ((aParenthese.IsMatch(query)) && (!(matchingParentheses.IsMatch(query))))
                return false;

            if (aPlusSign.IsMatch(query))
            {
                var qList = SplitOrQuery(query);
                foreach (string q in qList)
                {
                    if (q.Trim() == string.Empty)
                        return false;
                }
            }

            if ((aQuoteSign.IsMatch(query)) && (!(matchingQuoteSigns.IsMatch(query))))
                return false;

            if ((aStar.IsMatch(query)) && (!(noAloneStarLeft.IsMatch(query))) && (!(noAloneStarRight.IsMatch(query))))
                return false;

            // Not query.
            if ((aMinusSign.IsMatch(query)) && (!(somethingAfterMinusSign.IsMatch(query))))
                return false;

            if (twoStars.IsMatch(query)) return false;

            return true;
        }

        public static List<string> SplitOrQuery(string query)
        {
            return query.Split('+').ToList();
        }

        public static List<string> SplitWhiteSpace(string query)
        {
            return query.Split(null).ToList();
        }

        public static List<List<int>> MergeAndResults(List<List<int>> andQueryItemsResultsDocIds)
        {
            for (int i = 0; i < andQueryItemsResultsDocIds.Count - 1; i++)
            {
                var andMergedList = new List<int>();
                int a = 0;
                int b = 0;
                while (a < andQueryItemsResultsDocIds[i].Count && b < andQueryItemsResultsDocIds[i + 1].Count)
                {
                    if (andQueryItemsResultsDocIds[i][a] == andQueryItemsResultsDocIds[i + 1][b])
                    {
                        andMergedList.Add(andQueryItemsResultsDocIds[i][a]);
                        a++;
                        b++;
                    }
                    else
                    {
                        if (andQueryItemsResultsDocIds[i][a] < andQueryItemsResultsDocIds[i + 1][b])
                            a++;
                        else
                            b++;
                    }
                }
                andQueryItemsResultsDocIds[i + 1] = andMergedList;
            }
            return andQueryItemsResultsDocIds;
        }

        public static List<List<int>> MergeAndNotResults(List<List<int>> andNotQueryItemsResultsDocIds)
        {
            for (int i = 0; i < andNotQueryItemsResultsDocIds.Count - 1; i++)
            {
                var andNotMergedList = new List<int>();
                int a = 0;
                int b = 0;
                while (a < andNotQueryItemsResultsDocIds[i].Count)
                {
                    if (b >= andNotQueryItemsResultsDocIds[i + 1].Count)
                    {
                        andNotMergedList.Add(andNotQueryItemsResultsDocIds[i][a]);
                        a++;
                    }
                    else if (andNotQueryItemsResultsDocIds[i][a] == andNotQueryItemsResultsDocIds[i + 1][b])
                    {
                        a++;
                        b++;
                    }
                    else
                    {
                        if (andNotQueryItemsResultsDocIds[i][a] < andNotQueryItemsResultsDocIds[i + 1][b])
                        {
                            andNotMergedList.Add(andNotQueryItemsResultsDocIds[i][a]);
                            a++;
                        }
                        else
                            b++;
                    }
                }
                andNotQueryItemsResultsDocIds[i + 1] = andNotMergedList;
            }
            return andNotQueryItemsResultsDocIds;
        }

        public static List<List<int>> MergeOrResults(List<List<int>> orQueryItemsResultsDocIds)
        {
            for (int i = 0; i < orQueryItemsResultsDocIds.Count - 1; i++)
            {
                var orMergedList = new List<int>();
                int a = 0;
                int b = 0;
                while (a < orQueryItemsResultsDocIds[i].Count || b < orQueryItemsResultsDocIds[i + 1].Count)
                {
                    if (b >= orQueryItemsResultsDocIds[i + 1].Count)
                    {
                        orMergedList.Add(orQueryItemsResultsDocIds[i][a]);
                        a++;
                    }
                    else if (a >= orQueryItemsResultsDocIds[i].Count)
                    {
                        orMergedList.Add(orQueryItemsResultsDocIds[i + 1][b]);
                        b++;
                    }
                    else if (orQueryItemsResultsDocIds[i][a] == orQueryItemsResultsDocIds[i + 1][b])
                    {
                        orMergedList.Add(orQueryItemsResultsDocIds[i][a]);
                        a++;
                        b++;
                    }
                    else
                    {
                        if (orQueryItemsResultsDocIds[i][a] < orQueryItemsResultsDocIds[i + 1][b])
                        {
                            orMergedList.Add(orQueryItemsResultsDocIds[i][a]);
                            a++;
                        }
                        else
                        {
                            orMergedList.Add(orQueryItemsResultsDocIds[i + 1][b]);
                            b++;
                        }
                    }
                }
                orQueryItemsResultsDocIds[i + 1] = orMergedList;
            }
            return orQueryItemsResultsDocIds;
        }

        public static List<int> ProcessWildcardQuery(string query, DiskPositionalIndex index)
        {
            var orQuery = KGramIndex.GenerateNormalQuery(query);
            var words = SplitOrQuery(orQuery);
            var orQueryItemsResultsDocIds = new List<List<int>>();
            foreach (string word in words)
            {
                var term = PorterStemmer.ProcessToken(word.Trim());
                orQueryItemsResultsDocIds.Add(GetDocIds(index.GetPostings(term, false)));

                //Add the term to to  liste of the found term
                if (!FoundTerms.Contains(term))
                    FoundTerms.Add(term);
            }

            return MergeOrResults(orQueryItemsResultsDocIds).Last();
        }

        public static IEnumerable<KeyValuePair<int, double>> ProcessRankQuery(string query,
            DiskPositionalIndex index, string folder)
        {
            if (string.IsNullOrWhiteSpace(query)) return null;

            double numberOfDocuments = index.FileNames.Count;

            var ads = new Dictionary<int, double>();

            var reader = new FileStream(Path.Combine(folder, "docWeights.bin"), FileMode.Open, FileAccess.Read);

            for (int i = 0; i < numberOfDocuments; i++)
            {
                ads.Add(i, 0);
            }

            foreach (var term in SplitWhiteSpace(query))
            {
                var processedTerm = PorterStemmer.ProcessToken(term);
                var postings = index.GetPostings(processedTerm, true);

                if (postings != null)
                {
                    //Add the term to to  liste of the found term
                    if (!FoundTerms.Contains(processedTerm))
                        FoundTerms.Add(processedTerm);

                    double dft = postings.Count();

                    double wqt = Math.Log(1.0 + (numberOfDocuments / dft));

                    for (int j = 0; j < postings.Count(); j++)
                    {
                        double tftd = postings[j].Count() - 1;

                        double wdt = 1.0 + Math.Log(tftd);

                        ads[postings[j][0]] += wqt * wdt;
                    }
                }
                    }

            for (int i = 0; i < ads.Count; i++)
            {
                if (ads.ElementAt(i).Value > 0)
                {
                        // Read Ld in file and divide Ad by Ld.
                    reader.Seek(ads.ElementAt(i).Key * 8, SeekOrigin.Begin);
                        var buffer = new byte[8];
                        reader.Read(buffer, 0, buffer.Length);
                        if (BitConverter.IsLittleEndian)
                            Array.Reverse(buffer);
                        double ld = BitConverter.ToDouble(buffer, 0);

                    ads[ads.ElementAt(i).Key] = Math.Truncate(10000000 * ads.ElementAt(i).Value / ld) / 10000000;
                            }
                        }

            reader.Close();
            return ads.OrderByDescending(i => i.Value);
        }

        public static List<int> ProcessQuery(string query, DiskPositionalIndex index)
        {
            //Empty the potential misspelled words
            PotentialMisspelledWords.Clear();

            // Trim the query.
            query = query.Trim();

            if (query == string.Empty)
                return new List<int>();

            // Verify the syntax is correct.
            if (!IsQuerySyntaxCorrect(query))
                return null;

            // Split by +, it gives us all the Qs. We will process the "OR" later.
            var qList = SplitOrQuery(query);

            // The list that will contain the final result of the query as docids.
            var finalResultsDocIds = new List<int>();

            var orQueryItemsResultsDocIds = new List<List<int>>();
            // Process each Q: 
            foreach (string qTemp in qList)
            {
                int positiveLiterals = 0;
                bool notQuery;

                string q = qTemp.Trim();

                var andQueryItemsResultsDocIds = new List<List<int>>();
                var andNotQueryItemsResultsDocIds = new List<List<int>>();
                var notQueriesTempList = new List<List<int>>();

                // Parentheses.
                if (Regex.IsMatch(q, @"-\((.+?)\)")) notQuery = true;
                else
                {
                    notQuery = false;
                    positiveLiterals++;
                }
                var parentheses = Regex.Matches(q, @"\((.+?)\)")
                    .Cast<Match>()
                    .Select(m => m.Groups[1].Value)
                    .ToList();
                foreach (string expression in parentheses)
                {
                    var andQueryTerms = SplitWhiteSpace(expression);
                    var secondAndQueryItemsResultsDocIds = new List<List<int>>();

                    foreach (string termTemp in andQueryTerms)
                    {
                        string term = termTemp.Trim();
                        if (term == string.Empty) continue;

                        // If Wildcard query.
                        if (Regex.IsMatch(term, @"(.*\*.*)+"))
                            secondAndQueryItemsResultsDocIds.Add(ProcessWildcardQuery(term, index));
                        else
                        {
                            var processedTerm = PorterStemmer.ProcessToken(term);
                            var postings = index.GetPostings(processedTerm, false);

                            // Add the term to the list of the found term.
                            if (!FoundTerms.Contains(processedTerm))
                                FoundTerms.Add(processedTerm);
                            if (postings == null)
                            {
                                secondAndQueryItemsResultsDocIds.Add(new List<int>());
                                if (!PotentialMisspelledWords.Contains(term))
                                    PotentialMisspelledWords.Add(term);
                            }
                            else
                            {
                                secondAndQueryItemsResultsDocIds.Add(GetDocIds(postings));
                                if (!PotentialMisspelledWords.Contains(term) && postings.Count() < 5)
                                    PotentialMisspelledWords.Add(term);
                            }
                        }
                    }
                    if (secondAndQueryItemsResultsDocIds.Count > 0)
                        andQueryItemsResultsDocIds.Add(MergeAndResults(secondAndQueryItemsResultsDocIds).Last());
                }
                // Remove parentheses from the Q.
                q = Regex.Replace(q, @"\((.+?)\)", "");

                // Phrase queries with " ".
                if (Regex.IsMatch(q, "-\"(.+?)\"")) notQuery = true;
                else
                {
                    notQuery = false;
                    positiveLiterals++;
                }
                var phraseQueries = Regex.Matches(q, "\"(.+?)\"")
                    .Cast<Match>()
                    .Select(m => m.Groups[1].Value)
                    .ToList();
                foreach (string phraseQuery in phraseQueries)
                {
                    var phraseQueryTerms = SplitWhiteSpace(phraseQuery.Trim());
                    var results = ProcessPhraseQuery(index, phraseQueryTerms);
                    if (results == null)
                    {
                        if (notQuery) notQueriesTempList.Add(new List<int>());
                        else andQueryItemsResultsDocIds.Add(new List<int>());
                    }
                    else
                    {
                        if (notQuery) notQueriesTempList.Add(results.Keys.ToList());
                        else andQueryItemsResultsDocIds.Add(results.Keys.ToList());
                    }
                }
                // Remove phrase queries from the Q.
                if (notQuery) q = Regex.Replace(q, "-\"(.+?)\"", "");
                else q = Regex.Replace(q, "\"(.+?)\"", "");

                // In the Q, it only remains simple words.
                if (q != string.Empty)
                {
                    var terms = SplitWhiteSpace(q);
                    foreach (string termTemp in terms)
                    {
                        string term = termTemp.Trim();
                        if (term != string.Empty)
                        {
                            // If Wildcard query.
                            if (Regex.IsMatch(term, @"(-.*\*.*)+"))
                            {
                                positiveLiterals++;
                                notQueriesTempList.Add(ProcessWildcardQuery(term, index));
                            }
                            else if (Regex.IsMatch(term, @"(.*\*.*)+"))
                                andQueryItemsResultsDocIds.Add(ProcessWildcardQuery(term, index));

                            // Not query.
                            else if (Regex.IsMatch(term, @"-\S+"))
                            {
                                term = term.Replace("-", "");
                                var processedTerm = PorterStemmer.ProcessToken(term);
                                var postings = index.GetPostings(processedTerm, false);
                                if (postings != null)
                                {
                                    //Add the term to to  liste of the found term
                                    if (!FoundTerms.Contains(processedTerm))
                                        FoundTerms.Add(processedTerm);

                                    notQueriesTempList.Add(GetDocIds(postings));
                                }
                            }

                            // Simple word.  
                            else
                            {
                                var processedTerm = PorterStemmer.ProcessToken(term);
                                var postings = index.GetPostings(processedTerm, false);
                                if (postings == null)
                                {
                                    andQueryItemsResultsDocIds.Add(new List<int>());
                                    if (!PotentialMisspelledWords.Contains(term))
                                        PotentialMisspelledWords.Add(term);
                                }
                                else
                                {
                                    andQueryItemsResultsDocIds.Add(GetDocIds(postings));
                                    //Add the term to to  liste of the found term
                                    if (!FoundTerms.Contains(processedTerm))
                                        FoundTerms.Add(processedTerm);
                                    if (!PotentialMisspelledWords.Contains(term) && postings.Count() < 5)
                                        PotentialMisspelledWords.Add(term);
                                }
                            }
                        }
                    }
                }

                if (positiveLiterals == 0) return null;

                // If there are NOT queries.
                if (notQueriesTempList.Count > 0)
                {
                    if (andQueryItemsResultsDocIds.Count > 0)
                        andNotQueryItemsResultsDocIds.Add(MergeAndResults(andQueryItemsResultsDocIds).Last());

                    andNotQueryItemsResultsDocIds.AddRange(notQueriesTempList);

                    orQueryItemsResultsDocIds.Add(MergeAndNotResults(andNotQueryItemsResultsDocIds).Last());
                }

                else
                    // Merge all the results in a AND query.
                    if (andQueryItemsResultsDocIds.Count > 0)
                    orQueryItemsResultsDocIds.Add(MergeAndResults(andQueryItemsResultsDocIds).Last());
            }

            // Merge all the OR query items results.
            if (orQueryItemsResultsDocIds.Count > 0)
            {
                orQueryItemsResultsDocIds = MergeOrResults(orQueryItemsResultsDocIds);
                finalResultsDocIds.AddRange(orQueryItemsResultsDocIds.Last());
            }

            return finalResultsDocIds;
        }

        public static Dictionary<int, List<int>> ProcessPhraseQuery(DiskPositionalIndex index, List<string> wordsList)
        {
            int[][] word1Postings = null;

            foreach (var word in wordsList)
            {
                if (word1Postings == null)
                {
                    word1Postings = index.GetPostings(PorterStemmer.ProcessToken(word.Trim()), true);

                    //Check if the word could be mispelled
                    if (word1Postings != null)
                    {
                        if (word1Postings.Count() < 5 && !PotentialMisspelledWords.Contains(word))
                            PotentialMisspelledWords.Add(word);
                    }
                    else if (!PotentialMisspelledWords.Contains(word))
                        PotentialMisspelledWords.Add(word);
                }

                else
                {
                    var word2Postings = index.GetPostings(PorterStemmer.ProcessToken(word.Trim()), true);
                    if (word2Postings == null)
                    {
                        if (!PotentialMisspelledWords.Contains(word))
                            PotentialMisspelledWords.Add(word);
                        return null;
                    }

                    if (word1Postings.Count() < 5 && !PotentialMisspelledWords.Contains(word))
                        PotentialMisspelledWords.Add(word);

                    word1Postings = Process2WordPhraseQuery(word1Postings, word2Postings);
                }
                if (word1Postings == null)
                    return null;
            }

            //Add phrase query to found words as a group
            string tmp = "";
            foreach (var word in wordsList)
            {
                tmp += word + " ";
            }
            FoundTerms.Add(tmp.Trim());

            var resultPostingsList = new Dictionary<int, List<int>>();
            for (int i = 0; i < word1Postings.Length; i++)
            {
                resultPostingsList.Add(word1Postings[i][0], new List<int>());
                for (int j = 1; j < word1Postings[i].Length; j++)
                {
                    resultPostingsList[word1Postings[i][0]].Add(word1Postings[i][j]);
                }
            }
            return resultPostingsList;
        }

        private static int[][] Process2WordPhraseQuery(int[][] word1Postings, int[][] word2Postings)
        {
            var newPostingList = new Dictionary<int, List<int>>();
            var docPointer1 = 0;
            var docPointer2 = 0;

            while (docPointer1 < word1Postings.Length && docPointer2 < word2Postings.Length)
            {
                var word1DocId = word1Postings[docPointer1][0];
                var word2DocId = word2Postings[docPointer2][0];

                if (word1DocId == word2DocId)
                {
                    var posPointer1 = 1;
                    var posPointer2 = 1;
                    while (posPointer1 <= word1Postings[docPointer1].Length - 1 &&
                            posPointer2 <= word2Postings[docPointer2].Length - 1)
                    {
                        var word1Pos = word1Postings[docPointer1][posPointer1];
                        var word2Pos = word2Postings[docPointer2][posPointer2];
                        if (word2Pos - word1Pos == 1)
                        {
                            if (newPostingList.ContainsKey(word2DocId))
                                newPostingList[word2DocId].Add(word2Pos);
                            else
                                newPostingList.Add(word2DocId, new List<int>() { word2Pos });
                            posPointer1++;
                            posPointer2++;
                        }
                        else
                        {
                            if (word1Pos <= word2Pos)
                                posPointer1++;
                            else
                                posPointer2++;
                        }
                    }
                    docPointer1++;
                    docPointer2++;
                }
                else if (word1DocId <= word2DocId)
                    docPointer1++;
                else
                    docPointer2++;
            }

            //Convert the Dictionnary to an array
            var newPostingArray = new int[newPostingList.Count][];
            int i = 0;
            foreach (var docId in newPostingList.Keys)
            {
                newPostingArray[i] = new int[newPostingList[docId].Count + 1];
                newPostingArray[i][0] = docId;
                int j = 1;
                foreach (var position in newPostingList[docId])
                {
                    newPostingArray[i][j] = position;
                    j++;
                }
                i++;
            }

            return newPostingArray;
        }

        private static List<int> GetDocIds(int[][] postings)
        {
            var idsList = new List<int>();
            for (int i = 0; i < postings.Count(); i++)
            {
                idsList.Add(postings[i][0]);
            }
            return idsList;
        }

    }
}