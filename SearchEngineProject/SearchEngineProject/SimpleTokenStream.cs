using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Text.RegularExpressions;

namespace SearchEngineProject
{
    public class SimpleTokenStream : ITokenStream
    {
        private IEnumerable<string> _mFileLines;
        private IEnumerator<string> _mWordsEnumerator;

        public SimpleTokenStream(String filePath)
        {
            _mFileLines =
                from line in File.ReadLines(filePath)
                from word in line.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries)
                select word;
            _mWordsEnumerator = _mFileLines.GetEnumerator();
            HasNextToken = _mWordsEnumerator.MoveNext();
        }

        public bool HasNextToken
        {
            get; private set;
        }

        public string NextToken()
        {
            if (!HasNextToken)
                return null;
            var token = Regex.Replace(_mWordsEnumerator.Current, @"[^-\w\s]*", "").ToLower();
            HasNextToken = _mWordsEnumerator.MoveNext();
            return token;
        }

        public void Close()
        {
            _mWordsEnumerator = null;
            _mFileLines = null;
        }
    }
}
