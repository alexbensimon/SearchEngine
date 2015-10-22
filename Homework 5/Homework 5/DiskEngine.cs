using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cecs429 {
	class DiskEngine {
		static void Main(string[] args) {
			Console.WriteLine("Menu:");
			Console.WriteLine("1) Build index");
			Console.WriteLine("2) Read and query index");
			Console.WriteLine("Choose a selection:");
			int menuChoice = Convert.ToInt32(Console.ReadLine());

			switch (menuChoice) {
				case 1:
					Console.WriteLine("Enter the name of a directory to index: ");
					string folder = Console.ReadLine();

					IndexWriter writer = new IndexWriter(folder);
					writer.BuildIndex();
					break;

				case 2:
					Console.WriteLine("Enter the name of an index to read:");
					string indexName = Console.ReadLine();

					DiskInvertedIndex index = new DiskInvertedIndex(indexName);

					while (true) {
						Console.WriteLine("Enter a search term:");
						string token = Console.ReadLine();

						if (token.Equals("EXIT"))
							break;

						int[] postingsList = index.GetPostings(PorterStemmer.ProcessToken(token.ToLower()));
						if (postingsList == null)
							Console.WriteLine("Term not found");
						else {
							Console.Write("Docs: ");
							foreach (int post in postingsList) {
								Console.Write("{0} ", index.FileNames[post]);
							}
							Console.WriteLine();
							Console.WriteLine();
						}
					}

					break;
			}
		}
	}
}
