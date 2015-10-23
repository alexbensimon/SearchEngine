using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace SearchEngineProject
{
    public class DiskPositionalIndex : IDisposable
    {
        private string mPath;
        private FileStream mVocabList;
        private FileStream mPostings;
        private long[] mVocabTable;
        private List<string> mFileNames;

        public DiskPositionalIndex(string path)
        {
            // open the vocabulary table and read it into memory. 
            // we will end up with an array of T pairs of longs, where the first value is
            // a position in the vocabularyTable file, and the second is a position in
            // the postings file.
            
            mPath = path;

            mVocabList = new FileStream(Path.Combine(path, "vocab.bin"), FileMode.Open, FileAccess.Read);
            mPostings = new FileStream(Path.Combine(path, "postings.bin"), FileMode.Open, FileAccess.Read);

            mVocabTable = ReadVocabTable(path);
            mFileNames = ReadFileNames(path);
        }

        private static int[][] ReadPostingsFromFile(FileStream postings, long postingsPosition, bool positionsRequested)
        {
            // seek the specified position in the file
            postings.Seek(postingsPosition, SeekOrigin.Begin);

            // read 4 bytes from the file into a buffer, for the document frequency
            byte[] buffer = new byte[4];
            postings.Read(buffer, 0, buffer.Length);

            // the next two lines deal with Endianness issues and should be used every time
            // a read is done.
            if (BitConverter.IsLittleEndian)
                Array.Reverse(buffer);

            // convert the byte array to an int
            int documentFrequency = BitConverter.ToInt32(buffer, 0);

            // initialize the array of document IDs to return.
            int[][] postingsArray = new int[documentFrequency][];

            int previousDocId = 0;
            for (int i = 0; i < documentFrequency; i++)
            {
                //Read the document ID
                buffer = new byte[4];
                postings.Read(buffer, 0, buffer.Length);

                if (BitConverter.IsLittleEndian)
                    Array.Reverse(buffer);

                int docGap = BitConverter.ToInt32(buffer, 0);
                previousDocId += docGap;

                //Read the term frequency in the document
                buffer = new byte[4];
                postings.Read(buffer, 0, buffer.Length);

                if (BitConverter.IsLittleEndian)
                    Array.Reverse(buffer);

                int termFrequency = BitConverter.ToInt32(buffer, 0);

                if (positionsRequested)
                {
                    postingsArray[i] = new int[termFrequency+1];
                    postingsArray[i][0] = previousDocId;

                    int previousPos = 0;
                    for (int j = 0; j < termFrequency; j++)
                    {
                        //Read a position
                        buffer = new byte[4];
                        postings.Read(buffer, 0, buffer.Length);

                        if (BitConverter.IsLittleEndian)
                            Array.Reverse(buffer);

                        int posGap = BitConverter.ToInt32(buffer, 0);
                        previousPos += posGap;
                        postingsArray[i][j+1] = previousPos;
                    }
                }
                else
                {
                    postingsArray[i] = new int[1];
                    postingsArray[i][0] = previousDocId;

                    //TODO Ameliorer cett partie, on peut seek plus loin peut etre
                    buffer = new byte[4*termFrequency];
                    postings.Read(buffer, 0, buffer.Length);

                    if (BitConverter.IsLittleEndian)
                        Array.Reverse(buffer);
                } 
            }

            return postingsArray;
        }

        public int[][] GetPostings(string term, bool positionsRequested)
        {
            long postingsPosition = BinarySearchVocabulary(term);
            if (postingsPosition >= 0)
                return ReadPostingsFromFile(mPostings, postingsPosition, positionsRequested);
            return null;
        }

        private long BinarySearchVocabulary(string term)
        {
            // do a binary search over the vocabulary, using the vocabTable and the file vocabList.
            int i = 0, j = mVocabTable.Length / 2 - 1;
            while (i <= j)
            {
                int m = (i + j) / 2;
                long vListPosition = mVocabTable[m * 2];
                int termLength;
                if (m == mVocabTable.Length / 2 - 1)
                {
                    termLength = (int)(mVocabList.Length - mVocabTable[m * 2]);
                }
                else
                {
                    termLength = (int)(mVocabTable[(m + 1) * 2] - vListPosition);
                }
                mVocabList.Seek(vListPosition, SeekOrigin.Begin);

                byte[] buffer = new byte[termLength];
                mVocabList.Read(buffer, 0, termLength);
                string fileTerm = Encoding.ASCII.GetString(buffer);

                int compareValue = term.CompareTo(fileTerm);
                if (compareValue == 0)
                {
                    // found it!
                    return mVocabTable[m * 2 + 1];
                }
                else if (compareValue < 0)
                {
                    j = m - 1;
                }
                else
                {
                    i = m + 1;
                }
            }
            return -1;
        }

        private static List<string> ReadFileNames(string indexName)
        {
            List<string> names = new List<string>();
            foreach (string fileName in Directory.EnumerateFiles(
                Path.Combine(Environment.CurrentDirectory, indexName)))
            {
                if (fileName.EndsWith(".txt"))
                {
                    names.Add(Path.GetFileName(fileName));
                }
            }
            return names;
        }

        private static long[] ReadVocabTable(string indexName)
        {
            long[] vocabTable;

            FileStream tableFile = new FileStream(
                Path.Combine(indexName, "vocabTable.bin"),
                FileMode.Open, FileAccess.Read);

            byte[] byteBuffer = new byte[4];
            tableFile.Read(byteBuffer, 0, byteBuffer.Length);
            if (BitConverter.IsLittleEndian)
                Array.Reverse(byteBuffer);

            int tableIndex = 0;
            vocabTable = new long[BitConverter.ToInt32(byteBuffer, 0) * 2];
            byteBuffer = new byte[8];

            while (tableFile.Read(byteBuffer, 0, byteBuffer.Length) > 0)
            { // while we keep reading 4 bytes
                if (BitConverter.IsLittleEndian)
                    Array.Reverse(byteBuffer);
                vocabTable[tableIndex] = BitConverter.ToInt64(byteBuffer, 0);
                tableIndex++;
            }
            tableFile.Close();
            return vocabTable;
        }

        public List<string> FileNames
        {
            get { return mFileNames; }
        }

        public int TermCount
        {
            get { return mVocabTable.Length / 2; }
        }

        public void Dispose()
        {
            if (mVocabList != null)
                mVocabList.Close();
            if (mPostings != null)
                mPostings.Close();
        }
    }
}
