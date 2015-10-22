﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cecs429 {
	public interface ITokenStream	 {
		/// <summary>
		/// Returns true if the stream has more tokens remaining.
		/// </summary>
		bool HasNextToken { get; }

		/// <summary>
		/// Returns the next token from the stream, or null if there are no more remaining tokens.
		/// </summary>
		/// <returns></returns>
		string NextToken();
	}
}
