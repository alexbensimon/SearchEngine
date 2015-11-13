using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace SearchEngineProject
{
    public class IndexWriter
    {
        private readonly string _mPath;

        public IndexWriter(string path)
        {
            _mPath = path;
        }

        public void BuildIndex(MainWindow mainWindow)
        {
            BuildIndexForDirectory(_mPath, mainWindow);
        }

        private static void BuildIndexForDirectory(string folder, MainWindow window)
        {
            // The inverted index.
            var index = new PositionalInvertedIndex();

            //Initiate the progress Bar
            window.InitiateprogressBar(folder);

            // Index the directory using a naive index
            IndexFiles(folder, index, window);
            foreach (var subfolder in Directory.EnumerateDirectories(folder))
            {
                IndexFiles(subfolder, index, window);
            }

            //Hide the progress bar to allow the user to start searching for terms
            window.HideProgressBar();

            index.ComputeStatistics();
            index.StatToDisk(folder);

            // at this point, "index" contains the in-memory inverted index 
            // now we save the index to disk, building three files: the postings index,
            // the vocabulary list, and the vocabulary table.

            // the array of terms
            string[] dictionary = index.GetDictionary();
            // an array of positions in the vocabulary file
            long[] vocabPositions = new long[dictionary.Length];

            BuildVocabFile(folder, dictionary, vocabPositions);
            BuildPostingsFile(folder, index, dictionary, vocabPositions);
        }

        private static void BuildPostingsFile(string folder, PositionalInvertedIndex index, string[] dictionary, long[] vocabPositions)
        {
            // now build the postings file.
            FileStream postingsFile = new FileStream(Path.Combine(folder, "postings.bin"), FileMode.Create);
            // simultaneously build the vocabulary table on disk, mapping a term index to a 
            // file location in the postings file.
            FileStream vocabTable = new FileStream(Path.Combine(folder, "vocabTable.bin"), FileMode.Create);


            // the first thing we must write to the vocabTable file is the number of vocab terms.
            byte[] tSize = BitConverter.GetBytes(dictionary.Length);
            if (BitConverter.IsLittleEndian)
                Array.Reverse(tSize);
            vocabTable.Write(tSize, 0, tSize.Length);

            int vocabI = 0;
            foreach (string s in dictionary)
            {
                // for each string in dictionary, retrieve its postings.
                var postings = index.GetPostings(s);

                // write the vocab table entry for this term: the byte location of the term in the vocab list file,
                // and the byte location of the postings for the term in the postings file.
                byte[] vPositionBytes = BitConverter.GetBytes(vocabPositions[vocabI]);
                if (BitConverter.IsLittleEndian)
                    Array.Reverse(vPositionBytes);
                vocabTable.Write(vPositionBytes, 0, vPositionBytes.Length);

                byte[] pPositionBytes = BitConverter.GetBytes(postingsFile.Position);
                if (BitConverter.IsLittleEndian)
                    Array.Reverse(pPositionBytes);
                vocabTable.Write(pPositionBytes, 0, pPositionBytes.Length);

                // Number of documents
                byte[] docFreqBytes = BitConverter.GetBytes(postings.Count);
                if (BitConverter.IsLittleEndian)
                    Array.Reverse(docFreqBytes);
                postingsFile.Write(docFreqBytes, 0, docFreqBytes.Length);

                //Document IDs as gaps
                int lastDocId = 0;
                foreach (int docId in postings.Keys)
                {
                    byte[] docIdBytes = BitConverter.GetBytes(docId - lastDocId);
                    if (BitConverter.IsLittleEndian)
                        Array.Reverse(docIdBytes);
                    postingsFile.Write(docIdBytes, 0, docIdBytes.Length);
                    lastDocId = docId;

                    //Number of positions
                    byte[] posFreqBytes = BitConverter.GetBytes(postings[docId].Count);
                    if (BitConverter.IsLittleEndian)
                        Array.Reverse(posFreqBytes);
                    postingsFile.Write(posFreqBytes, 0, posFreqBytes.Length);

                    //Positions as gaps
                    int lastPos = 0;
                    foreach (var position in postings[docId])
                    {
                        byte[] posBytes = BitConverter.GetBytes(position - lastPos);
                        if (BitConverter.IsLittleEndian)
                            Array.Reverse(posBytes);
                        postingsFile.Write(posBytes, 0, posBytes.Length);
                        lastPos = position;
                    }
                }

                vocabI++;
            }
            vocabTable.Close();
            postingsFile.Close();
        }

        private static void BuildVocabFile(string folder, string[] dictionary, long[] vocabPositions)
        {
            // first build the vocabulary list: a file of each vocab word concatenated together.
            // also build an array associating each term with its byte location in this file.
            int vocabI = 0;
            StreamWriter vocabList = new StreamWriter(Path.Combine(folder, "vocab.bin"), false, Encoding.ASCII);

            int vocabPos = 0;
            foreach (string vocabWord in dictionary)
            {
                // for each string in dictionary, save the byte position where that term will start in the vocab file.
                vocabPositions[vocabI] = vocabPos;
                vocabList.Write(vocabWord); // then write the string
                vocabI++;
                vocabPos += vocabWord.Length;
            }
            vocabList.Close();
        }

        private static void IndexFiles(string folder, PositionalInvertedIndex index, MainWindow window)
        {
            var documentId = 0;
            FileStream writer = new FileStream(Path.Combine(folder, "docWeights.bin"), FileMode.Create);

            foreach (string fileName in Directory.EnumerateFiles(Path.Combine(Environment.CurrentDirectory,
                folder)))
            {
                if (fileName.EndsWith(".txt"))
                {
                    var termToOccurence = IndexFile(fileName, index, documentId);
                    
                    // Calculate document weight.    
                    // Compute all wdts.
                    var wdts = new List<double>();
                    foreach (var pair in termToOccurence)
                        wdts.Add(1.0 + Math.Log(pair.Value));

                    // Calculate ld for this document.
                    double sumTemp = 0.0;
                    foreach (var wdt in wdts)
                        sumTemp += wdt*wdt;
                    double ld = Math.Sqrt(sumTemp);

                    // Write ld in docWeights.bin.
                    var buffer = BitConverter.GetBytes(ld);
                    if (BitConverter.IsLittleEndian)
                        Array.Reverse(buffer);
                    writer.Write(buffer, 0, buffer.Length);

                    documentId++;
                }

                window.IncrementProgressBar();
            }
            writer.Close();
        }

        private static Dictionary<string, int> IndexFile(string fileName, PositionalInvertedIndex index, int documentId)
        {
            var tftds = new Dictionary<string, int>();

            try
            {
                SimpleTokenStream stream = new SimpleTokenStream(fileName);
                var position = 0;

                while (stream.HasNextToken)
                {
                    var token = stream.NextToken();
                    if (token.Replace("-", "") == "") continue;
                    if (token.Contains("-"))
                    {
                        foreach (var tokenHyphen in token.Split('-'))
                        {
                            var termHyphen = PorterStemmer.ProcessToken(tokenHyphen);
                            index.AddTerm(termHyphen, documentId, position);
                            if (tftds.ContainsKey(termHyphen))
                                tftds[termHyphen]++;
                            else
                            {
                                tftds.Add(termHyphen, 1);
                            }
                            KGramIndex.GenerateKgrams(tokenHyphen, true, 0);
                        }
                    }
                    var term = PorterStemmer.ProcessToken(token.Replace("-", ""));
                    index.AddTerm(term, documentId, position);
                    if (tftds.ContainsKey(term))
                        tftds[term]++;
                    else
                    {
                        tftds.Add(term, 1);
                    }
                    KGramIndex.GenerateKgrams(token.Replace("-", ""), true, 0);
                    position++;
                }
                stream.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }

            return tftds;
        }
    }
}
