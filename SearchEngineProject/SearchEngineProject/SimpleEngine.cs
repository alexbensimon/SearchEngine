using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace SearchEngineProject
{
    public class SimpleEngine
    {
        public static bool IsQuerySyntaxCorrect(string query)
        {
            // Testing patterns.
            var aParenthese = new Regex(@"\(|\)");
            var aPlusSign = new Regex(@"\+");
            var aQuoteSign = new Regex(@"""");
            var aStar = new Regex(@"\*");

            // Correct patterns.
            var matchingParentheses = new Regex(@"^([^()]*\([^()]*\)[^()]*)+$");
            var matchingQuoteSigns = new Regex(@"^([^""]*""[^""]*""[^""]*)+$");
            var noAloneStarLeft = new Regex(@"^([^*]*\S\*[^*]*)+$");
            var noAloneStarRight = new Regex(@"^([^*]*\*\S[^*]*)+$");

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

        public static List<int> ProcessWildcardQuery(string query, PositionalInvertedIndex index)
        {
            var orQuery = KGramIndex.GenerateNormalQuery(query);
            var terms = SplitOrQuery(orQuery);
            var orQueryItemsResultsDocIds = new List<List<int>>();
            foreach (string term in terms)
            {
                orQueryItemsResultsDocIds.Add(index.GetPostings(PorterStemmer.ProcessToken(term.Trim())).Keys.ToList());
            }

            return MergeOrResults(orQueryItemsResultsDocIds).Last();
        }

        public static List<int> ProcessQuery(string query, PositionalInvertedIndex index)
        {
            // Trim the query.
            query = query.Trim().Replace("-", "");

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
                string q = qTemp.Trim();

                var andQueryItemsResultsDocIds = new List<List<int>>();

                // Parentheses.
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
                        if (term != string.Empty)
                        {
                            var postings = index.GetPostings(PorterStemmer.ProcessToken(term));
                            // If Wildcard query.
                            if (Regex.IsMatch(term, @"(.*\*.*)+"))
                                secondAndQueryItemsResultsDocIds.Add(ProcessWildcardQuery(term, index));
                            else if (postings == null)
                                secondAndQueryItemsResultsDocIds.Add(new List<int>());
                            else
                                secondAndQueryItemsResultsDocIds.Add(postings.Keys.ToList());
                        }
                    }
                    if (secondAndQueryItemsResultsDocIds.Count > 0)
                        andQueryItemsResultsDocIds.Add(MergeAndResults(secondAndQueryItemsResultsDocIds).Last());
                }
                // Remove parentheses from the Q.
                q = Regex.Replace(q, @"\((.+?)\)", "");

                // Phrase queries with " ".
                var phraseQueries = Regex.Matches(q, "\"(.+?)\"")
                    .Cast<Match>()
                    .Select(m => m.Groups[1].Value)
                    .ToList();
                foreach (string phraseQuery in phraseQueries)
                {
                    var phraseQueryTerms = SplitWhiteSpace(phraseQuery.Trim());
                    var results = ProcessPhraseQuery(index, phraseQueryTerms);
                    if (results == null)
                        andQueryItemsResultsDocIds.Add(new List<int>());
                    else
                        andQueryItemsResultsDocIds.Add(results.Keys.ToList());
                }
                // Remove phrase queries from the Q.
                q = Regex.Replace(q, "\"(.+?)\"", "");

                // In the Q, it only remains simple words.
                if (q != string.Empty)
                {
                    var terms = SplitWhiteSpace(q);
                    foreach (string termTemp in terms)
                    {
                        string term = termTemp.Trim();
                        if (term != string.Empty)
                        {
                            var postings = index.GetPostings(PorterStemmer.ProcessToken(term));
                            // If Wildcard query.
                            if (Regex.IsMatch(term, @"(.*\*.*)+"))
                                andQueryItemsResultsDocIds.Add(ProcessWildcardQuery(term, index));
                            else if (postings == null)
                                andQueryItemsResultsDocIds.Add(new List<int>());
                            else
                                andQueryItemsResultsDocIds.Add(postings.Keys.ToList());
                        }
                    }
                }

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

        public static Dictionary<int, List<int>> ProcessPhraseQuery(PositionalInvertedIndex index, List<string> wordsList)
        {
            Dictionary<int, List<int>> word1Postings = null;

            foreach (var word in wordsList)
            {
                if (word1Postings == null)
                    word1Postings = index.GetPostings(PorterStemmer.ProcessToken(word.Trim()));
                else
                {
                    var word2Postings = index.GetPostings(PorterStemmer.ProcessToken(word.Trim()));
                    if (word2Postings == null)
                        return null;
                    word1Postings = Process2WordPhraseQuery(word1Postings, word2Postings);
                }
                if (word1Postings == null)
                    return null;
            }

            return word1Postings;
        }

        private static Dictionary<int, List<int>> Process2WordPhraseQuery(Dictionary<int, List<int>> word1Postings,
            Dictionary<int, List<int>> word2Postings)
        {
            var newPostingList = new Dictionary<int, List<int>>();
            var docPointer1 = 0;
            var docPointer2 = 0;
            while (docPointer1 < word1Postings.Count && docPointer2 < word2Postings.Count)
            {
                var word1DocId = word1Postings.ElementAt(docPointer1).Key;
                var word2DocId = word2Postings.ElementAt(docPointer2).Key;
                if (word1DocId == word2DocId)
                {
                    var posPointer1 = 0;
                    var posPointer2 = 0;
                    while (posPointer1 < word1Postings[word1DocId].Count &&
                            posPointer2 < word2Postings[word2DocId].Count)
                    {
                        var word1Pos = word1Postings[word1DocId].ElementAt(posPointer1);
                        var word2Pos = word2Postings[word2DocId].ElementAt(posPointer2);
                        if (Math.Abs(word1Pos - word2Pos) == 1)
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
            return newPostingList;
        }

        /// <summary>
        ///     Indexes a file by reading a series of tokens from the file, treating each
        ///     token read as a term, and then adding the given document's ID to the inverted
        ///     index for the term.
        /// </summary>
        /// <param name="fileName">
        ///     the name of the file to open, which can be used as a parameter
        ///     to the SimpleTokenStream constructor.
        /// </param>
        /// <param name="index">
        ///     the current state of the index for the files that have
        ///     already been processed.
        /// </param>
        /// <param name="documentId">
        ///     the integer ID of the current document, needed when
        ///     indexing each term from the document.
        /// </param>
        public static void IndexFile(string fileName, PositionalInvertedIndex index, int documentId)
        {
            // TO-DO: finish this method for indexing a particular file.
            // Construct a SimpleTokenStream for the given File.
            // Read each token from the stream and add it to the index.
            var simpleTokenStream = new SimpleTokenStream(fileName);
            var position = 0;

            while (simpleTokenStream.HasNextToken)
            {
                var token = simpleTokenStream.NextToken();
                if (token.Replace("-", "") == "") continue;
                if (token.Contains("-"))
                {
                    foreach (var tokenHyphen in token.Split('-'))
                    {
                        index.AddTerm(PorterStemmer.ProcessToken(tokenHyphen), documentId, position);
                        KGramIndex.AddType(tokenHyphen);
                    }
                }
                index.AddTerm(PorterStemmer.ProcessToken(token.Replace("-", "")), documentId, position);
                KGramIndex.AddType(token.Replace("-", ""));
                position++;
            }
            simpleTokenStream.Close();
        }

        /// <summary>
        ///     Prints the inverted index.
        /// </summary>
        public static void PrintResults(PositionalInvertedIndex index, List<string> fileNames)
        {
            var terms = index.GetDictionary();
            var maxlength = 0;
            foreach (var term in terms)
            {
                if (term.Length > maxlength)
                    maxlength = term.Length;
            }

            foreach (var term in terms)
            {
                Console.Write("{0,-" + maxlength + "} ", term + ":");
                foreach (var id in index.GetPostings(term).Keys)
                {
                    Console.Write(fileNames[id] + " ");
                }
                Console.Write("\n");
            }
        }
    }
}