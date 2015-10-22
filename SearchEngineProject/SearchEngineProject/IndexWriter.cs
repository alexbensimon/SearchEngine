using System;
using System.IO;
using System.Text;

namespace SearchEngineProject
{
    public class IndexWriter
    {
        private string mPath;

        public IndexWriter(string path)
        {
            mPath = path;
        }

        public void BuildIndex()
        {
            BuildIndexForDirectory(mPath);
        }

        private static void BuildIndexForDirectory(string folder)
        {
            PositionalInvertedIndex index = new PositionalInvertedIndex();

            // Index the directory using a naive index
            IndexFiles(folder, index);

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

                // write the postings file for this term. first, the document frequency for the term, then
                // the document IDs, encoded as gaps.
                byte[] docFreqBytes = BitConverter.GetBytes(postings.Count);
                if (BitConverter.IsLittleEndian)
                    Array.Reverse(docFreqBytes);
                postingsFile.Write(docFreqBytes, 0, docFreqBytes.Length);

                int lastDocId = 0;
                foreach (int docId in postings.Keys)
                {
                    byte[] docIdBytes = BitConverter.GetBytes(docId - lastDocId); // encode a gap, not a doc ID
                    if (BitConverter.IsLittleEndian)
                        Array.Reverse(docIdBytes);
                    postingsFile.Write(docIdBytes, 0, docIdBytes.Length);
                    lastDocId = docId;
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

        private static void IndexFiles(string folder, PositionalInvertedIndex index)
        {
            int documentId = 0;

            Console.WriteLine("Indexing " + Path.Combine(Environment.CurrentDirectory, folder));
            foreach (string fileName in Directory.EnumerateFiles(
                Path.Combine(Environment.CurrentDirectory, folder)))
            {
                if (fileName.EndsWith(".txt"))
                {
                    IndexFile(fileName, index, documentId);
                    documentId++;
                }
            }
        }

        private static void IndexFile(string fileName, PositionalInvertedIndex index,
         int documentId)
        {

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
                            index.AddTerm(PorterStemmer.ProcessToken(tokenHyphen), documentId, position);
                            KGramIndex.AddType(tokenHyphen);
                        }
                    }
                    index.AddTerm(PorterStemmer.ProcessToken(token.Replace("-", "")), documentId, position);
                    KGramIndex.AddType(token.Replace("-", ""));
                    position++;
                }
                stream.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }
    }
}
