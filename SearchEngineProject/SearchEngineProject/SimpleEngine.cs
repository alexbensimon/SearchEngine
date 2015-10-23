﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
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

        public static List<int> ProcessWildcardQuery(string query, DiskPositionalIndex index)
        {
            var orQuery = KGramIndex.GenerateNormalQuery(query);
            var terms = SplitOrQuery(orQuery);
            var orQueryItemsResultsDocIds = new List<List<int>>();
            foreach (string term in terms)
            {
                orQueryItemsResultsDocIds.Add(index.GetPostings(PorterStemmer.ProcessToken(term.Trim()), true)[0].ToList());
            }

            return MergeOrResults(orQueryItemsResultsDocIds).Last();
        }

        public static List<int> ProcessQuery(string query, DiskPositionalIndex index)
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
                            var postings = index.GetPostings(PorterStemmer.ProcessToken(term), true);
                            // If Wildcard query.
                            if (Regex.IsMatch(term, @"(.*\*.*)+"))
                                secondAndQueryItemsResultsDocIds.Add(ProcessWildcardQuery(term, index));
                            else if (postings == null)
                                secondAndQueryItemsResultsDocIds.Add(new List<int>());
                            else
                                secondAndQueryItemsResultsDocIds.Add(postings[0].ToList());
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
                            var postings = index.GetPostings(PorterStemmer.ProcessToken(term), true);
                            // If Wildcard query.
                            if (Regex.IsMatch(term, @"(.*\*.*)+"))
                                andQueryItemsResultsDocIds.Add(ProcessWildcardQuery(term, index));
                            else if (postings == null)
                                andQueryItemsResultsDocIds.Add(new List<int>());
                            else
                                andQueryItemsResultsDocIds.Add(postings[0].ToList());
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

        public static Dictionary<int, List<int>> ProcessPhraseQuery(DiskPositionalIndex index, List<string> wordsList)
        {
            int[][] word1Postings = null;

            foreach (var word in wordsList)
            {
                if (word1Postings == null)
                    word1Postings = index.GetPostings(PorterStemmer.ProcessToken(word.Trim()), true);
                else
                {
                    var word2Postings = index.GetPostings(PorterStemmer.ProcessToken(word.Trim()), true);
                    if (word2Postings == null)
                        return null;
                    word1Postings = Process2WordPhraseQuery(word1Postings, word2Postings);
                }
                if (word1Postings == null)
                    return null;
            }

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
                    while (posPointer1 < word1Postings[docPointer1].Length - 1 &&
                            posPointer2 < word2Postings[docPointer2].Length - 1)
                    {
                        var word1Pos = word1Postings[word1DocId][posPointer1];
                        var word2Pos = word2Postings[word2DocId][posPointer2];
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
                newPostingArray[i] = new int[newPostingList[docId].Count];
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

    }
}