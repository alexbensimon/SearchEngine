using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using SearchEngineProject.Properties;

namespace SearchEngineProject
{
    public class SimpleEngine
    {
        public static bool IsQuerySyntaxCorrect(string query)
        {
            // Testing patterns.
            var aParenthese = new Regex(@"\(|\)");
            var aPlusSign = new Regex(@"\+");

            // Correct patterns.
            var matchingParentheses = new Regex(@"^.*\(.*\).*$");
            var noEmptyQi = new Regex(@"^.*\S+.*\+.*\S+.*$");

            // If the query follows a certain pattern, verify the partern is correct.
            if (aParenthese.IsMatch(query))
                if (!(matchingParentheses.IsMatch(query)))
                    return false;
            if(aPlusSign.IsMatch(query))
                if (!(noEmptyQi.IsMatch(query)))
                    return false;

            return true;
        }

        public static IList<string> SplitOrQuery(string query)
        {
            return query.Split('+').ToList();
        }

        public static IList<string> SplitWhiteSpace(string query)
        {
            return query.Split(null).ToList();
        }

        public static List<int> ProcessAndQuery(string query, NaiveInvertedIndex index)
        {
            var andQueryTerms = SplitWhiteSpace(query);
            var andQueryItemsResultsDocsIds = new List<List<int>>();

            foreach (string term in andQueryTerms)
            {
                andQueryItemsResultsDocsIds.Add(
                    index.GetPostings(PorterStemmer.ProcessToken(term.Trim())).Keys.ToList());
            }

            // Merge the AND query results.
            for (int i = 0; i < andQueryItemsResultsDocsIds.Count - 1; i++)
            {
                var andMergedList = new List<int>();
                int a = 0;
                int b = 0;
                while (a < andQueryItemsResultsDocsIds[i].Count && b < andQueryItemsResultsDocsIds[i + 1].Count)
                {
                    if (andQueryItemsResultsDocsIds[i][a] == andQueryItemsResultsDocsIds[i + 1][b])
                    {
                        andMergedList.Add(andQueryItemsResultsDocsIds[i][a]);
                        a++;
                        b++;
                    }
                    else
                    {
                        if (andQueryItemsResultsDocsIds[i][a] < andQueryItemsResultsDocsIds[i + 1][b])
                            a++;
                        else
                            b++;
                    }
                }
                andQueryItemsResultsDocsIds[i + 1] = andMergedList;
            }
            return andQueryItemsResultsDocsIds.Last();
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

        public static string ProcessQuery(string query, NaiveInvertedIndex index, IList<string> fileNames)
        {
            // Verify the syntax is correct.
            if (!IsQuerySyntaxCorrect(query))
                return null;

            // Split by +, it gives us all the Qs. We will process the "OR" later.
            var qList = SplitOrQuery(query);

            // The list that will contain the final result of the query as docids.
            var finalResultsDocIds = new List<int>();

            var orQueryItemsResultsDocIds = new List<List<int>>();
            // Process each Q: 
            foreach (string q in qList)
            {
                // Parentheses.
                var parentheses = Regex.Matches(q, @"\((.+?)\)")
                    .Cast<Match>()
                    .Select(m => m.Groups[1].Value)
                    .ToList();
                foreach (string expression in parentheses)
                {
                    orQueryItemsResultsDocIds.Add(ProcessAndQuery(expression, index));
                }

                // Phrase queries with " ".
                var phraseQueries = Regex.Matches(q, "\"(.+?)\"")
                    .Cast<Match>()
                    .Select(m => m.Groups[1].Value)
                    .ToList();
                foreach (string phraseQuery in phraseQueries)
                {
                    //TODO: phrase queries
                    var phraseQueryTerms = SplitWhiteSpace(phraseQuery);
                    var phraseQueryTermsPostings = new List<Dictionary<int, IList<int>>>();
                    foreach (string term in phraseQueryTerms)
                    {
                        phraseQueryTermsPostings.Add(index.GetPostings(PorterStemmer.ProcessToken(term.Trim())));
                    }
                }

                // AND queries.
                if (q.Trim().Contains(" "))
                {
                    orQueryItemsResultsDocIds.Add(ProcessAndQuery(q, index));
                }

                // Wildcard queries.
                else if (Regex.IsMatch(q, @"(.*\*.*)+"))
                {
                    var termsList = SplitOrQuery(KGramIndex.GenerateNormalQuery(q));
                    foreach (var term in termsList)
                    {
                        orQueryItemsResultsDocIds.Add(index.GetPostings(PorterStemmer.ProcessToken(q.Trim())).Keys.ToList());
                    }
                }

                // Simple queries.
                else if(index.GetPostings(PorterStemmer.ProcessToken(q.Trim())) != null)
                        orQueryItemsResultsDocIds.Add(index.GetPostings(PorterStemmer.ProcessToken(q.Trim())).Keys.ToList());
            }

            // Merge all the OR query items results
            orQueryItemsResultsDocIds = MergeOrResults(orQueryItemsResultsDocIds);
            if(orQueryItemsResultsDocIds.Count > 0)
                finalResultsDocIds.AddRange(orQueryItemsResultsDocIds.Last());

            // If there isn't any result.
            if (finalResultsDocIds.Count == 0)
                return string.Empty;
            // Build the results.
            var finalResults = new StringBuilder();
            foreach (int docId in finalResultsDocIds)
            {
                finalResults.Append(fileNames[docId]);
                finalResults.AppendLine();
                finalResults.AppendLine();
            }

            return finalResults.ToString();
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
        public static void IndexFile(string fileName, NaiveInvertedIndex index, int documentId)
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
        public static void PrintResults(NaiveInvertedIndex index, IList<string> fileNames)
        {
            // TO-DO: print the inverted index.
            // Retrieve the dictionary of terms from the index. (It will already be sorted.)
            // For each term in the dictionary, retrieve the postings list for the
            // term. Use the postings list to print the list of document names that
            // contain the term. (The document ID in a postings list corresponds to 
            // an index in the fileNames list.)

            // Print the postings list so they are all left-aligned starting at the
            // same column, one space after the longest of the term lengths. Example:
            // 
            // as:      document0 document3 document4 document5
            // engines: document1
            // search:  document2 document4 
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