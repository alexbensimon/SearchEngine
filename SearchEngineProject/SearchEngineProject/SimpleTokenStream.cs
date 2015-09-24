using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Text.RegularExpressions;

namespace Cecs429 {
	public class SimpleTokenStream : ITokenStream {
		private IEnumerable<string> mFileLines;
		private IEnumerator<string> mWordsEnumerator;

		public SimpleTokenStream(String filePath) {
			mFileLines =
				from line in File.ReadLines(filePath)
				from word in line.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries)
				select word;
			mWordsEnumerator = mFileLines.GetEnumerator();
			HasNextToken = mWordsEnumerator.MoveNext();
		}

		public bool HasNextToken {
			get; private set;
		}

		public string NextToken() {
			if (!HasNextToken)
				return null;
			string token = Regex.Replace(mWordsEnumerator.Current, @"[^\w\s]*", "").ToLower();
			HasNextToken = mWordsEnumerator.MoveNext();
			return token;
		}

		public void Close() {
			mWordsEnumerator = null;
			mFileLines = null;
		}
	}
}
